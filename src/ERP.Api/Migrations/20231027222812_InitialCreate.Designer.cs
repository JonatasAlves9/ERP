﻿// <auto-generated />
using System;
using ERP.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ERP.Api.Migrations
{
    [DbContext(typeof(ErpContext))]
    [Migration("20231027222812_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("Latin1_General_CI_AI")
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ERP.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Avatar")
                        .HasColumnType("text")
                        .HasColumnName("avatar");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<Guid?>("DeletedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("deleted_by");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<Guid?>("InvitedBy")
                        .HasColumnType("uuid")
                        .HasColumnName("invited_by");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<DateTime>("RevokeIn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("revoke_in");

                    b.Property<string>("Token")
                        .HasColumnType("text")
                        .HasColumnName("token");

                    b.Property<string>("TokenRefresh")
                        .HasColumnType("text")
                        .HasColumnName("token_refresh");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.HasKey("Id");

                    b.ToTable("users");
                });

            modelBuilder.Entity("ERP.Domain.Entities.Workspace", b =>
                {
                    b.Property<Guid>("WorkspaceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("AdminId")
                        .HasColumnType("uuid")
                        .HasColumnName("admin_id");

                    b.Property<string>("BusinessColor")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("business_color");

                    b.Property<string>("BusinessLogo")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("business_logo");

                    b.Property<string>("BusinessName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("business_name");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<char>("TemplateMode")
                        .HasColumnType("character(1)")
                        .HasColumnName("template_mode");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("WorkspaceId");

                    b.HasIndex("AdminId");

                    b.ToTable("workspaces");
                });

            modelBuilder.Entity("ERP.Domain.Entities.Workspace", b =>
                {
                    b.HasOne("ERP.Domain.Entities.User", "Admin")
                        .WithMany()
                        .HasForeignKey("AdminId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Admin");
                });
#pragma warning restore 612, 618
        }
    }
}