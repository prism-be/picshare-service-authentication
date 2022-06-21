// -----------------------------------------------------------------------
//  <copyright file = "TestsExtensions.cs" company = "Prism">
//  Copyright (c) Prism.All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

using Dapr.Client;
using Moq;

namespace Prism.Picshare.Services.Authentication.Tests;

public static class TestsExtensions
{
    public static void VerifySaveState<TExpected>(this Mock<DaprClient> mock, string expectedStore)
    {
        mock.Verify(x => x.SaveStateAsync(expectedStore, It.IsAny<string>(), It.IsAny<TExpected>(), It.IsAny<StateOptions>(), It.IsAny<IReadOnlyDictionary<string, string>>(), default), Times.Once);
    }
}