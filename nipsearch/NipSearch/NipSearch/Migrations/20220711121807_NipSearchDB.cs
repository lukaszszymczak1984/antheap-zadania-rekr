using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NipSearch.Migrations
{
    public partial class NipSearchDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    IdSubject = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusVat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Regon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pesel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Krs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResidenceAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkingAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistrationLegalDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistrationDenialDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistrationDenialBasis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RestorationDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RestorationBasis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RemovalDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RemovalBasis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasVirtualAccounts = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.IdSubject);
                });

            migrationBuilder.CreateTable(
                name: "AccountNumbers",
                columns: table => new
                {
                    IdAccountNumber = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdSubject = table.Column<int>(type: "int", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountNumbers", x => x.IdAccountNumber);
                    table.ForeignKey(
                        name: "FK_AccountNumbers_Subjects_IdSubject",
                        column: x => x.IdSubject,
                        principalTable: "Subjects",
                        principalColumn: "IdSubject",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    IdPerson = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdSubjectRepresentative = table.Column<int>(type: "int", nullable: true),
                    IdSubjectAuthorizedClerk = table.Column<int>(type: "int", nullable: true),
                    IdSubjectPartner = table.Column<int>(type: "int", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pesel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nip = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.IdPerson);
                    table.ForeignKey(
                        name: "FK_Persons_Subjects_IdSubjectAuthorizedClerk",
                        column: x => x.IdSubjectAuthorizedClerk,
                        principalTable: "Subjects",
                        principalColumn: "IdSubject");
                    table.ForeignKey(
                        name: "FK_Persons_Subjects_IdSubjectPartner",
                        column: x => x.IdSubjectPartner,
                        principalTable: "Subjects",
                        principalColumn: "IdSubject");
                    table.ForeignKey(
                        name: "FK_Persons_Subjects_IdSubjectRepresentative",
                        column: x => x.IdSubjectRepresentative,
                        principalTable: "Subjects",
                        principalColumn: "IdSubject");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountNumbers_IdSubject",
                table: "AccountNumbers",
                column: "IdSubject");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_IdSubjectAuthorizedClerk",
                table: "Persons",
                column: "IdSubjectAuthorizedClerk");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_IdSubjectPartner",
                table: "Persons",
                column: "IdSubjectPartner");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_IdSubjectRepresentative",
                table: "Persons",
                column: "IdSubjectRepresentative");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountNumbers");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Subjects");
        }
    }
}
