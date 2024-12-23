﻿// <auto-generated />
using System;
using CaminoManager.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using NpgsqlTypes;

#nullable disable

namespace CaminoManager.Data.Migrations
{
    [DbContext(typeof(CaminoManagerDbContext))]
    partial class CaminoManagerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CaminoManager.Data.Models.Belong", b =>
                {
                    b.Property<Guid>("PersonId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CommunityId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TeamId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsResponsibleForTheTeam")
                        .HasColumnType("boolean");

                    b.HasKey("PersonId", "CommunityId", "TeamId");

                    b.HasIndex("CommunityId");

                    b.HasIndex("TeamId");

                    b.ToTable("Belongs", (string)null);
                });

            modelBuilder.Entity("CaminoManager.Data.Models.Brother", b =>
                {
                    b.Property<Guid>("PersonId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CommunityId")
                        .HasColumnType("uuid");

                    b.HasKey("PersonId", "CommunityId");

                    b.HasIndex("CommunityId");

                    b.ToTable("Brothers", (string)null);
                });

            modelBuilder.Entity("CaminoManager.Data.Models.City", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CountryId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("StateId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.HasIndex("StateId");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("CaminoManager.Data.Models.Community", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("ActualBrothers")
                        .HasColumnType("integer");

                    b.Property<int>("BornBrothers")
                        .HasColumnType("integer");

                    b.Property<DateTime>("BornDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("CatechistTeamId")
                        .HasColumnType("uuid");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("OldCatechist")
                        .HasColumnType("text");

                    b.Property<Guid>("ParishId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("StepWayDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("StepWayId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Communities", (string)null);
                });

            modelBuilder.Entity("CaminoManager.Data.Models.Country", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("CaminoManager.Data.Models.Parish", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("CityId")
                        .HasColumnType("uuid");

                    b.Property<string>("Diocese")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("Parishes", (string)null);
                });

            modelBuilder.Entity("CaminoManager.Data.Models.ParishTeam", b =>
                {
                    b.Property<Guid>("ParishId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TeamId")
                        .HasColumnType("uuid");

                    b.HasKey("ParishId", "TeamId");

                    b.HasIndex("TeamId");

                    b.ToTable("ParishTeams", (string)null);
                });

            modelBuilder.Entity("CaminoManager.Data.Models.Person", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<int>("GenderId")
                        .HasColumnType("integer");

                    b.Property<string>("Mobile")
                        .HasColumnType("text");

                    b.Property<string>("PersonName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PersonTypeId")
                        .HasColumnType("integer");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.Property<NpgsqlTsVector>("SearchVector")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("tsvector")
                        .HasAnnotation("Npgsql:TsVectorConfig", "spanish")
                        .HasAnnotation("Npgsql:TsVectorProperties", new[] { "PersonName" });

                    b.Property<Guid?>("SpouseId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("SearchVector");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex("SearchVector"), "GIN");

                    b.HasIndex("SpouseId");

                    b.ToTable("People", (string)null);
                });

            modelBuilder.Entity("CaminoManager.Data.Models.Priest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsParishPriest")
                        .HasColumnType("boolean");

                    b.Property<Guid>("ParishId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("PersonId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ParishId");

                    b.HasIndex("PersonId");

                    b.ToTable("Priests", (string)null);
                });

            modelBuilder.Entity("CaminoManager.Data.Models.State", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CountryId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("States");
                });

            modelBuilder.Entity("CaminoManager.Data.Models.StepWay", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Order")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("StepWays", (string)null);
                });

            modelBuilder.Entity("CaminoManager.Data.Models.Team", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CommunityId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("TeamTypeId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CommunityId");

                    b.HasIndex("TeamTypeId");

                    b.ToTable("Teams", (string)null);
                });

            modelBuilder.Entity("CaminoManager.Data.Models.TeamType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Order")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("TeamTypes", (string)null);
                });

            modelBuilder.Entity("CaminoManager.Data.Models.Belong", b =>
                {
                    b.HasOne("CaminoManager.Data.Models.Community", "Community")
                        .WithMany()
                        .HasForeignKey("CommunityId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CaminoManager.Data.Models.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CaminoManager.Data.Models.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CaminoManager.Data.Models.Brother", null)
                        .WithMany("Belongs")
                        .HasForeignKey("PersonId", "CommunityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Community");

                    b.Navigation("Person");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("CaminoManager.Data.Models.Brother", b =>
                {
                    b.HasOne("CaminoManager.Data.Models.Community", "Community")
                        .WithMany("Brothers")
                        .HasForeignKey("CommunityId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CaminoManager.Data.Models.Person", "Person")
                        .WithMany("Brothers")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Community");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("CaminoManager.Data.Models.City", b =>
                {
                    b.HasOne("CaminoManager.Data.Models.Country", "Country")
                        .WithMany("Cities")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CaminoManager.Data.Models.State", "State")
                        .WithMany("Cities")
                        .HasForeignKey("StateId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Country");

                    b.Navigation("State");
                });

            modelBuilder.Entity("CaminoManager.Data.Models.Parish", b =>
                {
                    b.HasOne("CaminoManager.Data.Models.City", "City")
                        .WithMany()
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("CaminoManager.Data.Models.ParishTeam", b =>
                {
                    b.HasOne("CaminoManager.Data.Models.Parish", "Parish")
                        .WithMany("ParishTeams")
                        .HasForeignKey("ParishId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CaminoManager.Data.Models.Team", "Team")
                        .WithMany("ParishTeams")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Parish");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("CaminoManager.Data.Models.Person", b =>
                {
                    b.HasOne("CaminoManager.Data.Models.Person", "Spouse")
                        .WithMany()
                        .HasForeignKey("SpouseId");

                    b.Navigation("Spouse");
                });

            modelBuilder.Entity("CaminoManager.Data.Models.Priest", b =>
                {
                    b.HasOne("CaminoManager.Data.Models.Parish", "Parish")
                        .WithMany("Priests")
                        .HasForeignKey("ParishId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CaminoManager.Data.Models.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Parish");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("CaminoManager.Data.Models.State", b =>
                {
                    b.HasOne("CaminoManager.Data.Models.Country", "Country")
                        .WithMany("States")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("CaminoManager.Data.Models.Team", b =>
                {
                    b.HasOne("CaminoManager.Data.Models.Community", "Community")
                        .WithMany()
                        .HasForeignKey("CommunityId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CaminoManager.Data.Models.TeamType", "TeamType")
                        .WithMany("Teams")
                        .HasForeignKey("TeamTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Community");

                    b.Navigation("TeamType");
                });

            modelBuilder.Entity("CaminoManager.Data.Models.Brother", b =>
                {
                    b.Navigation("Belongs");
                });

            modelBuilder.Entity("CaminoManager.Data.Models.Community", b =>
                {
                    b.Navigation("Brothers");
                });

            modelBuilder.Entity("CaminoManager.Data.Models.Country", b =>
                {
                    b.Navigation("Cities");

                    b.Navigation("States");
                });

            modelBuilder.Entity("CaminoManager.Data.Models.Parish", b =>
                {
                    b.Navigation("ParishTeams");

                    b.Navigation("Priests");
                });

            modelBuilder.Entity("CaminoManager.Data.Models.Person", b =>
                {
                    b.Navigation("Brothers");
                });

            modelBuilder.Entity("CaminoManager.Data.Models.State", b =>
                {
                    b.Navigation("Cities");
                });

            modelBuilder.Entity("CaminoManager.Data.Models.Team", b =>
                {
                    b.Navigation("ParishTeams");
                });

            modelBuilder.Entity("CaminoManager.Data.Models.TeamType", b =>
                {
                    b.Navigation("Teams");
                });
#pragma warning restore 612, 618
        }
    }
}
