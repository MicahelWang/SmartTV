using System.Data.Entity.ModelConfiguration;
using YeahTVApi.DomainModel.Models;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    public class OrderProductsMap : EntityTypeConfiguration<OrderProducts>
    {
        public OrderProductsMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Id });


            this.ToTable("OrderProducts", "YeahTV");

            this.Property(t => t.Id).HasMaxLength(64);
            this.Property(t => t.OrderId).HasMaxLength(32);
            this.Property(t => t.ProductName).HasMaxLength(200);
            this.Property(t => t.ProductId).HasMaxLength(36);
            this.Property(t => t.ProductInfo).HasMaxLength(4000);

            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.OrderId).HasColumnName("OrderId");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.UnitPrice).HasColumnName("UnitPrice");
            this.Property(t => t.Quantity).HasColumnName("Quantity");
            this.Property(t => t.ProductName).HasColumnName("ProductName");
            this.Property(t => t.ProductId).HasColumnName("ProductId");
            this.Property(t => t.ProductInfo).HasColumnName("ProductInfo");


            this.HasRequired(m => m.StoreOrder)
                .WithMany(m => m.OrderProducts)
                .HasForeignKey(m => m.OrderId);

        }
    }
}
