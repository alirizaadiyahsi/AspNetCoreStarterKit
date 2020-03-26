using AspNetCoreStarterKit.Domain.Entities.Authorization;
using AspNetCoreStarterKit.Domain.StaticData.Authorization;
using System.Globalization;
using System.Linq;

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
            AdminRolesAndRoleClaims();
            AdminUsersAndUserRolesAndUserClaims();
            AdminUserRoles();
        }

        private void AdminUsersAndUserRolesAndUserClaims()
        {
            if (_dbContext.Users.Any(u => u.UserName == AdminUserName)) return;

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

        private void AdminRolesAndRoleClaims()
        {
            if (_dbContext.Roles.Any(r => r.Name == AdminRoleName)) return;

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

        private void AdminUserRoles()
        {
            var adminUser = _dbContext.Users.FirstOrDefault(u => u.UserName == AdminUserName);
            var adminRole = _dbContext.Roles.FirstOrDefault(r => r.Name == AdminRoleName);
            if (_dbContext.UserRoles.Any(ur => ur.UserId == adminUser.Id && ur.RoleId == adminRole.Id)) return;

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
