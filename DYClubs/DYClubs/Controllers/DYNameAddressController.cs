/*
 * Author: Dong Yan
 * section:PROG2230 sec 04
 * student No.: 5944970
 * email: dyan4970@conestogac.on.ca
 * Purpose: DYCountryController
 * Revision History: on November 14,2020 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DYClubs.Models;
using System.Data.Entity.Validation;

namespace DYClubs.Controllers
{
    public class DYNameAddressController : Controller
    {
        private readonly ClubsContext _context;

        public DYNameAddressController(ClubsContext context)
        {
            _context = context;
        }

        // GET: DYNameAddress
        public async Task<IActionResult> Index()
        {
            var nameAddresses = _context.NameAddress.Include(n => n.ProvinceCodeNavigation);
            return View(await nameAddresses.ToListAsync());
        }

        // GET: DYNameAddress/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nameAddress = await _context.NameAddress
                .Include(n => n.ProvinceCodeNavigation)
                .FirstOrDefaultAsync(m => m.NameAddressId == id);
            if (nameAddress == null)
            {
                return NotFound();
            }

            return View(nameAddress);
        }

        // GET: DYNameAddress/Create
        public IActionResult Create()
        {
            ViewData["ProvinceCode"] = new SelectList(_context.Province, "ProvinceCode", "ProvinceCode");
            return View();
        }

        // POST: DYNameAddress/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NameAddressId,FirstName,LastName,CompanyName,StreetAddress,City,PostalCode,ProvinceCode,Email,Phone")] NameAddress nameAddress)
        {
            try
            {
                if (ModelState.IsValid)
                {              
                     _context.Add(nameAddress);
                    await _context.SaveChangesAsync();
                    TempData["message"] = "Name & Address created successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", $"error inserting new name: { ex.GetBaseException().Message}");
            }
            //ViewData["ProvinceCode"] = new SelectList(_context.Province, "ProvinceCode", "ProvinceCode", nameAddress.ProvinceCode);
            return View(nameAddress);
        }

        // GET: DYNameAddress/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nameAddress = await _context.NameAddress.FindAsync(id);
            if (nameAddress == null)
            {
                return NotFound();
            }
            ViewData["ProvinceCode"] = new SelectList(_context.Province.OrderBy(p => p.Name), "ProvinceCode", "Name", nameAddress.ProvinceCodeNavigation);
            return View(nameAddress);
        }

        // POST: DYNameAddress/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NameAddressId,FirstName,LastName,CompanyName,StreetAddress,City,PostalCode,ProvinceCode,Email,Phone")] NameAddress nameAddress)
        {
            if (id != nameAddress.NameAddressId)
            {
                return NotFound();
            }
            

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(nameAddress);
                        await _context.SaveChangesAsync();
                        TempData["message"] = "Name & Address updated successfully";
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        if (!NameAddressExists(nameAddress.NameAddressId))
                        {
                            ModelState.AddModelError("", $"nameAddressId is not on file{nameAddress.NameAddressId}");
                         }
                        else
                        {
                        ModelState.AddModelError("", $"concurrency exception: { ex.GetBaseException().Message}");
                        }
                    }catch (Exception ex)
                    {
                    ModelState.AddModelError("", $"error inserting new name: { ex.GetBaseException().Message}");
                }
                }

                ViewData["ProvinceCode"] = new SelectList(_context.Province.OrderBy(p => p.Name), "ProvinceCode", "Name", nameAddress.ProvinceCodeNavigation);
                return View(nameAddress);     
        }

        // GET: DYNameAddress/Delete/5
            public async Task<IActionResult> Delete(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var nameAddress = await _context.NameAddress
                    .Include(n => n.ProvinceCodeNavigation)
                    .FirstOrDefaultAsync(m => m.NameAddressId == id);
                if (nameAddress == null)
                {
                    return NotFound();
                }

                return View(nameAddress);
            }

            // POST: DYNameAddress/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
            try
            {
                var nameAddress = await _context.NameAddress.FindAsync(id);
                _context.NameAddress.Remove(nameAddress);
                await _context.SaveChangesAsync();
                TempData["message"] = "Name & Address deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                TempData["message"] = "error on delete" + ex.GetBaseException().Message;
            }
            return RedirectToAction("Delete", new { ID = id });
        }

            private bool NameAddressExists(int id)
            {
                return _context.NameAddress.Any(e => e.NameAddressId == id);
            }
        }
    }
