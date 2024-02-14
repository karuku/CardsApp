using Contracts.DtoModels;
using Contracts.Mapper.Profiles.Base;
using Contracts.ReqModels;
using Contracts.ResModels;
using Domain.Entities;

namespace Contracts.Mapper.Profiles
{
	public class AppProfile : AppServicesProfileBase
	{
		public override void AppReqProfile()
		{
			CreateMap<AddCardReq, EditCardReq> ();
			CreateMap<UpdateCardReq, EditCardReq>();

			CreateReqToEntityUniMapper<EditCardReq, Card>();
		}

		public override void AppResProfile()
		{
			CreateEntityToDtoUniMapper<Card, CardDto>();
			CreateMap<ApplicationUser, AuthUserRes>()
				.ForMember(c => c.UserId, a => a.MapFrom(d => d.Id))
				.ForMember(c => c.UserName, a => a.MapFrom(d => d.UserName));

		}
	}

}
