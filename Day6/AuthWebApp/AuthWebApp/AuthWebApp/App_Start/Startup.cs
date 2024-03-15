using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System.Configuration;

[assembly: OwinStartup(typeof(AuthWebApp.Startup))]

namespace AuthWebApp
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                ClientId = "f166e912-8b8e-4788-a072-e668ed810e66",
                ClientSecret = "DX-8Q~qcYXKgHaTTW4UVGPTp_jfXp2Zwc9IkTafu",
                Authority = "https://login.microsoftonline.com/f0093ae3-bfa4-46e1-9b91-668278209d56/v2.0",
                RedirectUri = "https://localhost:44380/home",
                PostLogoutRedirectUri = "http://localhost:44380",
                ResponseType= "code id_token",
                TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    NameClaimType = "name",
                    ValidateIssuer = false // Change to true if you want to validate the issuer
                },
                SignInAsAuthenticationType = CookieAuthenticationDefaults.AuthenticationType
            });
        }
    }
}
