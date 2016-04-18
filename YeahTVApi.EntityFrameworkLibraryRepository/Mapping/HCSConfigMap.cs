using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using YeahTVApi.DomainModel.Models;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    class HCSConfigMap : EntityTypeConfiguration<HCSConfig>
    {
        public HCSConfigMap()
        {
            this.HasKey(t => t.Id);

            this.Property(t => t.Id)
                            .IsRequired()
                            .HasMaxLength(64);

            this.Property(t => t.ServerId)
                            .IsRequired()
                            .HasMaxLength(128);

            this.Property(t => t.Type)
                            .IsRequired()
                            .HasMaxLength(45);

            // Table & Column Mappings
            this.ToTable("HCSConfig", "YeahTV");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ServerId).HasColumnName("ServerId");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.Value).HasColumnName("Value");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
            this.Property(t => t.UpdateUser).HasColumnName("UpdateUser");
        }
    }
}
