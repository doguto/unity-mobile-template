using Cysharp.Threading.Tasks;

namespace Project.Scripts.Application.UseCase
{
    // RegisterEntryPoint が呼ぶ AsImplementedInterfaces は基底クラスを登録しないため、
    // SceneSetupUseCase を Scope 側の記述無しで解決するための登録キーとして必要
    public interface ISceneSetup
    {
        UniTask Completion { get; }
    }
}
