using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace JR_NK_MVC_Core.Entities
{
    public partial class JRDBContext : DbContext
    {
        public JRDBContext()
        {
        }

        public JRDBContext(DbContextOptions<JRDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AdminMenu> AdminMenus { get; set; }
        public virtual DbSet<AdminRole> AdminRoles { get; set; }
        public virtual DbSet<AdminRoleMenu> AdminRoleMenus { get; set; }
        public virtual DbSet<AdminUser> AdminUsers { get; set; }
        public virtual DbSet<AdminUserRole> AdminUserRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Chinese_PRC_CI_AS");

            modelBuilder.Entity<AdminMenu>(entity =>
            {
                entity.ToTable("admin_menu");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Code)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.Icon)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Link)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Permission)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.Pid).HasColumnName("PID");

                entity.Property(e => e.Type).HasComment("1级菜单2二级菜单3三级菜单4页面按钮");
            });

            modelBuilder.Entity<AdminRole>(entity =>
            {
                entity.ToTable("admin_role");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Code)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Remark)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasComment("备注");
            });

            modelBuilder.Entity<AdminRoleMenu>(entity =>
            {
                entity.ToTable("admin_role_menu");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.MenuId).HasComment("菜单ID");

                entity.Property(e => e.RoleId).HasComment("角色ID");
            });

            modelBuilder.Entity<AdminUser>(entity =>
            {
                entity.ToTable("admin_user");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Account)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NickName)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(64)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AdminUserRole>(entity =>
            {
                entity.ToTable("admin_user_role");

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
