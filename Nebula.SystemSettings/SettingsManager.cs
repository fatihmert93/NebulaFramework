using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Nebula.SystemSettings.Repositories;

namespace Nebula.SystemSettings
{
    public interface ISettingsManager
    {
        void Create(SettingsEntity settings);
        void Create(string key, string value);
        void Update(string key, string newValue);
        void Update(SettingsEntity model);
        SettingsEntity GetValue(string key);
        SettingsEntity FindById(Guid id);
        SettingsEntity FindByIdAsNoTracking(Guid id);
        IQueryable<SettingsEntity> Query();
        IEnumerable<SettingsEntity> GetAll();
        Dictionary<string, string> GetAllAsDictionary();
        void Delete(string key);
        void Delete(Guid id);
        void Delete(SettingsEntity model);
    }

    public class SettingsManager : ISettingsManager
    {
        private readonly ISystemSettingsRepository _settingsRepository;

        public SettingsManager(ISystemSettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }


        public void Create(SettingsEntity settings)
        {
            if(_settingsRepository.Query(v => v.Key == settings.Key).Any())
                throw new ArgumentException("key already exists!");
            _settingsRepository.Create(settings);
        }

        public void Create(string key, string value)
        {
            SettingsEntity model = new SettingsEntity()
            {
                Key = key,
                Value = value
            };
            Create(model);
        }

        public void Update(string key, string newValue)
        {
            var model = GetValue(key);
            if(model == null)
                throw new ArgumentNullException();
            model.Value = newValue;
            Update(model);
        }

        public void Update(SettingsEntity model)
        {
            var entity = _settingsRepository.FindAsNoTracking(model.Id);
            entity.Value = model.Value;
            entity.Description = model.Description;
            _settingsRepository.Update(entity);
        }

        public SettingsEntity GetValue(string key)
        {
            return _settingsRepository.Query(v => v.Key == key).SingleOrDefault();
        }

        public SettingsEntity FindById(Guid id)
        {
            return _settingsRepository.Find(id);
        }

        public SettingsEntity FindByIdAsNoTracking(Guid id)
        {
            return _settingsRepository.Query(v => v.Id == id).AsNoTracking().FirstOrDefault();
        }

        public IQueryable<SettingsEntity> Query()
        {
            return _settingsRepository.Query();
        }

        public IEnumerable<SettingsEntity> GetAll()
        {
            return _settingsRepository.Query().AsNoTracking().ToList();
        }

        public Dictionary<string, string> GetAllAsDictionary()
        {
            var list = GetAll();

            return list.ToDictionary(entity => entity.Key, entity => entity.Value);
        }

        public void Delete(string key)
        {
            var model = GetValue(key);
            Delete(model);
        }

        public void Delete(Guid id)
        {
            var model = _settingsRepository.FindAsNoTracking(id);
            Delete(model);
        }

        public void Delete(SettingsEntity model)
        {
            _settingsRepository.Delete(model);
        }
    }
}
