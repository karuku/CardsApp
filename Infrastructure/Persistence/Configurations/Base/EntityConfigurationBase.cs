using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Persistence
{
    public class EntityConfigurationBase<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : Entity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            #region EntityConfigurationBase
            //builder.ToTable(nameof(TEntity));

            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.Id).ValueGeneratedOnAdd();

            //builder.Ignore(c => c.IdValue);

            #endregion

        }
    }

    public class AuditEntityConfigurationBase<TEntity> : EntityConfigurationBase<TEntity>, IEntityTypeConfiguration<TEntity>
        where TEntity : AuditEntity
    {
        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            base.Configure(builder);

            #region AuditEntityConfigurationBase

            builder.Property(entity => entity.IsDeleted);
                //.HasDefaultValue(false).HasDefaultValueSql("0");

            builder.Property(entity => entity.CreatedBy).HasMaxLength(200).IsUnicode(false).IsRequired();

            builder.Property(entity => entity.DateCreated)
                //.HasColumnType("datetime").HasDefaultValueSql("getutcdate()")
                .IsRequired();

            builder.Property(entity => entity.ModifiedBy).HasMaxLength(200).IsUnicode(false);

            builder.Property(entity => entity.DateModified);
                //.HasColumnType("datetime");
            #endregion

        }
    }

}
