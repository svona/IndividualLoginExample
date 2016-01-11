using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using IndividualLoginExample.Models;

namespace IndividualLoginExample
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error()
        {
            var error = Server.GetLastError();

            if (error is DbEntityValidationException)
            {
                var ex = (DbEntityValidationException)error;

                foreach (DbEntityValidationResult entityValidationError in ex.EntityValidationErrors)
                {
                    foreach (DbValidationError validationError in entityValidationError.ValidationErrors)
                    {
#if DEBUG
                        System.Diagnostics.Debug.WriteLine("{0}:{1}", validationError.PropertyName, validationError.ErrorMessage);
#endif
                    }
                }
            }

        }
    }
}
