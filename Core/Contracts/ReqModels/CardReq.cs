using Contracts.ReqModels.Base;
using Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Contracts.ReqModels
{
    public class CardReq : ReqBase
	{
		
	}
	public class CardsReq : PaginationReq
	{
		public CardsReq(int pageIndex, int pageSize) : base(pageIndex, pageSize)
		{
		}
		public string SearchTerm { get; set; }
		public CardStatusTypes? Status { get; set; }
		//public DateTime? CreatedDate { get; set; }
		public string? CreatedDate { get; set; }
		public string? OrderBy { get; set; }
	}
	public class AddCardReq : AddReqBase
	{
		[Required]
		[StringLength(200)]
		public string Name { get; set; }
		[StringLength(500)]
		public string Description { get; set; }
		[StringLength(6)]
		public string Color { get; set; }
	}
	public class UpdateCardReq : UpdateReqBase
	{
		[Required]
		[StringLength(200)]
		public string Name { get; set; }
		[StringLength(500)]
		public string Description { get; set; }
		[StringLength(6)]
		public string Color { get; set; }
		public CardStatusTypes CardStatus { get; set; }
	}
	public class EditCardReq : EditReqBase
	{
		[Required]
		[StringLength(200)]
		public string Name { get; set; }
		[StringLength(500)]
		public string Description { get; set; }
		[StringLength(6)]
		public string Color { get; set; }
		public CardStatusTypes CardStatus { get; set; }
	}

}
