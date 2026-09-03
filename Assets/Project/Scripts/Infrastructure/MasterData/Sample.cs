using MasterMemory;
using MessagePack;
using Project.Scripts.Core.ValueObject;

namespace Project.Scripts.Infrastructure.MasterData
{
    // MasterMemory は [MemoryTable] 属性付きクラスを Source Generator で検出し、
    // MemoryDatabase / DatabaseBuilder をコンパイル時に自動生成する。
    // このクラスはテーブル定義が1つも無いと MemoryDatabase / DatabaseBuilder 自体が生成されず
    // GameDatabase / BinaryGenerator / DataBinaryIntegrater がコンパイルできなくなるためのプレースホルダー。
    // 実際のテーブルを1つ以上定義したら削除して構わない
    [MemoryTable("Sample"), MessagePackObject(true)]
    public sealed class Sample
    {
        [PrimaryKey]
        public SampleId Id { get; set; }
        public string Name { get; set; }
    }
}
