using System.Data.Entity.ModelConfiguration;
using YeahTVApi.DomainModel.Models;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    public class HotelMovieTraceMap : EntityTypeConfiguration<HotelMovieTrace>
    {
        public HotelMovieTraceMap()
        {
            // Primary Key
            this.HasKey(t => new { t.HotelId, t.MovieId });
            this.Ignore(t => t.Id);

            // Properties

            this.HasRequired(t => t.Movie)
                .WithMany(t => t.HotelMovieTraces)
                .HasForeignKey(t => t.MovieId);

            this.HasRequired(t => t.MovieTemplate)
               .WithMany(t => t.HotelMovieTraces)
               .HasForeignKey(t => t.MoiveTemplateId);

            // Table & Column Mappings
            this.ToTable("HotelMovieTrace", "YeahTV");
            this.Property(t => t.HotelId).HasColumnName("HotelId");
            this.Property(t => t.IsDownload).HasColumnName("IsDownload");
            this.Property(t => t.LastViewTime).HasColumnName("LastViewTime");
            this.Property(t => t.MoiveTemplateId).HasColumnName("MoiveTemplateId");
            this.Property(t => t.MovieId).HasColumnName("MovieId");
            this.Property(t => t.VideoServerAddress).HasColumnName("VideoServerAddress");
            this.Property(t => t.ViewCount).HasColumnName("ViewCount");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.Order).HasColumnName("Order");
        }
    }
}
