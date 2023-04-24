using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TriviaAPI.Models;

public partial class Sistem21TriviaContext : DbContext
{
    public Sistem21TriviaContext()
    {
    }

    public Sistem21TriviaContext(DbContextOptions<Sistem21TriviaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Preguntas> Preguntas { get; set; }

    public virtual DbSet<Respuestaserroneas> Respuestaserroneas { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8_general_ci")
            .HasCharSet("utf8");

        modelBuilder.Entity<Preguntas>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("preguntas");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Pregunta).HasMaxLength(255);
            entity.Property(e => e.Respuesta).HasColumnType("tinytext");
        });

        modelBuilder.Entity<Respuestaserroneas>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("respuestaserroneas");

            entity.HasIndex(e => e.Idpregunta, "fkPregunta_Respuesta_idx");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Idpregunta)
                .HasColumnType("int(11)")
                .HasColumnName("idpregunta");
            entity.Property(e => e.Respuesta).HasColumnType("tinytext");

            entity.HasOne(d => d.IdpreguntaNavigation).WithMany(p => p.Respuestaserroneas)
                .HasForeignKey(d => d.Idpregunta)
                .HasConstraintName("fkPregunta_Respuesta");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
