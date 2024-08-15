using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace todoapi.Migrations
{
    /// <inheritdoc />
    public partial class ServiceHandling : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_toDoList",
                table: "toDoList");

            migrationBuilder.RenameTable(
                name: "toDoList",
                newName: "ToDoList");

            migrationBuilder.RenameColumn(
                name: "toDoContent",
                table: "ToDoList",
                newName: "ToDoContent");

            migrationBuilder.RenameColumn(
                name: "istoDoDone",
                table: "ToDoList",
                newName: "IstoDoDone");

            migrationBuilder.RenameColumn(
                name: "toDoID",
                table: "ToDoList",
                newName: "ToDoID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ToDoList",
                table: "ToDoList",
                column: "ToDoID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ToDoList",
                table: "ToDoList");

            migrationBuilder.RenameTable(
                name: "ToDoList",
                newName: "toDoList");

            migrationBuilder.RenameColumn(
                name: "ToDoContent",
                table: "toDoList",
                newName: "toDoContent");

            migrationBuilder.RenameColumn(
                name: "IstoDoDone",
                table: "toDoList",
                newName: "istoDoDone");

            migrationBuilder.RenameColumn(
                name: "ToDoID",
                table: "toDoList",
                newName: "toDoID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_toDoList",
                table: "toDoList",
                column: "toDoID");
        }
    }
}
