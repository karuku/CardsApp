using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
	public class Card : AuditEntity
	{
		[StringLength(200)]
		public string Name { get; set; }
		[StringLength(500)]
		public string Description { get; set; }
		[StringLength(6)]
		public string Color { get; set; }
		public CardStatusTypes CardStatus { get; set; }

	}
}
