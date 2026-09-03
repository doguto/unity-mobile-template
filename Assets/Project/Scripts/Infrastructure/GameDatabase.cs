using MasterMemory;
using MessagePack;
using MessagePack.Resolvers;

namespace Project.Scripts.Infrastructure
{
    public class GameDatabase
    {
#if UNITY_EDITOR
        public static GameDatabase EditorPreloadedInstance { get; set; }
#endif

        public MemoryDatabase Master { get; private set; }

        public void SetupMaster(byte[] dataBinary)
        {
            var messagePackResolvers = CompositeResolver.Create(
                MasterMemoryResolver.Instance,
                UnityMessagePackResolver.Instance,
                StandardResolver.Instance
            );
            var options = MessagePackSerializerOptions.Standard.WithResolver(messagePackResolvers);
            MessagePackSerializer.DefaultOptions = options;

            Master = new MemoryDatabase(dataBinary);
        }
    }
}
