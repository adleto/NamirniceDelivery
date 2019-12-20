using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NamirniceDelivery.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kanton",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(nullable: true),
                    Oznaka = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kanton", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kategorija",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategorija", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Popust",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Opis = table.Column<string>(nullable: true),
                    Iznos = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Popust", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipTransakcije",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NazivTipa = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipTransakcije", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Opcina",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(nullable: true),
                    KantonId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Opcina", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Opcina_Kanton_KantonId",
                        column: x => x.KantonId,
                        principalTable: "Kanton",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Namirnica",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(nullable: true),
                    KategorijaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Namirnica", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Namirnica_Kategorija_KategorijaId",
                        column: x => x.KategorijaId,
                        principalTable: "Kategorija",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Podruznica",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Adresa = table.Column<string>(nullable: true),
                    OpcinaId = table.Column<int>(nullable: false),
                    Opis = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Podruznica", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Podruznica_Opcina_OpcinaId",
                        column: x => x.OpcinaId,
                        principalTable: "Opcina",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NamirnicaPodruznica",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NamirnicaId = table.Column<int>(nullable: false),
                    PodruznicaId = table.Column<int>(nullable: false),
                    Cijena = table.Column<decimal>(nullable: false),
                    Aktivna = table.Column<bool>(nullable: false),
                    KolicinaNaStanju = table.Column<int>(nullable: false),
                    PopustId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NamirnicaPodruznica", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NamirnicaPodruznica_Namirnica_NamirnicaId",
                        column: x => x.NamirnicaId,
                        principalTable: "Namirnica",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NamirnicaPodruznica_Podruznica_PodruznicaId",
                        column: x => x.PodruznicaId,
                        principalTable: "Podruznica",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NamirnicaPodruznica_Popust_PopustId",
                        column: x => x.PopustId,
                        principalTable: "Popust",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "KorpaStavka",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KupacId = table.Column<string>(nullable: true),
                    Kolicina = table.Column<int>(nullable: false),
                    NamirnicaPodruznicaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KorpaStavka", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KorpaStavka_NamirnicaPodruznica_NamirnicaPodruznicaId",
                        column: x => x.NamirnicaPodruznicaId,
                        principalTable: "NamirnicaPodruznica",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KupacSpremljeneNamirnice",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KupacId = table.Column<string>(nullable: false),
                    NamirnicaPodruznicaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KupacSpremljeneNamirnice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KupacSpremljeneNamirnice_NamirnicaPodruznica_NamirnicaPodruznicaId",
                        column: x => x.NamirnicaPodruznicaId,
                        principalTable: "NamirnicaPodruznica",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "KupacSpremljenePodruznice",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KupacId = table.Column<string>(nullable: false),
                    PodruznicaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KupacSpremljenePodruznice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KupacSpremljenePodruznice_Podruznica_PodruznicaId",
                        column: x => x.PodruznicaId,
                        principalTable: "Podruznica",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Transakcija",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DostavaUspjesna = table.Column<bool>(nullable: false),
                    DatumUspjesneDostave = table.Column<DateTime>(nullable: false),
                    NarudzbaPrihvacenaOdRadnika = table.Column<bool>(nullable: false),
                    DatumPrihvacanjaNarudzbe = table.Column<DateTime>(nullable: false),
                    RadnikOstavioDojam = table.Column<bool>(nullable: false),
                    KupacOstavioDojam = table.Column<bool>(nullable: false),
                    DatumIniciranjaTransakcije = table.Column<DateTime>(nullable: false),
                    AdministrativniRadnikId = table.Column<string>(nullable: false),
                    KupacId = table.Column<string>(nullable: false),
                    TipTransakcijeId = table.Column<int>(nullable: false),
                    DojamRadnik = table.Column<string>(nullable: true),
                    DojamKupac = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transakcija", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transakcija_TipTransakcije_TipTransakcijeId",
                        column: x => x.TipTransakcijeId,
                        principalTable: "TipTransakcije",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "KupljeneNamirnice",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransakcijaId = table.Column<int>(nullable: false),
                    Cijena = table.Column<decimal>(nullable: false),
                    Kolicina = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KupljeneNamirnice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KupljeneNamirnice_Transakcija_TransakcijaId",
                        column: x => x.TransakcijaId,
                        principalTable: "Transakcija",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vozilo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipVozila = table.Column<string>(nullable: true),
                    RegistarskeOznake = table.Column<string>(nullable: true),
                    MarkaVozila = table.Column<string>(nullable: true),
                    VozacId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vozilo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    Ime = table.Column<string>(nullable: true),
                    Prezime = table.Column<string>(nullable: true),
                    OpcinaBoravkaId = table.Column<int>(nullable: true),
                    OpcinaRodjenjaId = table.Column<int>(nullable: true),
                    JMBG = table.Column<string>(nullable: true),
                    PodruznicaId = table.Column<int>(nullable: true),
                    Adresa = table.Column<string>(nullable: true),
                    KategorijaVozackeDozvole = table.Column<string>(nullable: true),
                    VoziloId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Podruznica_PodruznicaId",
                        column: x => x.PodruznicaId,
                        principalTable: "Podruznica",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Opcina_OpcinaBoravkaId",
                        column: x => x.OpcinaBoravkaId,
                        principalTable: "Opcina",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Opcina_OpcinaRodjenjaId",
                        column: x => x.OpcinaRodjenjaId,
                        principalTable: "Opcina",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Vozilo_VoziloId",
                        column: x => x.VoziloId,
                        principalTable: "Vozilo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Voznja",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PreuzetaRoba = table.Column<bool>(nullable: false),
                    ObavljenaVoznja = table.Column<bool>(nullable: false),
                    VoznjaPocetak = table.Column<DateTime>(nullable: false),
                    VoznjaKraj = table.Column<DateTime>(nullable: false),
                    PodruznicaPocetakId = table.Column<int>(nullable: false),
                    PodruznicaKrajId = table.Column<int>(nullable: false),
                    VozacId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voznja", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Voznja_Podruznica_PodruznicaKrajId",
                        column: x => x.PodruznicaKrajId,
                        principalTable: "Podruznica",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Voznja_Podruznica_PodruznicaPocetakId",
                        column: x => x.PodruznicaPocetakId,
                        principalTable: "Podruznica",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Voznja_AspNetUsers_VozacId",
                        column: x => x.VozacId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NamirnicaVoznja",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VoznjaId = table.Column<int>(nullable: false),
                    NamirnicaId = table.Column<int>(nullable: false),
                    KolicinaPrevezena = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NamirnicaVoznja", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NamirnicaVoznja_Namirnica_NamirnicaId",
                        column: x => x.NamirnicaId,
                        principalTable: "Namirnica",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_NamirnicaVoznja_Voznja_VoznjaId",
                        column: x => x.VoznjaId,
                        principalTable: "Voznja",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PodruznicaId",
                table: "AspNetUsers",
                column: "PodruznicaId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_OpcinaBoravkaId",
                table: "AspNetUsers",
                column: "OpcinaBoravkaId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_OpcinaRodjenjaId",
                table: "AspNetUsers",
                column: "OpcinaRodjenjaId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_VoziloId",
                table: "AspNetUsers",
                column: "VoziloId",
                unique: true,
                filter: "[VoziloId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_KorpaStavka_KupacId",
                table: "KorpaStavka",
                column: "KupacId");

            migrationBuilder.CreateIndex(
                name: "IX_KorpaStavka_NamirnicaPodruznicaId",
                table: "KorpaStavka",
                column: "NamirnicaPodruznicaId");

            migrationBuilder.CreateIndex(
                name: "IX_KupacSpremljeneNamirnice_KupacId",
                table: "KupacSpremljeneNamirnice",
                column: "KupacId");

            migrationBuilder.CreateIndex(
                name: "IX_KupacSpremljeneNamirnice_NamirnicaPodruznicaId",
                table: "KupacSpremljeneNamirnice",
                column: "NamirnicaPodruznicaId");

            migrationBuilder.CreateIndex(
                name: "IX_KupacSpremljenePodruznice_KupacId",
                table: "KupacSpremljenePodruznice",
                column: "KupacId");

            migrationBuilder.CreateIndex(
                name: "IX_KupacSpremljenePodruznice_PodruznicaId",
                table: "KupacSpremljenePodruznice",
                column: "PodruznicaId");

            migrationBuilder.CreateIndex(
                name: "IX_KupljeneNamirnice_TransakcijaId",
                table: "KupljeneNamirnice",
                column: "TransakcijaId");

            migrationBuilder.CreateIndex(
                name: "IX_Namirnica_KategorijaId",
                table: "Namirnica",
                column: "KategorijaId");

            migrationBuilder.CreateIndex(
                name: "IX_NamirnicaPodruznica_NamirnicaId",
                table: "NamirnicaPodruznica",
                column: "NamirnicaId");

            migrationBuilder.CreateIndex(
                name: "IX_NamirnicaPodruznica_PodruznicaId",
                table: "NamirnicaPodruznica",
                column: "PodruznicaId");

            migrationBuilder.CreateIndex(
                name: "IX_NamirnicaPodruznica_PopustId",
                table: "NamirnicaPodruznica",
                column: "PopustId");

            migrationBuilder.CreateIndex(
                name: "IX_NamirnicaVoznja_NamirnicaId",
                table: "NamirnicaVoznja",
                column: "NamirnicaId");

            migrationBuilder.CreateIndex(
                name: "IX_NamirnicaVoznja_VoznjaId",
                table: "NamirnicaVoznja",
                column: "VoznjaId");

            migrationBuilder.CreateIndex(
                name: "IX_Opcina_KantonId",
                table: "Opcina",
                column: "KantonId");

            migrationBuilder.CreateIndex(
                name: "IX_Podruznica_OpcinaId",
                table: "Podruznica",
                column: "OpcinaId");

            migrationBuilder.CreateIndex(
                name: "IX_Transakcija_AdministrativniRadnikId",
                table: "Transakcija",
                column: "AdministrativniRadnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Transakcija_KupacId",
                table: "Transakcija",
                column: "KupacId");

            migrationBuilder.CreateIndex(
                name: "IX_Transakcija_TipTransakcijeId",
                table: "Transakcija",
                column: "TipTransakcijeId");

            migrationBuilder.CreateIndex(
                name: "IX_Vozilo_VozacId",
                table: "Vozilo",
                column: "VozacId");

            migrationBuilder.CreateIndex(
                name: "IX_Voznja_PodruznicaKrajId",
                table: "Voznja",
                column: "PodruznicaKrajId");

            migrationBuilder.CreateIndex(
                name: "IX_Voznja_PodruznicaPocetakId",
                table: "Voznja",
                column: "PodruznicaPocetakId");

            migrationBuilder.CreateIndex(
                name: "IX_Voznja_VozacId",
                table: "Voznja",
                column: "VozacId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KorpaStavka_AspNetUsers_KupacId",
                table: "KorpaStavka",
                column: "KupacId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_KupacSpremljeneNamirnice_AspNetUsers_KupacId",
                table: "KupacSpremljeneNamirnice",
                column: "KupacId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_KupacSpremljenePodruznice_AspNetUsers_KupacId",
                table: "KupacSpremljenePodruznice",
                column: "KupacId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Transakcija_AspNetUsers_AdministrativniRadnikId",
                table: "Transakcija",
                column: "AdministrativniRadnikId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Transakcija_AspNetUsers_KupacId",
                table: "Transakcija",
                column: "KupacId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Vozilo_AspNetUsers_VozacId",
                table: "Vozilo",
                column: "VozacId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vozilo_AspNetUsers_VozacId",
                table: "Vozilo");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "KorpaStavka");

            migrationBuilder.DropTable(
                name: "KupacSpremljeneNamirnice");

            migrationBuilder.DropTable(
                name: "KupacSpremljenePodruznice");

            migrationBuilder.DropTable(
                name: "KupljeneNamirnice");

            migrationBuilder.DropTable(
                name: "NamirnicaVoznja");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "NamirnicaPodruznica");

            migrationBuilder.DropTable(
                name: "Transakcija");

            migrationBuilder.DropTable(
                name: "Voznja");

            migrationBuilder.DropTable(
                name: "Namirnica");

            migrationBuilder.DropTable(
                name: "Popust");

            migrationBuilder.DropTable(
                name: "TipTransakcije");

            migrationBuilder.DropTable(
                name: "Kategorija");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Podruznica");

            migrationBuilder.DropTable(
                name: "Vozilo");

            migrationBuilder.DropTable(
                name: "Opcina");

            migrationBuilder.DropTable(
                name: "Kanton");
        }
    }
}
