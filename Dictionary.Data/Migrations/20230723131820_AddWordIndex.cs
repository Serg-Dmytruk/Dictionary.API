using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dictionary.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddWordIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_words_value",
                table: "words",
                column: "value",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_words_value",
                table: "words");
        }
    }
}
