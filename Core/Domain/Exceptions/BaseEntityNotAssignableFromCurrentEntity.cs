using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
	public sealed class BaseEntityNotAssignableFromCurrentEntity : BadRequestException
	{
		public BaseEntityNotAssignableFromCurrentEntity(Type currentEntity, Type baseEntity)
			: base($"Entity: {nameof(currentEntity)} passed is not compatible with base entity: {nameof(baseEntity)}")
		{
		}
	}
}
