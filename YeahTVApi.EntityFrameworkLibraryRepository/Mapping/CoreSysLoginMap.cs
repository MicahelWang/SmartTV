using System.Data.Entity.ModelConfiguration;
using YeahTVApi.DomainModel.Models;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    public class CoreSysLoginMap : EntityTypeConfiguration<CoreSysLogin>
    {
        public CoreSysLoginMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.LoginName)
                .HasMaxLength(20);

            this.Property(t => t.UserName)
                .HasMaxLength(20);

            this.Property(t => t.Password)
                .HasMaxLength(50);

            this.Property(t => t.AddUserId)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("CoreSysLogin", "YeahTV");
            this.Property(t => t.Id).HasColumnName("UserId");
            this.Property(t => t.LoginName).HasColumnName("LoginName");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.Password).HasColumnName("Password");
            this.Property(t => t.RegDate).HasColumnName("RegDate");
            this.Property(t => t.LastLoginDate).HasColumnName("LastLoginDate");
            this.Property(t => t.AddUserId).HasColumnName("AddUserId");
            this.Property(t => t.State).HasColumnName("State");
            this.Property(t => t.IsDelete).HasColumnName("IsDelete");
        }
    }
}
