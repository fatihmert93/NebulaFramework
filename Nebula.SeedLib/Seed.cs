using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Nebula.CoreLibrary.IOC;
using Nebula.CoreLibrary.Shared;
using Nebula.CoreLibrary.Utilities;

namespace Nebula.SeedLib
{
    
    // old generic seed class
    public abstract class Seed<T> where T : EntityBase, new()
    {
        private readonly IRepository<T> _repository;

        protected Seed()
        {
            _entityCollection = new List<T>();
            _repository = DependencyService.GetService<IRepository<T>>();
        }


        public void Run()
        {
            DeclareEntities();
            foreach (var entity in _entityCollection)
            {
                _repository.Create(entity);
            }
        }

        public void AddCollection(T entity)
        {
            _entityCollection.Add(entity);
        }

        private readonly ICollection<T> _entityCollection;
        public abstract SeedType SeedType { get; set; }
        protected abstract void DeclareEntities();
    }

    
    
    /// <summary>
    /// Non-generic seed manager. Using is so easy.
    /// Explanation to how to use:
    ///     First, extend new class from seed for personal perpose
    ///     Second, after created the extandable class, in DeclareEntities method, you need to declare some entities
    ///     and Add generic repository this entity object by using AddCollection method
    /// </summary>
    public abstract class Seed
    {
        private SeedHistory _history;
        private readonly ICollection<EntityBase> _collection;
        private readonly IRepository<SeedHistory> _seedHistory;
        private readonly IRepository<SeedHistoryItem> _itemRepository;
        private readonly dynamic _repository;
        private readonly ICollection<string> _serilizedObjects;

        public bool DetectPropertyChange { get; set; } = true;

        protected Seed()
        {
            _repository = DependencyService.GetService(typeof(IRepository));
            _seedHistory = DependencyService.GetService<IRepository<SeedHistory>>();
            _itemRepository = DependencyService.GetService<IRepository<SeedHistoryItem>>();
            _collection = new List<EntityBase>();
            _serilizedObjects = new List<string>();
            
        }

        private void CheckSeedTypes()
        {
            if(SeedTypes.Count == 0) throw new Exception("There is no any seed type!");
        }

        
        /// <summary>
        /// For insert seed data to database. Just use in DeclareEntities method.
        /// </summary>
        /// <param name="entity">This is the base class of all entities. That mean, you can give any class as param that extandable from EntityBase</param>
        protected void AddCollection(EntityBase entity)
        {
            if (IsExists(entity)) return; 
            if(entity.Id == Guid.Empty)
                entity.Id = Guid.NewGuid();
            _repository.Add(entity);
            TryCreate(entity);
        }

        internal void Run()
        {
            CheckSeedTypes();
            var typename = GetType().Name;

            if (!_seedHistory.Query(v => v.SeedName == typename).Any())
            {
                var newHistory = new SeedHistory
                {
                    Id = Guid.NewGuid(),
                    SeedName = typename
                };
                _seedHistory.Add(newHistory);
                _history = newHistory;
            }
            else
            {
                _history = _seedHistory.Query().FirstOrDefault(v => v.SeedName == typename);
            }

            DeclareEntities();

            _repository.Commit();
            _seedHistory.Commit();
            _itemRepository.Commit();

        }

        private bool IsExists(EntityBase entity)
        {
            var serializedObject = ConverterUtility.ObjectPropertySerializer(entity);
            return !_serilizedObjects.Contains(serializedObject) && _itemRepository.Query(v => v.SerializedObject == serializedObject).Any();
        }

        private void RemoveOld(EntityBase entity)
        {
            var typename = entity.GetType().Name;
            IEnumerable<string> idlist = _itemRepository.Query(v => v.ClassName == typename).Select(v => "\'" + v.ObjectId + "\'").ToList();
            var ent = _itemRepository.Find(entity.Id);
            if (!idlist.Any()) return;
            var stridlist = string.Join(',', idlist);
            var script = "delete from \"" + typename + "\" WHERE \"Id\" IN (" + stridlist + ");";
;
            _repository.ExecuteSql(script);
        }
        

        private void TryCreate(EntityBase entity)
        {
            if(entity.Id == Guid.Empty)
                entity.Id = Guid.NewGuid();
            
            RemoveOld(entity);
            
            SeedHistoryItem seedHistoryItem = new SeedHistoryItem();
            string serializedObject = ConverterUtility.ObjectPropertySerializer(entity);
            if(!_serilizedObjects.Contains(serializedObject))
                _serilizedObjects.Add(serializedObject);

            seedHistoryItem.SeedHistoryId = _history.Id;
            seedHistoryItem.ClassName = entity.GetType().Name;
            if (entity.Id != Guid.Empty)
                seedHistoryItem.ObjectId = entity.Id.ToString();
            
            seedHistoryItem.SerializedObject = serializedObject;
            _itemRepository.Add(seedHistoryItem);
            //_repository.Add(entity);
        }

        public ICollection<SeedType> SeedTypes { get; set; } = new List<SeedType>();
        
        
        /// <summary>
        /// Use AddCollection method in this method for insert entity that using like seed data
        /// </summary>
        protected abstract void DeclareEntities();
    }
}
