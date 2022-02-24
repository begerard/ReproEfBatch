using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace ReproEfBatch.Migrations
{
    public partial class AddTimeProps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Duration>(
                name: "DurationProp",
                table: "Entities",
                type: "interval",
                nullable: false,
                defaultValue: NodaTime.Duration.Zero);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "TimeOnlyProp",
                table: "Entities",
                type: "time without time zone",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TimeSpanProp",
                table: "Entities",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationProp",
                table: "Entities");

            migrationBuilder.DropColumn(
                name: "TimeOnlyProp",
                table: "Entities");

            migrationBuilder.DropColumn(
                name: "TimeSpanProp",
                table: "Entities");
        }
    }
}
