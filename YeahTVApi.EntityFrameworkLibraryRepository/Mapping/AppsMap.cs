namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    
    using YeahTVApi.DomainModel.Models;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    public class AppsMap : EntityTypeConfiguration<Apps>
    {
        public AppsMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id) 
                .HasMaxLength(64);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(32);

            this.Property(t => t.Platfrom)
                .IsRequired()
                .HasMaxLength(32);

            this.Property(t => t.Description)
                .HasMaxLength(500);

            this.Property(t => t.PackageName)
                .HasMaxLength(64);

            this.Property(t => t.AppKey)
                .IsRequired()
                .HasMaxLength(64);

            this.Property(t => t.LastUpdater)
                .HasMaxLength(32);

            this.Property(t => t.SecureKey)
                .IsRequired()
                .HasMaxLength(64);

            // Table & Column Mappings
            this.ToTable("Apps");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Platfrom).HasColumnName("Platfrom");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.PackageName).HasColumnName("PackageName");
            this.Property(t => t.AppKey).HasColumnName("AppKey");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.LastUpdater).HasColumnName("LastUpdater");
            this.Property(t => t.LastUpdateTime).HasColumnName("LastUpdateTime");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.SecureKey).HasColumnName("SecureKey");
            this.Property(t => t.ShowInStroe).HasColumnName("ShowInStroe");
            this.Property(t => t.IconUrl).HasColumnName("IconUrl");
            this.Property(t => t.IsSystem).HasColumnName("IsSystem");
        }
    }
}
