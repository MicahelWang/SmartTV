using System.Data.Entity.ModelConfiguration;
using YeahTVApi.DomainModel.Models;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    public class VODOrderMap : EntityTypeConfiguration<VODOrder>
    {
        public VODOrderMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Id });

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(20);


            // Table & Column Mappings
            this.ToTable("VODOrder", "YeahTV");
            this.Property(t => t.Id).HasColumnName("OrderId");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.State).HasColumnName("State");
            this.Property(t => t.CompleteTime).HasColumnName("CompleteTime");
            this.Property(t => t.VodRequestId).HasColumnName("VodRequestId");
            this.Property(t => t.MovieId).HasColumnName("MovieId");
            this.Property(t => t.SeriseCode).HasColumnName("SeriseCode");
            this.Property(t => t.RoomNo).HasColumnName("RoomNo");
            this.Property(t => t.HotelId).HasColumnName("Hotelid");
            this.Property(t => t.GoodsName).HasColumnName("GoodsName");
            this.Property(t => t.GoodsDesc).HasColumnName("GoodsDesc");
            this.Property(t => t.PayInfo).HasColumnName("PayInfo");
            this.Property(t => t.PayType).HasColumnName("PayType");
            this.Property(t => t.IsDelete).HasColumnName("IsDelete");
            this.Property(t => t.InvoicesTitle).HasColumnName("InvoicesTitle");
        }
    }
}
