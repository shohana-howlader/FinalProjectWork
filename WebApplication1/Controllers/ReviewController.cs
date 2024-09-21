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
        private readonly UserManager<ApplicationUser> _userManager; // Assuming you're using ASP.NET Identity

        public ReviewController(TravelDBContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/review/package/5
        [HttpGet("package/{packageId}")]
        public IActionResult ReviewRating(int packageId)
        {
            var package = _context.Packages
                .Include(p => p.Reviews)
                .ThenInclude(r => r.User)
                .FirstOrDefault(p => p.PackageID == packageId);

            if (package == null)
            {
                return NotFound(new { message = "Package not found" });
            }

            return Ok(package);
        }

        // POST: api/review/submit
        [Authorize]
        [HttpPost("submit")]
        public IActionResult SubmitReview(int packageId, int rating, string comment)
        {
            var package = _context.Packages.FirstOrDefault(p => p.PackageID == packageId);
            if (package == null)
            {
                return NotFound(new { message = "Package not found" });
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

            return Ok(new { message = "Review submitted successfully" });
        }


    }
}
