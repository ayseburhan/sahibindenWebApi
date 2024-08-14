using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace sahibindenWebApi.Models;

public partial class DBsahibindenContext : DbContext
{
    public DBsahibindenContext()
    {
    }

    public DBsahibindenContext(DbContextOptions<DBsahibindenContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Favori> Favoris { get; set; }

    public virtual DbSet<Kategori> Kategoris { get; set; }

    public virtual DbSet<Kullanici> Kullanicis { get; set; }

    public virtual DbSet<Sepet> Sepets { get; set; }

    public virtual DbSet<Urunler> Urunlers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\localDbBBB;Initial Catalog=dBsahibinden");

    public override EntityEntry<TEntity> Update<TEntity>(TEntity entity)
    {
        return base.Update(entity);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Favori>(entity =>
        {
            entity.ToTable("favori");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.KullaniciId).HasColumnName("kullaniciId");
            entity.Property(e => e.UrunId).HasColumnName("urunId");
        });

        modelBuilder.Entity<Kategori>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__kategori__3213E83FE9AC4163");

            entity.ToTable("kategori");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.KategoriAdi)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("kategoriAdi");
        });

        modelBuilder.Entity<Kullanici>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__kullanic__E011F77B073D8F2E");

            entity.ToTable("kullanici");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.KullaniciAdi)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.KullaniciAdres).HasColumnType("text");
            entity.Property(e => e.KullaniciMail)
                .HasMaxLength(30)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.KullaniciSifre).HasMaxLength(100);
            entity.Property(e => e.KullaniciSoyadi)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Sepet>(entity =>
        {
            entity.ToTable("sepet");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.KullaniciId).HasColumnName("kullaniciId");
            entity.Property(e => e.UrunId).HasColumnName("urunId");
        });

        modelBuilder.Entity<Urunler>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__urunler__3213E83F5DF9542D");

            entity.ToTable("urunler");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EklenmeTarihi)
                .HasColumnType("datetime")
                .HasColumnName("eklenmeTarihi");
            entity.Property(e => e.KategoriId).HasColumnName("kategoriId");
            entity.Property(e => e.UrunAciklama)
                .HasColumnType("text")
                .HasColumnName("urunAciklama");
            entity.Property(e => e.UrunAdi)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("urunAdi");
            entity.Property(e => e.UrunDurum)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("urunDurum");
            entity.Property(e => e.UrunFiyat)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("urunFiyat");
            entity.Property(e => e.UrunGorsel).HasColumnName("urunGorsel");
            entity.Property(e => e.UrunStok).HasColumnName("urunStok");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
