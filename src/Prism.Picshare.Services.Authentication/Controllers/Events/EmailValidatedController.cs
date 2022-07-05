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

namespace Prism.Picshare.Services.Authentication.Controllers.Events;

public class EmailValidatedController
{
    private readonly IMediator _mediator;

    public EmailValidatedController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Topic(DaprConfiguration.PubSub, Topics.Email.Validated)]
    [HttpPost(Topics.RoutePrefix + Topics.Email.Validated)]
    public Task<IActionResult> Validate(User user)
    {
        throw new NotImplementedException();
    }
}