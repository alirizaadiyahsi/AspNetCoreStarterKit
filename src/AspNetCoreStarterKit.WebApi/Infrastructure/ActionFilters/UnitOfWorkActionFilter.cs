using AspNetCoreStarterKit.EntityFramework;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AspNetCoreStarterKit.WebApi.Infrastructure.ActionFilters
{
    public class UnitOfWorkActionFilter : ActionFilterAttribute
    {
        private readonly AspNetCoreStarterKitDbContext _dbContext;

        public UnitOfWorkActionFilter(AspNetCoreStarterKitDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null || !context.ModelState.IsValid) return;

            try
            {
                _dbContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                dbUpdateConcurrencyException.Entries.Single().Reload();
                _dbContext.SaveChanges();
            }
        }
    }
}
