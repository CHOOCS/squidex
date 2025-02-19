﻿// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschraenkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using NJsonSchema;
using NJsonSchema.Generation;
using Squidex.Web;

namespace Squidex.Areas.Api.Config.OpenApi;

public sealed class RequiredSchemaProcessor : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        if (context.ContextualType.GetAttribute<OpenApiRequestAttribute>() != null)
        {
            return;
        }

        FixRequired(context.Schema);

        foreach (var schema in context.Schema.AllOf)
        {
            FixRequired(schema);
        }

        foreach (var schema in context.Schema.OneOf)
        {
            FixRequired(schema);
        }

        static void FixRequired(JsonSchema schema)
        {
            foreach (var property in schema.Properties.Values)
            {
                if (!property.IsNullable(SchemaType.OpenApi3))
                {
                    property.IsRequired = true;
                }
            }
        }
    }
}
