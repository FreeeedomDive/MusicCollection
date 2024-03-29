﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MusicCollection.BusinessLogic.Repositories.Database;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MusicCollection.BusinessLogic.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20221113162548_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc.1.22426.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MusicCollection.BusinessLogic.Repositories.Auth.UserStorageElement", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("UsersStorage");
                });

            modelBuilder.Entity("MusicCollection.BusinessLogic.Repositories.Files.Nodes.NodeStorageElement", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Hidden")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uuid");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Id", "Path");

                    b.ToTable("NodesStorage");
                });

            modelBuilder.Entity("MusicCollection.BusinessLogic.Repositories.Files.Roots.RootStorageElement", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("RootsStorage");
                });

            modelBuilder.Entity("MusicCollection.BusinessLogic.Repositories.Files.Tags.AudioFileTagsStorageElement", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Album")
                        .HasColumnType("text");

                    b.Property<string>("Artist")
                        .HasColumnType("text");

                    b.Property<int?>("BitDepth")
                        .HasColumnType("integer");

                    b.Property<int?>("BitRate")
                        .HasColumnType("integer");

                    b.Property<string>("Duration")
                        .HasColumnType("text");

                    b.Property<string>("Format")
                        .HasColumnType("text");

                    b.Property<int?>("SampleFrequency")
                        .HasColumnType("integer");

                    b.Property<string>("TrackName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("TagsStorage");
                });
#pragma warning restore 612, 618
        }
    }
}
