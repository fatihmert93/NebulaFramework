using Nebula.SeedLib;
using Nebula.TestConsole;

namespace Nebula.TestApi.Seeds
{
    public class SampleSeed : Seed
    {
        public SampleSeed()
        {
            SeedTypes.Add(SeedType.Dev);
        }
        
        protected override void DeclareEntities()
        {
            var sample = new SampleEntity();
            sample.Emrah = "fatih1";
            AddCollection(sample);
        }
    }
}