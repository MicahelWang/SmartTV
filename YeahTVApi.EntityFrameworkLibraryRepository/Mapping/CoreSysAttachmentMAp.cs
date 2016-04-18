namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    
    using YeahTVApi.DomainModel.Models;
    using System.Data.Entity.ModelConfiguration;

    public class CoreSysAttachmentMap : EntityTypeConfiguration<CoreSysAttachment>
    {
        public CoreSysAttachmentMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties


            // Table & Column Mappings
            this.ToTable("CoreSysAttachment");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.FileName).HasColumnName("FileName");
            this.Property(t => t.FileType).HasColumnName("FileType");
            this.Property(t => t.FilePath).HasColumnName("FilePath");
            this.Property(t => t.FileSize).HasColumnName("FileSize");
            this.Property(t => t.Unit).HasColumnName("Unit");
            this.Property(t => t.CrateTime).HasColumnName("CrateTime");
        }
    }
}
