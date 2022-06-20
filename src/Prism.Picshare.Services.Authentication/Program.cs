// -----------------------------------------------------------------------
//  <copyright file = "Program.cs" company = "Prism">
//  Copyright (c) Prism.All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

using FluentValidation;
using Grpc.Net.Client;
using MediatR;
using Prism.Picshare.Behaviors;
using Prism.Picshare.Insights;
using Prism.Picshare.Services.Authentication.Commands;

var builder = WebApplication.CreateBuilder(args);

var applicationAssembly = typeof(Program).Assembly;
builder.Services.AddMediatR(applicationAssembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(SaveCommandBehaviour<,>));
builder.Services.AddValidatorsFromAssembly(applicationAssembly);

builder.Services.AddInsights();

builder.Services.AddDaprClient(config =>
{
    config.UseGrpcChannelOptions(new GrpcChannelOptions
    {
        MaxReceiveMessageSize = 30 * 1024 * 1024,
        MaxSendMessageSize = 30 * 1024 * 1024
    });
});

builder.Services.AddHealthChecks();
builder.Services.AddControllers();

var app = builder.Build();

app.MapSubscribeHandler();
app.UseCloudEvents();
app.UseHealthChecks("/health");

app.MapControllers();

app.Run();