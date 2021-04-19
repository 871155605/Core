using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging.Debug;

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

        public virtual DbSet<SysMenu> SysMenus { get; set; }
        public virtual DbSet<SysRole> SysRoles { get; set; }
        public virtual DbSet<SysRoleMenu> SysRoleMenus { get; set; }
        public virtual DbSet<SysUser> SysUsers { get; set; }
        public virtual DbSet<SysUserRole> SysUserRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Chinese_PRC_CI_AS");

            modelBuilder.Entity<SysMenu>(entity =>
            {
                entity.ToTable("sys_menu");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

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
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.Permission)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.Pid).HasColumnName("PID");
            });

            modelBuilder.Entity<SysRole>(entity =>
            {
                entity.ToTable("sys_role");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID")
                    .HasComment(" ");

                entity.Property(e => e.Code)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.Remark)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasComment("备注");
            });

            modelBuilder.Entity<SysRoleMenu>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("sys_role_menu");

                entity.HasIndex(e => new { e.SysRoleId, e.SysMenuId }, "index_role_menu")
                    .IsUnique();

                entity.Property(e => e.SysMenuId).HasComment("菜单ID");

                entity.Property(e => e.SysRoleId).HasComment("角色ID");

                entity.HasOne(d => d.SysMenu)
                    .WithMany()
                    .HasForeignKey(d => d.SysMenuId)
                    .HasConstraintName("fk_rm_menu");

                entity.HasOne(d => d.SysRole)
                    .WithMany()
                    .HasForeignKey(d => d.SysRoleId)
                    .HasConstraintName("fk_rm_role");
            });

            modelBuilder.Entity<SysUser>(entity =>
            {
                entity.ToTable("sys_user");

                entity.HasIndex(e => e.Account, "index_u_a")
                    .IsUnique();

                entity.HasIndex(e => new { e.Account, e.Password }, "index_u_a_p");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Account)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.Avatar)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.Birthday).HasColumnType("date");

                entity.Property(e => e.Email)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.NickName)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(64)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SysUserRole>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("sys_user_role");

                entity.HasIndex(e => new { e.SysUserId, e.SysRoleId }, "index_user_role")
                    .IsUnique();

                entity.HasOne(d => d.SysRole)
                    .WithMany()
                    .HasForeignKey(d => d.SysRoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ur_role");

                entity.HasOne(d => d.SysUser)
                    .WithMany()
                    .HasForeignKey(d => d.SysUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ur_user");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
