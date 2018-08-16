using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Nebula.DataAccessLibrary.EntityFramework;

namespace Nebula.Membership.Repositories.Contexts
{
    public class NpgMembershipContext : NebulaNpgEntityContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.UserGroup)
                .WithMany(ug => ug.Users)
                .HasForeignKey(v => v.UserGroupId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Company).WithMany(c => c.Users)
                .HasForeignKey(v => v.CompanyId);

            modelBuilder.Entity<User>().HasMany(u => u.Employees).WithOne(u => u.Manager)
                .HasForeignKey(f => f.ManagerId);

            modelBuilder.Entity<UserGroupRole>().HasKey(u => new {u.RoleId, u.UserGroupId});
            modelBuilder.Entity<UserGroupRole>().HasOne(v => v.UserGroup)
                .WithMany(v => v.UserGroupRoles).HasForeignKey(v => v.UserGroupId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserGroupRole>().HasOne(v => v.Role).WithMany(v => v.UserGroupRoles)
                .HasForeignKey(v => v.RoleId);

            modelBuilder.Entity<UserGroup>()
                .HasOne(ug => ug.Company)
                .WithMany(c => c.UserGroups).HasForeignKey(v => v.CompanyId);

        }
    }
}
