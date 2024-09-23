using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class StateController : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
               CREATE PROCEDURE GetStates
                AS
                BEGIN
                    SELECT StateID, StateName, CountryID
                    FROM States;
                END;

            ");

            migrationBuilder.Sql(@"
               CREATE PROCEDURE GetStateById
                @StateID INT
            AS
            BEGIN
                SELECT StateID, StateName, CountryID
                FROM States
                WHERE StateID = @StateID;
            END;


                    ");

            migrationBuilder.Sql(@"
             CREATE PROCEDURE CreateState
                @StateName NVARCHAR(100),
                @CountryID INT
            AS
            BEGIN
                INSERT INTO States (StateName, CountryID)
                VALUES (@StateName, @CountryID);
    
                SELECT SCOPE_IDENTITY() AS NewStateID;
            END;

                    ");

            migrationBuilder.Sql(@"CREATE PROCEDURE UpdateState
                @StateID INT,
                @StateName NVARCHAR(100),
                @CountryID INT
            AS
            BEGIN
                UPDATE States
                SET StateName = @StateName,
                    CountryID = @CountryID
                WHERE StateID = @StateID;
            END;
            ");

            migrationBuilder.Sql(@"CREATE PROCEDURE DeleteState
                    @StateID INT
                AS
                BEGIN
                    DELETE FROM States
                    WHERE StateID = @StateID;
                END;
            ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetStates");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetStateById");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS CreateState");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS UpdateState");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS DeleteState");
        }
    }
}
