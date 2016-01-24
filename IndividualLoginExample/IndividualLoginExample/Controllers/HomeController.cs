using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IndividualLoginExample.Models;

namespace IndividualLoginExample.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [AllowAnonymous]
        public async Task<ActionResult> Test()
        {
            string message = String.Empty;
            try
            {
                var myAdmin = await UserManager.FindByIdAsync(2);

                var sb = new StringBuilder();

                foreach (var item in myAdmin.UserRoles)
                {
                    sb.AppendLine(item.Role.Name);
                }

                message = sb.ToString();
            }
            catch (Exception ex)
            {
                message = ex.ToString();
            }

            ViewBag.Message = message;

            return View();
        }
    }
}