using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreStarterKit.Domain.Entities.Authorization;
using AspNetCoreStarterKit.EntityFramework;
using AspNetCoreStarterKit.WebApi.Infrastructure.ActionFilters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AspNetCoreStarterKit.Tests.WebApi.ActionFilters
{
    public class UnitOfWorkFilterTest : ApiTestBase
    {
        private readonly AspNetCoreStarterKitDbContext _dbContext;
        private readonly IServiceProvider _serviceProvider;

        public UnitOfWorkFilterTest()
        {
            _serviceProvider = GetNewHostServiceProvider();
            _dbContext = _serviceProvider.GetRequiredService<AspNetCoreStarterKitDbContext>();
        }

        [Fact]
        public async Task Should_UnitOfWork_Action_Filter_Save_Changes()
        {
            var testRole = new Role
            {
                Id = Guid.NewGuid(),
                Name = "TestRole_" + Guid.NewGuid()
            };

            var unitOfWorkActionFilter = new UnitOfWorkActionFilter(_dbContext);
            var actionContext = new ActionContext(
                new DefaultHttpContext
                {
                    Request =
                    {
                            Method = "Post"
                    }
                },
                new RouteData(),
                new ActionDescriptor()
            );

            var actionExecutedContext = new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), null);
            await _dbContext.Roles.AddAsync(testRole);

            var dbContextFromAnotherScope = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<AspNetCoreStarterKitDbContext>();
            var insertedTestRole = await dbContextFromAnotherScope.Roles.FindAsync(testRole.Id);
            Assert.Null(insertedTestRole);

            unitOfWorkActionFilter.OnActionExecuted(actionExecutedContext);

            insertedTestRole = await dbContextFromAnotherScope.Roles.FindAsync(testRole.Id);
            Assert.NotNull(insertedTestRole);
        }
    }
}
