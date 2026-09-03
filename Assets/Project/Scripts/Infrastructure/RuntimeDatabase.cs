namespace Project.Scripts.Infrastructure
{
    // ゲーム中に変化するデータを保持するインメモリストア。Domain の *Entity は一切持たない。
    // Entity への変換は Entity/*EntityRepository が担う。RuntimeData が増えたらフィールドを追加していく
    public class RuntimeDatabase
    {
        public int SampleCount { get; set; }
    }
}
