using JobSearcher.CoreDomains.StorageDomains.SafetyPermissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobSearcher.CoreStorage.EntityConfigurations;

public class RoleConfigurations:IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.Property(prop => prop.Rolename).IsRequired(false);
    }
}