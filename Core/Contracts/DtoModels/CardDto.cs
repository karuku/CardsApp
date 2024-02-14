using Domain.Enums;

namespace Contracts.DtoModels
{
	public class CardDto : DtoBase
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string Color { get; set; }
		public CardStatusTypes CardStatus { get; set; }
	}
	
}
