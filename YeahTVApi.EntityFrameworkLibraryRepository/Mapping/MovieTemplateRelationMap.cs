using System.Data.Entity.ModelConfiguration;
using YeahTVApi.DomainModel.Models;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    public class MovieTemplateRelationMap : EntityTypeConfiguration<MovieTemplateRelation>
    {
        public MovieTemplateRelationMap()
        {
            // Primary Key
            this.HasKey(t => new { t.MovieTemplateId, t.MovieId });
            this.Ignore(t => t.Id);

            // Properties

            this.HasRequired(t => t.Movie)
                .WithMany(t => t.MovieTemplateRelations)
                .HasForeignKey(t => t.MovieId );

            this.HasRequired(t => t.MovieTemplate)
              .WithMany(t => t.MovieTemplateRelations)
              .HasForeignKey(t => t.MovieTemplateId );

            // Table & Column Mappings
            this.ToTable("MovieTemplateRelation", "YeahTV");
            this.Property(t => t.LastUpdateTime).HasColumnName("LastUpdateTime");
            this.Property(t => t.LastUpdateUser).HasColumnName("LastUpdateUser");
            this.Property(t => t.MovieId).HasColumnName("MovieId");
            this.Property(t => t.MovieTemplateId).HasColumnName("MovieTemplateId");
            this.Property(t => t.IsFree).HasColumnName("IsFree");
            this.Property(t => t.Price).HasColumnName("Price");
        }
    }
}
