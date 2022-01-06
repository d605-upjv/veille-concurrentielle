﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VeilleConcurrentielle.ProductService.WebApp.Data;

#nullable disable

namespace VeilleConcurrentielle.ProductService.WebApp.Migrations
{
    [DbContext(typeof(ProductDbContext))]
    [Migration("20211225231125_StrategyAndCompetitorIndices")]
    partial class StrategyAndCompetitorIndices
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.1");

            modelBuilder.Entity("VeilleConcurrentielle.Infrastructure.Core.Data.Entities.ReceivedEventEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DispatchedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("EventId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SerializedPayload")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Source")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ReceivedEvents");
                });

            modelBuilder.Entity("VeilleConcurrentielle.ProductService.WebApp.Data.Entities.CompetitorConfigEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("CompetitorId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SerializedHolder")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("CompetitorConfigs");
                });

            modelBuilder.Entity("VeilleConcurrentielle.ProductService.WebApp.Data.Entities.ProductEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("VeilleConcurrentielle.ProductService.WebApp.Data.Entities.StrategyEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("StrategyId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Strategies");
                });

            modelBuilder.Entity("VeilleConcurrentielle.ProductService.WebApp.Data.Entities.CompetitorConfigEntity", b =>
                {
                    b.HasOne("VeilleConcurrentielle.ProductService.WebApp.Data.Entities.ProductEntity", "Product")
                        .WithMany("CompetitorConfigs")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("VeilleConcurrentielle.ProductService.WebApp.Data.Entities.StrategyEntity", b =>
                {
                    b.HasOne("VeilleConcurrentielle.ProductService.WebApp.Data.Entities.ProductEntity", "Product")
                        .WithMany("Strategies")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("VeilleConcurrentielle.ProductService.WebApp.Data.Entities.ProductEntity", b =>
                {
                    b.Navigation("CompetitorConfigs");

                    b.Navigation("Strategies");
                });
#pragma warning restore 612, 618
        }
    }
}
