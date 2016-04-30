using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IndividualLoginExample.Models
{
    /// <summary>
    /// Model Binder for DTParameterModel (DataTables)
    /// Binds the new Datatables 1.0 server parameters into enumerable classes and properties for easier retrieval
    /// Implemented from Stack Overflow 
    /// http://stackoverflow.com/a/28223358
    /// </summary>
    public class DataTablesParamsModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            base.BindModel(controllerContext, bindingContext);
            var request = controllerContext.HttpContext.Request;

            // Retrieve request data
            var draw = Convert.ToInt32(request["draw"]);
            var start = Convert.ToInt32(request["start"]);
            var length = Convert.ToInt32(request["length"]);

            // Search
            var search = new DTSearch
            {
                Value = request["search[value]"],
                Regex = Convert.ToBoolean(request["search[regex]"])
            };

            // Order
            var o = 0;
            var order = new List<DTOrder>();

            while (request["order[" + o + "][column]"] != null)
            {
                order.Add(new DTOrder
                {
                    Column = Convert.ToInt32(request["order[" + o + "][column]"]),
                    Dir = request["order[" + o + "][dir]"]
                });

                o++;
            }

            // Columns
            var c = 0;
            var columns = new List<DTColumn>();

            while (request["columns[" + c + "][name]"] != null)
            {
                columns.Add(new DTColumn
                {
                    Data = request["columns[" + c + "][data]"],
                    Name = request["columns[" + c + "][name]"],
                    Orderable = Convert.ToBoolean(request["columns[" + c + "][orderable]"]),
                    Searchable = Convert.ToBoolean(request["columns[" + c + "][searchable]"]),
                    Search = new DTSearch
                    {
                        Value = request["columns[" + c + "][search][value]"],
                        Regex = Convert.ToBoolean(request["columns[" + c + "][search][regex]"])
                    }
                });

                c++;
            }

            NewCustomDatatablesParamModel model = null;

            try
            {
                // BIND CUSTOM PARAMETERS TO THE PARAM MODEL
                bool? isOpen = request["IsOpen"] == null ? null as bool? : Convert.ToBoolean(request["IsOpen"]);

                string name = request["Name"];
                string nameContains = request["NameContains"];

                bool? reviewed = String.IsNullOrWhiteSpace(request["Reviewed"]) || String.Equals(request["Reviewed"], "null", StringComparison.OrdinalIgnoreCase) ? null as bool? : Convert.ToBoolean(request["Reviewed"]);

                model = new NewCustomDatatablesParamModel
                {
                    Draw = draw,
                    Start = start,
                    Length = length,
                    Search = search,
                    Order = order,
                    Columns = columns,

                    Name = name,
                    NameContains = nameContains
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return model;
        }
    }
}