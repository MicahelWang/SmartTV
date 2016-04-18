using System.Data.Entity.ModelConfiguration;
using YeahTVApi.DomainModel.Models;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    public class VODRequestMap : EntityTypeConfiguration<VODRequest>
    {
        public VODRequestMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Id });

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(64);


            // Table & Column Mappings
            this.ToTable("VODRequest", "YeahTV");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.MovieId).HasColumnName("MovieId");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.SeriseCode).HasColumnName("SeriseCode");
            this.Property(t => t.ResultType).HasColumnName("ResultType");
            this.Property(t => t.ResultMessage).HasColumnName("ResultMessage");
            this.Property(t => t.PayInfo).HasColumnName("PayInfo");
        }
    }
}
