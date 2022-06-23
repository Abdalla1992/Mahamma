using Mahamma.Identity.Domain._SharedKernel;
using Mahamma.Identity.Domain.Language.Entity;
using Mahamma.Identity.Domain.Role.Entity;
using Mahamma.Identity.Domain.User.Entity;
using Mahamma.Identity.Domain.UserRole.Entity;
using Mahamma.Identity.Infrastructure.EntityConfigurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Identity.Infrastructure.Context
{
    public class IdentityContext : IdentityDbContext<User, Role, long, IdentityUserClaim<long>, UserRole, IdentityUserLogin<long>, IdentityRoleClaim<long>, IdentityUserToken<long>>, IUnitOfWork
    {
        #region DBSet
        public DbSet<RolePagePermission> RolePagePermission { get; set; }
        public DbSet<Page> Page { get; set; }
        public DbSet<PagePermission> PagePermission { get; set; }
        public DbSet<Language> Language { get; set; }
        public DbSet<PermissionLocalization> PermissionLocalization { get; set; }
        public DbSet<PageLocalization> PageLocalization { get; set; }
        public DbSet<UserProfileSection> UserProfileSection { get; set; }
        #endregion

        #region CTRS
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {
        }
        #endregion

        #region Model Creation
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PageEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PagePermissionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RolePagePermissionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LanguageEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionLocalizationEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PageLocalizationEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserProfileSectionEntityTypeConfiguration());
            modelBuilder.BuildEnums();
            //calling the method from base is a must for AspNetCore.Identity to be put the keys for its entities
            base.OnModelCreating(modelBuilder);
            //configuration that modify on indexs have to be called after the OnModelCreating to be able to drop the indexes
            modelBuilder.ApplyConfiguration(new RoleEntityTypeConfiguration());
        }
        #endregion

        #region Save Object
        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await base.SaveChangesAsync(cancellationToken) > default(int);
            return result;
        }
        #endregion

    }
}
