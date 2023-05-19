﻿// <auto-generated />
using System;
using HistrixAPI.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HistrixAPI.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230519000720_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HistrixAPI.Models.Entities.Bot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CryptoPairId")
                        .HasColumnType("int");

                    b.Property<int>("StrategyId")
                        .HasColumnType("int");

                    b.Property<int>("TimeframeId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CryptoPairId");

                    b.HasIndex("StrategyId");

                    b.HasIndex("TimeframeId");

                    b.HasIndex("UserId");

                    b.ToTable("Bots");
                });

            modelBuilder.Entity("HistrixAPI.Models.Entities.CandleEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Close")
                        .HasColumnType("float");

                    b.Property<int>("CryptoPairId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<double>("High")
                        .HasColumnType("float");

                    b.Property<double>("Low")
                        .HasColumnType("float");

                    b.Property<double>("Open")
                        .HasColumnType("float");

                    b.Property<int>("TimeframeId")
                        .HasColumnType("int");

                    b.Property<double>("Volume")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("CryptoPairId");

                    b.HasIndex("TimeframeId");

                    b.ToTable("Candles");
                });

            modelBuilder.Entity("HistrixAPI.Models.Entities.CryptoPair", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CryptoPairName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CryptoPairs");
                });

            modelBuilder.Entity("HistrixAPI.Models.Entities.Position", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BotId")
                        .HasColumnType("int");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int>("Side")
                        .HasColumnType("int");

                    b.Property<double>("Volume")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("BotId");

                    b.ToTable("Positions");
                });

            modelBuilder.Entity("HistrixAPI.Models.Entities.Strategy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StrategyName")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Strategies");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Strategy");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("HistrixAPI.Models.Entities.Timeframe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("TimeframeDuration")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Timeframes");
                });

            modelBuilder.Entity("HistrixAPI.Models.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Api")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecretKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("HistrixAPI.Models.Entities.GridStrategy", b =>
                {
                    b.HasBaseType("HistrixAPI.Models.Entities.Strategy");

                    b.Property<int>("LevelsCount")
                        .HasColumnType("int");

                    b.Property<double>("LevelsDistance")
                        .HasColumnType("float");

                    b.HasDiscriminator().HasValue("GridStrategy");
                });

            modelBuilder.Entity("HistrixAPI.Models.Entities.SMAStrategy", b =>
                {
                    b.HasBaseType("HistrixAPI.Models.Entities.Strategy");

                    b.Property<int>("FastSMA")
                        .HasColumnType("int");

                    b.Property<int>("SlowSMA")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("SMAStrategy");
                });

            modelBuilder.Entity("HistrixAPI.Models.Entities.Bot", b =>
                {
                    b.HasOne("HistrixAPI.Models.Entities.CryptoPair", "CryptoPair")
                        .WithMany()
                        .HasForeignKey("CryptoPairId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("HistrixAPI.Models.Entities.Strategy", "Strategy")
                        .WithMany()
                        .HasForeignKey("StrategyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("HistrixAPI.Models.Entities.Timeframe", "Timeframe")
                        .WithMany()
                        .HasForeignKey("TimeframeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("HistrixAPI.Models.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CryptoPair");

                    b.Navigation("Strategy");

                    b.Navigation("Timeframe");

                    b.Navigation("User");
                });

            modelBuilder.Entity("HistrixAPI.Models.Entities.CandleEntity", b =>
                {
                    b.HasOne("HistrixAPI.Models.Entities.CryptoPair", "CryptoPair")
                        .WithMany("Candles")
                        .HasForeignKey("CryptoPairId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HistrixAPI.Models.Entities.Timeframe", "Timeframe")
                        .WithMany("Candles")
                        .HasForeignKey("TimeframeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CryptoPair");

                    b.Navigation("Timeframe");
                });

            modelBuilder.Entity("HistrixAPI.Models.Entities.Position", b =>
                {
                    b.HasOne("HistrixAPI.Models.Entities.Bot", "Bot")
                        .WithMany()
                        .HasForeignKey("BotId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Bot");
                });

            modelBuilder.Entity("HistrixAPI.Models.Entities.CryptoPair", b =>
                {
                    b.Navigation("Candles");
                });

            modelBuilder.Entity("HistrixAPI.Models.Entities.Timeframe", b =>
                {
                    b.Navigation("Candles");
                });
#pragma warning restore 612, 618
        }
    }
}
