using System.Collections.Generic;
using MasterMemory;
using MessagePack;
using MessagePack.Resolvers;
using Project.Scripts.Infrastructure.MasterData;

namespace Project.Scripts.Infrastructure
{
    // マスタデータの元(ScriptableObject / CSV / スプレッドシート等)は問わない。
    // IMasterDataAsset を実装したものを渡してもらうだけで、データソースの実体は一切知らない
    public class BinaryGenerator
    {
        public byte[] Generate(IEnumerable<IMasterDataAsset> assets)
        {
            var messagePackResolver = CompositeResolver.Create(
                MasterMemoryResolver.Instance,
                UnityMessagePackResolver.Instance,
                StandardResolver.Instance
            );
            var options = MessagePackSerializerOptions.Standard.WithResolver(messagePackResolver);
            MessagePackSerializer.DefaultOptions = options;

            var databaseBuilder = new DatabaseBuilder();
            foreach (var asset in assets)
                asset.AppendTo(databaseBuilder);
            return databaseBuilder.Build();
        }
    }
}
