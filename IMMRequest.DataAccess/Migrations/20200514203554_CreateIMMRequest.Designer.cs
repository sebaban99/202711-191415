﻿// <auto-generated />
using System;
using IMMRequest.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace IMMRequest.DataAccess.Migrations
{
    [DbContext(typeof(IMMRequestContext))]
    [Migration("20200514203554_CreateIMMRequest")]
    partial class CreateIMMRequest
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("IMMRequest.Domain.AFValue", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AdditionalFieldID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RequestId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AdditionalFieldID");

                    b.HasIndex("RequestId");

                    b.ToTable("AFValues");
                });

            modelBuilder.Entity("IMMRequest.Domain.AdditionalField", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("FieldType")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("TypeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TypeId");

                    b.ToTable("AdditionalFields");
                });

            modelBuilder.Entity("IMMRequest.Domain.Admin", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasFilter("[Email] IS NOT NULL");

                    b.ToTable("Administrators");
                });

            modelBuilder.Entity("IMMRequest.Domain.Area", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Areas");
                });

            modelBuilder.Entity("IMMRequest.Domain.Log", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ActionType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("AdminId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("IMMRequest.Domain.Range", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AdditionalFieldId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AdditionalFieldId");

                    b.ToTable("RangeValues");
                });

            modelBuilder.Entity("IMMRequest.Domain.Request", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Details")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RequestNumber")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<Guid>("TypeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TypeId");

                    b.ToTable("Requests");
                });

            modelBuilder.Entity("IMMRequest.Domain.Session", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AdminId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("IMMRequest.Domain.Topic", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AreaId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AreaId");

                    b.ToTable("Topics");
                });

            modelBuilder.Entity("IMMRequest.Domain.Type", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("TopicId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TopicId");

                    b.ToTable("Types");
                });

            modelBuilder.Entity("IMMRequest.Domain.AFValue", b =>
                {
                    b.HasOne("IMMRequest.Domain.AdditionalField", "AdditionalField")
                        .WithMany()
                        .HasForeignKey("AdditionalFieldID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("IMMRequest.Domain.Request", "Request")
                        .WithMany("AddFieldValues")
                        .HasForeignKey("RequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("IMMRequest.Domain.AdditionalField", b =>
                {
                    b.HasOne("IMMRequest.Domain.Type", "Type")
                        .WithMany("AdditionalFields")
                        .HasForeignKey("TypeId");
                });

            modelBuilder.Entity("IMMRequest.Domain.Log", b =>
                {
                    b.HasOne("IMMRequest.Domain.Admin", "Admin")
                        .WithMany()
                        .HasForeignKey("AdminId");
                });

            modelBuilder.Entity("IMMRequest.Domain.Range", b =>
                {
                    b.HasOne("IMMRequest.Domain.AdditionalField", "AdditionalField")
                        .WithMany("Range")
                        .HasForeignKey("AdditionalFieldId");
                });

            modelBuilder.Entity("IMMRequest.Domain.Request", b =>
                {
                    b.HasOne("IMMRequest.Domain.Type", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("IMMRequest.Domain.Topic", b =>
                {
                    b.HasOne("IMMRequest.Domain.Area", "Area")
                        .WithMany("Topics")
                        .HasForeignKey("AreaId");
                });

            modelBuilder.Entity("IMMRequest.Domain.Type", b =>
                {
                    b.HasOne("IMMRequest.Domain.Topic", "Topic")
                        .WithMany("Types")
                        .HasForeignKey("TopicId");
                });
#pragma warning restore 612, 618
        }
    }
}
