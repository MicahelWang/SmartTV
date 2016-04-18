using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using YeahTVApi.DomainModel.Models;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    public class HotelTVChannelMap : EntityTypeConfiguration<HotelTVChannel>
    {
        public HotelTVChannelMap()
        {
            // Primary Key
            this.HasKey(t => new { t.HotelId, t.ChannelId });
            this.Ignore(t => t.Id);

            // Properties
            this.Property(t => t.HotelId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ChannelId)
                .HasMaxLength(64);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(45);

            this.Property(t => t.NameEn)
                .HasMaxLength(45);

            this.Property(t => t.Icon)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Category)
                .IsRequired()
                .HasMaxLength(45);

            this.Property(t => t.CategoryEn)
                .HasMaxLength(45);

            this.Property(t => t.HostAddress)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("HotelTvChannel", "YeahTV");
            this.Property(t => t.HotelId).HasColumnName("HotelId");
            this.Property(t => t.ChannelId).HasColumnName("ChannelId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.NameEn).HasColumnName("NameEn");
            this.Property(t => t.Icon).HasColumnName("Icon");
            this.Property(t => t.Category).HasColumnName("Category");
            this.Property(t => t.CategoryEn).HasColumnName("EnCategory");
            this.Property(t => t.ChannelCode).HasColumnName("ChannelCode");
            this.Property(t => t.HostAddress).HasColumnName("HostAddress");
            this.Property(t => t.ChannelOrder).HasColumnName("ChannelOrder");
            this.Property(t => t.LastUpdateTime).HasColumnName("LastUpdateTime");
            this.Property(t => t.LastUpdateUser).HasColumnName("LastUpdateUser");
        }
    }
}
