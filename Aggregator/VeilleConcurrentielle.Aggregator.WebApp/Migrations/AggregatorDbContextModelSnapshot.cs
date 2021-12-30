﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VeilleConcurrentielle.Aggregator.WebApp.Data;

#nullable disable

namespace VeilleConcurrentielle.Aggregator.WebApp.Migrations
{
    [DbContext(typeof(AggregatorDbContext))]
    partial class AggregatorDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.1");

            modelBuilder.Entity("VeilleConcurrentielle.Aggregator.WebApp.Data.Entities.CompetitorEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("LogoUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Competitors");

                    b.HasData(
                        new
                        {
                            Id = "ShopA",
                            LogoUrl = "https://www.icone-png.com/png/43/43296.png",
                            Name = "Shop A"
                        },
                        new
                        {
                            Id = "ShopB",
                            LogoUrl = "https://www.icone-png.com/png/43/43302.png",
                            Name = "Shop B"
                        },
                        new
                        {
                            Id = "ShopC",
                            LogoUrl = "https://www.icone-png.com/png/33/32570.png",
                            Name = "Shop C"
                        });
                });

            modelBuilder.Entity("VeilleConcurrentielle.Aggregator.WebApp.Data.Entities.ProductAggregateCompetitorConfigEntity", b =>
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

                    b.ToTable("ProductAggregateCompetitorConfigs");
                });

            modelBuilder.Entity("VeilleConcurrentielle.Aggregator.WebApp.Data.Entities.ProductAggregateEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("MaxPrice")
                        .HasColumnType("REAL");

                    b.Property<string>("MaxPriceCompetitorId")
                        .HasColumnType("TEXT");

                    b.Property<int?>("MaxPriceQuantity")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("MinPrice")
                        .HasColumnType("REAL");

                    b.Property<string>("MinPriceCompetitorId")
                        .HasColumnType("TEXT");

                    b.Property<int?>("MinPriceQuantity")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ShopProductId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ShopProductUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ProductAggregates");
                });

            modelBuilder.Entity("VeilleConcurrentielle.Aggregator.WebApp.Data.Entities.ProductAggregatePriceEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("CompetitorId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductAggregatePrices");
                });

            modelBuilder.Entity("VeilleConcurrentielle.Aggregator.WebApp.Data.Entities.ProductAggregateRecommendationEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<double>("CurrentPrice")
                        .HasColumnType("REAL");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("StrategyId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductRecommendations");
                });

            modelBuilder.Entity("VeilleConcurrentielle.Aggregator.WebApp.Data.Entities.ProductAggregateStrategyEntity", b =>
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

                    b.ToTable("ProductAggregateStrategies");
                });

            modelBuilder.Entity("VeilleConcurrentielle.Aggregator.WebApp.Data.Entities.RecommendationAlertEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<double>("CurrentPrice")
                        .HasColumnType("REAL");

                    b.Property<bool>("IsSeen")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("SeenAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("StrategyId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("IsSeen");

                    b.HasIndex("ProductId");

                    b.ToTable("RecommendationAlerts");
                });

            modelBuilder.Entity("VeilleConcurrentielle.Aggregator.WebApp.Data.Entities.StrategyEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Strategies");

                    b.HasData(
                        new
                        {
                            Id = "OverallAveragePrice",
                            Name = "Dans la moyenne"
                        },
                        new
                        {
                            Id = "OverallCheaperPrice",
                            Name = "Le moins cher"
                        },
                        new
                        {
                            Id = "FivePercentAboveMeanPrice",
                            Name = "5% plus cher que la moyenne"
                        });
                });

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

            modelBuilder.Entity("VeilleConcurrentielle.Aggregator.WebApp.Data.Entities.ProductAggregateCompetitorConfigEntity", b =>
                {
                    b.HasOne("VeilleConcurrentielle.Aggregator.WebApp.Data.Entities.ProductAggregateEntity", "Product")
                        .WithMany("CompetitorConfigs")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("VeilleConcurrentielle.Aggregator.WebApp.Data.Entities.ProductAggregatePriceEntity", b =>
                {
                    b.HasOne("VeilleConcurrentielle.Aggregator.WebApp.Data.Entities.ProductAggregateEntity", "Product")
                        .WithMany("LastPrices")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("VeilleConcurrentielle.Aggregator.WebApp.Data.Entities.ProductAggregateRecommendationEntity", b =>
                {
                    b.HasOne("VeilleConcurrentielle.Aggregator.WebApp.Data.Entities.ProductAggregateEntity", "Product")
                        .WithMany("Recommendations")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("VeilleConcurrentielle.Aggregator.WebApp.Data.Entities.ProductAggregateStrategyEntity", b =>
                {
                    b.HasOne("VeilleConcurrentielle.Aggregator.WebApp.Data.Entities.ProductAggregateEntity", "Product")
                        .WithMany("Strategies")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("VeilleConcurrentielle.Aggregator.WebApp.Data.Entities.ProductAggregateEntity", b =>
                {
                    b.Navigation("CompetitorConfigs");

                    b.Navigation("LastPrices");

                    b.Navigation("Recommendations");

                    b.Navigation("Strategies");
                });
#pragma warning restore 612, 618
        }
    }
}
