using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class ReviewController : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE GetPackageReviews
                 @PackageID INT
             AS
             BEGIN
                 SELECT p.PackageID, p.PackageTitle, r.Id AS ReviewID, r.UserId, r.Rating, r.Comment, r.DatePosted
                 FROM Packages p
                 LEFT JOIN Reviews r ON p.PackageID = r.PackageID
                 WHERE p.PackageID = @PackageID;
             END;
            ");

                

                        migrationBuilder.Sql(@"
                CREATE PROCEDURE SubmitOrUpdateReview
                 @PackageID INT,
                 @UserID NVARCHAR(450), 
                 @Rating INT,
                 @Comment NVARCHAR(MAX)
             AS
             BEGIN
                 -- Check if the review already exists
                 IF EXISTS (SELECT 1 FROM Reviews WHERE PackageID = @PackageID AND UserID = @UserID)
                 BEGIN
                     -- Update the existing review
                     UPDATE Reviews
                     SET Rating = @Rating,
                         Comment = @Comment,
                         DatePosted = GETDATE()
                     WHERE PackageID = @PackageID AND UserID = @UserID;
                 END
                 ELSE
                 BEGIN
                     -- Insert a new review
                     INSERT INTO Reviews (PackageID, UserID, Rating, Comment, DatePosted)
                     VALUES (@PackageID, @UserID, @Rating, @Comment, GETDATE());
                 END
             END;
            ");


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetPackageReviews");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS SubmitOrUpdateReview");
        }
    }
}
