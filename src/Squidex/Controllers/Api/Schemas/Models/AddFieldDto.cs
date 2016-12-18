﻿// ==========================================================================
//  AddFieldDto.cs
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex Group
//  All rights reserved.
// ==========================================================================

using System.ComponentModel.DataAnnotations;

// ReSharper disable ConvertIfStatementToReturnStatement

namespace Squidex.Controllers.Api.Schemas.Models
{
    public sealed class AddFieldDto
    {
        /// <summary>
        /// The name of the field. Must be unique within the schema.
        /// </summary>
        [Required]
        [RegularExpression("^[a-z0-9]+(\\-[a-z0-9]+)*$")]
        public string Name { get; set; }

        /// <summary>
        /// The field properties.
        /// </summary>
        [Required]
        public FieldPropertiesDto Properties { get; set; }
    }
}

