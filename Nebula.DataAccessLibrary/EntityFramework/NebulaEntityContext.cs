using Microsoft.EntityFrameworkCore;
using Nebula.CoreLibrary;
using Nebula.CoreLibrary.Shared;
using Nebula.CoreLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nebula.DataAccessLibrary.EntityFramework
{
    public class NebulaEntityContext : DbContext
    {
        public NebulaEntityContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ApplicationSettings.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            List<Type> entityTypes = ReflectionUtility.FindSubClassesOf<EntityBase>().ToList();
            modelBuilder.Entity<EntityBase>().HasQueryFilter(v => !v.IsDeleted);
            foreach (var entityType in entityTypes)
            {
                
                modelBuilder.Entity(entityType).ToTable(entityType.Name); 
            }

            base.OnModelCreating(modelBuilder);

        }

        
    }
}
