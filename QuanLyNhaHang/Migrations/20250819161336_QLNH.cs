using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyNhaHang.Migrations
{
    /// <inheritdoc />
    public partial class QLNH : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreateBy",
                table: "UserRole",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "UserRole",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Dept",
                table: "UserLogin",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "UserLogin",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "UserLogin",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Grade",
                table: "UserLogin",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Group",
                table: "UserLogin",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IDCard",
                table: "UserLogin",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "UserLogin",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "UserLogin",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Shift",
                table: "UserLogin",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Role",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateBy",
                table: "UserRole");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "UserRole");

            migrationBuilder.DropColumn(
                name: "Dept",
                table: "UserLogin");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "UserLogin");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "UserLogin");

            migrationBuilder.DropColumn(
                name: "Grade",
                table: "UserLogin");

            migrationBuilder.DropColumn(
                name: "Group",
                table: "UserLogin");

            migrationBuilder.DropColumn(
                name: "IDCard",
                table: "UserLogin");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "UserLogin");

            migrationBuilder.DropColumn(
                name: "Picture",
                table: "UserLogin");

            migrationBuilder.DropColumn(
                name: "Shift",
                table: "UserLogin");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Role");
        }
    }
}
