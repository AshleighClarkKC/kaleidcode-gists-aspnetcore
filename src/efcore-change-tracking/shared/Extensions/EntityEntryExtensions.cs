using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Kaleidocode.Gists.ChangeTracking.Shared.Extensions;

public static class EntityEntryExtensions
{
    public static bool IsTracked<TModel>(this EntityEntry<TModel> entity) where TModel : class
        => !entity.State.Equals(EntityState.Detached) && !entity.State.Equals(EntityState.Unchanged);
}
