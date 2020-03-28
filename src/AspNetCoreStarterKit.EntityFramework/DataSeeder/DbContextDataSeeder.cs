using AspNetCoreStarterKit.Domain.Entities.Authorization;
using AspNetCoreStarterKit.Domain.StaticData.Authorization;
using System.Globalization;
using System.Linq;

namespace AspNetCoreStarterKit.EntityFramework.DataSeeder
{
    public class DbContextDataSeeder
    {
        private readonly AspNetCoreStarterKitDbContext _dbContext;
        public static string AdminRoleName = "Admin";
        public static string AdminUserName = "admin";
        public static string AdminUserEmail = "admin@mail.com";

        public static string MemberRoleName = "Member";
        public static string MemberUserName = "member";
        public static string MemberUserEmail = "member@mail.com";

        private const string PasswordHashFor123Qwe = "AM4OLBpptxBYmM79lGOX9egzZk3vIQU3d/gFCJzaBjAPXzYIK3tQ2N7X4fcrHtElTw=="; //123qwe

        public DbContextDataSeeder(AspNetCoreStarterKitDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual void SeedData()
        {
            InitializeAdminRole();
            InitializeAdminUser();
            InitializeAdminUserRoles();
            InitializeMemberRole();
            InitializeMemberUser();
            InitializeMemberUserRoles();
        }

        private void InitializeAdminUser()
        {
            if (_dbContext.Users.Any(u => u.UserName == AdminUserName)) return;

            var adminUser = new User
            {
                UserName = AdminUserName,
                Email = AdminUserEmail,
                EmailConfirmed = true,
                NormalizedUserName = AdminUserName.ToUpper(CultureInfo.GetCultureInfo("en-US")),
                NormalizedEmail = AdminUserEmail.ToUpper(CultureInfo.GetCultureInfo("en-US")),
                AccessFailedCount = 5,
                PasswordHash = PasswordHashFor123Qwe
            };

            _dbContext.Users.Add(adminUser);
            _dbContext.SaveChanges();

            var userClaims = Permissions.GetAll().Select(permission => new UserClaim { ClaimType = CustomClaimTypes.Permission, ClaimValue = permission, UserId = adminUser.Id}).ToList();
            _dbContext.UserClaims.AddRange(userClaims);
            _dbContext.SaveChanges();
        }

        private void InitializeMemberUser()
        {
            if (_dbContext.Users.Any(u => u.UserName == MemberUserName)) return;

            var memberUser = new User
            {
                UserName = MemberUserName,
                Email = MemberUserEmail,
                EmailConfirmed = true,
                NormalizedUserName = MemberUserName.ToUpper(CultureInfo.GetCultureInfo("en-US")),
                NormalizedEmail = MemberUserEmail.ToUpper(CultureInfo.GetCultureInfo("en-US")),
                AccessFailedCount = 5,
                PasswordHash = PasswordHashFor123Qwe
            };

            _dbContext.Users.Add(memberUser);
            _dbContext.SaveChanges();
        }

        private void InitializeAdminRole()
        {
            if (_dbContext.Roles.Any(r => r.Name == AdminRoleName)) return;

            var adminRole = new Role
            {
                Name = AdminRoleName,
                NormalizedName = AdminRoleName.ToUpper(CultureInfo.GetCultureInfo("en-US")),
                IsSystemDefault = true
            };

            _dbContext.Roles.Add(adminRole);
            _dbContext.SaveChanges();

            var roleClaims = Permissions.GetAll().Select(permission => new RoleClaim { ClaimType = CustomClaimTypes.Permission, ClaimValue = permission, RoleId = adminRole.Id}).ToList();
            _dbContext.RoleClaims.AddRange(roleClaims);
            _dbContext.SaveChanges();
        }

        private void InitializeMemberRole()
        {
            if (_dbContext.Roles.Any(r => r.Name == MemberRoleName)) return;

            var memberRole = new Role
            {
                Name = MemberRoleName,
                NormalizedName = MemberRoleName.ToUpper(CultureInfo.GetCultureInfo("en-US")),
                IsSystemDefault = true
            };

            _dbContext.Roles.Add(memberRole);
            _dbContext.SaveChanges();
        }

        private void InitializeAdminUserRoles()
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

        private void InitializeMemberUserRoles()
        {
            var memberUser = _dbContext.Users.FirstOrDefault(u => u.UserName == MemberUserName);
            var membeRole = _dbContext.Roles.FirstOrDefault(r => r.Name == MemberRoleName);
            if (_dbContext.UserRoles.Any(ur => ur.UserId == memberUser.Id && ur.RoleId == membeRole.Id)) return;

            var userRole = new UserRole
            {
                Role = membeRole,
                User = memberUser
            };

            _dbContext.UserRoles.Add(userRole);
            _dbContext.SaveChanges();
        }
    }
}
