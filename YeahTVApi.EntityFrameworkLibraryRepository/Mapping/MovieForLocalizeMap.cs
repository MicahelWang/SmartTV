using System.Data.Entity.ModelConfiguration;
using YeahTVApi.DomainModel.Models;

namespace YeahTVApi.DomainModel.Models
{
    public class MovieForLocalizeMap : EntityTypeConfiguration<MovieForLocalize>
    {
        public MovieForLocalizeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(64);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(64);

            this.Property(t => t.Director)
                .HasMaxLength(64);

            this.Property(t => t.Starred)
                .HasMaxLength(64);

            this.Property(t => t.District)
                .HasMaxLength(64);

            this.Property(t => t.Mins)
                .HasMaxLength(64);

            this.Property(t => t.MovieReview)
                .HasMaxLength(64);

            this.Property(t => t.PosterAddress)
                .HasMaxLength(300);

            this.Property(t => t.CoverAddress)
                .HasMaxLength(100);

            this.Property(t => t.TagIds)
                .HasMaxLength(100);

            this.Property(t => t.VodUrl)
                .HasMaxLength(500);

            this.Property(t => t.MD5)
                .HasMaxLength(256);

            this.Property(t => t.Language)
                .HasMaxLength(100);

            this.Property(t => t.CurrencyType)
                .HasMaxLength(100);

            this.Property(t => t.Attribute)
                .HasMaxLength(100);

            this.Property(t => t.LastUpdateUser)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.HotelCount)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("MovieForLocalize", "YeahTV");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Director).HasColumnName("Director");
            this.Property(t => t.Starred).HasColumnName("Starred");
            this.Property(t => t.District).HasColumnName("District");
            this.Property(t => t.Mins).HasColumnName("Mins");
            this.Property(t => t.Vintage).HasColumnName("Vintage");
            this.Property(t => t.MovieReview).HasColumnName("MovieReview");
            this.Property(t => t.PosterAddress).HasColumnName("PosterAddress");
            this.Property(t => t.CoverAddress).HasColumnName("CoverAddress");
            this.Property(t => t.TagIds).HasColumnName("Tags");
            this.Property(t => t.VodUrl).HasColumnName("VodUrl");
            this.Property(t => t.MD5).HasColumnName("MD5");
            this.Property(t => t.Language).HasColumnName("Language");
            this.Property(t => t.Rate).HasColumnName("Rate");
            this.Property(t => t.DefaultAmount).HasColumnName("DefaultAmount");
            this.Property(t => t.CurrencyType).HasColumnName("CurrencyType");
            this.Property(t => t.Attribute).HasColumnName("Attribute");
            this.Property(t => t.HotelCount).HasColumnName("HotelCount");
            this.Property(t => t.Order).HasColumnName("Order");
            this.Property(t => t.IsTop).HasColumnName("IsTop");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.LastUpdateTime).HasColumnName("LastUpdateTime");
            this.Property(t => t.LastUpdateUser).HasColumnName("LastUpdateUser");
            this.Property(t => t.DistributeAll).HasColumnName("DistributeAll");
            this.Property(t => t.FirstWord).HasColumnName("FirstWord");
        }
    }
}
