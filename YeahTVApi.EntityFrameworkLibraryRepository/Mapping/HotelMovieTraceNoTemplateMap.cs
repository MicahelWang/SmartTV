using System.Data.Entity.ModelConfiguration;
using YeahTVApi.DomainModel.Models;

namespace YeahTVApi.DomainModel.Models
{
    public class HotelMovieTraceNoTemplateMap : EntityTypeConfiguration<HotelMovieTraceNoTemplate>
    {
        public HotelMovieTraceNoTemplateMap()
        {
            // Primary Key
            this.HasKey(t => new { t.HotelId, t.MovieId });
            this.Ignore(t => t.Id);

            // Properties
            this.Property(t => t.HotelId)
                .IsRequired()
                .HasMaxLength(64);

            this.Property(t => t.MovieId)
                .IsRequired()
                .HasMaxLength(64);

            this.Property(t => t.DownloadStatus)
                .HasMaxLength(20);

            this.Property(t => t.LastUpdateUser)
                .IsRequired()
                .HasMaxLength(50);

            this.HasRequired(t => t.MovieForLocalize)
                .WithMany(t => t.HotelMovieTraceNoTemplates)
                .HasForeignKey(t=>t.MovieId);

            // Table & Column Mappings
            this.ToTable("HotelMovieTraceNoTemplate", "YeahTV");
            this.Property(t => t.HotelId).HasColumnName("HotelId");
            this.Property(t => t.MovieId).HasColumnName("MovieId");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.ViewCount).HasColumnName("ViewCount");
            this.Property(t => t.DownloadStatus).HasColumnName("DownloadStatus");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.Order).HasColumnName("Order");
            this.Property(t => t.IsTop).HasColumnName("IsTop");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.LastViewTime).HasColumnName("LastViewTime");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.LastUpdateUser).HasColumnName("LastUpdateUser");
            this.Property(t => t.IsDelete).HasColumnName("IsDelete");
        }
    }
}
