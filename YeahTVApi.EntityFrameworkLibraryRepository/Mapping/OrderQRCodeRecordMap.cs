namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    
    using YeahTVApi.DomainModel.Models;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    public class OrderQRCodeRecordMap : EntityTypeConfiguration<OrderQRCodeRecord>
    {
        public OrderQRCodeRecordMap()
        {
            this.HasKey(t => new { t.Id });
            this.Property(t => t.Id).HasMaxLength(64);
            this.Property(t => t.OrderId).HasMaxLength(64);
            this.Property(t => t.OrderType).HasMaxLength(100);
            this.Property(t => t.Ticket).HasMaxLength(1000);


            this.ToTable("OrderQRCodeRecord", "YeahTV");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.OrderId).HasColumnName("OrderId");
            this.Property(t => t.OrderType).HasColumnName("OrderType");
            this.Property(t => t.Ticket).HasColumnName("Ticket");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");  
        }
    }
}
