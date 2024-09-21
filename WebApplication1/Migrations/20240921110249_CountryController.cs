using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class CountryController : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
                        migrationBuilder.Sql(@"
                CREATE PROCEDURE GetCountries
                AS
                BEGIN
                    SELECT CountryID, CountryName FROM Countries;
                END;
            ");

                        migrationBuilder.Sql(@"
                CREATE PROCEDURE GetCountryById
                    @CountryID INT
                AS
                BEGIN
                    SELECT CountryID, CountryName 
                    FROM Countries 
                    WHERE CountryID = @CountryID;
                END;
            ");

                        migrationBuilder.Sql(@"
                CREATE PROCEDURE InsertCountry
                    @CountryName NVARCHAR(100)
                AS
                BEGIN
                    INSERT INTO Countries (CountryName)
                    VALUES (@CountryName);

                    SELECT SCOPE_IDENTITY() AS NewCountryID;
                END;
            ");

                        migrationBuilder.Sql(@"
                CREATE PROCEDURE UpdateCountry
                    @CountryID INT,
                    @CountryName NVARCHAR(100)
                AS
                BEGIN
                    UPDATE Countries
                    SET CountryName = @CountryName
                    WHERE CountryID = @CountryID;

                    IF @@ROWCOUNT = 0
                    BEGIN
                        RAISERROR('Country not found', 16, 1);
                    END;
                END;
            ");

                        migrationBuilder.Sql(@"
                CREATE PROCEDURE DeleteCountry
                    @CountryID INT
                AS
                BEGIN
                    DELETE FROM Countries 
                    WHERE CountryID = @CountryID;

                    IF @@ROWCOUNT = 0
                    BEGIN
                        RAISERROR('Country not found', 16, 1);
                    END;
                END;
            ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetCountries");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetCountryById");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS InsertCountry");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS UpdateCountry");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS DeleteCountry");
        }
    }
}
