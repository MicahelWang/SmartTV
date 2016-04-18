using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using YeahTVApi.DomainModel.Models;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    public class HCSDownloadTaskMap : EntityTypeConfiguration<HCSDownloadTask>
    {
        public HCSDownloadTaskMap()
        {
            this.HasKey(t => t.Id);

            this.Property(t => t.Id)
                            .IsRequired()
                            .HasMaxLength(64);

            this.Property(t => t.TaskNo)
                            .IsRequired()
                            .HasMaxLength(64);

            this.Property(t => t.ServerId)
                            .IsRequired()
                            .HasMaxLength(100);

            this.Property(t => t.Type)
                            .IsRequired()
                            .HasMaxLength(45);

            this.Property(t => t.Status)
                            .IsRequired()
                            .HasMaxLength(45);

            this.Property(t => t.ResultStatus)
                            .HasMaxLength(45);

            this.Property(t => t.Config)
                            .HasMaxLength(300);

            this.Property(t => t.ErrorMessage)
                            .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("HCSDownloadTask", "YeahTV");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.TaskNo).HasColumnName("TaskNo");
            this.Property(t => t.ServerId).HasColumnName("ServerId");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.ResultStatus).HasColumnName("ResultStatus");
            this.Property(t => t.Config).HasColumnName("Config");
            this.Property(t => t.ErrorMessage).HasColumnName("ErrorMessage");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
            this.Property(t => t.LastUpdateUser).HasColumnName("LastUpdateUser");
        }
    }
}
