using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Dictionary.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "words",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_words", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "possible_translations",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    word_id = table.Column<int>(type: "integer", nullable: false),
                    explanation = table.Column<string>(type: "text", nullable: true),
                    translation = table.Column<string>(type: "text", nullable: true),
                    example = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_possible_translations", x => x.id);
                    table.ForeignKey(
                        name: "fk_possible_translations_words_word_id",
                        column: x => x.word_id,
                        principalTable: "words",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "relations",
                columns: table => new
                {
                    word_id = table.Column<int>(type: "integer", nullable: false),
                    related_word_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_relations", x => new { x.word_id, x.related_word_id });
                    table.ForeignKey(
                        name: "fk_relations_words_related_word_id",
                        column: x => x.related_word_id,
                        principalTable: "words",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_relations_words_word_id",
                        column: x => x.word_id,
                        principalTable: "words",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_possible_translations_example",
                table: "possible_translations",
                column: "example");

            migrationBuilder.CreateIndex(
                name: "ix_possible_translations_explanation",
                table: "possible_translations",
                column: "explanation");

            migrationBuilder.CreateIndex(
                name: "ix_possible_translations_translation",
                table: "possible_translations",
                column: "translation");

            migrationBuilder.CreateIndex(
                name: "ix_possible_translations_word_id",
                table: "possible_translations",
                column: "word_id");

            migrationBuilder.CreateIndex(
                name: "ix_relations_related_word_id",
                table: "relations",
                column: "related_word_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "possible_translations");

            migrationBuilder.DropTable(
                name: "relations");

            migrationBuilder.DropTable(
                name: "words");
        }
    }
}
