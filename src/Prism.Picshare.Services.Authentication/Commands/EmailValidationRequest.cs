﻿// -----------------------------------------------------------------------
//  <copyright file = "EmailValidationRequest.cs" company = "Prism">
//  Copyright (c) Prism.All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

using Dapr.Client;
using FluentValidation;
using MediatR;
using Prism.Picshare.Domain;
using Prism.Picshare.Services.Authentication.Configuration;

namespace Prism.Picshare.Services.Authentication.Commands;

public record EmailValidationRequest(Guid OrganisationId, Guid UserId) : IRequest<ResponseCodes>;

public class EmailValidationRequestValidator : AbstractValidator<EmailValidationRequest>
{
    public EmailValidationRequestValidator()
    {
        RuleFor(x => x.OrganisationId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}

public class EmailValidationRequestHandler : IRequestHandler<EmailValidationRequest, ResponseCodes>
{
    private readonly DaprClient _daprClient;
    private readonly ILogger<EmailValidationRequestHandler> _logger;

    public EmailValidationRequestHandler(ILogger<EmailValidationRequestHandler> logger, DaprClient daprClient)
    {
        _logger = logger;
        _daprClient = daprClient;
    }

    public async Task<ResponseCodes> Handle(EmailValidationRequest request, CancellationToken cancellationToken)
    {
        var key = EntityReference.ComputeKey(request.OrganisationId, request.UserId);
        var user = await _daprClient.GetStateAsync<User>(Stores.Users, key, cancellationToken: cancellationToken);

        if (user == null)
        {
            _logger.LogWarning("The user with reference {key} does not exists", key);

            return ResponseCodes.UserNotFound;
        }

        user.EmailValidated = true;
        await _daprClient.SaveStateAsync(Stores.Users, key, user, cancellationToken: cancellationToken);

        return ResponseCodes.Ok;
    }
}