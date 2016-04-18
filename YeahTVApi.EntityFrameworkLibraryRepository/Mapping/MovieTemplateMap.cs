using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using YeahTVApi.DomainModel.Models;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    public class MovieTemplateMap : EntityTypeConfiguration<MovieTemplate>
    {
        public MovieTemplateMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
               .IsRequired()
               .HasMaxLength(64);

            // Table & Column Mappings
            this.ToTable("MovieTemplate", "YeahTV");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.LastUpdateTime).HasColumnName("LastUpdateTime");
            this.Property(t => t.LastUpdateUser).HasColumnName("LastUpdateUser");
            this.Property(t => t.Tags).HasColumnName("Tags");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.HotelCount).HasColumnName("HotelCount");
            this.Property(t => t.MovieCount).HasColumnName("MovieCount");
        }
    }
}
