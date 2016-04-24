using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DevTrends.MvcDonutCaching;
using IndividualLoginExample.Models;
using Microsoft.AspNet.Identity;

namespace IndividualLoginExample.Controllers
{
    [AllowAnonymous]
    public class RoleController : BaseController
    {
        #region Action Results
        // GET: Role
        public ActionResult Index()
        {
            var model = Enumerable.Range(0, 100)
                .Select(b => new Role { Name = Guid.NewGuid().ToString().Replace("-", String.Empty) })
                .ToList();

            return View(model);

            // return View(this.RoleManager.Roles.ToList());
        }

        [DonutOutputCache(Duration = 1)]
        public JsonResult RoleDataTableHandler(NewCustomDatatablesParamModel param)
        {
            // http://www.datatables.net/forums/discussion/21518/migrate-to-datatables-1-10-fnserverdata

            int totalRecords = 0;

            var model = db.GetRoles(ref totalRecords, rowsToSkip: param.Start, rowsToTake: param.Length).ToList();

            var count = 10;

            var temp = model.FirstOrDefault();

            if (temp != null)
            {
                count = totalRecords;
            }

            var result = new
            {
                draw = param.Draw,
                recordsTotal = count,
                recordsFiltered = count,
                data = model
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Role model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = this.RoleManager.Create(model);

            if (!result.Succeeded)
            {
                AddErrors(result);
                return View(model);
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Details
        public ActionResult Details(int roleId)
        {
            var model = this.RoleManager.FindById(roleId);
            return View(model);
        }
        #endregion

        #region Edit
        public ActionResult Edit(int roleId)
        {
            var model = this.RoleManager.FindById(roleId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Role model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = this.RoleManager.Update(model);

            if (!result.Succeeded)
            {
                AddErrors(result);
                return View(model);
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        public ActionResult Delete(int roleId)
        {
            var model = this.RoleManager.FindById(roleId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Role model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = RoleManager.Delete(model);

            if (!result.Succeeded)
            {
                AddErrors(result);
                return View(model);
            }

            return RedirectToAction("Index");
        }
        #endregion
    }
}