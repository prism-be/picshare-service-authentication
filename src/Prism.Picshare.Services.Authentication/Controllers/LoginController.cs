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

public class LoginController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<LoginController> _logger;

    public LoginController(IMediator mediator, ILogger<LoginController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("/api/authentication/register")]
    public async Task<IActionResult> Register([FromBody] RegisterAccountRequest request)
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