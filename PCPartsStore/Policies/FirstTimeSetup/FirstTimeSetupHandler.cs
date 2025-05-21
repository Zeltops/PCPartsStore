using Microsoft.AspNetCore.Authorization;
using PCPartsStore.Data;

namespace PCPartsStore.Policies.FirstTimeSetup;

public class FirstTimeSetupHandler : AuthorizationHandler<FirstTimeSetupRequirements>
{
    private readonly ApplicationDbContext _dbContext;

    public FirstTimeSetupHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        FirstTimeSetupRequirements requirement)
    {
        int userCount = _dbContext.Users.Count();

        if (userCount < requirement.UserCount)
        {
            context.Fail();
        }
        else context.Succeed(requirement);

        return Task.CompletedTask;
    }
}