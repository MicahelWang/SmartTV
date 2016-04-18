using System.Data.Entity.ModelConfiguration;
using YeahTVApi.DomainModel.Models;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    public class StoreOrderMap : EntityTypeConfiguration<StoreOrder>
    {
        public StoreOrderMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Id });


            this.ToTable("StoreOrder", "YeahTV");

            this.Property(t => t.SeriseCode).HasMaxLength(128);
            this.Property(t => t.RoomNo).HasMaxLength(7);
            this.Property(t => t.Hotelid).HasMaxLength(50);
            this.Property(t => t.HotelName).HasMaxLength(1000);
            this.Property(t => t.GoodsName).HasMaxLength(32);
            this.Property(t => t.GoodsDesc).HasMaxLength(100);
            this.Property(t => t.DeliveryType).HasMaxLength(100);
            
            this.Property(t => t.PayInfo).HasMaxLength(200);

            this.Property(t => t.Id).HasColumnName("OrderId");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.CompleteTime).HasColumnName("CompleteTime");
            this.Property(t => t.SeriseCode).HasColumnName("SeriseCode");
            this.Property(t => t.RoomNo).HasColumnName("RoomNo");
            this.Property(t => t.Hotelid).HasColumnName("Hotelid");
            this.Property(t => t.HotelName).HasColumnName("HotelName");
            this.Property(t => t.GoodsName).HasColumnName("GoodsName");
            this.Property(t => t.GoodsDesc).HasColumnName("GoodsDesc");
            this.Property(t => t.PayInfo).HasColumnName("PayInfo");
            this.Property(t => t.IsDelete).HasColumnName("IsDelete");
            this.Property(t => t.DeliveryState).HasColumnName("DeliveryState");
            this.Property(t => t.ExpirationDate).HasColumnName("ExpirationDate");
            this.Property(t => t.InvoicesTitle).HasColumnName("InvoicesTitle");
            this.Property(t => t.DeliveryType).HasColumnName("DeliveryType");	

        }
    }
}
