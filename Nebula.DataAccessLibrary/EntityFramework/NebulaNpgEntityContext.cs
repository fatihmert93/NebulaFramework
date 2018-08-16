using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Nebula.CoreLibrary;
using Nebula.CoreLibrary.Shared;
using Nebula.CoreLibrary.Utilities;

namespace Nebula.DataAccessLibrary.EntityFramework
{
    public class NebulaNpgEntityContext : DbContext
    {
        public NebulaNpgEntityContext()
        {

        }

        public void DetachAllEntities()
        {
            var changedEntriesCopy = this.ChangeTracker.Entries()
                .ToList();
            foreach (var entity in changedEntriesCopy)
            {
                this.Entry(entity.Entity).State = EntityState.Detached;
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(ApplicationSettings.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            List<Type> entityTypes = ReflectionUtility.FindSubClassesOf<EntityBase>().ToList();
            foreach (var entityType in entityTypes)
            {
                modelBuilder.Entity(entityType).ToTable(entityType.Name);
            }
            
            base.OnModelCreating(modelBuilder);

            

        }

    }
}
