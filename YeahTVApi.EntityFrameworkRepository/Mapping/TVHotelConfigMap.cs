namespace YeahTVApi.EntityFrameworkRepository.Mapping
{
    
    using YeahTVApi.DomainModel.Models;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    public class TVHotelConfigMap : EntityTypeConfiguration<TVHotelConfig>
    {
        public TVHotelConfigMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.HotelId)
                .HasMaxLength(32);

            this.Property(t => t.ConfigCode)
                .HasMaxLength(32);

            this.Property(t => t.ConfigName)
                .HasMaxLength(32);

            this.Property(t => t.ConfigValue)
                .HasMaxLength(500);

            this.Property(t => t.LastUpdater)
                .HasMaxLength(32);

            // Table & Column Mappings
            this.ToTable("TVHotelConfig");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.HotelId).HasColumnName("HotelId");
            this.Property(t => t.ConfigCode).HasColumnName("ConfigCode");
            this.Property(t => t.ConfigName).HasColumnName("ConfigName");
            this.Property(t => t.ConfigValue).HasColumnName("ConfigValue");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.LastUpdater).HasColumnName("LastUpdater");
            this.Property(t => t.LastUpdateTime).HasColumnName("LastUpdateTime");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
    }
}
