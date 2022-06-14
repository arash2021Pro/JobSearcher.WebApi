using JobSearcher.CoreDomains.StorageDomains.Expertise;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobSearcher.CoreStorage.EntityConfigurations;

public class UserExpertiseConfiguration:IEntityTypeConfiguration<UserExpertise>
{
    public void Configure(EntityTypeBuilder<UserExpertise> builder)
    {
        builder.HasOne(x => x.User).WithMany(x => x.UserExpertises).HasForeignKey(x => x.userId);
        builder.Property(x => x.Age).HasMaxLength(2);
        builder.Property(x => x.Gender).HasMaxLength(1);
        builder.Property(x => x.Place).IsRequired(false);
        builder.Property(x => x.Position).IsRequired(false);
        builder.Property(x => x.Skills).IsRequired(false);
        builder.Property(x => x.FullName).IsRequired(false);
        builder.Property(x => x.HistoryTime).IsRequired(false);
        builder.Property(x => x.FullName).IsRequired(false);
        builder.Property(x => x.NationalSerial).IsRequired(false);
    }
}