using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using YeahTVApi.DomainModel.Models;

namespace YeahCentre.EntityFrameworkRepository.Mapping
{
    public class TvTemplateElementMap : EntityTypeConfiguration<TvTemplateElement>
    {
        public TvTemplateElementMap()
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

            // Table & Column Mappings
            this.ToTable("TvTemplateElement", "YeahTV");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.TemplateType).HasColumnName("TemplateType");
            this.Property(t => t.ParentId).HasColumnName("ParentId");
            this.Property(t => t.Orders).HasColumnName("Orders");
            this.Property(t => t.Editable).HasColumnName("Editable");
        }
    }
}
