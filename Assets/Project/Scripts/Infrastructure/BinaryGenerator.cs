using MasterMemory;
using MessagePack;
using MessagePack.Resolvers;

namespace Project.Scripts.Infrastructure
{
    // マスタデータの元(ScriptableObject / CSV / スプレッドシート等)は問わない。
    // 元データから集めた各テーブルの IEnumerable<T> を databaseBuilder.Append(...) で積んでから Build() する
    public class BinaryGenerator
    {
        public byte[] Generate()
        {
            var messagePackResolver = CompositeResolver.Create(
                MasterMemoryResolver.Instance,
                UnityMessagePackResolver.Instance,
                StandardResolver.Instance
            );
            var options = MessagePackSerializerOptions.Standard.WithResolver(messagePackResolver);
            MessagePackSerializer.DefaultOptions = options;

            var databaseBuilder = new DatabaseBuilder();
            // TODO: [MemoryTable] を付けたテーブル型を定義し、databaseBuilder.Append(items) でデータを積む
            return databaseBuilder.Build();
        }
    }
}
