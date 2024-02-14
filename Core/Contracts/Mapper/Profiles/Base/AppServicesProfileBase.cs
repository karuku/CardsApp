using AutoMapper;
using Contracts.DtoModels;
using Contracts.ReqModels.Base;
using Domain.Entities;

namespace Contracts.Mapper.Profiles.Base
{
    public abstract class AppServicesProfileBase : ProfileBase
	{
		public AppServicesProfileBase()
		{
			AppReqProfile();
			AppResProfile();
		}

		public abstract void AppResProfile();
		public abstract void AppReqProfile();

		protected virtual IMappingExpression<TSource, TDestination> CreateEntityToDtoUniMapper<TSource, TDestination>()
			 where TSource : Entity
	   where TDestination : DtoBase
		{
			var mappingExp = CreateUniMapper<TSource, TDestination>();

			return mappingExp;
		}
		protected virtual IMappingExpression<TSource, TDestination> CreateReqToEntityUniMapper<TSource, TDestination>()
					 where TSource : ReqBase
			   where TDestination : Entity
		{
			var mappingExp = CreateUniMapper<TSource, TDestination>();

			return mappingExp;
		}

	}
}
