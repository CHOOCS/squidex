﻿// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschraenkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;

#pragma warning disable MA0073 // Avoid comparison with bool constant

namespace Squidex.Web.Pipeline;

public sealed class CachingFilter : IAsyncActionFilter
{
    private readonly CachingManager cachingManager;

    public CachingFilter(CachingManager cachingManager)
    {
        this.cachingManager = cachingManager;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (IgnoreFilter(context))
        {
            await next();
            return;
        }

        cachingManager.Start(context.HttpContext);

        var resultContext = await next();

        cachingManager.Finish(context.HttpContext);

        if (context.HttpContext.Response.HasStarted == false &&
            context.HttpContext.Response.Headers.TryGetString(HeaderNames.ETag, out var etag) &&
            IsCacheable(context.HttpContext, etag))
        {
            resultContext.Result = new StatusCodeResult(304);
        }
    }

    private static bool IsCacheable(HttpContext httpContext, string etag)
    {
        if (!HttpMethods.IsGet(httpContext.Request.Method) || httpContext.Response.StatusCode != 200)
        {
            return false;
        }

        if (!httpContext.Request.Headers.TryGetString(HeaderNames.IfNoneMatch, out var noneMatchValue))
        {
            return false;
        }

        return ETagUtils.IsSameEtag(noneMatchValue, etag);
    }

    private static bool IgnoreFilter(ActionExecutingContext context)
    {
        return context.ActionDescriptor.EndpointMetadata.Any(x => x is IgnoreCacheFilterAttribute);
    }
}
