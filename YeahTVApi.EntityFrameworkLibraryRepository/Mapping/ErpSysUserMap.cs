using System.Data.Entity.ModelConfiguration;
using YeahTVApi.DomainModel.Models;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    public class ErpSysUserMap : EntityTypeConfiguration<ErpSysUser>
    {
        public ErpSysUserMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.GroupId)
                .HasMaxLength(50);

            this.Property(t => t.HotelId)
                .HasMaxLength(50);

            this.Property(t => t.UserName)
                .HasMaxLength(20);

            this.Property(t => t.Phone)
                .HasMaxLength(20);

            this.Property(t => t.Email)
                .HasMaxLength(50);

            this.Property(t => t.RoleId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.CreateUser)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ModifyUser)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ErpSysUser", "YeahTV");
            this.Property(t => t.Id).HasColumnName("UserId");
            this.Property(t => t.GroupId).HasColumnName("GroupId");
            this.Property(t => t.HotelId).HasColumnName("HotelId");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.UserType).HasColumnName("UserType");
            this.Property(t => t.Phone).HasColumnName("Phone");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.RoleId).HasColumnName("RoleId");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.ModifyDate).HasColumnName("ModifyDate");
            this.Property(t => t.CreateUser).HasColumnName("CreateUser");
            this.Property(t => t.ModifyUser).HasColumnName("ModifyUser");
            this.Property(t => t.State).HasColumnName("State");
            this.Property(t => t.IsDelete).HasColumnName("IsDelete");

            this.HasRequired(m => m.CoreSysLogin)
                .WithRequiredPrincipal(m => m.ErpSysUser);
            //this.HasRequired(m => m.CoreSysGroup)
            //    .WithMany(m => m.ErpSysUser)
            //    .HasForeignKey(m => m.GroupId);

            //this.HasRequired(m => m.CoreSysHotel)
            //    .WithMany(m => m.ErpSysUser)
            //    .HasForeignKey(m => m.HotelId);

            this.HasRequired(m => m.ErpPowerRole)
                .WithMany(m => m.ErpSysUser)
                .HasForeignKey(m => m.RoleId);
        }
    }
}
