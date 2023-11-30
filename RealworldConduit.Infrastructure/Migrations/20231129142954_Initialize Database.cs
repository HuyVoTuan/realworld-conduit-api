using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealworldConduit.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitializeDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "blog");

            migrationBuilder.EnsureSchema(
                name: "user");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "Tag",
                schema: "blog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "user",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Username = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    ProfileImage = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Bio = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Blog",
                schema: "blog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Blog_User_AuthorId",
                        column: x => x.AuthorId,
                        principalSchema: "user",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                schema: "user",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccessToken = table.Column<string>(type: "text", nullable: false),
                    ExpiredDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "user",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserFollower",
                schema: "user",
                columns: table => new
                {
                    FollowedUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    FollowerId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFollower", x => new { x.FollowedUserId, x.FollowerId });
                    table.ForeignKey(
                        name: "FK_UserFollower_User_FollowedUserId",
                        column: x => x.FollowedUserId,
                        principalSchema: "user",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFollower_User_FollowerId",
                        column: x => x.FollowerId,
                        principalSchema: "user",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlogTag",
                schema: "blog",
                columns: table => new
                {
                    BlogId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogTag", x => new { x.BlogId, x.TagId });
                    table.ForeignKey(
                        name: "FK_BlogTag_Blog_BlogId",
                        column: x => x.BlogId,
                        principalSchema: "blog",
                        principalTable: "Blog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlogTag_Tag_TagId",
                        column: x => x.TagId,
                        principalSchema: "blog",
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FavoriteBlog",
                schema: "blog",
                columns: table => new
                {
                    BlogId = table.Column<Guid>(type: "uuid", nullable: false),
                    FavoritedById = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteBlog", x => new { x.BlogId, x.FavoritedById });
                    table.ForeignKey(
                        name: "FK_FavoriteBlog_Blog_BlogId",
                        column: x => x.BlogId,
                        principalSchema: "blog",
                        principalTable: "Blog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteBlog_User_FavoritedById",
                        column: x => x.FavoritedById,
                        principalSchema: "user",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blog_AuthorId",
                schema: "blog",
                table: "Blog",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Blog_Title",
                schema: "blog",
                table: "Blog",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogTag_TagId",
                schema: "blog",
                table: "BlogTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteBlog_FavoritedById",
                schema: "blog",
                table: "FavoriteBlog",
                column: "FavoritedById");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_AccessToken",
                schema: "user",
                table: "RefreshToken",
                column: "AccessToken");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_ExpiredDate",
                schema: "user",
                table: "RefreshToken",
                column: "ExpiredDate");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserId",
                schema: "user",
                table: "RefreshToken",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_Name",
                schema: "blog",
                table: "Tag",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                schema: "user",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Username",
                schema: "user",
                table: "User",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserFollower_FollowerId",
                schema: "user",
                table: "UserFollower",
                column: "FollowerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogTag",
                schema: "blog");

            migrationBuilder.DropTable(
                name: "FavoriteBlog",
                schema: "blog");

            migrationBuilder.DropTable(
                name: "RefreshToken",
                schema: "user");

            migrationBuilder.DropTable(
                name: "UserFollower",
                schema: "user");

            migrationBuilder.DropTable(
                name: "Tag",
                schema: "blog");

            migrationBuilder.DropTable(
                name: "Blog",
                schema: "blog");

            migrationBuilder.DropTable(
                name: "User",
                schema: "user");
        }
    }
}
