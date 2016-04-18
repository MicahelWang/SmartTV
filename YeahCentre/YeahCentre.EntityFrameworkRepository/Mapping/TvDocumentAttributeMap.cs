using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using YeahTVApi.DomainModel.Models;

namespace YeahCentre.EntityFrameworkRepository.Mapping
{
    public class TvDocumentAttributeMap : EntityTypeConfiguration<TvDocumentAttribute>
    {
        public TvDocumentAttributeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ElementId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Text)
                .HasMaxLength(50);

            this.Property(t => t.TemplateAttributeId)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TvDocumentAttribute", "YeahTV");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Value).HasColumnName("Value");
            this.Property(t => t.ElementId).HasColumnName("ElementId");
            this.Property(t => t.Text).HasColumnName("Text");
            this.Property(t => t.TemplateAttributeId).HasColumnName("TemplateAttributeId");
            this.Property(t => t.ParentId).HasColumnName("ParentId");

            this.HasRequired(m => m.Element)
                .WithMany(m => m.Attributes)
                .HasForeignKey(m => m.ElementId);
            this.HasRequired(m => m.TemplateAttribute)
               .WithMany(m => m.DocumentAttributes)
               .HasForeignKey(m => m.TemplateAttributeId);
        }
    }
}
