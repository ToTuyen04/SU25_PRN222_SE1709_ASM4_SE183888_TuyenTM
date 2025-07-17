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
using Microsoft.AspNetCore.Authorization;
using DrugPrevention.MVCWebApp.TuyenTM.Helpers;

namespace DrugPrevention.MVCWebApp.TuyenTM.Controllers
{
    [Authorize]
    public class OrganizationProgramsTuyenTMsController : Controller
    {
        private readonly IServiceProviders _serviceProviders;

        public OrganizationProgramsTuyenTMsController(IServiceProviders serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            // Check if user has role = 3, if so, deny access
            var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            if (userRole == "3")
            {
                return RedirectToAction("Forbidden", "Account");
            }

            var organizationPrograms = await _serviceProviders.OrganizationProgramsTuyenTMService.GetAllAsync();

            int pageSize = 10;
            return View(PaginatedList<OrganizationProgramsTuyenTM>.Create(organizationPrograms, pageNumber ?? 1, pageSize));
        }
        public async Task<IActionResult> Details(int? id)
        {
            // Check if user has role = 3, if so, deny access
            var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            if (userRole == "3")
            {
                return RedirectToAction("Forbidden", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }
            var organizationProgramsTuyenTM = await _serviceProviders.OrganizationProgramsTuyenTMService.GetByIdAsync(id.Value);
            if (organizationProgramsTuyenTM == null)
            {
                return NotFound();
            }

            var organizations = await _serviceProviders.OrganizationsTuyenTMService.GetAllAsync();
            var programs = await _serviceProviders.CommunityProgramToanNSService.GetAllAsync();

            organizationProgramsTuyenTM.Organization = organizations.FirstOrDefault(o => o.OrganizationTuyenTMID == organizationProgramsTuyenTM.OrganizationID);
            organizationProgramsTuyenTM.ProgramToanNS = programs.FirstOrDefault(p => p.ProgramToanNSID == organizationProgramsTuyenTM.ProgramToanNSID);

            return View(organizationProgramsTuyenTM);
        }

        public async Task<IActionResult> Create()
        {
            // Check if user has role = 3, if so, deny access
            var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            if (userRole == "3")
            {
                return RedirectToAction("Forbidden", "Account");
            }

            var organizations = await _serviceProviders.OrganizationsTuyenTMService.GetAllAsync();
            var programs = await _serviceProviders.CommunityProgramToanNSService.GetAllAsync();
            ViewData["OrganizationID"] = new SelectList(organizations, "OrganizationTuyenTMID", "OrganizationName");
            ViewData["ProgramToanNSID"] = new SelectList(programs, "ProgramToanNSID", "ProgramName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrganizationProgramsTuyenTM organizationProgramsTuyenTM)
        {
            // Check if user has role = 3, if so, deny access
            var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            if (userRole == "3")
            {
                return RedirectToAction("Forbidden", "Account");
            }

            organizationProgramsTuyenTM.JoinedDate = DateTime.Now;

            if (organizationProgramsTuyenTM.LastUpdated.HasValue &&
                (organizationProgramsTuyenTM.LastUpdated.Value == DateTime.MinValue || organizationProgramsTuyenTM.LastUpdated.Value < new DateTime(1753, 1, 1)))
                organizationProgramsTuyenTM.LastUpdated = null;

            if (ModelState.IsValid)
            {
                await _serviceProviders.OrganizationProgramsTuyenTMService.AddAsync(organizationProgramsTuyenTM);
                return RedirectToAction(nameof(Index));
            }
            var organizations = await _serviceProviders.OrganizationsTuyenTMService.GetAllAsync();
            var programs = await _serviceProviders.CommunityProgramToanNSService.GetAllAsync();
            ViewData["OrganizationID"] = new SelectList(organizations, "OrganizationTuyenTMID", "OrganizationName", organizationProgramsTuyenTM.OrganizationID);
            ViewData["ProgramToanNSID"] = new SelectList(programs, "ProgramToanNSID", "ProgramName", organizationProgramsTuyenTM.ProgramToanNSID);
            return View(organizationProgramsTuyenTM);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            // Check if user has role = 3, if so, deny access
            var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            if (userRole == "3")
            {
                return RedirectToAction("Forbidden", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrganizationProgramsTuyenTM organizationProgramsTuyenTM)
        {
            // Check if user has role = 3, if so, deny access
            var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            if (userRole == "3")
            {
                return RedirectToAction("Forbidden", "Account");
            }

            if (id != organizationProgramsTuyenTM.OrganizationProgramTuyenTMID)
            {
                return NotFound();
            }

            if (organizationProgramsTuyenTM.JoinedDate == DateTime.MinValue || organizationProgramsTuyenTM.JoinedDate < new DateTime(1753, 1, 1))
                organizationProgramsTuyenTM.JoinedDate = DateTime.Now;

            if (!organizationProgramsTuyenTM.LastUpdated.HasValue ||
                organizationProgramsTuyenTM.LastUpdated.Value == DateTime.MinValue ||
                organizationProgramsTuyenTM.LastUpdated.Value < new DateTime(1753, 1, 1))
            {
                organizationProgramsTuyenTM.LastUpdated = DateTime.Now;
            }

            if (ModelState.IsValid)
            {
                try
                {
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

        public async Task<IActionResult> Delete(int? id)
        {
            // Check if user has role = 2, if so, deny access
            var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            if (userRole == "2")
            {
                return RedirectToAction("Forbidden", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var organizationProgramsTuyenTM = await _serviceProviders.OrganizationProgramsTuyenTMService.GetByIdAsync(id.Value);
            if (organizationProgramsTuyenTM == null)
            {
                return NotFound();
            }

            var organizations = await _serviceProviders.OrganizationsTuyenTMService.GetAllAsync();
            var programs = await _serviceProviders.CommunityProgramToanNSService.GetAllAsync();

            organizationProgramsTuyenTM.Organization = organizations.FirstOrDefault(o => o.OrganizationTuyenTMID == organizationProgramsTuyenTM.OrganizationID);
            organizationProgramsTuyenTM.ProgramToanNS = programs.FirstOrDefault(p => p.ProgramToanNSID == organizationProgramsTuyenTM.ProgramToanNSID);

            return View(organizationProgramsTuyenTM);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Check if user has role = 2, if so, deny access
            var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            if (userRole == "2")
            {
                return RedirectToAction("Forbidden", "Account");
            }

            var organizationProgramsTuyenTM = await _serviceProviders.OrganizationProgramsTuyenTMService.GetByIdAsync(id);
            if (organizationProgramsTuyenTM != null)
            {
                await _serviceProviders.OrganizationProgramsTuyenTMService.DeleteAsync(organizationProgramsTuyenTM.OrganizationProgramTuyenTMID);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Search(int? pageNumber, int id = 0, string organizationName = "", string programName = "")
        {
            // Check if user has role = 3, if so, deny access
            var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            if (userRole == "3")
            {
                return RedirectToAction("Forbidden", "Account");
            }

            var organizationPrograms = await _serviceProviders.OrganizationProgramsTuyenTMService.SearchAsync(id, organizationName, programName);

            int pageSize = 10;
            ViewData["id"] = id;
            ViewData["organizationName"] = organizationName;
            ViewData["programName"] = programName;

            return View("Index", PaginatedList<OrganizationProgramsTuyenTM>.Create(organizationPrograms, pageNumber ?? 1, pageSize));
        }

        private bool OrganizationProgramsTuyenTMExists(int id)
        {
            return _serviceProviders.OrganizationProgramsTuyenTMService.Exists(id);
        }
    }
}
