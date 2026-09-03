using VContainer;
using VContainer.Unity;

namespace Project.Scripts.Scope
{
    // Project 単位（全シーン共通）の DI 登録をここに置く。
    // Scene 単位の登録は Scenes/<SceneName>/Scripts/Scope/<SceneName>SceneLifetimeScope に置く
    public class BaseLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // TODO: Repository / DataSource / 全シーン共通 Service・ViewModel をここで登録する
        }
    }
}
