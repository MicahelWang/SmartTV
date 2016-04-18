namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    
    using YeahTVApi.DomainModel.Models;
    using System.Data.Entity.ModelConfiguration;

    public class AuthCertigierManagerMap : EntityTypeConfiguration<AuthCertigierManager>
    {
        public AuthCertigierManagerMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.UserId)
                .HasMaxLength(50);

            this.Property(t => t.UserName)
                .HasMaxLength(50);

            this.Property(t => t.Token)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("AuthCertigierManager", "YeahTV");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.Token).HasColumnName("Token");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.ExpireTime).HasColumnName("ExpireTime");
            this.Property(t => t.Type).HasColumnName("Type");
        }
    }
}
