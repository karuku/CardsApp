using Contracts.DtoModels;
using Contracts.ResModels;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services.Abstractions
{
	public interface IReqServiceBase
	{
		ApiResBase<TDto> GetByExp<TEntity, TDto>(Expression<Func<TEntity, bool>> exp, 
			List<Expression<Func<TEntity, object>>> includes = null, bool disableTracking = true)
			where TDto : DtoBase
			where TEntity : Entity;

		ApiListResBase<TDto> GetAll<TEntity, TDto>(Expression<Func<TEntity, bool>> exp = null,
			int page = 0, int length = 10, List<string> includes = null,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
			bool disableTracking = true, bool showDeletedRecords = false)
			where TDto : DtoBase
			where TEntity : DeleteEntity;
		IResBase Delete<TEntity>(long id,bool isSoftDelete=true)
			where TEntity : DeleteEntity;
	}
}
