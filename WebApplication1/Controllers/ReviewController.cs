using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly TravelDBContext _context;

        public ReviewController(TravelDBContext context)
        {
            _context = context;
        }
        public IActionResult ReviewRating(int packageId)
        {
            var package = _context.Packages
                .Include(p => p.Reviews)
                .ThenInclude(r => r.User)
                .FirstOrDefault(p => p.PackageID == packageId);

            if (package == null)
            {
                return NotFound();
            }

            return View(package);
        }

        [Authorize]
        [HttpPost]
        public IActionResult SubmitReview(int packageId, int rating, string comment)
        {
            var package = _context.Packages.FirstOrDefault(p => p.PackageID == packageId);
            if (package == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);

            var existingReview = _context.Reviews.FirstOrDefault(r => r.PackageId == packageId && r.UserId == userId);

            if (existingReview != null)
            {
                existingReview.Rating = rating;
                existingReview.Comment = comment;
                existingReview.DatePosted = DateTime.Now;
            }
            else
            {
                var newReview = new Review
                {
                    PackageId = packageId,
                    UserId = userId,
                    Rating = rating,
                    Comment = comment,
                    DatePosted = DateTime.Now
                };

                _context.Reviews.Add(newReview);
            }

            _context.SaveChanges();

            return RedirectToAction("PackageDetails", new { Id = packageId });
        }


    }
}
