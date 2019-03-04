using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Azure.Identity.KeyVaultMSI.Controllers
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

            AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();

            KeyVaultClient.AuthenticationCallback callback = new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback);

            KeyVaultClient keyVaultClient = new KeyVaultClient(callback);

            SecretBundle secretBundle = await keyVaultClient.GetSecretAsync(keyVaultEndPoint, "keyvault-secret").ConfigureAwait(false);

            ViewBag.Message = secretBundle.Value;

            ViewBag.Principal = azureServiceTokenProvider.PrincipalUsed;

            return View();
        }
    }
}