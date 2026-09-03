using System;
using Cysharp.Threading.Tasks;
using R3;
using VContainer.Unity;

namespace Project.Scripts.Application.UseCase
{
    public abstract class EventUseCase<T> : IInitializable, IDisposable
    {
        DisposableBag disposableBag;

        public void Initialize()
        {
            OnInitialize();
            CreateTrigger().Subscribe(x => Execute(x).Forget()).AddTo(ref disposableBag);
        }

        public void Dispose() => disposableBag.Dispose();

        protected virtual void OnInitialize() { }

        protected abstract Observable<T> CreateTrigger();

        protected abstract UniTask Execute(T value);
    }
}
