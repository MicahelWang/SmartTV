using System.Data.Entity.ModelConfiguration;
using YeahTVApi.DomainModel.Models;

namespace YeahTVApi.DomainModel.Models
{
    public class TagMap : EntityTypeConfiguration<Tag>
    {
        public TagMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            
            // Properties
            // Table & Column Mappings
            this.ToTable("Tag", "YeahTV");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RescorceId).HasColumnName("RescorceId");
            this.Property(t => t.ParentId).HasColumnName("ParentId");
            this.Property(t => t.Icon).HasColumnName("Icon");
        }
    }
}
