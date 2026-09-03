using MasterMemory;

namespace Project.Scripts.Infrastructure.MasterData
{
    // データの元(ScriptableObject / CSV / スプレッドシート等)を BinaryGenerator から隠すための抽象。
    // 実装を差し替えたくなったら、このインターフェースを実装するクラスを用意すればよい
    public interface IMasterDataAsset
    {
        void AppendTo(DatabaseBuilder builder);
    }
}
