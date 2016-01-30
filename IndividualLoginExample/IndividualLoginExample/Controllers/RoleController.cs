using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IndividualLoginExample.Models;

namespace IndividualLoginExample.Controllers
{
    [AllowAnonymous]
    public class RoleController : BaseController
    {
        #region Action Results
        // GET: Role
        public ActionResult Index()
        {
            return View(this.RoleManager.Roles.ToList());
        }
        #endregion

        #region Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Role model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await this.RoleManager.CreateAsync(model);

            if (!result.Succeeded)
            {
                AddErrors(result);
                return View(model);
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Edit
        public async Task<ActionResult> Edit(int roleId)
        {
            var model = await this.RoleManager.FindByIdAsync(roleId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Role model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await this.RoleManager.UpdateAsync(model);

            if (!result.Succeeded)
            {
                AddErrors(result);
                return View(model);
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        public async Task<ActionResult> Delete(int roleId)
        {
            var model = await this.RoleManager.FindByIdAsync(roleId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Role model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await RoleManager.DeleteAsync(model);

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