using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Models.DomainModels;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    public class GlobalConfigMap : EntityTypeConfiguration<GlobalConfig>
    {
        public GlobalConfigMap()
        {

            this.HasKey(t => t.Id);

            this.Property(t => t.PermitionType)
                .HasMaxLength(45);
            this.Property(t => t.TypeId)
                .HasMaxLength(64);
            this.Property(t => t.ConfigName)
                .HasMaxLength(50);
            this.Property(t => t.ConfigValue)
                .HasMaxLength(4000);
            this.Property(t => t.ConfigDescribe)
                .HasMaxLength(4000);
            this.Property(t => t.LastUpdateUser)
                .HasMaxLength(64);

            this.ToTable("GlobalConfig", "YeahTV");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PermitionType).HasColumnName("PermitionType");
            this.Property(t => t.TypeId).HasColumnName("TypeId");
            this.Property(t => t.ConfigName).HasColumnName("ConfigName");
            this.Property(t => t.ConfigValue).HasColumnName("ConfigValue");
            this.Property(t => t.ConfigDescribe).HasColumnName("ConfigDescribe");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.PriorityLevel).HasColumnName("PriorityLevel");
            this.Property(t => t.LastUpdateUser).HasColumnName("LastUpdateUser");
            this.Property(t => t.LastUpdateTime).HasColumnName("LastUpdateTime");
        }
    }
}
