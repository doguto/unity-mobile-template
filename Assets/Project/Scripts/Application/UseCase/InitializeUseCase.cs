using VContainer.Unity;

namespace Project.Scripts.Application.UseCase
{
    public abstract class InitializeUseCase : IInitializable
    {
        public void Initialize() => Execute();

        protected abstract void Execute();
    }
}
