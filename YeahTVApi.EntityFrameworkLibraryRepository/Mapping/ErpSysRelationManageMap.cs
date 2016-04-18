using System.Data.Entity.ModelConfiguration;
using YeahTVApi.DomainModel.Models;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    public class ErpSysRelationManageMap : EntityTypeConfiguration<ErpSysRelationManage>
    {
        public ErpSysRelationManageMap()
        {
            // Primary Key
            this.HasKey(t => new { t.UserId, t.Type, t.ObjectId });

            // Properties
            this.Property(t => t.UserId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Type)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.ObjectId)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ErpSysRelationManage", "YeahTV");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.ObjectId).HasColumnName("ObjectId");
        }
    }
}
