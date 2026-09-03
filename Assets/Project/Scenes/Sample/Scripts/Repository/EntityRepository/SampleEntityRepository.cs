using Project.Scenes.Sample.Scripts.Domain;
using Project.Scripts.Core.ValueObject;
using Project.Scripts.Infrastructure;

namespace Project.Scenes.Sample.Scripts.Repository.EntityRepository
{
    public class SampleEntityRepository : ISampleEntityRepository
    {
        readonly GameDatabase gameDatabase;
        readonly RuntimeDatabase runtimeDatabase;

        public SampleEntityRepository(GameDatabase gameDatabase, RuntimeDatabase runtimeDatabase)
        {
            this.gameDatabase = gameDatabase;
            this.runtimeDatabase = runtimeDatabase;
        }

        public SampleEntity Get()
        {
            var count = runtimeDatabase.SampleCount;
            var id = new SampleId(count);
            var name = gameDatabase.Master.SampleTable.TryFindById(id, out var master) ? master.Name : "";
            return new SampleEntity(id, name, count);
        }
    }
}
