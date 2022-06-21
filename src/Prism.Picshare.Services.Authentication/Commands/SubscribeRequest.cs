// -----------------------------------------------------------------------
//  <copyright file = "SubscribeRequest.cs" company = "Prism">
//  Copyright (c) Prism.All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

using System.Text.Json.Serialization;
using Dapr.Client;
using FluentValidation;
using MediatR;
using Prism.Picshare.Domain;
using Prism.Picshare.Services.Authentication.Configuration;

namespace Prism.Picshare.Services.Authentication.Commands;

public record SubscribeRequest(
    [property: JsonPropertyName("login")]
    string Login,
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
        var organisations = await _daprClient.QueryStateAsync<Organisation>(Stores.Organisations, $" {{ \"filter\" : {{ \"EQ\": {{ \"name\": \"{request.Organisation}\" }} }} }}",
            cancellationToken: cancellationToken);

        if (organisations?.Results.Any() == true)
        {
            return ResponseCodes.ExistingOrganisation;
        }

        return ResponseCodes.Ok;
    }
}