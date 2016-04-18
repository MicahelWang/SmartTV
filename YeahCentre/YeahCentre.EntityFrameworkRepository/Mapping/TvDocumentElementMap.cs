using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using YeahTVApi.DomainModel.Models;

namespace YeahCentre.EntityFrameworkRepository.Mapping
{
    public class TvDocumentElementMap : EntityTypeConfiguration<TvDocumentElement>
    {
        public TvDocumentElementMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Name)
                .HasMaxLength(50);

            this.Property(t => t.ParentId)
                .HasMaxLength(50);

            this.Property(t => t.TemplateId)
                .IsRequired()
                .HasMaxLength(50);
            this.Property(t => t.TemplateElementId)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TvDocumentElement", "YeahTV");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.ParentId).HasColumnName("ParentId");
            this.Property(t => t.TemplateId).HasColumnName("TemplateId");
            this.Property(t => t.Orders).HasColumnName("Orders");
            this.Property(t => t.TemplateElementId).HasColumnName("TemplateElementId");
        }
    }
}
