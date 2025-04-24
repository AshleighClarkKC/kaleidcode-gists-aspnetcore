using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kaleidocode.Gists.ChangeTracking.Models.Base;

namespace Kaleidocode.Gists.ChangeTracking.Repositories.Contracts
{
    public interface IBaseRepository<TEntity> where TEntity : BaseModel
    {
        void Add(TEntity entity);

        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        void AddRange(IEnumerable<TEntity> entities);

        Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        void Update(TEntity entity);

        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        void UpdateRange(IEnumerable<TEntity> entities);

        Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        void Remove(TEntity entity);

        Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default);

        void RemoveRange(IEnumerable<TEntity> entities);

        Task RemoveRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    }
}
