using Microsoft.AspNetCore.Authorization;

namespace BarshaiedAPI.Authorization
{
    public class UserOwnerOrAdminRequirement :IAuthorizationRequirement
    {
    }
}
