﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using OrderService.Infrastructure;

#nullable disable

namespace OrderService.Infrastructure.Migrations
{
    [DbContext(typeof(OrdersDbContext))]
    partial class OrdersDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("OrderService.Domain.Orders.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.HasKey("Id")
                        .HasName("pk_orders");

                    b.ToTable("orders", "core");
                });

            modelBuilder.Entity("OrderService.Domain.Orders.OrderItem", b =>
                {
                    b.Property<Guid>("order_id")
                        .HasColumnType("uuid")
                        .HasColumnName("order_id");

                    b.Property<string>("Sku")
                        .HasColumnType("text")
                        .HasColumnName("sku");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("numeric")
                        .HasColumnName("quantity");

                    b.HasKey("order_id", "Sku")
                        .HasName("pk_order_items");

                    b.ToTable("order_items", "core");
                });

            modelBuilder.Entity("OrderService.Domain.Orders.OrderItem", b =>
                {
                    b.HasOne("OrderService.Domain.Orders.Order", null)
                        .WithMany("Items")
                        .HasForeignKey("order_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_order_items_orders_order_id");
                });

            modelBuilder.Entity("OrderService.Domain.Orders.Order", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
