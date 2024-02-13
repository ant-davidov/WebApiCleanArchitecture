using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC.Contracts;
using MVC.Models;
using MVC.Services;
using MVC.Services.Base;

namespace MVC.Controllers
{
    public class LeaveTypesController : Controller
    {
        private readonly ILeaveTypeService _leaveTypeService;
        private readonly ILeaveAllocationService _leaveAllocationService;

        public LeaveTypesController(ILeaveTypeService leaveTypeService, ILeaveAllocationService leaveAllocationService)
        {
            _leaveTypeService = leaveTypeService;
            _leaveAllocationService = leaveAllocationService;
        }
       
        public async Task<ActionResult> Index()
        {
            var model = await _leaveTypeService.GetLeaveTypes();
            return View(model);
        }

     
        public async Task<ActionResult> Details(int id)
        {
            var model = await _leaveTypeService.GetLeaveTypeDetails(id);
            return View(model);
        }

       
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateLeaveTypeVM createLeaveType)
        {
            try
            {
                var response = await _leaveTypeService.CreateLeaveType(createLeaveType);
                if (response.Success)
                    return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", response.ValidationErrors);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message); // delete this
            }

            return View(createLeaveType);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var model = await _leaveTypeService.GetLeaveTypeDetails(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, LeaveTypeVM leaveType)
        {
            try
            {
                var response = await _leaveTypeService.UpdateLeaveType(id, leaveType);
                if (response.Success)
                    return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", response.ValidationErrors);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(leaveType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var response = await _leaveTypeService.DeleteLeaveType(id);
                if (response.Success)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", response.ValidationErrors);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Allocate(int id)
        {
            try
            {
                var response = await _leaveAllocationService.CreateLeaveAllocations(id);
                if (response.Success)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return BadRequest();
        }
    }
}
