using JobSearcher.CoreDomains.StorageDomains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobSearcher.CoreStorage.EntityConfigurations;

public class PermissionConfigurations:IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.Property(prop => prop.PermissionName).HasMaxLength(50).IsRequired(false);
        
    }
}