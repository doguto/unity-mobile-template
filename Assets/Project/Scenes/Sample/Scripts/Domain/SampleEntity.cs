using Project.Scripts.Core.ValueObject;

namespace Project.Scenes.Sample.Scripts.Domain
{
    public class SampleEntity
    {
        public SampleId Id { get; }
        public string Name { get; }
        public int Count { get; }

        public SampleEntity(SampleId id, string name, int count)
        {
            Id = id;
            Name = name;
            Count = count;
        }
    }
}
