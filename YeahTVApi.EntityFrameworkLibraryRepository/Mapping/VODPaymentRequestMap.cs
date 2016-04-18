using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Models;
using System.Data.Entity.ModelConfiguration;


namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    public class VODPaymentRequestMap : EntityTypeConfiguration<VODPaymentRequest>
    {
        public VODPaymentRequestMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(64);

          

            // Table & Column Mappings
            this.ToTable("VODPaymentRequest", "YeahTV");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.OrderId).HasColumnName("OrderId");
            this.Property(t => t.OrderAmount).HasColumnName("OrderAmount");
            this.Property(t => t.PayInfo).HasColumnName("PayInfo");
            this.Property(t => t.GoodsId).HasColumnName("GoodsId");
            this.Property(t => t.GoodsName).HasColumnName("GoodsName");
            this.Property(t => t.GoodsDesc).HasColumnName("GoodsDesc");


            this.Property(t => t.BizHotle).HasColumnName("BizHotle");
            this.Property(t => t.BizRoom).HasColumnName("BizRoom");
            this.Property(t => t.BizDevice).HasColumnName("BizDevice");

            this.Property(t => t.BizMember).HasColumnName("BizMember");
            this.Property(t => t.NotifyUrl).HasColumnName("NotifyUrl");
            this.Property(t => t.Memo).HasColumnName("Memo");

            this.Property(t => t.Pid).HasColumnName("Pid");
            this.Property(t => t.RequestSign).HasColumnName("RequestSign");
            this.Property(t => t.ResultSign).HasColumnName("ResultSign");

            this.Property(t => t.ResultMessage).HasColumnName("ResultMessage");
            this.Property(t => t.ResultCode).HasColumnName("ResultCode");
            this.Property(t => t.ResultQrcodeUrl).HasColumnName("ResultQrcodeUrl");


        }
    }
}
