using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.DomainModels;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    public class ScoreExchangMap : EntityTypeConfiguration<ScoreExchang>
    {
        public ScoreExchangMap()
        {

            this.HasKey(t => t.Id);

            this.Property(t => t.Id).HasMaxLength(64);
            this.Property(t => t.OrderType).HasMaxLength(100);
            this.Property(t => t.OrderId).HasMaxLength(64);
            this.Property(t => t.RunningNumber).HasMaxLength(200);
            this.Property(t => t.ScoreRate).HasMaxLength(20);
            this.Property(t => t.Productid).HasMaxLength(64);
            this.Property(t => t.Reqtime).HasMaxLength(14);
            this.Property(t => t.Remark).HasMaxLength(4000);

            this.ToTable("ScoreExchang", "YeahTV");
            this.Property(t => t.Id).HasColumnName("Id"); 
            this.Property(t => t.OrderType).HasColumnName("OrderType");
            this.Property(t => t.OrderId).HasColumnName("OrderId");
            this.Property(t => t.RunningNumber).HasColumnName("RunningNumber");
            this.Property(t => t.Score).HasColumnName("Score");
            this.Property(t => t.LastUpdateTime).HasColumnName("LastUpdateTime");
            this.Property(t => t.ScoreRate).HasColumnName("ScoreRate");
            this.Property(t => t.Productid).HasColumnName("Productid");
            this.Property(t => t.Reqtime).HasColumnName("Reqtime");
            this.Property(t => t.Remark).HasColumnName("Remark");  
        }
    }
}
