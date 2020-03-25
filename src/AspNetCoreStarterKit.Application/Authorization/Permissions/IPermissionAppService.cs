using System.Threading.Tasks;

namespace AspNetCoreStarterKit.Application.Authorization.Permissions
{
    public interface IPermissionAppService
    {
        Task<bool> IsUserGrantedToPermissionAsync(string userName, string permission);
    }
}