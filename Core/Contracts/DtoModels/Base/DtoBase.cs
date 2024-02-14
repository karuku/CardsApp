using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Contracts.DtoModels
{
	public interface IDto
	{
		public long Id { get; set; }
	}
	public abstract class AuditDto
	{
		public DateTime? DateCreated { get; private set; }
		public string CreatedBy { get; private set; }
		public DateTime? DateModified { get; private set; }
		public string ModifiedBy { get; private set; }
	}
	public abstract class DtoBase: AuditDto, IDto
	{
		public long Id { get; set; }
	}
}
