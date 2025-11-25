using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobSearch.Migrations
{
    public partial class AddSalaryValidationTrigger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Tworzymy trigger SQL
            migrationBuilder.Sql(@"
                CREATE TRIGGER TRG_JobOffer_SalaryCheck
                ON JobOffer
                AFTER INSERT, UPDATE
                AS
                BEGIN
                    SET NOCOUNT ON;
                    
                    -- Sprawdź, czy w nowo dodawanych/edytowanych wierszach (tabela 'inserted')
                    -- występuje sytuacja, gdzie Min > Max (i oba pola są wypełnione)
                    IF EXISTS (
                        SELECT 1 FROM inserted
                        WHERE SalaryMin IS NOT NULL 
                          AND SalaryMax IS NOT NULL 
                          AND SalaryMin > SalaryMax
                    )
                    BEGIN
                        -- Jeśli tak, zgłoś błąd i cofnij transakcję
                        RAISERROR ('Błąd bazy danych: Wynagrodzenie minimalne nie może być wyższe od maksymalnego!', 16, 1);
                        ROLLBACK TRANSACTION;
                    END
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Usuwamy trigger w razie cofnięcia migracji
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS TRG_JobOffer_SalaryCheck");
        }
    }
}