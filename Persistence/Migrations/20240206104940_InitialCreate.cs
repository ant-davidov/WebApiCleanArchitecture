﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LeaveTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    DefaultDays = table.Column<int>(type: "INTEGER", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeaveAllocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NumberOfDays = table.Column<int>(type: "INTEGER", nullable: false),
                    LeaveTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Period = table.Column<int>(type: "INTEGER", nullable: false),
                    EmployeeId = table.Column<string>(type: "TEXT", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveAllocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaveAllocations_LeaveTypes_LeaveTypeId",
                        column: x => x.LeaveTypeId,
                        principalTable: "LeaveTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeaveRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LeaveTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    DateRequested = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RequestComments = table.Column<string>(type: "TEXT", nullable: false),
                    DateActioned = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Approved = table.Column<bool>(type: "INTEGER", nullable: true),
                    Cancelled = table.Column<bool>(type: "INTEGER", nullable: false),
                    RequestingEmployeeId = table.Column<string>(type: "TEXT", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaveRequests_LeaveTypes_LeaveTypeId",
                        column: x => x.LeaveTypeId,
                        principalTable: "LeaveTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "LeaveTypes",
                columns: new[] { "Id", "CreatedBy", "DateCreated", "DefaultDays", "LastModifiedBy", "LastModifiedDate", "Name" },
                values: new object[,]
                {
                    { 1, "user", new DateTime(2024, 2, 6, 13, 49, 40, 360, DateTimeKind.Local).AddTicks(3452), 10, "user", new DateTime(2024, 2, 6, 13, 59, 40, 360, DateTimeKind.Local).AddTicks(3463), "Vacation" },
                    { 2, "user", new DateTime(2024, 2, 6, 13, 49, 40, 360, DateTimeKind.Local).AddTicks(3467), 12, "user", new DateTime(2024, 2, 6, 13, 59, 40, 360, DateTimeKind.Local).AddTicks(3468), "Sick" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeaveAllocations_LeaveTypeId",
                table: "LeaveAllocations",
                column: "LeaveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_LeaveTypeId",
                table: "LeaveRequests",
                column: "LeaveTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeaveAllocations");

            migrationBuilder.DropTable(
                name: "LeaveRequests");

            migrationBuilder.DropTable(
                name: "LeaveTypes");
        }
    }
}
