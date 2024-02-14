using Services.Abstractions;
using Domain.Repositories;
using AutoMapper;
using Contracts.Mapper;
using Contracts.ResModels;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using Contracts.DtoModels;
using Domain.Extensions;
using Domain;
using Domain.Models;
using Domain.Entities;

namespace Services.Services
{
	public class ReqServiceBase: IReqServiceBase
	{
		protected ICurrentUser _currentUser { get; private set; }
		protected IUnitOfWork _uow { get; private set; }
		protected IMapper Mapper => AppObjMapper.Mapper;
		 
		public ReqServiceBase(IUnitOfWork unitOfWork, ICurrentUser currentUser)
		{
			this._currentUser = currentUser;
			this._uow = unitOfWork;
		}

		protected IRepository<TEntity> GetRepo<TEntity>()

			where TEntity : Entity
		{
			var res = _uow.Repository<TEntity>();
			return res;
		}
		protected IResBase<TEntity> EditIfExists<TEntity>(long id)

					where TEntity : Entity
		{
			var repo = _uow.Repository<TEntity>();
			var entity = repo
					.Get(c => c.Id == id).FirstOrDefault();
			if (entity == null)
				return ResBase.NotFoundRes<TEntity>();

			return ResBase.SuccessRes<TEntity>(entity);
		}

		protected List<Expression<Func<TEntity, object>>> GetIncludes<TEntity>(List<Expression<Func<TEntity, object>>> includes)
			where TEntity : Entity
		{
			return includes;
		}

		public ApiListResBase<TDto> GetAll<TEntity, TDto>(Expression<Func<TEntity, bool>> exp = null,
			int pageIndex = 0, int pageLength = 10, List<string> includes = null,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
			bool disableTracking = true, bool showDeletedRecords = false)
			where TEntity : DeleteEntity
			where TDto : DtoBase
		{
			EntityQueryRes<TEntity> res = _uow.Repository<TEntity>().Get(pageIndex, pageLength, exp, includes?.ToArray(), orderBy, disableTracking);
			var datas = showDeletedRecords ? res.QueryData : res.QueryData?.Where(c => c.IsDeleted != true);
			var dtos = AppObjMapper.Mapper.Map<IEnumerable<TDto>>(datas);
			return ApiListResBase<TDto>.SuccessRes(dtos, res.TotalCount);
		}
		
		private ApiResBase<TEntity> GetSingle<TEntity>(Expression<Func<TEntity, bool>> exp = null,
			List<Expression<Func<TEntity, object>>> includes = null, bool disableTracking = true)
			where TEntity : Entity
		{
			var pageIndex = 0;
			var pageLength = 10;
			var res = _uow.Repository<TEntity>().Get(pageIndex, pageLength, exp, includes?.ToArray(), null, disableTracking);
			var data = res.QueryData?.FirstOrDefault();
			return ApiResBase<TEntity>.SuccessRes(data);
		}
		private ApiResBase<TEntity> GetSingle<TEntity>(Expression<Func<TEntity, bool>> exp = null,
			List<string> includes = null, bool disableTracking = true)
			where TEntity : Entity
		{
			var pageIndex = 0;
			var pageLength = 10;
			var res = _uow.Repository<TEntity>().Get(pageIndex,pageLength, exp, includes?.ToArray(), null, disableTracking);
			var data = res.QueryData?.FirstOrDefault();
			return ApiResBase<TEntity>.SuccessRes(data);
		}
		
		public ApiResBase<TEntity> GetById<TEntity>(long id, List<Expression<Func<TEntity,object>>> includes=null,bool disableTracking=true)

			where TEntity : Entity
		{
			var res = GetSingle(c => c.Id == id, includes,disableTracking);
			var data = res.Data;
			return ApiResBase<TEntity>.SuccessRes(data);
		}
		public ApiResBase<TEntity> GetById<TEntity>(long id, List<string> includes = null, bool disableTracking = true)

			where TEntity : Entity
		{
			var res = GetSingle<TEntity>(c => c.Id == id, includes, disableTracking);
			var data = res.Data;
			return ApiResBase<TEntity>.SuccessRes(data);
		}
		
		public ApiResBase<TDto> GetByExp<TEntity, TDto>(
			Expression<Func<TEntity, bool>> exp, 
			List<Expression<Func<TEntity, object>>> includes = null, bool disableTracking = true)
			where TDto : DtoBase
			where TEntity : Entity
		{
			var res = GetSingle<TEntity>(exp, includes,disableTracking);
			var data = res.Data;
			var dtos = AppObjMapper.Mapper.Map<TDto>(data);
			return ApiResBase<TDto>.SuccessRes(dtos);
		}
		
		public IResBase CommitRes(bool isEdit)
		{
			var res = _uow.Commit();
			if (!res) return ResBase.ErrorRes();
			if (isEdit) { return ResBase.SuccessUpdateRes(); }
			return ResBase.SuccessAddRes();
		}
		public IResBase Delete<TEntity>(long id, bool isSoftDelete = true)
			where TEntity : DeleteEntity
		{
			List<string> includes = null;
			var getRes = GetById<TEntity>(id,includes);
			if (getRes.Data == null) return ResBase.NotFoundRes();
			var dbEntity = getRes.Data;
			 
			_uow.Repository<TEntity>().Remove(dbEntity,isSoftDelete);

			return DeleteRes();
		}
		public IResBase DeleteRes()
		{
			var res = _uow.Commit();
			if (!res) return ResBase.ErrorRes();
			return ResBase.SuccessDeleteRes();
		}
	}
}
