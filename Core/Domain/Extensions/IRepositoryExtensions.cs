using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Models;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Domain.Extensions
{
	public static class IRepositoryExtensions
	{
		/// <summary>
		/// Use when paging is required. 
		/// NB: disabling tracking is useful for readonly scenarios(enable tracking while editing)
		/// </summary>
		/// <typeparam name="T">Entity Model to be queried.</typeparam>
		/// <param name="repo"></param>
		/// <param name="expression">Predicate to query against the Object.</param>
		/// <param name="page">page number(zero indexed).</param>
		/// <param name="length">page size.</param>
		/// <param name="includes">Includes Entity Model objects related to the main object.</param>
		/// <param name="orderBy">Sets order by clause.</param>
		/// <param name="disableTracking">disable tracking for readonly scenarios(enable tracking while editing)</param>
		/// <returns>EntityQueryRes<T> object</returns>
		public static EntityQueryRes<T> Get<T>(this IRepository<T> repo, int page = 0, int length = 10,
			Expression<Func<T, bool>> expression = null, 
			Expression<Func<T, object>>[] includes = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, bool disableTracking = true)
		   where T : Entity
		{
			var query = repo.Get(expression);

			//count total records before paging
			var count = query.Count();

			//disabling tracking is useful for readonly scenarios(enable tracking while editing)
			if (disableTracking) query = query.AsNoTracking();

			//include related entities
			if (includes != null && includes.Length > 0) query = includes.Aggregate(query, (current, include) => current.Include(include));

			//order by clause 
			query = addOrderBy(orderBy, query);

			//pagination clause
			var (skip, limit) = addPaging(page, length);
			query = query.Skip(skip).Take(limit);

			return EntityQueryRes<T>.Res(query, count);
		}
		public static EntityQueryRes<T> Get<T>(this IRepository<T> repo, int page = 0, int length = 10,
			Expression<Func<T, bool>> expression = null,string[] includes = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, bool disableTracking = true)
		   where T : Entity
		{
			var query = repo.Get(expression);
			
			//count total records before paging
			var count = query.Count();

			//disabling tracking is useful for readonly scenarios(enable tracking while editing)
			if (disableTracking) query = query.AsNoTracking();

			//include related entities
			if (includes != null && includes.Length>0) query = includes.Aggregate(query, (current, include) => current.Include(include));

			//order by clause 
			query = addOrderBy(orderBy, query);

			//pagination clause
			var (skip, limit) = addPaging(page, length);
			query = query.Skip(skip).Take(limit);

			return EntityQueryRes<T>.Res(query, count);
		}

		//helpers

		private static (int skip, int limit) addPaging(int page=0, int pageLimit=10)
		{
			//if (page <= 0)
			//	page = 1;
			if (pageLimit <= 0)
			{
				//pageLimit = int.MaxValue;
				pageLimit = 30;
			}
				

			//var skip = (page - 1) * pageLimit;
			var skip = page;
			if (page > 0) skip = page - 1;
			else skip = 0;

			return (skip, pageLimit);
		}
		private static IOrderedQueryable<T> addOrderBy<T>(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, IQueryable<T> query)
		where T : IEntity
		{
			if (orderBy != null) { return orderBy(query); }
			else { orderBy = o => o.OrderByDescending(c => c.Id); }
			
			return orderBy(query);
		}

	}
}
