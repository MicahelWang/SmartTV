using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using YeahTVApi.DomainModel.Models;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    public class HCSDownLoadJobMap : EntityTypeConfiguration<HCSDownLoadJob>
    {
        public HCSDownLoadJobMap()
        {
            this.HasKey(t => t.Id);

            this.Property(t => t.Id)
                            .IsRequired()
                            .HasMaxLength(64);

            this.Property(t => t.TaskId)
                            .IsRequired()
                            .HasMaxLength(64);

            this.Property(t => t.Name)
                            .IsRequired()
                            .HasMaxLength(200);

            this.Property(t => t.Operation)
                            .IsRequired()
                            .HasMaxLength(45);

            this.Property(t => t.MD5)
                            .IsRequired()
                            .HasMaxLength(512);

            this.Property(t => t.Priority)
                            .IsRequired()
                            .HasMaxLength(10);

            this.Property(t => t.Type)
                            .HasMaxLength(64);

            this.Property(t => t.Url)
                            .HasMaxLength(5000);

            this.Property(t => t.Status)
                            .HasMaxLength(45);

            this.Property(t => t.BizNo)
                            .HasMaxLength(50);

            this.Property(t => t.ErrorMessage)
                            .HasMaxLength(500);

            this.HasRequired(t => t.HCSDownloadTask)
                                .WithMany(t => t.HCSDownLoadJobs)
                                .HasForeignKey(t => t.TaskId);

            // Table & Column Mappings
            this.ToTable("HCSJob", "YeahTV");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.TaskId).HasColumnName("TaskId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Operation).HasColumnName("Operation");
            this.Property(t => t.MD5).HasColumnName("MD5");
            this.Property(t => t.Priority).HasColumnName("Priority");
            this.Property(t => t.Type).HasColumnName("BizType");
            this.Property(t => t.Url).HasColumnName("Url");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.BizNo).HasColumnName("BizNo");
            this.Property(t => t.ErrorMessage).HasColumnName("ErrorMessage");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
            this.Property(t => t.LastUpdateUser).HasColumnName("LastUpdateUser");
        }
    }
}
