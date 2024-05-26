using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using apbd5.Models;

namespace apbd5.Context
{
    public partial class Context : DbContext
    {
        public Context()
        {
        }

        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Client> Clients { get; set; } = null!;
        public virtual DbSet<ClientTrip> ClientTrips { get; set; } = null!;
        public virtual DbSet<Country> Countries { get; set; } = null!;
        public virtual DbSet<Trip> Trips { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureClientEntity(modelBuilder);
            ConfigureClientTripEntity(modelBuilder);
            ConfigureCountryEntity(modelBuilder);
            ConfigureTripEntity(modelBuilder);

            OnModelCreatingPartial(modelBuilder);
        }

        private static void ConfigureClientEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.IdClient)
                    .HasName("Client_pk");

                entity.ToTable("Client", "trip");

                entity.Property(e => e.IdClient).ValueGeneratedNever();
                entity.Property(e => e.Email).HasMaxLength(50);
                entity.Property(e => e.FirstName).HasMaxLength(50);
                entity.Property(e => e.LastName).HasMaxLength(50);
                entity.Property(e => e.Pesel).HasMaxLength(50);
                entity.Property(e => e.Telephone).HasMaxLength(50);
            });
        }

        private static void ConfigureClientTripEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientTrip>(entity =>
            {
                entity.HasKey(e => new { e.IdClient, e.IdTrip })
                    .HasName("Client_Trip_pk");

                entity.ToTable("Client_Trip", "trip");

                entity.Property(e => e.PaymentDate).HasColumnType("datetime");
                entity.Property(e => e.RegisteredAt).HasColumnType("datetime");

                entity.HasOne(d => d.IdClientNavigation)
                    .WithMany(p => p.ClientTrips)
                    .HasForeignKey(d => d.IdClient)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Table_5_Client");

                entity.HasOne(d => d.IdTripNavigation)
                    .WithMany(p => p.ClientTrips)
                    .HasForeignKey(d => d.IdTrip)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Table_5_Trip");
            });
        }

        private static void ConfigureCountryEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.IdCountry)
                    .HasName("Country_pk");

                entity.ToTable("Country", "trip");

                entity.Property(e => e.IdCountry).ValueGeneratedNever();
                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasMany(d => d.IdTrips)
                    .WithMany(p => p.IdCountries)
                    .UsingEntity<Dictionary<string, object>>(
                        "CountryTrip",
                        l => l.HasOne<Trip>().WithMany().HasForeignKey("IdTrip").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("Country_Trip_Trip"),
                        r => r.HasOne<Country>().WithMany().HasForeignKey("IdCountry").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("Country_Trip_Country"),
                        j =>
                        {
                            j.HasKey("IdCountry", "IdTrip").HasName("Country_Trip_pk");
                            j.ToTable("Country_Trip", "trip");
                        });
            });
        }

        private static void ConfigureTripEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trip>(entity =>
            {
                entity.HasKey(e => e.IdTrip)
                    .HasName("Trip_pk");

                entity.ToTable("Trip", "trip");

                entity.Property(e => e.IdTrip).ValueGeneratedNever();
                entity.Property(e => e.DateFrom).HasColumnType("datetime");
                entity.Property(e => e.DateTo).HasColumnType("datetime");
                entity.Property(e => e.Description).HasMaxLength(50);
                entity.Property(e => e.Name).HasMaxLength(50);
            });
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}