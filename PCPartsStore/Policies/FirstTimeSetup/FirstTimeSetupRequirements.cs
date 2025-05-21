using Microsoft.AspNetCore.Authorization;

namespace PCPartsStore.Policies.FirstTimeSetup;

public class FirstTimeSetupRequirements : IAuthorizationRequirement
{
    public int UserCount { get; }

    public FirstTimeSetupRequirements(int userCount)
    {
        this.UserCount = userCount;
    }
}