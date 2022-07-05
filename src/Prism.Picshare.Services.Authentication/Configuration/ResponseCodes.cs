// -----------------------------------------------------------------------
//  <copyright file = "StatusCodes.cs" company = "Prism">
//  Copyright (c) Prism.All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Prism.Picshare.Services.Authentication.Configuration;

public enum ResponseCodes
{
    Ok = 0,
    UserNotFound = 40402,
    ExistingOrganisation = 40901,
    ExistingUsername = 40902,
}