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

        void Update(TEntity entity);

        void Delete(TEntity entity);
    }
}
