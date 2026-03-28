using Microsoft.EntityFrameworkCore;
using MvcLab1.Models;

namespace MvcLab1.Data
{
    public class AppDbContext : DbContext
    {
        // Пустой конструктор для миграций
        public AppDbContext()
        {
        }

        // Конструктор для DI
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DbSet представляет таблицу Recipes в базе данных
        public DbSet<Recipe> Recipes { get; set; }

        // Метод для миграций
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=RecipeDb;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        // Настройка модели
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Recipe>(entity =>
            {
                // Первичный ключ
                entity.HasKey(r => r.Id);

                // Настройка поля Title
                entity.Property(r => r.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                // Настройка поля Cuisine
                entity.Property(r => r.Cuisine)
                    .IsRequired()
                    .HasMaxLength(50);

                // Настройка поля PrepTime
                entity.Property(r => r.PrepTime)
                    .IsRequired();

                // Настройка поля CookTime
                entity.Property(r => r.CookTime)
                    .IsRequired();

                // Настройка поля Difficulty
                entity.Property(r => r.Difficulty)
                    .IsRequired()
                    .HasMaxLength(50);

                // Настройка поля Ingredients - используем nvarchar(max)
                entity.Property(r => r.Ingredients)
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                // Настройка поля Instructions - используем nvarchar(max)
                entity.Property(r => r.Instructions)
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                // Настройка поля CreatedDate
                entity.Property(r => r.CreatedDate)
                    .IsRequired();

                // Индексы для быстрого поиска
                entity.HasIndex(r => r.Cuisine)
                    .HasDatabaseName("IX_Recipes_Cuisine");

                entity.HasIndex(r => r.Difficulty)
                    .HasDatabaseName("IX_Recipes_Difficulty");
            });
        }
    }
}