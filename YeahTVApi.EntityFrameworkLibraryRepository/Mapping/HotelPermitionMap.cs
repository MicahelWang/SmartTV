using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Models.DomainModels;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    public class HotelPermitionMap : EntityTypeConfiguration<HotelPermition>
    {
        public HotelPermitionMap()
        {
            this.HasKey(t => new { t.Id });
            // Properties

            // Table & Column Mappings
            this.ToTable("HotelPermition", "YeahTV");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PermitionType).HasColumnName("PermitionType");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.TypeId).HasColumnName("TypeId");
        }
    }
}
