using Contracts.ResModels;
using Domain;
using Contracts.Mapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using Contracts.ReqModels.Base;
using Services.Abstractions;
using Domain.Repositories;
using Contracts.DtoModels;
using Contracts.ReqModels;
using Domain.Entities;
using Contracts.Extensions;
using Domain.Enums;

namespace Services.Services
{
	public class CardService:ReqServiceBase, ICardService
	{
		public CardService(IUnitOfWork unitOfWork, ICurrentUser currentUser) :
			base(unitOfWork, currentUser)
		{ }

		public ApiResBase<CardDto> GetCardById(long id)
		{
			Expression<Func<Card, bool>> exp =
				c => c.Id == id && c.IsDeleted != true;

			var includes = new List<Expression<Func<Card, object>>>();
			includes = null;
			var res = GetByExp<Card, CardDto>(exp, includes);
			if(res.Data.CreatedBy != _currentUser.UserName && _currentUser.Role != SysRoleTypes.Admin.ToString())
				return ApiResBase<CardDto>.ErrorRes("Cannot access this card.");
			
			return res;
		}
		public ApiListResBase<CardDto> GetCards(CardsReq req)
		{
			var exp = GetCardsExpression(req);

			var includes = new List<string>();

			Func<IQueryable<Card>, IOrderedQueryable<Card>> orderBy = null;
			if (!req.OrderBy.IsNullOrWhiteSpace())
			{
				switch (req.OrderBy.ToLower())
				{
					case "name":
						orderBy = o => o.OrderBy(c => c.Name);
						break;
					case "color":
						orderBy = o => o.OrderBy(c => c.Color);
						break;
					case "status":
						orderBy = o => o.OrderBy(c => c.Name);
						break;
					case "datecreated":
						orderBy = o => o.OrderBy(c => c.Color);
						break;
					case "all":
						orderBy =
				   o => o
				   .OrderBy(c => c.Name)
				   .ThenBy(c => c.Color)
				   .ThenBy(c => c.CardStatus)
				   .ThenBy(c => c.DateCreated);
						break;
				}
			}

			var res = GetAll<Card, CardDto>(exp, req.PageIndex, req.PageSize, includes,orderBy);
			return res;
		}

		public IResBase EditCard(EditCardReq req)
		{
			var repo = GetRepo<Card>();

			if (req.IsEdit)
			{
				var entity = repo
					.Get(c => c.Id == req.Id).FirstOrDefault();
				if (entity == null)
					return ResBase.NotFoundRes("Not Found.");

				if (entity.CreatedBy != _currentUser.UserName && _currentUser.Role != SysRoleTypes.Admin.ToString())
					return ResBase.ErrorRes("Cannot access this card.");

				entity.Name = req.Name;
				entity.Description = req.Description;
				entity.CardStatus = req.CardStatus;
				entity.Color = req.Color;
				 
				repo.Update(entity);
			}
			else
			{
				var dbEntity = repo
					.Get(c => c.Id == req.Id || c.Name == req.Name).FirstOrDefault();
				if (dbEntity != null)
					return ResBase.ExistsRes();

				var entity = AppObjMapper.Mapper.Map<Card>(req);
				repo.Add(entity);
			}

			var res = _uow.Commit();
			if (res)
				return req.IsEdit ? ResBase.SuccessUpdateRes() : ResBase.SuccessAddRes();

			return ResBase.ErrorRes();
		}
		public IResBase DeleteCard(long id)
		{
			var repo = GetRepo<Card>();

			var entity = repo
					.Get(c => c.Id == id).FirstOrDefault();
			if (entity == null)
				return ResBase.NotFoundRes("Not Found");

			if (entity.CreatedBy != _currentUser.UserName && _currentUser.Role != SysRoleTypes.Admin.ToString())
				return ResBase.ErrorRes("Cannot access this card.");

			entity.IsDeleted = true;
			
			repo.Update(entity);

			var res = _uow.Commit();
			if (res)
				return ResBase.SuccessDeleteRes();

			return ResBase.ErrorRes();
		}

		//helpers
		
		private Expression<Func<Card, bool>> GetCardsExpression(CardsReq req)
		{
			DateTime? createdDate = null;
			DateTime? upperDateLimit = createdDate;
			if (!req.CreatedDate.IsNullOrWhiteSpace())
			{
				createdDate = req.CreatedDate.ToDateTime();
				upperDateLimit = createdDate.Value.AddDays(1).AddMinutes(-1);
			}

			Expression<Func<Card, bool>> exp =
				_currentUser.Role != SysRoleTypes.Admin.ToString()?
				!req.SearchTerm.IsNullOrWhiteSpace() && req.CreatedDate == null && req.Status == null ?
				c => c.CreatedBy == _currentUser.UserName && c.IsDeleted != true && (c.Name.ToLower().Contains(req.SearchTerm.ToLower()) || c.Description.ToLower().Contains(req.SearchTerm.ToLower()) || c.Color.ToLower().Contains(req.SearchTerm.ToLower())) :
				!req.SearchTerm.IsNullOrWhiteSpace() && req.CreatedDate != null && req.Status == null ?
				c => c.CreatedBy == _currentUser.UserName && c.IsDeleted != true && (c.DateCreated > createdDate && c.DateCreated < upperDateLimit) && (c.Name.ToLower().Contains(req.SearchTerm.ToLower()) || c.Description.ToLower().Contains(req.SearchTerm.ToLower()) || c.Color.ToLower().Contains(req.SearchTerm.ToLower())) :
				req.SearchTerm.IsNullOrWhiteSpace() && req.CreatedDate != null && req.Status == null ?
				c => c.CreatedBy == _currentUser.UserName && c.IsDeleted != true && (c.DateCreated > createdDate && c.DateCreated < upperDateLimit) :

				 !req.SearchTerm.IsNullOrWhiteSpace() && req.CreatedDate == null && req.Status != null ?
				c => c.CreatedBy == _currentUser.UserName && c.IsDeleted != true && c.CardStatus == req.Status && (c.Name.ToLower().Contains(req.SearchTerm.ToLower()) || c.Description.ToLower().Contains(req.SearchTerm.ToLower()) || c.Color.ToLower().Contains(req.SearchTerm.ToLower())) :
				!req.SearchTerm.IsNullOrWhiteSpace() && req.CreatedDate != null && req.Status != null ?
				c => c.CreatedBy == _currentUser.UserName && c.IsDeleted != true && c.CardStatus == req.Status && (c.DateCreated > createdDate && c.DateCreated < upperDateLimit) && (c.Name.ToLower().Contains(req.SearchTerm.ToLower()) || c.Description.ToLower().Contains(req.SearchTerm.ToLower()) || c.Color.ToLower().Contains(req.SearchTerm.ToLower())) :
				req.SearchTerm.IsNullOrWhiteSpace() && req.CreatedDate != null && req.Status != null ?
				c => c.CreatedBy == _currentUser.UserName && c.IsDeleted != true && c.CardStatus == req.Status && (c.DateCreated > createdDate && c.DateCreated < upperDateLimit) :
				c => c.CreatedBy == _currentUser.UserName && c.IsDeleted != true :

				!req.SearchTerm.IsNullOrWhiteSpace() && req.CreatedDate == null && req.Status == null ?
				c => c.IsDeleted != true && (c.Name.ToLower().Contains(req.SearchTerm.ToLower()) || c.Description.ToLower().Contains(req.SearchTerm.ToLower()) || c.Color.ToLower().Contains(req.SearchTerm.ToLower())) :
				!req.SearchTerm.IsNullOrWhiteSpace() && req.CreatedDate != null && req.Status == null ?
				c => c.IsDeleted != true && (c.DateCreated > createdDate && c.DateCreated < upperDateLimit) && (c.Name.ToLower().Contains(req.SearchTerm.ToLower()) || c.Description.ToLower().Contains(req.SearchTerm.ToLower()) || c.Color.ToLower().Contains(req.SearchTerm.ToLower())) :
				req.SearchTerm.IsNullOrWhiteSpace() && req.CreatedDate != null && req.Status == null ?
				c => c.IsDeleted != true && (c.DateCreated > createdDate && c.DateCreated < upperDateLimit) :

				!req.SearchTerm.IsNullOrWhiteSpace() && req.CreatedDate == null && req.Status != null ?
				c => c.IsDeleted != true && c.CardStatus == req.Status && (c.Name.ToLower().Contains(req.SearchTerm.ToLower()) || c.Description.ToLower().Contains(req.SearchTerm.ToLower()) || c.Color.ToLower().Contains(req.SearchTerm.ToLower())) :
				!req.SearchTerm.IsNullOrWhiteSpace() && req.CreatedDate != null && req.Status != null ?
				c => c.IsDeleted != true && c.CardStatus == req.Status && (c.DateCreated > createdDate && c.DateCreated < upperDateLimit) && (c.Name.ToLower().Contains(req.SearchTerm.ToLower()) || c.Description.ToLower().Contains(req.SearchTerm.ToLower()) || c.Color.ToLower().Contains(req.SearchTerm.ToLower())) :
				req.SearchTerm.IsNullOrWhiteSpace() && req.CreatedDate != null && req.Status != null ?
				c => c.IsDeleted != true && c.CardStatus == req.Status && (c.DateCreated > createdDate && c.DateCreated < upperDateLimit) :
				c => c.IsDeleted != true;

			return exp;
		}

	}
}
