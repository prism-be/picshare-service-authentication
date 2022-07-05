// -----------------------------------------------------------------------
//  <copyright file = "EmailValidatedController.cs" company = "Prism">
//  Copyright (c) Prism.All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

using Dapr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Prism.Picshare.Domain;
using Prism.Picshare.Events;
using Prism.Picshare.Services.Authentication.Commands;
using Prism.Picshare.Services.Authentication.Configuration;

namespace Prism.Picshare.Services.Authentication.Controllers.Events;

public class EmailValidatedController: Controller
{
    private readonly IMediator _mediator;

    public EmailValidatedController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Topic(DaprConfiguration.PubSub, Topics.Email.Validated)]
    [HttpPost(Topics.RoutePrefix + Topics.Email.Validated)]
    public async Task<IActionResult> Validate(User user)
    {
        var result = await _mediator.Send(new EmailValidationRequest(user.OrganisationId, user.Id));

        return result == ResponseCodes.Ok ? Ok() : NotFound();
    }
}