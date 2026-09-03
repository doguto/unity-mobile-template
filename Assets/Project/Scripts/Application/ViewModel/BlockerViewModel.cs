using R3;

namespace Project.Scripts.Application.ViewModel
{
    public class BlockerViewModel
    {
        public ReactiveProperty<bool> IsActive { get; } = new(false);
    }
}
