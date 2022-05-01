﻿// <auto-generated />
using System;
using Db;
using Db.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Db.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "discord_entity_type", new[] { "channel", "role" });
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Db.Models.Config", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ConfigOptionId")
                        .HasColumnType("integer")
                        .HasColumnName("config_option_id");

                    b.Property<decimal>("GuildId")
                        .HasColumnType("numeric(20,0)")
                        .HasColumnName("guild_id");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("value");

                    b.HasKey("Id")
                        .HasName("pk_configs");

                    b.HasIndex("GuildId")
                        .HasDatabaseName("ix_configs_guild_id");

                    b.ToTable("configs", (string)null);
                });

            modelBuilder.Entity("Db.Models.DiscordEntity", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("numeric(20,0)")
                        .HasColumnName("id");

                    b.Property<DiscordEntityType>("DiscordEntityType")
                        .HasColumnType("discord_entity_type")
                        .HasColumnName("discord_entity_type");

                    b.Property<decimal>("GuildId")
                        .HasColumnType("numeric(20,0)")
                        .HasColumnName("guild_id");

                    b.HasKey("Id")
                        .HasName("pk_discord_entities");

                    b.HasIndex("GuildId")
                        .HasDatabaseName("ix_discord_entities_guild_id");

                    b.ToTable("discord_entities", (string)null);
                });

            modelBuilder.Entity("Db.Models.Guild", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("numeric(20,0)")
                        .HasColumnName("id");

                    b.HasKey("Id")
                        .HasName("pk_guilds");

                    b.ToTable("guilds", (string)null);
                });

            modelBuilder.Entity("Db.Models.Member", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("numeric(20,0)")
                        .HasColumnName("id");

                    b.Property<decimal>("GuildId")
                        .HasColumnType("numeric(20,0)")
                        .HasColumnName("guild_id");

                    b.HasKey("Id")
                        .HasName("pk_members");

                    b.HasIndex("GuildId")
                        .HasDatabaseName("ix_members_guild_id");

                    b.ToTable("members", (string)null);
                });

            modelBuilder.Entity("Db.Models.Quote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date");

                    b.Property<decimal>("MemberId")
                        .HasColumnType("numeric(20,0)")
                        .HasColumnName("member_id");

                    b.Property<decimal>("MessageId")
                        .HasColumnType("numeric(20,0)")
                        .HasColumnName("message_id");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("text");

                    b.HasKey("Id")
                        .HasName("pk_quotes");

                    b.HasIndex("MemberId")
                        .HasDatabaseName("ix_quotes_member_id");

                    b.ToTable("quotes", (string)null);
                });

            modelBuilder.Entity("Db.Models.SelfAssignMenu", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<decimal>("GuildId")
                        .HasColumnType("numeric(20,0)")
                        .HasColumnName("guild_id");

                    b.Property<decimal?>("RequiredRoleId")
                        .HasColumnType("numeric(20,0)")
                        .HasColumnName("required_role_id");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_self_assign_menus");

                    b.HasIndex("GuildId")
                        .HasDatabaseName("ix_self_assign_menus_guild_id");

                    b.HasIndex("RequiredRoleId")
                        .HasDatabaseName("ix_self_assign_menus_required_role_id");

                    b.ToTable("self_assign_menus", (string)null);
                });

            modelBuilder.Entity("Db.Models.SelfAssignMenuDiscordEntityAssignment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal>("DiscordEntityId")
                        .HasColumnType("numeric(20,0)")
                        .HasColumnName("discord_entity_id");

                    b.Property<int>("SelfAssignMenuId")
                        .HasColumnType("integer")
                        .HasColumnName("self_assign_menu_id");

                    b.HasKey("Id")
                        .HasName("pk_self_assign_menu_discord_entity_assignments");

                    b.HasIndex("DiscordEntityId")
                        .HasDatabaseName("ix_self_assign_menu_discord_entity_assignments_discord_entity_");

                    b.HasIndex("SelfAssignMenuId")
                        .HasDatabaseName("ix_self_assign_menu_discord_entity_assignments_self_assign_men");

                    b.ToTable("self_assign_menu_discord_entity_assignments", (string)null);
                });

            modelBuilder.Entity("Db.Models.Config", b =>
                {
                    b.HasOne("Db.Models.Guild", "Guild")
                        .WithMany("Configs")
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_configs_guilds_guild_id");

                    b.Navigation("Guild");
                });

            modelBuilder.Entity("Db.Models.DiscordEntity", b =>
                {
                    b.HasOne("Db.Models.Guild", "Guild")
                        .WithMany("DiscordEntities")
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_discord_entities_guilds_guild_id");

                    b.Navigation("Guild");
                });

            modelBuilder.Entity("Db.Models.Member", b =>
                {
                    b.HasOne("Db.Models.Guild", "Guild")
                        .WithMany("Members")
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_members_guilds_guild_id");

                    b.Navigation("Guild");
                });

            modelBuilder.Entity("Db.Models.Quote", b =>
                {
                    b.HasOne("Db.Models.Member", "Member")
                        .WithMany("Quotes")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_quotes_members_member_id");

                    b.Navigation("Member");
                });

            modelBuilder.Entity("Db.Models.SelfAssignMenu", b =>
                {
                    b.HasOne("Db.Models.Guild", "Guild")
                        .WithMany("SelfAssignMenus")
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_self_assign_menus_guilds_guild_id");

                    b.HasOne("Db.Models.DiscordEntity", "RequiredRole")
                        .WithMany("SelfAssignMenus")
                        .HasForeignKey("RequiredRoleId")
                        .HasConstraintName("fk_self_assign_menus_discord_entities_required_role_id");

                    b.Navigation("Guild");

                    b.Navigation("RequiredRole");
                });

            modelBuilder.Entity("Db.Models.SelfAssignMenuDiscordEntityAssignment", b =>
                {
                    b.HasOne("Db.Models.DiscordEntity", "DiscordEntity")
                        .WithMany("SelfAssignMenuDiscordEntityAssignments")
                        .HasForeignKey("DiscordEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_self_assign_menu_discord_entity_assignments_discord_entitie");

                    b.HasOne("Db.Models.SelfAssignMenu", "SelfAssignMenu")
                        .WithMany("SelfAssignMenuDiscordEntityAssignments")
                        .HasForeignKey("SelfAssignMenuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_self_assign_menu_discord_entity_assignments_self_assign_men");

                    b.Navigation("DiscordEntity");

                    b.Navigation("SelfAssignMenu");
                });

            modelBuilder.Entity("Db.Models.DiscordEntity", b =>
                {
                    b.Navigation("SelfAssignMenuDiscordEntityAssignments");

                    b.Navigation("SelfAssignMenus");
                });

            modelBuilder.Entity("Db.Models.Guild", b =>
                {
                    b.Navigation("Configs");

                    b.Navigation("DiscordEntities");

                    b.Navigation("Members");

                    b.Navigation("SelfAssignMenus");
                });

            modelBuilder.Entity("Db.Models.Member", b =>
                {
                    b.Navigation("Quotes");
                });

            modelBuilder.Entity("Db.Models.SelfAssignMenu", b =>
                {
                    b.Navigation("SelfAssignMenuDiscordEntityAssignments");
                });
#pragma warning restore 612, 618
        }
    }
}
