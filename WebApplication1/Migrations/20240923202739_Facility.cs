using Humanizer;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class Facility : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE GetFacilities
                AS
                BEGIN
                    SELECT FacilityID, FacilityName, Description, IsAvailable
                    FROM Facilities;
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE GetFacilityById
                @FacilityID INT
            AS
            BEGIN
                SELECT FacilityID, FacilityName, Description, IsAvailable
                FROM Facilities
                WHERE FacilityID = @FacilityID;
            END;

                    ");

            migrationBuilder.Sql(@"
              CREATE PROCEDURE CreateFacility
            @FacilityName NVARCHAR(100),
            @Description NVARCHAR(500),
            @IsAvailable BIT
        AS
        BEGIN
            INSERT INTO Facilities (FacilityName, Description, IsAvailable)
            VALUES (@FacilityName, @Description, @IsAvailable);
    
            -- Optionally return the ID of the new facility
            SELECT SCOPE_IDENTITY() AS NewFacilityID;
        END;
                    ");

            migrationBuilder.Sql(@"CREATE PROCEDURE DeleteFacility
                @FacilityID INT
            AS
            BEGIN
                DELETE FROM Facilities
                WHERE FacilityID = @FacilityID;
                        END;");

            migrationBuilder.Sql(@"CREATE PROCEDURE UpdateFacility
                @FacilityID INT,
                @FacilityName NVARCHAR(100),
                @Description NVARCHAR(500),
                @IsAvailable BIT
            AS
            BEGIN
                UPDATE Facilities
                SET FacilityName = @FacilityName,
                    Description = @Description,
                    IsAvailable = @IsAvailable
                WHERE FacilityID = @FacilityID;
            END;
            ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetFacilities");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetFacilityById");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS CreateFacility");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS DeleteFacility");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS UpdateFacility");

        }
    }
}
