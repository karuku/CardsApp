using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence
{
    
    internal class EntityConfiguration
    {
	}

	public class CardEntityConfiguration : AuditEntityConfigurationBase<Card>
	{
		public override void Configure(EntityTypeBuilder<Card> builder)
		{
			base.Configure(builder);
			builder.HasIndex(e => e.Name);
			builder.HasIndex(e => e.Color);
			builder.HasIndex(e => e.CardStatus);
			builder.HasIndex(e => e.DateCreated);
			builder.HasIndex(e => e.CreatedBy);
			builder.Property(e => e.Name).IsUnicode(false).IsRequired();
			builder.Property(e => e.Description).IsUnicode(false);
			builder.Property(e => e.Color).IsUnicode(false);
			
		}
	}
}
