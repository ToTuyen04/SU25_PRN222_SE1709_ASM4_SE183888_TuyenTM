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
        private readonly SU25_PRN222_SE1709_G2_DrugPreventionSystemContext _context;
        private readonly IServiceProviders _serviceProviders;

        public OrganizationProgramsTuyenTMsController(ServiceProviders serviceProvider) => _serviceProviders = serviceProvider;

        // GET: OrganizationProgramsTuyenTMs
        public async Task<IActionResult> Index()
        {
            //var sU25_PRN222_SE1709_G2_DrugPreventionSystemContext = _context.OrganizationProgramsTuyenTMs.Include(o => o.Organization).Include(o => o.ProgramToanNS);
            //return View(await sU25_PRN222_SE1709_G2_DrugPreventionSystemContext.ToListAsync());
            var organizationPrograms = await _serviceProviders.OrganizationProgramsTuyenTMService.GetAllAsync();
            return (organizationPrograms != null) ? Ok(organizationPrograms) : NotFound(); 
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

            return View(organizationProgramsTuyenTM);
        }

        // GET: OrganizationProgramsTuyenTMs/Create
        public IActionResult Create()
        {
            ViewData["OrganizationID"] = new SelectList(_context.OrganizationsTuyenTMs, "OrganizationTuyenTMID", "OrganizationName");
            ViewData["ProgramToanNSID"] = new SelectList(_context.CommunityProgramsToanNs, "ProgramToanNSID", "ProgramName");
            return View();
        }

        // POST: OrganizationProgramsTuyenTMs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrganizationProgramTuyenTMID,OrganizationID,ProgramToanNSID,JoinedDate,ContributionDescription,IsSponsor,IsOrganizer,ProgramRole,FundingAmount,Feedback,LastUpdated")] OrganizationProgramsTuyenTM organizationProgramsTuyenTM)
        {
            if (ModelState.IsValid)
            {
                //_context.Add(organizationProgramsTuyenTM);
                //await _context.SaveChangesAsync();
                await _serviceProviders.OrganizationProgramsTuyenTMService.AddAsync(organizationProgramsTuyenTM);
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrganizationID"] = new SelectList(_context.OrganizationsTuyenTMs, "OrganizationTuyenTMID", "OrganizationName", organizationProgramsTuyenTM.OrganizationID);
            ViewData["ProgramToanNSID"] = new SelectList(_context.CommunityProgramsToanNs, "ProgramToanNSID", "ProgramName", organizationProgramsTuyenTM.ProgramToanNSID);
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
            ViewData["OrganizationID"] = new SelectList(_context.OrganizationsTuyenTMs, "OrganizationTuyenTMID", "OrganizationName", organizationProgramsTuyenTM.OrganizationID);
            ViewData["ProgramToanNSID"] = new SelectList(_context.CommunityProgramsToanNs, "ProgramToanNSID", "ProgramName", organizationProgramsTuyenTM.ProgramToanNSID);
            return View(organizationProgramsTuyenTM);
        }

        // POST: OrganizationProgramsTuyenTMs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrganizationProgramTuyenTMID,OrganizationID,ProgramToanNSID,JoinedDate,ContributionDescription,IsSponsor,IsOrganizer,ProgramRole,FundingAmount,Feedback,LastUpdated")] OrganizationProgramsTuyenTM organizationProgramsTuyenTM)
        {
            if (id != organizationProgramsTuyenTM.OrganizationProgramTuyenTMID)
            {
                return NotFound();
            }

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
            ViewData["OrganizationID"] = new SelectList(_context.OrganizationsTuyenTMs, "OrganizationTuyenTMID", "OrganizationName", organizationProgramsTuyenTM.OrganizationID);
            ViewData["ProgramToanNSID"] = new SelectList(_context.CommunityProgramsToanNs, "ProgramToanNSID", "ProgramName", organizationProgramsTuyenTM.ProgramToanNSID);
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
