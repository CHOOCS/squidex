﻿// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschraenkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using Squidex.Infrastructure.Reflection;
using Squidex.Infrastructure.Validation;
using Squidex.Shared.Identity;
using Squidex.Shared.Users;
using Squidex.Web;

namespace Squidex.Areas.Api.Controllers.Users.Models;

public sealed class UserDto : Resource
{
    /// <summary>
    /// The ID of the user.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The email of the user. Unique value.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// The display name (usually first name and last name) of the user.
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    /// Determines if the user is locked.
    /// </summary>
    public bool IsLocked { get; set; }

    /// <summary>
    /// Additional permissions for the user.
    /// </summary>
    public IEnumerable<string> Permissions { get; set; }

    public static UserDto FromDomain(IUser user, Resources resources)
    {
        var result = SimpleMapper.Map(user, new UserDto());

        result.DisplayName = user.Claims.DisplayName()!;
        result.Permissions = user.Claims.Permissions().ToIds();

        return result.CreateLinks(resources);
    }

    private UserDto CreateLinks(Resources resources)
    {
        var values = new { id = Id };

        if (resources.Controller is UserManagementController)
        {
            AddSelfLink(resources.Url<UserManagementController>(c => nameof(c.GetUser), values));
        }
        else
        {
            AddSelfLink(resources.Url<UsersController>(c => nameof(c.GetUser), values));
        }

        if (!resources.Controller.IsUser(Id))
        {
            if (resources.CanLockUser && !IsLocked)
            {
                AddPutLink("lock",
                    resources.Url<UserManagementController>(c => nameof(c.LockUser), values));
            }

            if (resources.CanUnlockUser && IsLocked)
            {
                AddPutLink("unlock",
                    resources.Url<UserManagementController>(c => nameof(c.UnlockUser), values));
            }

            AddDeleteLink("delete",
                resources.Url<UserManagementController>(x => nameof(x.DeleteUser), values));
        }

        if (resources.CanUpdateUser)
        {
            AddPutLink("update",
                resources.Url<UserManagementController>(c => nameof(c.PutUser), values));
        }

        AddGetLink("picture",
            resources.Url<UsersController>(c => nameof(c.GetUserPicture), values));

        return this;
    }
}
