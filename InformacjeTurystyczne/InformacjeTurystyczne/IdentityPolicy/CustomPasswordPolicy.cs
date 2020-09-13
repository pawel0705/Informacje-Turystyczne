using InformacjeTurystyczne.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InformacjeTurystyczne.IdentityPolicy
{
    public class CustomPasswordPolicy : PasswordValidator<AppUser>
    {
        public override async Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string password)
        {
            var result = await base.ValidateAsync(manager, user, password);

            List<IdentityError> errorIdentityList = result.Succeeded ? new List<IdentityError>() : result.Errors.ToList();

            /*
             * Custom errors like:
             * if(password.ToLower().Constains(something)
             * errorIdentityList.Add(new IdentityError { Description = "Error explanation"}
             */

            return errorIdentityList.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errorIdentityList.ToArray());
        }
    }

}
