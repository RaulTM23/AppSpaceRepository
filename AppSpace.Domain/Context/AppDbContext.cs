using System;
using System.Collections.Generic;
using AppSpace.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AppSpace.Domain.Context;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cinema> Cinemas { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<MovieGenre> MovieGenres { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:AppSpace");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cinema>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cinema__3214EC075BD52C08");

            entity.ToTable("Cinema");

            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.OpenSince).HasColumnType("datetime");

            entity.HasOne(d => d.City).WithMany(p => p.Cinemas)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cinema_City");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__City__3214EC07348A6ED0");

            entity.ToTable("City");

            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Genre__3214EC078FF51A6D");

            entity.ToTable("Genre");

            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Movie__3214EC0796FF4E1C");

            entity.ToTable("Movie");

            entity.Property(e => e.OriginalLanguage).HasMaxLength(255);
            entity.Property(e => e.OriginalTitle).HasMaxLength(512);
            entity.Property(e => e.ReleaseDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<MovieGenre>(entity =>
        {
            entity.HasKey(e => new { e.MovieId, e.GenreId });

            entity.ToTable("MovieGenre");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Room__3214EC076FE9599B");

            entity.ToTable("Room");

            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Size).HasMaxLength(255);

            entity.HasOne(d => d.Cinema).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.CinemaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Room_Cinema");
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Session__3214EC071C1F0547");

            entity.ToTable("Session");

            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.StartTime).HasColumnType("datetime");

            entity.HasOne(d => d.Movie).WithMany(p => p.Sessions)
                .HasForeignKey(d => d.MovieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Session_Movie");

            entity.HasOne(d => d.Room).WithMany(p => p.Sessions)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Session_Room");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
