﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VeilleConcurrentielle.EventOrchestrator.WebApp.Data;

#nullable disable

namespace VeilleConcurrentielle.EventOrchestrator.WebApp.Migrations
{
    [DbContext(typeof(EventDbContext))]
    [Migration("20211222080344_ReceivedEvents")]
    partial class ReceivedEvents
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.1");

            modelBuilder.Entity("VeilleConcurrentielle.EventOrchestrator.WebApp.Data.Entities.EventEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
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

                    b.HasIndex("CreatedAt");

                    b.HasIndex("Name");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("VeilleConcurrentielle.EventOrchestrator.WebApp.Data.Entities.EventSubscriberEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ApplicationName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("EventName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("EventName");

                    b.ToTable("EventSubscribers");

                    b.HasData(
                        new
                        {
                            Id = "212d189d-c176-4490-b7c8-edd0be4b3dff",
                            ApplicationName = "ProductService",
                            EventName = "AddOrUpdateProductRequested"
                        });
                });

            modelBuilder.Entity("VeilleConcurrentielle.Infrastructure.Core.Data.Entities.ReceivedEventEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
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

                    b.Property<DateTime>("SubmittedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ReceivedEvents");
                });
#pragma warning restore 612, 618
        }
    }
}
