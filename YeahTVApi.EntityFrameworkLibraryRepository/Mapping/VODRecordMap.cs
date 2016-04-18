using System.Data.Entity.ModelConfiguration;
using YeahTVApi.DomainModel.Models;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    public class VODRecordMap : EntityTypeConfiguration<VODRecord>
    {
        public VODRecordMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Id, t.ChargeType });

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(64);

            this.Property(t => t.ViewDate)
                .IsRequired()
                .HasMaxLength(45);

            this.Property(t => t.ChargeType)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.ChargeNo)
                .IsRequired()
                .HasMaxLength(64);

            // Table & Column Mappings
            this.ToTable("VODRecords", "YeahTV");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.MovieId).HasColumnName("MovieId");
            this.Property(t => t.ViewDate).HasColumnName("ViewDate");
            this.Property(t => t.ChargeType).HasColumnName("ChargeType");
            this.Property(t => t.ChargeNo).HasColumnName("ChargeSerialNumber");
        }
    }
}
