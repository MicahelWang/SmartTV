using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using YeahTVApi.DomainModel.Models;

namespace YeahCentre.EntityFrameworkRepository.Mapping
{
    public class CoreSysHotelSencondMap : EntityTypeConfiguration<CoreSysHotelSencond>
    {
        public CoreSysHotelSencondMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.WelcomeWord)
                .HasMaxLength(200);

            this.Property(t => t.LaunchBackground)
                .HasMaxLength(200);

            this.Property(t => t.LocalPMSUrl)
                .HasMaxLength(200);

            this.Property(t => t.AdUrl)
                .HasMaxLength(200);
            this.Property(t => t.BaseData)
                .HasMaxLength(8000);
            this.Property(t => t.LogoImageUrl)
                .HasMaxLength(8000);

            // Table & Column Mappings
            this.ToTable("CoreSysHotelSencond", "YeahTV");
            this.Property(t => t.Id).HasColumnName("HotelId");
            this.Property(t => t.AutoToHome).HasColumnName("AutoToHome");
            this.Property(t => t.Languages).HasColumnName("Languages");
            this.Property(t => t.WelcomeWord).HasColumnName("WelcomeWord");
            this.Property(t => t.LaunchBackground).HasColumnName("LaunchBackground");
            this.Property(t => t.LocalPMSUrl).HasColumnName("LocalPMSUrl");
            this.Property(t => t.PriceOfDay).HasColumnName("PriceOfDay");
            this.Property(t => t.AdUrl).HasColumnName("AdUrl");
            this.Property(t => t.BaseData).HasColumnName("BaseData");
            this.Property(t => t.LogoImageUrl).HasColumnName("LogoImageUrl");
            
        }
    }
}
