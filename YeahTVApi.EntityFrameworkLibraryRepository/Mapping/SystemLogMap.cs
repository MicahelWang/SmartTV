using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using YeahTVApi.DomainModel.Models;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    public class SystemLogMap : EntityTypeConfiguration<SystemLog>
    {
        public SystemLogMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.MessageInfo)
                .IsRequired()
                .HasMaxLength(65535);

            this.Property(t => t.MessageInfoEx)
                .HasMaxLength(65535);

            this.Property(t => t.MessageType)
                .HasMaxLength(10);

            this.Property(t => t.AppType)
                .HasMaxLength(64);

            // Table & Column Mappings
            this.ToTable("SystemLog", "YeahTV");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.MessageInfo).HasColumnName("MessageInfo");
            this.Property(t => t.MessageInfoEx).HasColumnName("MessageInfoEx");
            this.Property(t => t.MessageType).HasColumnName("MessageType");
            this.Property(t => t.AppType).HasColumnName("AppType");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
    }
}
