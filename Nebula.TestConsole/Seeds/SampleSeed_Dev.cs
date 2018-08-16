using System;
using System.Collections.Generic;
using System.Text;
using Nebula.CoreLibrary.Shared;
using Nebula.Membership;
using Nebula.SeedLib;

namespace Nebula.TestConsole.Seeds
{

    public class SeedSampleEntity : EntityBase
    {
        public string Name { get; set; }
    }

    public class SeedSample1Entity : EntityBase
    {
        public string Name { get; set; }
    }

    

    public class SampleSeedProd : Seed
    {
        public SampleSeedProd()
        {
            
            SeedTypes.Add(SeedType.Dev);
        }
        
        protected override void DeclareEntities()
        {
            var sampleEntity = new SampleEntity();
            sampleEntity.Id = new Guid("1dd672d5-b66e-4377-ba4a-f541c44c9a5e");
            sampleEntity.Emrah = "emrah";
            AddCollection(sampleEntity);

            var sampleEntity1 = new SampleEntity();
            sampleEntity1.Emrah = "emrah1";
            AddCollection(sampleEntity1);

            var sampleEntity2 = new SampleEntity();
            sampleEntity2.Emrah = "emrah2";
            AddCollection(sampleEntity2);

            var ent2 = new SampleEntity1();
            ent2.Emrah = "fatih";
            AddCollection(ent2);

        }
    }
}
