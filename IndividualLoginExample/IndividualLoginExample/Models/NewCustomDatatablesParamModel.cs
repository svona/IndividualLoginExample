using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IndividualLoginExample.Models
{
    [ModelBinder(typeof(DataTablesParamsModelBinder))]
    public class NewCustomDatatablesParamModel : NewDatatablesParamModel
    {
        public string Name { get; set; }

        public string NameContains { get; set; }
    }
}