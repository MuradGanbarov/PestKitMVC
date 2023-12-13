using PestKit.Utilites.Enums;

namespace PestKit.Areas.PestKitAdmin.Models.Utilites.Extensions
{
    public class AuthorizeRolesAttribute : Microsoft.AspNetCore.Authorization.AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params UserRoles[] allowedroles)
        {
            var allowedrolesAsString = allowedroles.Select(x => Enum.GetName(typeof(UserRoles), x));
            Roles = string.Join(",", allowedrolesAsString);
        }
    }
}
