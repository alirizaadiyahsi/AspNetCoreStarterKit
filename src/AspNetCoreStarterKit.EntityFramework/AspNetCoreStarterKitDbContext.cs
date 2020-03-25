using System;
using AspNetCoreStarterKit.Domain.Entities.Authorization;
using AspNetCoreStarterKit.Domain.Entities.OrganizationUnits;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNetCoreStarterKit.EntityFramework
{
    public class AspNetCoreStarterKitDbContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public AspNetCoreStarterKitDbContext(DbContextOptions options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<UserClaim>().ToTable("UserClaim");
            modelBuilder.Entity<UserLogin>().ToTable("UserLogin");
            modelBuilder.Entity<RoleClaim>().ToTable("RoleClaim");
            modelBuilder.Entity<UserToken>().ToTable("UserToken");
            modelBuilder.Entity<OrganizationUnit>().ToTable("UserToken");

            modelBuilder.Entity((Action<EntityTypeBuilder<UserRole>>)(b =>
            {
                b.ToTable("UserRole");

                b.HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId);

                b.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId);
            }));

            modelBuilder.Entity((Action<EntityTypeBuilder<OrganizationUnitUser>>)(b =>
            {
                b.ToTable("OrganizationUnitUser");

                b.HasOne(ur => ur.User)
                    .WithMany(u => u.OrganizationUnitUsers)
                    .HasForeignKey(ur => ur.UserId);

                b.HasOne(ur => ur.OrganizationUnit)
                    .WithMany(r => r.OrganizationUnitUsers)
                    .HasForeignKey(ur => ur.OrganizationUnitId);
            }));

            modelBuilder.Entity((Action<EntityTypeBuilder<OrganizationUnitRole>>)(b =>
            {
                b.ToTable("OrganizationUnitRole");

                b.HasOne(ur => ur.Role)
                    .WithMany(u => u.OrganizationUnitRoles)
                    .HasForeignKey(ur => ur.RoleId);

                b.HasOne(ur => ur.OrganizationUnit)
                    .WithMany(r => r.OrganizationUnitRoles)
                    .HasForeignKey(ur => ur.OrganizationUnitId);
            }));
        }
    }
}
