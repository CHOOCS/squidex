﻿// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschraenkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using Squidex.Domain.Apps.Core.Contents;
using Squidex.Domain.Apps.Core.Schemas;
using Squidex.Infrastructure;
using Squidex.Infrastructure.Json.Objects;

namespace Squidex.Domain.Apps.Core.ConvertContent;

public sealed class AddSchemaNames : IContentItemConverter
{
    private readonly ResolvedComponents components;

    public AddSchemaNames(ResolvedComponents components)
    {
        this.components = components;
    }

    public JsonObject ConvertItemBefore(IField parentField, JsonObject source, IEnumerable<IField> schema)
    {
        if (parentField is IArrayField)
        {
            return source;
        }

        if (source.ContainsKey("schemaName"))
        {
            return source;
        }

        if (!Component.IsValid(source, out var discriminator))
        {
            return source;
        }

        var id = DomainId.Create(discriminator);

        if (components.TryGetValue(id, out var component))
        {
            source["schemaName"] = component.Name;
        }

        return source;
    }
}
