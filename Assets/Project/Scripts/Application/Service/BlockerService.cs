using System;
using R3;
using Project.Scripts.Application.ViewModel;

namespace Project.Scripts.Application.Service
{
    public class BlockerService
    {
        readonly BlockerViewModel viewModel;
        int activeCount;

        public BlockerService(BlockerViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public void Activate()
        {
            activeCount++;
            viewModel.IsActive.Value = true;
        }

        public void Deactivate()
        {
            activeCount = Math.Max(0, activeCount - 1);
            if (activeCount == 0)
                viewModel.IsActive.Value = false;
        }

        public IDisposable ActivateScope()
        {
            Activate();
            return Disposable.Create(Deactivate);
        }
    }
}
