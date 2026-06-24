using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisitorMVC.Filters;
using VisitorMVC.Models.DTOs;
using VisitorMVC.Services.Interfaces;

namespace VisitorMVC.Controllers
{
    [Authorize]
    public class VisitorRequestController: Controller
    {
        private readonly IVisitorRequestService _visitorRequestService;

        public VisitorRequestController(IVisitorRequestService visitorRequestService)
        {
            _visitorRequestService = visitorRequestService;
        }

        [HttpGet]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Index()
        {
            var requests = await _visitorRequestService.GetMyRequestsAsync();
            return View(requests);
        }

        [HttpGet]
        [Authorize(Roles = "Employee")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Create(VisitorRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var result =
                await _visitorRequestService
                .CreateRequestAsync(dto);

            if (result)
            {
                TempData["Success"] ="Request created successfully";

                return RedirectToAction("Index");
            }

            ViewBag.Error ="Unable to create request";

            return View(dto);
        }




        [HttpGet]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Edit(int id)
        {
            var request =
                await _visitorRequestService
                .GetRequestByIdAsync(id);

            if (request == null)
            {
                return NotFound();
            }

            if (request.Status != "Pending")
            {
                TempData["Error"] =
                    "Only pending requests can be edited";

                return RedirectToAction(
                    "Index");
            }

            return View(request);
        }



        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Edit(VisitorRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var result =
                await _visitorRequestService
                .UpdateRequestAsync(dto);

            if (result)
            {
                TempData["Success"] =
                    "Request updated successfully";

                return RedirectToAction(
                    "Index");
            }

            ViewBag.Error =
                "Unable to update request";

            return View(dto);
        }



        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Delete(int id)
        {
            var request =
                await _visitorRequestService
                .GetRequestByIdAsync(id);

            if (request == null)
            {
                return NotFound();
            }

            if (request.Status != "Pending")
            {
                TempData["Error"] =
                    "Only pending requests can be deleted";

                return RedirectToAction(
                    "Index");
            }

            var result =
                await _visitorRequestService
                .DeleteRequestAsync(id);

            if (result)
            {
                TempData["Success"] =
                    "Request deleted successfully";
            }
            else
            {
                TempData["Error"] =
                    "Unable to delete request";
            }

            return RedirectToAction("Index");
        }
    }
}