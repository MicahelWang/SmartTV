using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using YeahTVApi.DomainModel.Models;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    public class MovieMap : EntityTypeConfiguration<Movie>
    {
        public MovieMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(64);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.NameEn)
                .HasMaxLength(100);

            this.Property(t => t.MovieReview)
                .IsRequired()
                .HasMaxLength(1073741823);

            this.Property(t => t.MovieReviewEn)
                .HasMaxLength(1073741823);

            this.Property(t => t.PosterAddress)
                .HasMaxLength(300);

            this.Property(t => t.CoverAddress)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Movie", "YeahTV");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.NameEn).HasColumnName("NameEn");
            this.Property(t => t.MovieReview).HasColumnName("MovieReview");
            this.Property(t => t.MovieReviewEn).HasColumnName("MovieReviewEn");
            this.Property(t => t.PosterAddress).HasColumnName("PosterAddress");
            this.Property(t => t.CoverAddress).HasColumnName("CoverAddress");
            this.Property(t => t.LastUpdateTime).HasColumnName("LastUpdateTime");
        }
    }
}
