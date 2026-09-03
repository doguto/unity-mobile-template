using UnitGenerator;

namespace Project.Scripts.Core.ValueObject
{
    // 実際の *Id 型を定義したらこのサンプルは削除して構わない
    [UnitOf(typeof(int), UnitGenerateOptions.Comparable | UnitGenerateOptions.MessagePackFormatter)]
    public readonly partial struct SampleId { }
}
