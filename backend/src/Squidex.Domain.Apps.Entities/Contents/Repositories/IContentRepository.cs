﻿// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschraenkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using NodaTime;
using Squidex.Domain.Apps.Core.Contents;
using Squidex.Domain.Apps.Entities.Apps;
using Squidex.Domain.Apps.Entities.Schemas;
using Squidex.Infrastructure;
using Squidex.Infrastructure.Queries;

namespace Squidex.Domain.Apps.Entities.Contents.Repositories;

public interface IContentRepository
{
    IAsyncEnumerable<IContentEntity> StreamScheduledWithoutDataAsync(Instant now, SearchScope scope,
        CancellationToken ct = default);

    IAsyncEnumerable<IContentEntity> StreamAll(DomainId appId, HashSet<DomainId>? schemaIds, SearchScope scope,
        CancellationToken ct = default);

    IAsyncEnumerable<IContentEntity> StreamReferencing(DomainId appId, DomainId references, int take, SearchScope scope,
        CancellationToken ct = default);

    Task<IResultList<IContentEntity>> QueryAsync(IAppEntity app, List<ISchemaEntity> schemas, Q q, SearchScope scope,
        CancellationToken ct = default);

    Task<IResultList<IContentEntity>> QueryAsync(IAppEntity app, ISchemaEntity schema, Q q, SearchScope scope,
        CancellationToken ct = default);

    Task<IReadOnlyList<ContentIdStatus>> QueryIdsAsync(DomainId appId, DomainId schemaId, FilterNode<ClrValue> filterNode, SearchScope scope,
        CancellationToken ct = default);

    Task<IReadOnlyList<ContentIdStatus>> QueryIdsAsync(DomainId appId, HashSet<DomainId> ids, SearchScope scope,
        CancellationToken ct = default);

    Task<IContentEntity?> FindContentAsync(IAppEntity app, ISchemaEntity schema, DomainId id, SearchScope scope,
        CancellationToken ct = default);

    Task<bool> HasReferrersAsync(DomainId appId, DomainId reference, SearchScope scope,
        CancellationToken ct = default);

    Task ResetScheduledAsync(DomainId documentId, SearchScope scope,
        CancellationToken ct = default);
}
