using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaEFood.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class BitacoraError : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductoPrecio_Productos_Idproducto",
                table: "ProductoPrecio");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductoPrecio_TiposPrecio_Idprecio",
                table: "ProductoPrecio");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductoPrecio_Productos_Idproducto",
                table: "ProductoPrecio",
                column: "Idproducto",
                principalTable: "Productos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductoPrecio_TiposPrecio_Idprecio",
                table: "ProductoPrecio",
                column: "Idprecio",
                principalTable: "TiposPrecio",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductoPrecio_Productos_Idproducto",
                table: "ProductoPrecio");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductoPrecio_TiposPrecio_Idprecio",
                table: "ProductoPrecio");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductoPrecio_Productos_Idproducto",
                table: "ProductoPrecio",
                column: "Idproducto",
                principalTable: "Productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductoPrecio_TiposPrecio_Idprecio",
                table: "ProductoPrecio",
                column: "Idprecio",
                principalTable: "TiposPrecio",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
