using System.Data.Entity.ModelConfiguration;
using YeahTVApi.DomainModel.Models;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    public class ErpPowerRoleResourceRelationMap : EntityTypeConfiguration<ErpPowerRoleResourceRelation>
    {
        public ErpPowerRoleResourceRelationMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.RoleId)
                .HasMaxLength(50);

            this.Property(t => t.ResourceId);

            // Table & Column Mappings
            this.ToTable("ErpPowerRoleResourceRelation", "YeahTV");
            this.Property(t => t.Id).HasColumnName("RelationId");
            this.Property(t => t.RoleId).HasColumnName("RoleId");
            this.Property(t => t.ResourceId).HasColumnName("ResourceId");
        }
    }
}
