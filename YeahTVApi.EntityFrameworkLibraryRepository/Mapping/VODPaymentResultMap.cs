using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Models;
using System.Data.Entity.ModelConfiguration;


namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    public class VODPaymentResultMap : EntityTypeConfiguration<VODPaymentResult>
    {
        public VODPaymentResultMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(64);

          

            // Table & Column Mappings
            this.ToTable("VODPaymentResult", "YeahTV");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.ResultSign).HasColumnName("ResultSign");
            this.Property(t => t.ResultMessage).HasColumnName("ResultMessage");
            this.Property(t => t.ResultCode).HasColumnName("ResultCode");
            this.Property(t => t.OrderId).HasColumnName("OrderId");
            this.Property(t => t.NotifyTime).HasColumnName("NotifyTime");
        }
    }
}
            