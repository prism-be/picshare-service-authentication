// -----------------------------------------------------------------------
//  <copyright file = "SubscribeRequest.cs" company = "Prism">
//  Copyright (c) Prism.All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

using System;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Dapr.Client;
using FluentValidation;
using Isopoh.Cryptography.Argon2;
using MediatR;
using Prism.Picshare.Domain;
using Prism.Picshare.Events;
using Prism.Picshare.Services.Authentication.Configuration;

namespace Prism.Picshare.Services.Authentication.Commands;

public record SubscribeRequest(
    [property: JsonPropertyName("login")] string Login,
    [property: JsonPropertyName("email")] string Email,
    [property: JsonPropertyName("password")]
    string Password,
    [property: JsonPropertyName("organisation")]
    string Organisation) : IRequest<ResponseCodes>;

public class SubscribeRequestValidator : AbstractValidator<SubscribeRequest>
{
    public SubscribeRequestValidator()
    {
        RuleFor(x => x.Organisation).NotEmpty().MaximumLength(Constants.MaxShortStringLength);
        RuleFor(x => x.Login).NotEmpty().MaximumLength(Constants.MaxShortStringLength);
        RuleFor(x => x.Email).NotEmpty().MaximumLength(Constants.MaxShortStringLength).EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MaximumLength(Constants.MaxShortStringLength);
    }
}

public class SubscribeRequestHandler : IRequestHandler<SubscribeRequest, ResponseCodes>
{
    private readonly DaprClient _daprClient;

    public SubscribeRequestHandler(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }

    public async Task<ResponseCodes> Handle(SubscribeRequest request, CancellationToken cancellationToken)
    {
        var organisationId = await _daprClient.GetStateAsync<Guid>(Stores.OrganisationsName, request.Organisation, cancellationToken: cancellationToken);

        if (organisationId != default)
        {
            return ResponseCodes.ExistingOrganisation;
        }

        var credentials = await _daprClient.GetStateAsync<Credentials>(Stores.Credentials, request.Login, cancellationToken: cancellationToken);

        if (credentials != default)
        {
            return ResponseCodes.ExistingUsername;
        }

        var organisation = new Organisation
        {
            Id = Guid.NewGuid(),
            Name = request.Organisation
        };

        credentials = new Credentials
        {
            Id = Security.GenerateIdentifier(),
            Login = request.Login,
            PasswordHash = Argon2.Hash(request.Password)
        };

        var user = new User
        {
            Id = credentials.Id,
            OrganisationId = organisation.Id,
            Email = request.Email,
            EmailValidated = false
        };

        await _daprClient.SaveStateAsync(Stores.OrganisationsName, organisation.Name, organisation.Id, cancellationToken: cancellationToken);
        await _daprClient.SaveStateAsync(Stores.Organisations, organisation.Id.ToString(), organisation, cancellationToken: cancellationToken);
        await _daprClient.SaveStateAsync(Stores.Credentials, credentials.Login, credentials, cancellationToken: cancellationToken);
        await _daprClient.SaveStateAsync(Stores.Users, user.Key, user, cancellationToken: cancellationToken);
        await _daprClient.PublishEventAsync(DaprConfiguration.PubSub, Topics.User.Register, user, cancellationToken);

        return ResponseCodes.Ok;
    }
}