using Contracts.Extensions;
using Domain;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Persistence.Repositories
{
	public class Repository<T> : IRepository<T>
		where T : Entity
	{
		private readonly ICurrentUser _currentUser;
		protected ApplicationDbContext _context { get; }
		protected DbSet<T> DbSet { get; }
		public Repository(ApplicationDbContext context, ICurrentUser currentUser)
		{
			_context = context;
			_currentUser = currentUser;
			DbSet = DbSet = _context.Set<T>();
		}

		public T Add(T entity)
		{
			// if the entity is of type AuditLog, assign creation info to it  
			if (typeof(IAuditEntity).IsAssignableFrom(typeof(T)))
			{
				var audit = (IAuditEntity)entity;
				audit.SetCreator(_currentUser.UserName, DateTime.Now.ToAppDateTime());
			}
			var res = DbSet.Add(entity); 
			return res.Entity;
		}
		 
		public void AddRange(IEnumerable<T> entities)
		{
			// if the entity is of type AuditLog, assign creation info to it  
			if (typeof(IAuditEntity).IsAssignableFrom(typeof(T)))
			{
				entities.ToList().ForEach(entity =>
				{
					var audit = (IAuditEntity)entity;
					audit.SetCreator(_currentUser.UserName, DateTime.Now.ToAppDateTime());
				});
			}
			DbSet.AddRange(entities);
		}

		public void Remove(T entity, bool isSoftDelete = true)
		{ 
			if (isSoftDelete)
			{
				if (typeof(IAuditEntity).IsAssignableFrom(typeof(T)))
				{
					//var now = DateTime.Now.ToAppDateTime();
					var now = DateTime.Now.ToAppDateTime();
					var audit = (IAuditEntity)entity;
					audit.SetDeleter(_currentUser.UserName, now);
				}
				else
				{
					var classType = entity.GetType().GetTypeInfo().UnderlyingSystemType;
					throw new BaseEntityNotAssignableFromCurrentEntity(classType, typeof(IAuditEntity));
				}
			
				DbSet.Update(entity);
			}
			else
				DbSet.Remove(entity);  
		}
		public void RemoveRange(IEnumerable<T> entities, bool isSoftDelete = true)
		{
			if (isSoftDelete)
			{
				if (typeof(IAuditEntity).IsAssignableFrom(typeof(T)))
				{
					entities.ToList().ForEach(entity =>
					{
						//var now = DateTime.Now.ToAppDateTime();
						var now = DateTime.Now.ToAppDateTime();
						var audit = (IAuditEntity)entity;
						audit.SetDeleter(_currentUser.UserName, now);
					});
				}
				else
				{
					var classType = entities.GetType().GetTypeInfo().UnderlyingSystemType;
					throw new BaseEntityNotAssignableFromCurrentEntity(classType, typeof(IAuditEntity));
				}

				DbSet.UpdateRange(entities);
			}
			else
				DbSet.RemoveRange(entities);
		}

		public IQueryable<T> Get(Expression<Func<T, bool>> expression = null)
		{
			IQueryable<T> res = DbSet.AsQueryable();
			
			if (expression != null)
				res = res.Where(expression);

			return res;
		}

		public void Update(T entity)
		{
			// if the entity is of type AuditLog, assign modification info to it  
			if (typeof(IAuditEntity).IsAssignableFrom(typeof(T)))
			{
				//var now = DateTime.Now.ToAppDateTime();
				var now = DateTime.Now.ToAppDateTime();
				var audit = (IAuditEntity)entity;
				audit.SetUpdater(_currentUser.UserName, now);
			}
			DbSet.Update(entity);
		}

		public void UpdateRange(IEnumerable<T> entities)
		{
			// if the entity is of type AuditLog, assign creation info to it  
			if (typeof(IAuditEntity).IsAssignableFrom(typeof(T)))
			{
				entities.ToList().ForEach(entity =>
				{
					var now = DateTime.Now.ToAppDateTime();
					var audit = (IAuditEntity)entity;
					audit.SetUpdater(_currentUser.UserName, now);
				});
			}
			
			DbSet.UpdateRange(entities);
		}

	}
}
