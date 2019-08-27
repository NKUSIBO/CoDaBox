using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inocrea.CodaBox.ApiServer.Migrations
{
    public partial class statementAccountTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CodaIdentities",
                columns: table => new
                {
                    CodaIdentityId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    XCompany = table.Column<string>(nullable: true),
                    Login = table.Column<string>(nullable: true),
                    Pwd = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodaIdentities", x => x.CodaIdentityId);
                });

            migrationBuilder.CreateTable(
                name: "CompteBancaire",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Iban = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    Bic = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    IdentificationNumber = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    CurrencyCode = table.Column<string>(unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("CompteBancaire_pk", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "Statements",
                columns: table => new
                {
                    StatementId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CompteBancaireId = table.Column<int>(nullable: false),
                    InitialBalance = table.Column<double>(nullable: false),
                    NewBalance = table.Column<double>(nullable: false),
                    InformationalMessage = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    Date = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Statements_pk", x => x.StatementId)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "Statements_CompteBanquare_fk",
                        column: x => x.CompteBancaireId,
                        principalTable: "CompteBancaire",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StructuredMessage = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ValueDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    Message = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    AdresseId = table.Column<int>(nullable: true),
                    StatementID = table.Column<int>(nullable: false),
                    CompteBancaireId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Transactions_pk", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "Transactions_CompteBancaires_fk",
                        column: x => x.CompteBancaireId,
                        principalTable: "CompteBancaire",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "Transactions_Statements_fk",
                        column: x => x.StatementID,
                        principalTable: "Statements",
                        principalColumn: "StatementId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SepaDirectDebits",
                columns: table => new
                {
                    SepaDirectDebitId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreditorIdentificationCode = table.Column<string>(nullable: true),
                    MandateReference = table.Column<string>(nullable: true),
                    PaidReason = table.Column<int>(nullable: false),
                    Scheme = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    TransactionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SepaDirectDebits", x => x.SepaDirectDebitId);
                    table.ForeignKey(
                        name: "FK_SepaDirectDebits_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SepaDirectDebits_TransactionId",
                table: "SepaDirectDebits",
                column: "TransactionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Statements_CompteBancaireId",
                table: "Statements",
                column: "CompteBancaireId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CompteBancaireId",
                table: "Transactions",
                column: "CompteBancaireId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_StatementID",
                table: "Transactions",
                column: "StatementID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CodaIdentities");

            migrationBuilder.DropTable(
                name: "SepaDirectDebits");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Statements");

            migrationBuilder.DropTable(
                name: "CompteBancaire");
        }
    }
}
