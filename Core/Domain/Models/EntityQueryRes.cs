using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Models
{
    public class EntityQueryRes<TEntity>
        where TEntity : Entity
    {
        public IQueryable<TEntity> QueryData { get; set; }
        public int TotalCount { get; set; }

        public static EntityQueryRes<TEntity> Res(IQueryable<TEntity> data,int count)
		{
            return new EntityQueryRes<TEntity>
            {
                QueryData = data,
                TotalCount = count
            };
		}
    }

}
