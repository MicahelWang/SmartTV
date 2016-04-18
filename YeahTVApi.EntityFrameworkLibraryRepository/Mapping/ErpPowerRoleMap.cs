using System.Data.Entity.ModelConfiguration;
using YeahTVApi.DomainModel.Models;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    public class ErpPowerRoleMap : EntityTypeConfiguration<ErpPowerRole>
    {
        public ErpPowerRoleMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
               .HasMaxLength(50);

            this.Property(t => t.RoleCode)
                .HasMaxLength(20);

            this.Property(t => t.RoleName)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("ErpPowerRole", "YeahTV");
            this.Property(t => t.Id).HasColumnName("RoleId");
            this.Property(t => t.RoleCode).HasColumnName("RoleCode");
            this.Property(t => t.RoleName).HasColumnName("RoleName");
        }
    }
}
