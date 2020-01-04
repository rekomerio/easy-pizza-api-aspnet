using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using EasyPizza.Entities;
using EasyPizza.Models.UserModels;

namespace EasyPizza.Helpers
{
    public static class ExtensionMethods
    {
        /* 
        * Read user id and usergroup from JWT
        */
        public static AuthorizationModel ReadUserDataFromJWT(this ClaimsIdentity identity)
        {
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;

                string uId = claims
                    .Where(x => x.Type == ClaimTypes.Name)
                    .FirstOrDefault().Value;

                string uGroup = claims
                    .Where(x => x.Type == ClaimTypes.Role)
                    .FirstOrDefault().Value;

                System.Diagnostics.Debug.WriteLine(uGroup);

                return new AuthorizationModel
                {
                    Id = Convert.ToInt64(uId),
                    userGroup = (UserGroup) Convert.ToInt32(uGroup)
                };
            }
            return null;
        }
        /*
        * Checks from JWT is the user id identical to id given to function
        */
        public static bool IsOwner(this AuthorizationModel auth, long id)
        {
            return auth.Id == id;
        }
        public static bool IsNotOwner(this AuthorizationModel auth, long id)
        {
            return auth.Id != id;
        }
        /*
        * Checks from JWT does the user belong into admin usergroup
        */
        public static bool IsAdmin(this AuthorizationModel auth)
        {
            return auth.userGroup == UserGroup.Admin;
        }
        public static bool IsNotAdmin(this AuthorizationModel auth)
        {
            return auth.userGroup != UserGroup.Admin;
        }
    }
}
