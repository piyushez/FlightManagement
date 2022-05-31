﻿// <auto-generated />
using System;
using InventoryService.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace InventoryService.Migrations
{
    [DbContext(typeof(InventoryDbContext))]
    [Migration("20220525190954_InventoryMigration")]
    partial class InventoryMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Common.Models.InventoryTbl", b =>
                {
                    b.Property<string>("FlightNumber")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AirlineNo");

                    b.Property<int?>("BusinessClassSeat");

                    b.Property<DateTime?>("EndDateTime");

                    b.Property<string>("FromPlace");

                    b.Property<string>("InstrumentUsed");

                    b.Property<string>("Meal");

                    b.Property<int?>("NoOfRows");

                    b.Property<int?>("NonBusinessClassSeat");

                    b.Property<string>("ScheduleDays");

                    b.Property<DateTime?>("StartDateTime");

                    b.Property<decimal?>("TicketCost");

                    b.Property<string>("ToPlace");

                    b.HasKey("FlightNumber");

                    b.ToTable("InventoryTbl");
                });
#pragma warning restore 612, 618
        }
    }
}
