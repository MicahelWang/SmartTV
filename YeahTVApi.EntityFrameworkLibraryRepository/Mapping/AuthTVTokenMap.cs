namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    
    using YeahTVApi.DomainModel.Models;
    using System.Data.Entity.ModelConfiguration;

    public class AuthTVTokenMap : EntityTypeConfiguration<AuthTVToken>
    {
        public AuthTVTokenMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(64);

            this.Property(t => t.AuthTicket)
                .HasMaxLength(500);

            this.Property(t => t.AuthToken)
                .HasMaxLength(64);

            this.Property(t => t.Code)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("AuthTVToken", "YeahTV");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.AuthTicket).HasColumnName("AuthTicket");
            this.Property(t => t.AuthToken).HasColumnName("AuthToken");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.ExpireTime).HasColumnName("ExpireTime");
            this.Property(t => t.Type).HasColumnName("Type");
        }
    }
}
