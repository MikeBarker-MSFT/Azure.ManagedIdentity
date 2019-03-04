using Microsoft.Azure.Services.AppAuthentication;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Azure.Identity.SqlMSI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Select()
        {
            AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
            var accessToken = await azureServiceTokenProvider.GetAccessTokenAsync("https://database.windows.net/")
                .ConfigureAwait(false);

            int result;

            using (var connection = new SqlConnection())
            {
                ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings["mi-sqldb"];
                connection.ConnectionString = connectionStringSettings.ConnectionString;

                connection.AccessToken = accessToken;

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT COUNT(*) FROM sys.objects WHERE type = 'U'";
                    command.CommandType = CommandType.Text;

                    connection.Open();

                    object objResult = command.ExecuteScalar();

                    result = (int)objResult;
                }
            }

            ViewBag.Message = $"There are {result} user tables in the database.";

            return View();
        }

        public async Task<ActionResult> Insert()
        {
            int someNumber = (new Random()).Next(int.MinValue, int.MaxValue);

            AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
            var accessToken = await azureServiceTokenProvider.GetAccessTokenAsync("https://database.windows.net/")
                .ConfigureAwait(false);

            int rowsAffected;

            using (var connection = new SqlConnection())
            {
                ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings["mi-sqldb"];
                connection.ConnectionString = connectionStringSettings.ConnectionString;

                connection.AccessToken = accessToken;

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"INSERT INTO SomeTable VALUES ({someNumber});";
                    command.CommandType = CommandType.Text;

                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
            }

            ViewBag.Message = $"There were {rowsAffected}.";

            return View();
        }
    }
}