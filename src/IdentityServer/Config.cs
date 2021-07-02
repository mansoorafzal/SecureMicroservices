using Common;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public class Config
    {
        public static IEnumerable<Client> Clients =>
            new Client[]
            {   
                new Client
                {
                    ClientId = Constant.Movies_Client_Id_Value,
                    ClientName = Constant.Movies_Client_Name,
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RequirePkce = false,
                    AllowRememberConsent = false,
                    RedirectUris = new List<string>() { Url.Sign_In },
                    PostLogoutRedirectUris = new List<string>() { Url.Sign_Out },
                    ClientSecrets = new List<Secret>
                    {
                        new Secret(Constant.Movies_Client_Secret.Sha256())
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Email,
                        Constant.Scope_Role_Value,
                        Constant.Scope_Movie_Api_Value
                    }
                }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope(Constant.Scope_Movie_Api_Value, Constant.Scope_Movie_Api_Text)
            };

        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                new IdentityResources.Email(),
                new IdentityResource(Constant.Scope_Role_Value, Constant.Scope_Role_Text, new List<string>() { Constant.Scope_Role_Value })
            };
    }
}
