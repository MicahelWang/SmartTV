using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using YeahTVApi.DomainModel.Models;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    public class TVChannelMap : EntityTypeConfiguration<TVChannel>
    {
        public TVChannelMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
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

            this.Property(t => t.DefaultCode)
                .HasMaxLength(45);

            // Table & Column Mappings
            this.ToTable("TvChannel", "YeahTV");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.NameEn).HasColumnName("NameEn");
            this.Property(t => t.Icon).HasColumnName("Icon");
            this.Property(t => t.Category).HasColumnName("Category");
            this.Property(t => t.CategoryEn).HasColumnName("EnCategory");
            this.Property(t => t.DefaultCode).HasColumnName("DefaultCode");
            this.Property(t => t.LastUpdateTime).HasColumnName("LastUpdateTime");
        }
    }
}
