using System;

namespace Contracts.ReqModels.Base
{
	public interface IReq
	{
	}
	public abstract class ReqBase: IReq
	{
	}
	public abstract class ReqBaseV2: IReq
	{
	}

	public abstract class GetReqBase : ReqBase
	{
		public long Id { get; set; }
	}
	public abstract class AddReqBase : ReqBase
	{
		
	}
	public abstract class UpdateReqBase : AddReqBase
	{
	}
	public abstract class EditReqBase : AddReqBase
	{
		public bool IsEdit { get; set; }
		public long Id { get; set; }
	}
	public class DateRangeReq 
	{
		public DateRangeReq(DateTime from,DateTime to)
		{
			From = from;
			To = to;
		}
		public DateTime From { get; set; }
		public DateTime To { get; set; }
	}
	public class PaginationReq : ReqBase
    {
        public PaginationReq(int pageIndex, int pageSize) { PageIndex = pageIndex; PageSize = pageSize; }

        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
    }
	 
}
