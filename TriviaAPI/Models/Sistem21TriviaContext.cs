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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=sistemas19.com;database=sistem21_trivia;user=sistem21_trivia;password=sistemas19_", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.5.17-mariadb"));

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
            entity.Property(e => e.Imagen)
                .HasColumnType("text")
                .HasColumnName("imagen");
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
