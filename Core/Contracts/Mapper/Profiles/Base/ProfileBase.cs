using AutoMapper;

namespace Contracts.Mapper.Profiles.Base
{
	public abstract class ProfileBase : Profile
	{
		/// <summary>
		/// One direction mapping.
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <typeparam name="TDestination"></typeparam>
		/// <returns></returns>
		protected virtual IMappingExpression<TSource, TDestination> CreateUniMapper<TSource, TDestination>()
			 where TSource : class
	   where TDestination : class
		{
			var mappingExp = CreateMap<TSource, TDestination>();

			return mappingExp;
		}
		protected virtual IMappingExpression<TDestination, TSource> CreateUniReverseMapper<TSource, TDestination>()
			 where TSource : class
	   where TDestination : class
		{
			var mappingExp = CreateMap<TSource, TDestination>().ReverseMap();

			return mappingExp;
		}

		/// <summary>
		/// Muti direction mapping.
		/// </summary>
		/// <typeparam name="TObj1"></typeparam>
		/// <typeparam name="TObj2"></typeparam>
		/// <returns></returns>
		protected virtual IMappingExpression<TObj1, TObj2> CreateOmniMapper<TObj1, TObj2>()
			where TObj1 : class
	  where TObj2 : class
		{
			var mappingExpDestObj2 = CreateMap<TObj1, TObj2>();

			var mappingExpDestObj1 = CreateMap<TObj2, TObj1>();

			return mappingExpDestObj2;
		}
		
		protected virtual IMappingExpression<TObj1, TObj2> CreateOmniReverseMapper<TObj1, TObj2>()
			where TObj1 : class
	  where TObj2 : class
		{
			var mappingExpDestObj2 = CreateMap<TObj1, TObj2>().ReverseMap();

			var mappingExpDestObj1 = CreateMap<TObj2, TObj1>().ReverseMap();

			return mappingExpDestObj1;
		}
		
	}


}
