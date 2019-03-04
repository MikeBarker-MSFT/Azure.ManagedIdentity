using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Azure.Identity.KeyVaultAppIdentity.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Secret()
        {
            string keyVaultEndPoint = ConfigurationManager.AppSettings["KeyVaultEndPoint"];

            KeyVaultClient.AuthenticationCallback callback = new KeyVaultClient.AuthenticationCallback(GetToken);

            KeyVaultClient keyVaultClient = new KeyVaultClient(callback);

            SecretBundle secretBundle = await keyVaultClient.GetSecretAsync(keyVaultEndPoint, "keyvault-secret").ConfigureAwait(false);

            ViewBag.Message = secretBundle.Value;

            return View();
        }

        public static async Task<string> GetToken(string authority, string resource, string scope)
        {
            var authContext = new AuthenticationContext(authority);

            ClientCredential clientCred = new ClientCredential(
                ConfigurationManager.AppSettings["ClientId"],
                ConfigurationManager.AppSettings["ClientSecret"]);

            AuthenticationResult result = await authContext.AcquireTokenAsync(resource, clientCred);

            if (result == null)
                throw new InvalidOperationException("Failed to obtain the JWT token");

            return result.AccessToken;
        }
    }
}