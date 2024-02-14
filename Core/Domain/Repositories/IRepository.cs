using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using Domain.Models;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IRepository<TEntity>
        where TEntity : Entity
    {
        IQueryable<TEntity> Get(Expression<Func<TEntity,bool>> expression = null);

		/// <summary>
		/// Begins tracking the given entity, and any other reachable entities that are not already being tracked, 
		/// in the Added state such that they will be inserted into the database when SaveChanges() is called.
		/// </summary>
		/// <param name="entity">Entity to be tracked</param>
		TEntity Add(TEntity entity);
        /// <summary>
        /// Begins tracking the given entities, and any other reachable entities that are not already being tracked, 
        /// in the Added state such that they will be inserted into the database when SaveChanges() is called.
        /// </summary>
        /// <param name="entities">entities to be tracked</param>
        void AddRange(IEnumerable<TEntity> entities);
        /// <summary>
        /// Begins tracking the given entity and entries reachable from the given entity using the Modified state by default, 
        /// but see below for cases when a different state will be used.
        ///Generally, no database interaction will be performed until SaveChanges() is called.
        ///For entity types with generated keys if an entity has its primary key value set then it will be tracked 
        ///in the Modified state. If the primary key value is not set then it will be tracked in the Added state. 
        ///This helps ensure new entities will be inserted, while existing entities will be updated. 
        ///An entity is considered to have its primary key value set if the primary key property is set to anything other 
        ///than the CLR default for the property type.
        /// </summary>
        /// <param name="entity">Entity to be tracked</param>
        void Update(TEntity entity);
        /// <summary>
        /// Just as Update but for a range of entities 'IEnumerable<TEntity> entities'
        /// </summary>
        /// <param name="entities">entities to be tracked</param>
        void UpdateRange(IEnumerable<TEntity> entities);
        /// <summary>
        /// Begins tracking the given entity, and any other reachable entities that are not already being tracked, 
        /// in the Deleted state such that they will be removed into the database when SaveChanges() is called.
        /// </summary>
        /// <param name="entity">Entity to be tracked</param>
        void Remove(TEntity entity, bool isSoftDelete = true);
        /// <summary>
        /// Just as Remove but for a range of entities 'IEnumerable<TEntity> entities'
        /// </summary>
        /// <param name="entities">entities to be tracked</param>
        void RemoveRange(IEnumerable<TEntity> entities, bool isSoftDelete = true);
    }
}
