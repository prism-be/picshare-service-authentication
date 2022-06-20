// -----------------------------------------------------------------------
//  <copyright file = "SaveCommandBehaviour.cs" company = "Prism">
//  Copyright (c) Prism.All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

using Dapr.Client;
using MediatR;
using Prism.Picshare.Services.Authentication.Configuration;

namespace Prism.Picshare.Services.Authentication.Commands;

public record CommandHistory<T>(string Key, DateTime Date, T Command);

public class SaveCommandBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly DaprClient _daprClient;

    public SaveCommandBehaviour(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var history = new CommandHistory<TRequest>(Guid.NewGuid().ToString(), DateTime.UtcNow, request);
        await _daprClient.SaveStateAsync(Stores.Commands, history.Key, history, cancellationToken: cancellationToken);

        return await next();
    }
}