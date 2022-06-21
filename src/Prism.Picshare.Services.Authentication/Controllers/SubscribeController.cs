// -----------------------------------------------------------------------
//  <copyright file = "SubscribeController.cs" company = "Prism">
//  Copyright (c) Prism.All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Prism.Picshare.Services.Authentication.Commands;
using Prism.Picshare.Services.Authentication.Configuration;

namespace Prism.Picshare.Services.Authentication.Controllers;

public class SubscribeController : Controller
{
    private readonly IMediator _mediator;

    public SubscribeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("/api/subscribe")]
    public async Task<IActionResult> Subscribe([FromBody] SubscribeRequest request)
    {
        var responseCode = await _mediator.Send(request);

        if (responseCode == ResponseCodes.Ok)
        {
            return NoContent();
        }

        return Conflict(new
        {
            code = responseCode
        });
    }
}