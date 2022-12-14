// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Wildberries.Shared;

#nullable disable

namespace Wildberries.Shared.Migrations
{
    [DbContext(typeof(WbDataBaseContext))]
    [Migration("20221229203423_AddDateOfAddition")]
    partial class AddDateOfAddition
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Wildberries.Shared.Domain.Entity.CardEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<long>("Article")
                        .HasColumnType("bigint");

                    b.Property<int>("AveragePrice")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("DateOfAddition")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("MainPrice")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<List<int>>("SalePrice")
                        .IsRequired()
                        .HasColumnType("integer[]");

                    b.Property<List<DateTimeOffset>>("Time")
                        .IsRequired()
                        .HasColumnType("timestamp with time zone[]");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Card");
                });

            modelBuilder.Entity("Wildberries.Shared.Domain.Entity.UserEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsBot")
                        .HasColumnType("boolean");

                    b.Property<string>("LanguageCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Wildberries.Shared.Domain.Entity.CardEntity", b =>
                {
                    b.HasOne("Wildberries.Shared.Domain.Entity.UserEntity", "User")
                        .WithMany("UserProduct")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Wildberries.Shared.Domain.Entity.UserEntity", b =>
                {
                    b.Navigation("UserProduct");
                });
#pragma warning restore 612, 618
        }
    }
}
