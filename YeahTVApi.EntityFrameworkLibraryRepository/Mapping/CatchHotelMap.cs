using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Models;
using System.Data.Entity.ModelConfiguration;


namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    public class CatchHotelMap : EntityTypeConfiguration<CatchHotel>
    {
        public CatchHotelMap() 
        {
            this.HasKey(t => new { t.HoteId});
            //proverty
            this.ToTable("CatchHotel", "YeahTV");
            this.Property(t => t.HoteId).HasColumnName("HoteId");
            this.Property(t => t.HotelAdress).HasColumnName("HotelAdress");
            this.Property(t => t.HotelName).HasColumnName("HotelName");
            this.Property(t => t.HotelNameEn).HasColumnName("HotelNameEn");
            this.Property(t => t.Image).HasColumnName("Image");
            this.Property(t => t.Lat).HasColumnName("Lat");
            this.Property(t => t.Lng).HasColumnName("Lng");
            this.Property(t => t.Tel).HasColumnName("Tel");
            this.Property(t => t.BrandId).HasColumnName("BrandId");
            this.Property(t => t.BrandName).HasColumnName("BrandName");
            this.Property(t => t.districtId).HasColumnName("districtId");
            this.Property(t => t.districtName).HasColumnName("districtName");

        }
    }
}
