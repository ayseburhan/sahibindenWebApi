using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sahibindenWebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokenFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdminId = table.Column<int>(type: "int", nullable: true),
                    AdminMail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdminSifre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdminAdi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdminSoyadi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdminAdres = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "favori",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    kullaniciId = table.Column<int>(type: "int", nullable: false),
                    urunId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_favori", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "kategori",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    kategoriAdi = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__kategori__3213E83FE9AC4163", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "kullanici",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KullaniciId = table.Column<int>(type: "int", nullable: false),
                    KullaniciMail = table.Column<string>(type: "char(30)", unicode: false, fixedLength: true, maxLength: 30, nullable: false),
                    KullaniciSifre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    KullaniciAdi = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    KullaniciSoyadi = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    KullaniciAdres = table.Column<string>(type: "text", nullable: false),
                    KullaniciYetki = table.Column<int>(type: "int", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__kullanic__E011F77B073D8F2E", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sepet",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    urunId = table.Column<int>(type: "int", nullable: false),
                    kullaniciId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sepet", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "urunler",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    kategoriId = table.Column<int>(type: "int", nullable: false),
                    urunAdi = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    urunAciklama = table.Column<string>(type: "text", nullable: false),
                    urunFiyat = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    urunStok = table.Column<int>(type: "int", nullable: false),
                    eklenmeTarihi = table.Column<DateTime>(type: "datetime", nullable: false),
                    urunDurum = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    urunGorsel = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__urunler__3213E83F5DF9542D", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "favori");

            migrationBuilder.DropTable(
                name: "kategori");

            migrationBuilder.DropTable(
                name: "kullanici");

            migrationBuilder.DropTable(
                name: "sepet");

            migrationBuilder.DropTable(
                name: "urunler");
        }
    }
}
