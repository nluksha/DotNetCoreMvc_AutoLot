using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DotNetCore_AutoLotDAL.Models;
using DotNetCore_AutoLotDAL.Repos;

namespace DotNetCoreMvc_AutoLotSite.Controllers
{
    public class InventoryController : Controller
    {
        private readonly IInventoryRepo repo;

        public InventoryController(IInventoryRepo repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            return View("IndexWithViewComponent");
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var inventory = repo.GetOne(id);
            if(inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Make,Color,PetName")]Inventory inventory)
        {
            if (!ModelState.IsValid)
            {
                return View(inventory);
            }

            try
            {
                repo.Add(inventory);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $@"Unable to create the record: {ex.Message}");

                return View(inventory);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var inventory = repo.GetOne(id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, [Bind("Make,Color,PetName,Id,Timestamp")] Inventory inventory)
        {
            if(id == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(inventory);
            }

            try
            {
                repo.Update(inventory);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                ModelState.AddModelError(string.Empty, $@"Unable to save the record. Another user has updated it. {ex.Message}");

                return View(inventory);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $@"Unable to save the record. {ex.Message}");
            }

            return RedirectToAction(nameof(Index));
        }

        public ActionResult Delete(int? id)
        {
            if(id == null)
            {
                return BadRequest();
            }

            var inventory = repo.GetOne(id);

            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete([Bind("Id,Timestamp")] Inventory inventory)
        {
            try
            {
                repo.Delete(inventory);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                ModelState.AddModelError(string.Empty, $@"Unable to delete the record. Another user has updated it. {ex.Message}");

                return View(inventory);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $@"Unable to delete the record. {ex.Message}");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
