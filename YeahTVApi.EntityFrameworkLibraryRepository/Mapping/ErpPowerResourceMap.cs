using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using YeahTVApi.DomainModel.Models;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    public class ErpPowerResourceMap : EntityTypeConfiguration<ErpPowerResource>
    {
        public ErpPowerResourceMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Type)
                .HasMaxLength(10);

            this.Property(t => t.Path)
                .HasMaxLength(200);

            this.Property(t => t.DisplayName)
                .HasMaxLength(20);

            this.Property(t => t.Controller)
                .HasMaxLength(50);

            this.Property(t => t.Action)
                .HasMaxLength(50);

            this.Property(t => t.Logo)
                .HasMaxLength(50);
            

            // Table & Column Mappings
            this.ToTable("ErpPowerResource", "YeahTV");
            this.Property(t => t.Id).HasColumnName("FuncId");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.Path).HasColumnName("Path");
            this.Property(t => t.DisplayName).HasColumnName("DisplayName");
            this.Property(t => t.ParentFuncId).HasColumnName("ParentFuncId");
            this.Property(t => t.Orders).HasColumnName("Orders");
            this.Property(t => t.Display).HasColumnName("Display");
            this.Property(t => t.Controller).HasColumnName("Controller");
            this.Property(t => t.Action).HasColumnName("Action");
            this.Property(t => t.Logo).HasColumnName("Logo");
        }
    }
}
