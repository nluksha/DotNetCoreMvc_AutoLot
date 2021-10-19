using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DotNetCore_AutoLotDAL.Repos;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace DotNetCoreMvc_AutoLotSite.ViewComponents
{
    public class InventoryViewComponent: ViewComponent
    {
        private readonly IInventoryRepo repo;

        public InventoryViewComponent(IInventoryRepo repo)
        {
            this.repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cars = repo.GetAll(x => x.Make, true);

            if(cars != null)
            {
                return View("InventoryPartialView", cars);
            }

            return new ContentViewComponentResult("Unable to lacate records");
        }
    }
}
