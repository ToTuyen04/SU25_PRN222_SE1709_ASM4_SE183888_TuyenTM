using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DrugPrevention.Repositories.TuyenTM.DBContext;
using DrugPrevention.Repositories.TuyenTM.Models;
using DrugPrevention.Services.TuyenTM;

namespace DrugPrevention.MVCWebApp.TuyenTM.Controllers
{
    public class OrganizationProgramsTuyenTMsController : Controller
    {
        private readonly IServiceProviders _serviceProviders;

        public OrganizationProgramsTuyenTMsController(IServiceProviders serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }

        // GET: OrganizationProgramsTuyenTMs
        public async Task<IActionResult> Index()
        {
            //var sU25_PRN222_SE1709_G2_DrugPreventionSystemContext = _context.OrganizationProgramsTuyenTMs.Include(o => o.Organization).Include(o => o.ProgramToanNS);
            //return View(await sU25_PRN222_SE1709_G2_DrugPreventionSystemContext.ToListAsync());
            var organizationPrograms = await _serviceProviders.OrganizationProgramsTuyenTMService.GetAllAsync();

            // Load navigation properties manually
            var organizations = await _serviceProviders.OrganizationsTuyenTMService.GetAllAsync();
            var programs = await _serviceProviders.CommunityProgramToanNSService.GetAllAsync();

            // Set navigation properties
            foreach (var item in organizationPrograms)
            {
                item.Organization = organizations.FirstOrDefault(o => o.OrganizationTuyenTMID == item.OrganizationID);
                item.ProgramToanNS = programs.FirstOrDefault(p => p.ProgramToanNSID == item.ProgramToanNSID);
            }

            return View(organizationPrograms);
        }

        // GET: OrganizationProgramsTuyenTMs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var organizationProgramsTuyenTM = await _context.OrganizationProgramsTuyenTMs
            //    .Include(o => o.Organization)
            //    .Include(o => o.ProgramToanNS)
            //    .FirstOrDefaultAsync(m => m.OrganizationProgramTuyenTMID == id);
            var organizationProgramsTuyenTM = await _serviceProviders.OrganizationProgramsTuyenTMService.GetByIdAsync(id.Value);
            if (organizationProgramsTuyenTM == null)
            {
                return NotFound();
            }

            // Load navigation properties manually
            var organizations = await _serviceProviders.OrganizationsTuyenTMService.GetAllAsync();
            var programs = await _serviceProviders.CommunityProgramToanNSService.GetAllAsync();

            organizationProgramsTuyenTM.Organization = organizations.FirstOrDefault(o => o.OrganizationTuyenTMID == organizationProgramsTuyenTM.OrganizationID);
            organizationProgramsTuyenTM.ProgramToanNS = programs.FirstOrDefault(p => p.ProgramToanNSID == organizationProgramsTuyenTM.ProgramToanNSID);

            return View(organizationProgramsTuyenTM);
        }

        // GET: OrganizationProgramsTuyenTMs/Create
        public async Task<IActionResult> Create()
        {
            var organizations = await _serviceProviders.OrganizationsTuyenTMService.GetAllAsync();
            var programs = await _serviceProviders.CommunityProgramToanNSService.GetAllAsync();
            ViewData["OrganizationID"] = new SelectList(organizations, "OrganizationTuyenTMID", "OrganizationName");
            ViewData["ProgramToanNSID"] = new SelectList(programs, "ProgramToanNSID", "ProgramName");
            return View();
        }

        // POST: OrganizationProgramsTuyenTMs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrganizationProgramsTuyenTM organizationProgramsTuyenTM)
        {
            // Handle dropdown values - empty string becomes null for nullable boolean
            // Values will be: "true", "false", or "" (empty for null)

            if (ModelState.IsValid)
            {
                //_context.Add(organizationProgramsTuyenTM);
                //await _context.SaveChangesAsync();
                await _serviceProviders.OrganizationProgramsTuyenTMService.AddAsync(organizationProgramsTuyenTM);
                return RedirectToAction(nameof(Index));
            }
            var organizations = await _serviceProviders.OrganizationsTuyenTMService.GetAllAsync();
            var programs = await _serviceProviders.CommunityProgramToanNSService.GetAllAsync();
            ViewData["OrganizationID"] = new SelectList(organizations, "OrganizationTuyenTMID", "OrganizationName", organizationProgramsTuyenTM.OrganizationID);
            ViewData["ProgramToanNSID"] = new SelectList(programs, "ProgramToanNSID", "ProgramName", organizationProgramsTuyenTM.ProgramToanNSID);
            return View(organizationProgramsTuyenTM);
        }

        // GET: OrganizationProgramsTuyenTMs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var organizationProgramsTuyenTM = await _context.OrganizationProgramsTuyenTMs.FindAsync(id);
            var organizationProgramsTuyenTM = await _serviceProviders.OrganizationProgramsTuyenTMService.GetByIdAsync(id.Value);
            if (organizationProgramsTuyenTM == null)
            {
                return NotFound();
            }
            var organizations = await _serviceProviders.OrganizationsTuyenTMService.GetAllAsync();
            var programs = await _serviceProviders.CommunityProgramToanNSService.GetAllAsync();
            ViewData["OrganizationID"] = new SelectList(organizations, "OrganizationTuyenTMID", "OrganizationName", organizationProgramsTuyenTM.OrganizationID);
            ViewData["ProgramToanNSID"] = new SelectList(programs, "ProgramToanNSID", "ProgramName", organizationProgramsTuyenTM.ProgramToanNSID);
            return View(organizationProgramsTuyenTM);
        }

        // POST: OrganizationProgramsTuyenTMs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrganizationProgramsTuyenTM organizationProgramsTuyenTM)
        {
            if (id != organizationProgramsTuyenTM.OrganizationProgramTuyenTMID)
            {
                return NotFound();
            }

            // Handle dropdown values - empty string becomes null for nullable boolean
            // Values will be: "true", "false", or "" (empty for null)

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(organizationProgramsTuyenTM);
                    //await _context.SaveChangesAsync();
                    await _serviceProviders.OrganizationProgramsTuyenTMService.UpdateAsync(organizationProgramsTuyenTM);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizationProgramsTuyenTMExists(organizationProgramsTuyenTM.OrganizationProgramTuyenTMID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            var organizations = await _serviceProviders.OrganizationsTuyenTMService.GetAllAsync();
            var programs = await _serviceProviders.CommunityProgramToanNSService.GetAllAsync();
            ViewData["OrganizationID"] = new SelectList(organizations, "OrganizationTuyenTMID", "OrganizationName", organizationProgramsTuyenTM.OrganizationID);
            ViewData["ProgramToanNSID"] = new SelectList(programs, "ProgramToanNSID", "ProgramName", organizationProgramsTuyenTM.ProgramToanNSID);
            return View(organizationProgramsTuyenTM);
        }

        // GET: OrganizationProgramsTuyenTMs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var organizationProgramsTuyenTM = await _context.OrganizationProgramsTuyenTMs
            //    .Include(o => o.Organization)
            //    .Include(o => o.ProgramToanNS)
            //    .FirstOrDefaultAsync(m => m.OrganizationProgramTuyenTMID == id);
            var organizationProgramsTuyenTM = await _serviceProviders.OrganizationProgramsTuyenTMService.GetByIdAsync(id.Value);
            if (organizationProgramsTuyenTM == null)
            {
                return NotFound();
            }

            // Load navigation properties manually
            var organizations = await _serviceProviders.OrganizationsTuyenTMService.GetAllAsync();
            var programs = await _serviceProviders.CommunityProgramToanNSService.GetAllAsync();

            organizationProgramsTuyenTM.Organization = organizations.FirstOrDefault(o => o.OrganizationTuyenTMID == organizationProgramsTuyenTM.OrganizationID);
            organizationProgramsTuyenTM.ProgramToanNS = programs.FirstOrDefault(p => p.ProgramToanNSID == organizationProgramsTuyenTM.ProgramToanNSID);

            return View(organizationProgramsTuyenTM);
        }

        // POST: OrganizationProgramsTuyenTMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var organizationProgramsTuyenTM = await _context.OrganizationProgramsTuyenTMs.FindAsync(id);
            var organizationProgramsTuyenTM = await _serviceProviders.OrganizationProgramsTuyenTMService.GetByIdAsync(id);
            if (organizationProgramsTuyenTM != null)
            {
                //_context.OrganizationProgramsTuyenTMs.Remove(organizationProgramsTuyenTM);
                await _serviceProviders.OrganizationProgramsTuyenTMService.DeleteAsync(organizationProgramsTuyenTM.OrganizationProgramTuyenTMID);
            }

            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrganizationProgramsTuyenTMExists(int id)
        {
            //return _context.OrganizationProgramsTuyenTMs.Any(e => e.OrganizationProgramTuyenTMID == id);
            return _serviceProviders.OrganizationProgramsTuyenTMService.Exists(id);
        }
    }
}
