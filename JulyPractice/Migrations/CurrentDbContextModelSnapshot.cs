﻿// <auto-generated />
using System;
using JulyPractice;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace JulyPractice.Migrations
{
    [DbContext(typeof(CurrentDbContext))]
    partial class CurrentDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.6");

            modelBuilder.Entity("JulyPractice.Album", b =>
                {
                    b.Property<Guid>("AlbumID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("MusicianID")
                        .HasColumnType("TEXT");

                    b.Property<int>("ReleaseYear")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("AlbumID");

                    b.HasIndex("MusicianID");

                    b.ToTable("Albums");
                });

            modelBuilder.Entity("JulyPractice.Country", b =>
                {
                    b.Property<int>("CountryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CountryName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("CountryID");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("JulyPractice.Musician", b =>
                {
                    b.Property<Guid>("MusicianID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("CountryID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("MusicianID");

                    b.HasIndex("CountryID");

                    b.ToTable("Musicians");
                });

            modelBuilder.Entity("JulyPractice.Song", b =>
                {
                    b.Property<Guid>("SongID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("AlbumID")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("MusicianID")
                        .HasColumnType("TEXT");

                    b.Property<int>("ReleaseYear")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("SongID");

                    b.HasIndex("AlbumID");

                    b.HasIndex("MusicianID");

                    b.ToTable("Songs");
                });

            modelBuilder.Entity("JulyPractice.User", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("JulyPractice.Album", b =>
                {
                    b.HasOne("JulyPractice.Musician", "Musician")
                        .WithMany("Albums")
                        .HasForeignKey("MusicianID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Musician");
                });

            modelBuilder.Entity("JulyPractice.Musician", b =>
                {
                    b.HasOne("JulyPractice.Country", "Country")
                        .WithMany("Musicians")
                        .HasForeignKey("CountryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("JulyPractice.Song", b =>
                {
                    b.HasOne("JulyPractice.Album", "Album")
                        .WithMany("Songs")
                        .HasForeignKey("AlbumID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("JulyPractice.Musician", "Musician")
                        .WithMany("Songs")
                        .HasForeignKey("MusicianID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Album");

                    b.Navigation("Musician");
                });

            modelBuilder.Entity("JulyPractice.Album", b =>
                {
                    b.Navigation("Songs");
                });

            modelBuilder.Entity("JulyPractice.Country", b =>
                {
                    b.Navigation("Musicians");
                });

            modelBuilder.Entity("JulyPractice.Musician", b =>
                {
                    b.Navigation("Albums");

                    b.Navigation("Songs");
                });
#pragma warning restore 612, 618
        }
    }
}
