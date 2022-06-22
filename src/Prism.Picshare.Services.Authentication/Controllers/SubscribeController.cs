// -----------------------------------------------------------------------
//  <copyright file = "SubscribeController.cs" company = "Prism">
//  Copyright (c) Prism.All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Prism.Picshare.Services.Authentication.Commands;
using Prism.Picshare.Services.Authentication.Configuration;

namespace Prism.Picshare.Services.Authentication.Controllers;

public class SubscribeController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<SubscribeController> _logger;

    public SubscribeController(IMediator mediator, ILogger<SubscribeController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("/api/subscribe")]
    public async Task<IActionResult> Subscribe([FromBody] SubscribeRequest request)
    {
        _logger.LogInformation("Processing incoming request : subscribe - {login} - {organisation}", request.Login, request.Organisation);
        
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