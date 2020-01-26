﻿// <auto-generated />
using System;
using GeoDoorServer3.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GeoDoorServer3.Migrations.UserDb
{
    [DbContext(typeof(UserDbContext))]
    [Migration("20200125185941_migration3")]
    partial class migration3
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1");

            modelBuilder.Entity("GeoDoorServer3.Models.DataModels.ConnectionLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Message")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("MsgDateTime")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ConnectionLogs");
                });

            modelBuilder.Entity("GeoDoorServer3.Models.DataModels.ErrorLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("LogLevel")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Message")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("MsgDateTime")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ErrorLogs");
                });

            modelBuilder.Entity("GeoDoorServer3.Models.DataModels.Settings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("DoorOpenHabLink")
                        .HasColumnType("TEXT");

                    b.Property<string>("GateOpenHabLink")
                        .HasColumnType("TEXT");

                    b.Property<int>("GateTimeout")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaxErrorLogRows")
                        .HasColumnType("INTEGER");

                    b.Property<string>("StatusOpenHabLink")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("GeoDoorServer3.Models.DataModels.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AccessRights")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastConnection")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PhoneId")
                        .IsUnique();

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
