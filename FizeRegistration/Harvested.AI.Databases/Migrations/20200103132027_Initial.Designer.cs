﻿// <auto-generated />
using System;
using Harvested.AI.Databases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Harvested.AI.Databases.Migrations
{
    [DbContext(typeof(HarvestedDbContext))]
    [Migration("20200103132027_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Harvested.AI.Domain.Entities.Identity.UserIdentity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("CanUserResetExpiredPassword");

                    b.Property<DateTime?>("Created")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("getutcdate()");

                    b.Property<string>("Email");

                    b.Property<bool>("ForceChangePassword");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsPasswordExpired");

                    b.Property<DateTime?>("LastLoggedIn");

                    b.Property<DateTime?>("LastModified");

                    b.Property<DateTime>("PasswordExpiresAt");

                    b.Property<string>("PasswordHash")
                        .IsRequired();

                    b.Property<string>("PasswordSalt")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasFilter("[Email] IS NOT NULL");

                    b.ToTable("UserIdentities");
                });
#pragma warning restore 612, 618
        }
    }
}
