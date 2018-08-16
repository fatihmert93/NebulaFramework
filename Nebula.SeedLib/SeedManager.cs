using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Migrations;
using Nebula.CoreLibrary.IOC;
using Nebula.CoreLibrary.Shared;
using Nebula.CoreLibrary.Utilities;

namespace Nebula.SeedLib
{
    public enum SeedType
    {
        Prod,
        Dev,
        PreProd,
        All
    }

    public interface ISeedManager
    {
        SeedType SeedType { get; set; }
        void ExecuteAll();
    }

    internal class SeedManager : ISeedManager
    {
        private readonly IHostingEnvironment _env;
        private readonly IRepository<SeedHistory> _historyRepository;

        public SeedManager(IRepository<SeedHistory> historyRepository)
        {
            _historyRepository = historyRepository;
        }

        public SeedManager(IHostingEnvironment env, IRepository<SeedHistory> historyRepository)
        {
            _env = env;
            _historyRepository = historyRepository;
            CheckEnvironment();
        }


        public SeedType SeedType { get; set; }

        public void ExecuteAll()
        {
            ExecuteGenericSeeds();
            ExecuteSeeds();
        }

        private void RunSeed(dynamic seed)
        {
            if (!seed.SeedTypes.Contains(SeedType.All) && !seed.SeedTypes.Contains(SeedType)) return;
            if (seed.DetectPropertyChange)
            {
                seed.Run();
            }
            else
            {
                string typename = seed.GetType().Name;
                var historyExists = _historyRepository.Query(v => v.SeedName == typename).Any();
                if (!historyExists)
                    seed.Run();
            }
        }

        private void RunSeeds(IEnumerable<Type> types)
        {
            foreach (var seedType in types)
            {
                dynamic seed = Activator.CreateInstance(seedType);
                RunSeed(seed);
            }
        }

        private void ExecuteGenericSeeds()
        {
            IEnumerable<Type> seedGenericTypes = ReflectionUtility.GetAllTypesImplementingOpenGenericType(typeof(Seed<>)).ToList();
            RunSeeds(seedGenericTypes);
        }

        private void ExecuteSeeds()
        {
            IEnumerable<Type> seedTypes = ReflectionUtility.FindSubClassesOf<Seed>().ToList();
            RunSeeds(seedTypes);
        }

        private void CheckEnvironment()
        {
            if (_env == null) return;
            switch (_env.EnvironmentName)
            {
                case "Development":
                    SeedType = SeedType.Dev;
                    break;
                case "PreProduction":
                    SeedType = SeedType.PreProd;
                    break;
                case "Production":
                    SeedType = SeedType.Prod;
                    break;
                default:
                    SeedType = SeedType.Dev;
                    break;
            }
        }

    }
}
