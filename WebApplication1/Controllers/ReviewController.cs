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

        [HttpGet("package/{packageId}")]
        public IActionResult ReviewRating(int packageId)
        {
            var packageWithReviews = _context.Packages
                .FromSqlRaw("EXEC GetPackageReviews @p0", packageId)
                .ToList();

            if (!packageWithReviews.Any())
            {
                return NotFound(new { message = "Package not found" });
            }

            return Ok(packageWithReviews);
        }


        [HttpGet("Avgpackage/{packageId}")]
        public IActionResult AvgReviewRating(int packageId)
        {
            var package = _context.Packages
                .Include(p => p.Reviews)
                .FirstOrDefault(p => p.PackageID == packageId);

            if (package == null)
            {
                return NotFound(new { message = "Package not found" });
            }

            // Check if the package has reviews
            if (package.Reviews == null || !package.Reviews.Any())
            {
                return Ok(new { message = "No reviews available", averageRating = 0 });
            }

            // Calculate the average rating
            var avgRating = package.Reviews.Average(r => r.Rating);

            return Ok(new { packageId = packageId, averageRating = avgRating });
        }



        [Authorize]
        [HttpPost("submit")]
        public IActionResult SubmitReview(int packageId, int rating, string comment)
        {
            var userId = _userManager.GetUserId(User);

            _context.Database.ExecuteSqlRaw(
                "EXEC SubmitOrUpdateReview @p0, @p1, @p2, @p3",
                packageId, userId, rating, comment);

            return Ok(new { message = "Review submitted successfully" });
        }


    }
}
