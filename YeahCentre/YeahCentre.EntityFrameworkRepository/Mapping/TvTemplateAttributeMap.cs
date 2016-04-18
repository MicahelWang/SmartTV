using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using YeahTVApi.DomainModel.Models;

namespace YeahCentre.EntityFrameworkRepository.Mapping
{
    public class TvTemplateAttributeMap : EntityTypeConfiguration<TvTemplateAttribute>
    {
        public TvTemplateAttributeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Text)
                .HasMaxLength(50);

            this.Property(t => t.ElementId)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TvTemplateAttribute", "YeahTV");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Text).HasColumnName("Text");
            this.Property(t => t.Value).HasColumnName("Value");
            this.Property(t => t.ElementId).HasColumnName("ElementId");
            this.Property(t => t.Editable).HasColumnName("Editable");
            this.Property(t => t.Required).HasColumnName("Required");
            this.HasRequired(m => m.Element).
                WithMany(m => m.Attributes).
                HasForeignKey(m => m.ElementId);
        }
    }
}
