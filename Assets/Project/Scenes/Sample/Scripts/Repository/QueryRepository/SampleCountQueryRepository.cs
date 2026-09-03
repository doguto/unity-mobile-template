using Project.Scenes.Sample.Scripts.Domain;
using Project.Scripts.Infrastructure;

namespace Project.Scenes.Sample.Scripts.Repository.QueryRepository
{
    public class SampleCountQueryRepository : ISampleCountQueryRepository
    {
        readonly RuntimeDatabase runtimeDatabase;

        public SampleCountQueryRepository(RuntimeDatabase runtimeDatabase)
        {
            this.runtimeDatabase = runtimeDatabase;
        }

        public void IncrementCount() => runtimeDatabase.SampleCount++;
    }
}
