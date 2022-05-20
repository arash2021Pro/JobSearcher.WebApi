using JobSearcher.CoreDomains.StorageDomains.SafetyPermissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobSearcher.CoreStorage.EntityConfigurations;

public class RolePermissionConfigurations:IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.HasOne(rel => rel.Role).WithMany(item => item.RolePermissions).HasForeignKey(k => k.RoleId);
        builder.HasOne(rel => rel.Permission).WithMany(item => item.RolePermissions).HasForeignKey(k => k.PermissionId);
    }
}