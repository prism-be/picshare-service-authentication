// -----------------------------------------------------------------------
//  <copyright file = "EmailValidationRequest.cs" company = "Prism">
//  Copyright (c) Prism.All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

using Dapr.Client;
using FluentValidation;
using MediatR;
using Prism.Picshare.Services.Authentication.Configuration;

namespace Prism.Picshare.Services.Authentication.Commands;

public record EmailValidationRequest(Guid OrganisationId, Guid UserId) : IRequest<ResponseCodes>;

public class EmailValidationRequestValidator : AbstractValidator<EmailValidationRequest>
{
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

    public Task<ResponseCodes> Handle(EmailValidationRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}