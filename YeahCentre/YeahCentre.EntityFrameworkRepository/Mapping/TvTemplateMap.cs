using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using YeahTVApi.DomainModel.Models;

namespace YeahCentre.EntityFrameworkRepository.Mapping
{
    public class TvTemplateMap : EntityTypeConfiguration<TvTemplate>
    {
        public TvTemplateMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Name)
                .HasMaxLength(100);

            this.Property(t => t.Description)
                .HasMaxLength(200);

            this.Property(t => t.CreateUser)
                .HasMaxLength(50);

            this.Property(t => t.ModifyUser)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TvTemplate", "YeahTV");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.TemplateTypeId).HasColumnName("TemplateTypeId");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.CreateUser).HasColumnName("CreateUser");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.ModifyUser).HasColumnName("ModifyUser");
            this.Property(t => t.ModifyDate).HasColumnName("ModifyDate");
        }
    }
}
