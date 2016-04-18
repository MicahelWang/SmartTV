using System.Data.Entity.ModelConfiguration;
using YeahTVApi.DomainModel.Models;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    public class CoreSysOtherLoginMap : EntityTypeConfiguration<CoreSysOtherLogin>
    {
        public CoreSysOtherLoginMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.UserId)
                .HasMaxLength(50);

            this.Property(t => t.OtherId)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("CoreSysOtherLogin", "YeahTV");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.OtherId).HasColumnName("OtherId");
            this.Property(t => t.Type).HasColumnName("Type");
        }
    }
}
