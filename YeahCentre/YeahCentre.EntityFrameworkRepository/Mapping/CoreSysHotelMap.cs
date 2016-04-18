using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using YeahTVApi.DomainModel.Models;

namespace YeahCentre.EntityFrameworkRepository.Mapping
{
    public class CoreSysHotelMap : EntityTypeConfiguration<CoreSysHotel>
    {
        public CoreSysHotelMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.HotelCode)
                .HasMaxLength(20);

            this.Property(t => t.HotelName)
                .HasMaxLength(20);
            this.Property(t => t.HotelNameEn)
                .HasMaxLength(20);
            

            this.Property(t => t.GroupId)
                .HasMaxLength(50);

            this.Property(t => t.BrandId)
                .HasMaxLength(50);

            this.Property(t => t.Tel)
                .HasMaxLength(20);

            this.Property(t => t.Address)
                .HasMaxLength(255);
            this.Property(t => t.TemplateId)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("CoreSysHotel", "YeahTV");
            this.Property(t => t.Id).HasColumnName("HotelId");
            this.Property(t => t.HotelCode).HasColumnName("HotelCode");
            this.Property(t => t.HotelName).HasColumnName("HotelName");
            this.Property(t => t.HotelNameEn).HasColumnName("HotelNameEn");
            this.Property(t => t.GroupId).HasColumnName("GroupId");
            this.Property(t => t.BrandId).HasColumnName("BrandId");
            this.Property(t => t.IsLocalPMS).HasColumnName("IsLocalPMS");
            this.Property(t => t.Tel).HasColumnName("Tel");
            this.Property(t => t.Province).HasColumnName("Province");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.Longitude).HasColumnName("Longitude");
            this.Property(t => t.Latitude).HasColumnName("Latitude");
            this.Property(t => t.TemplateId).HasColumnName("TemplateId");
            this.Property(t => t.IsDelete).HasColumnName("IsDelete");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");

            this.HasRequired(m => m.CoreSysBrand)
                .WithMany(m => m.CoreSysHotels)
                .HasForeignKey(m => m.BrandId);
            this.HasRequired(m => m.CoreSysHotelSencond)
                .WithRequiredPrincipal(m => m.CoreSysHotel);

            this.HasOptional(m => m.TvTemplate)
                .WithMany(m => m.CoreSysHotel)
                .HasForeignKey(m => m.TemplateId);

        }
    }
}
