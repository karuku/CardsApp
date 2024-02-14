using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
	public interface IEntity
	{
		long Id { get; set; }
	}
	public interface IDeleteEntity: IEntity
	{
		bool? IsDeleted { get; set; }
	}
	public interface IAuditEntity: IDeleteEntity
	{
		string CreatedBy { get; set; }
		DateTime DateCreated { get; set; }
		string ModifiedBy { get; set; }
		DateTime? DateModified { get; set; }

		void SetCreator(string userIdentifier, DateTime now);
		void SetDeleter(string userIdentifier, DateTime now);
		void SetUpdater(string userIdentifier, DateTime now);
	}

	public class Entity : IEntity
	{
		[Key]
		public long Id { get; set; }
	}
	
	public class DeleteEntity : Entity, IDeleteEntity
	{
		public bool? IsDeleted { get; set; }
	}
	
	public class AuditEntity : DeleteEntity, IAuditEntity
	{
		[StringLength(200)]
		public string CreatedBy { get; set; }
		public DateTime DateCreated { get; set; }
		[StringLength(200)]
		public string ModifiedBy { get; set; }
		public DateTime? DateModified { get; set; }

		public void SetCreator(string userIdentifier, DateTime now)
		{
			CreatedBy = userIdentifier;
			DateCreated = now;
		}

		public void SetDeleter(string userIdentifier, DateTime now)
		{
			ModifiedBy = userIdentifier;
			DateModified = now;
		}

		public void SetUpdater(string userIdentifier, DateTime now)
		{
			ModifiedBy = userIdentifier;
			DateModified = now;
		}
	}


}
