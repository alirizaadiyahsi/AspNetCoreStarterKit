using System.Globalization;
using System.Linq;
using AspNetCoreStarterKit.Domain.Entities.Authorization;
using AspNetCoreStarterKit.Domain.StaticData.Authorization;

namespace AspNetCoreStarterKit.EntityFramework.DataSeeder
{
    public class DbContextDataSeeder
    {
        private readonly AspNetCoreStarterKitDbContext _dbContext;
        private const string AdminRoleName = "Admin";
        private const string AdminUserName = "admin";
        private const string AdminUserEmail = "admin@mail.com";
        private const string PasswordHashFor123Qwe = "AM4OLBpptxBYmM79lGOX9egzZk3vIQU3d/gFCJzaBjAPXzYIK3tQ2N7X4fcrHtElTw=="; //123qwe

        public DbContextDataSeeder(AspNetCoreStarterKitDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void SeedData()
        {
            SeedRolesAndRoleClaims();
            SeedUsersAndUserRolesAndUserClaims();
            SeedUserRoles();
        }

        private void SeedUsersAndUserRolesAndUserClaims()
        {
            var userClaims = Permissions.GetAll().Select(permission => new UserClaim { ClaimType = CustomClaimTypes.Permission, ClaimValue = permission }).ToList();

            var adminUser = new User
            {
                UserName = AdminUserName,
                Email = AdminUserEmail,
                EmailConfirmed = true,
                NormalizedUserName = AdminUserName.ToUpper(CultureInfo.GetCultureInfo("en-US")),
                NormalizedEmail = AdminUserEmail.ToUpper(CultureInfo.GetCultureInfo("en-US")),
                AccessFailedCount = 5,
                PasswordHash = PasswordHashFor123Qwe,
                UserClaims = userClaims
            };

            _dbContext.Users.Add(adminUser);
            _dbContext.SaveChanges();
        }

        private void SeedRolesAndRoleClaims()
        {
            var roleClaims = Permissions.GetAll().Select(permission => new RoleClaim { ClaimType = CustomClaimTypes.Permission, ClaimValue = permission }).ToList();

            var adminRole = new Role
            {
                Name = AdminRoleName,
                NormalizedName = AdminRoleName.ToUpper(CultureInfo.GetCultureInfo("en-US")),
                IsSystemDefault = true,
                RoleClaims = roleClaims
            };

            _dbContext.Roles.Add(adminRole);
            _dbContext.SaveChanges();
        }

        private void SeedUserRoles()
        {
            var adminRole = _dbContext.Roles.FirstOrDefault(r => r.Name == AdminRoleName);
            var adminUser = _dbContext.Users.FirstOrDefault(u => u.UserName == AdminUserName);

            var userRole = new UserRole
            {
                Role = adminRole,
                User = adminUser
            };

            _dbContext.UserRoles.Add(userRole);
            _dbContext.SaveChanges();
        }
    }
}
