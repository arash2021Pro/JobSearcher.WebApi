using System.Reflection;
using DNTPersianUtils.Core;
using JobSearcher.CoreDomains.ApiDomains;
using JobSearcher.CoreDomains.BaseEntity;
using JobSearcher.CoreDomains.ReposistoryPattern;
using JobSearcher.CoreDomains.StorageDomains;
using JobSearcher.CoreDomains.StorageDomains.Expertise;
using JobSearcher.CoreDomains.StorageDomains.Otp;
using JobSearcher.CoreDomains.StorageDomains.SafetyPermissions;
using JobSearcher.CoreStorage.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace JobSearcher.CoreStorage.SqlContext;

public class ApplicationContext:DbContext,IUnitOfWork
{
    public ApplicationContext(DbContextOptions<ApplicationContext>options):base(options)
    {
        
    }
    public DbSet<User>Users { get; set; }
    public DbSet<Role>Roles { get; set; }
    public DbSet<Permission>Permissions { get; set; }
    public DbSet<RolePermission>RolePermissions { get; set; }
    public DbSet<OTP>Otps { get; set; }
    public DbSet<UserExpertise>UserExpertises { get; set; }

        public override DbSet<TEntity> Set<TEntity>()
        {
            return base.Set<TEntity>();
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return base.SaveChangesAsync(cancellationToken);
        }
    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(UserConfigurations)));

            var entities = modelBuilder
                .Model
                .GetEntityTypes()
                .Select(x => x.ClrType)
                .Where(x => x.BaseType == typeof(Core))
                .ToList();

            foreach (var type in entities)
            {
                var method = SetGlobalQueryMethod.MakeGenericMethod(type);
                method.Invoke(this, new object[] {modelBuilder});
            }
        }

        public static readonly MethodInfo SetGlobalQueryMethod = typeof(ApplicationContext)
            .GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Single(t => t.IsGenericMethod && t.Name == "SetGlobalQuery");

        public void SetGlobalQuery<T>(ModelBuilder builder) where T : Core
        {
            builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
        }


        private void changeEntitiesStates()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is Core && (
                    e.State == EntityState.Added
                    || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {

                if (entityEntry.State == EntityState.Added)
                {
                    ((Core) entityEntry.Entity).CurrentTime = DateTime.Now.ToShortPersianDateTimeString();
                }
                else if (entityEntry.State == EntityState.Modified)
                {
                    ((Core) entityEntry.Entity).OffsetModification = DateTimeOffset.Now.ToShortPersianDateTimeString();
                }
            }
        }
}