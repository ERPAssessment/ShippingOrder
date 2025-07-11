﻿using Microsoft.EntityFrameworkCore.ChangeTracking;

public static class EntityEntryExtensions
{
  public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
      entry.References.Any(r =>
          r.TargetEntry != null &&
          r.TargetEntry.Metadata.IsOwned() &&
          (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
}
