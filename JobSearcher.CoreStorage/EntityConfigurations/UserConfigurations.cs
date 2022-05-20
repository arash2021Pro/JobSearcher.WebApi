using JobSearcher.CoreDomains.ApiDomains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobSearcher.CoreStorage.EntityConfigurations;

public class UserConfigurations:IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(prop => prop.Phonenumber).IsRequired().HasMaxLength(11);
        builder.Property(prop => prop.Password).IsRequired();
        builder.Property(prop => prop.RoleId).IsRequired();
        builder.HasOne(rel => rel.Role).WithMany(item => item.Users).HasForeignKey(k => k.RoleId);
        
    }
}