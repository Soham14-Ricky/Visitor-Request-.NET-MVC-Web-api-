using Microsoft.AspNetCore.Mvc;

using VisitorMVC.Filters;

using VisitorMVC.Services.Interfaces;
using VisitorMVC.Models.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace VisitorMVC.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IVisitorRequestService _visitorRequestService;
        public AdminController(IVisitorRequestService visitorRequestService)
        {
            _visitorRequestService = visitorRequestService;
        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Index()
        {
            var requests = await _visitorRequestService.GetPendingRequestsAsync();
            return View(requests);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve(int id)
        {
            var result = await _visitorRequestService.ApproveRequestAsync(id);

            if (result)
            {
                TempData["Success"] ="Request approved successfully";
            }
            else
            {
                TempData["Error"] ="Unable to approve request";
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Reject(RejectRequestDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Remarks))
            {
                TempData["Error"] =
                    "Remarks are required";

                return RedirectToAction("Index");
            }

            var result =
                await _visitorRequestService
                .RejectRequestAsync(dto);

            if (result)
            {
                TempData["Success"] =
                    "Request rejected successfully";
            }
            

            return RedirectToAction("Index");
        }
    }
}