using Contracts.DtoModels;
using Contracts.ReqModels;
using Contracts.ReqModels.Base;
using Contracts.ResModels;
using System.Collections.Generic;

namespace Services.Abstractions
{
	public interface ICardService : IReqServiceBase
	{
		//IResBase CreateCard(AddCardReq req);
		//IResBase EditCard(UpdateCardReq req);
		IResBase EditCard(EditCardReq req);
		IResBase DeleteCard(long id);
		ApiResBase<CardDto> GetCardById(long id);
		ApiListResBase<CardDto> GetCards(CardsReq req);

	}
}
