using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Contracts.Mapper.Profiles;
using System;

namespace Contracts.Mapper
{
    // The best implementation of AutoMapper for class libraries -> https://www.abhith.net/blog/using-automapper-in-a-net-core-class-library/
    public static class AppObjMapper
    {
		private static readonly Lazy<IMapper> LazyConfig = new Lazy<IMapper>(() =>
		{
			var config = new MapperConfiguration(cfg =>
			{
				// This line ensures that internal properties are also mapped over.
				cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
				cfg.AddExpressionMapping();
				cfg.AddProfile<AppProfile>();
			});
			var mapper = config.CreateMapper();
			return mapper;
		});
		public static IMapper Mapper => LazyConfig.Value;
	}
	 
}
