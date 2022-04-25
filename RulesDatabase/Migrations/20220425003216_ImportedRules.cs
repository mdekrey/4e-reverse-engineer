using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RulesDatabase.Migrations
{
    public partial class ImportedRules : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImportedRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WizardsId = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    FlavorText = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    ShortDescription = table.Column<string>(type: "TEXT", nullable: true),
                    Category = table.Column<string>(type: "TEXT", nullable: true),
                    Prereqs = table.Column<string>(type: "TEXT", nullable: true),
                    EncounterUses = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    PowerUsage = table.Column<string>(type: "TEXT", nullable: true),
                    SkillPower_Id = table.Column<int>(type: "INTEGER", nullable: true),
                    SkillPower_WizardsId = table.Column<string>(type: "TEXT", nullable: true),
                    Display = table.Column<string>(type: "TEXT", nullable: true),
                    ActionType = table.Column<string>(type: "TEXT", nullable: true),
                    Class_Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Class_WizardsId = table.Column<string>(type: "TEXT", nullable: true),
                    Level = table.Column<string>(type: "TEXT", nullable: true),
                    PowerType = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportedRules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Keywords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    KeywordName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keywords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SourceName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImportedRules_AssociatedFeats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WizardsId = table.Column<string>(type: "TEXT", nullable: true),
                    ImportedRuleId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportedRules_AssociatedFeats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImportedRules_AssociatedFeats_ImportedRules_ImportedRuleId",
                        column: x => x.ImportedRuleId,
                        principalTable: "ImportedRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RulesTextEntry",
                columns: table => new
                {
                    RuleId = table.Column<int>(type: "INTEGER", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    Label = table.Column<string>(type: "TEXT", nullable: true),
                    Text = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RulesTextEntry", x => new { x.RuleId, x.Order });
                    table.ForeignKey(
                        name: "FK_RulesTextEntry_ImportedRules_RuleId",
                        column: x => x.RuleId,
                        principalTable: "ImportedRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImportedRuleKeyword",
                columns: table => new
                {
                    KeywordsId = table.Column<int>(type: "INTEGER", nullable: false),
                    RulesId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportedRuleKeyword", x => new { x.KeywordsId, x.RulesId });
                    table.ForeignKey(
                        name: "FK_ImportedRuleKeyword_ImportedRules_RulesId",
                        column: x => x.RulesId,
                        principalTable: "ImportedRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImportedRuleKeyword_Keywords_KeywordsId",
                        column: x => x.KeywordsId,
                        principalTable: "Keywords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImportedRuleSource",
                columns: table => new
                {
                    RulesId = table.Column<int>(type: "INTEGER", nullable: false),
                    SourcesId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportedRuleSource", x => new { x.RulesId, x.SourcesId });
                    table.ForeignKey(
                        name: "FK_ImportedRuleSource_ImportedRules_RulesId",
                        column: x => x.RulesId,
                        principalTable: "ImportedRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImportedRuleSource_Sources_SourcesId",
                        column: x => x.SourcesId,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImportedRuleKeyword_RulesId",
                table: "ImportedRuleKeyword",
                column: "RulesId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportedRules_Class_WizardsId",
                table: "ImportedRules",
                column: "Class_WizardsId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportedRules_Name",
                table: "ImportedRules",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ImportedRules_SkillPower_WizardsId",
                table: "ImportedRules",
                column: "SkillPower_WizardsId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportedRules_Type_Name",
                table: "ImportedRules",
                columns: new[] { "Type", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImportedRules_WizardsId",
                table: "ImportedRules",
                column: "WizardsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImportedRules_AssociatedFeats_ImportedRuleId",
                table: "ImportedRules_AssociatedFeats",
                column: "ImportedRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportedRules_AssociatedFeats_WizardsId",
                table: "ImportedRules_AssociatedFeats",
                column: "WizardsId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportedRuleSource_SourcesId",
                table: "ImportedRuleSource",
                column: "SourcesId");

            migrationBuilder.CreateIndex(
                name: "IX_Keywords_KeywordName",
                table: "Keywords",
                column: "KeywordName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sources_SourceName",
                table: "Sources",
                column: "SourceName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImportedRuleKeyword");

            migrationBuilder.DropTable(
                name: "ImportedRules_AssociatedFeats");

            migrationBuilder.DropTable(
                name: "ImportedRuleSource");

            migrationBuilder.DropTable(
                name: "RulesTextEntry");

            migrationBuilder.DropTable(
                name: "Keywords");

            migrationBuilder.DropTable(
                name: "Sources");

            migrationBuilder.DropTable(
                name: "ImportedRules");
        }
    }
}
