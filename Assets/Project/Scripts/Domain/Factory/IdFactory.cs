using System;

namespace Project.Scripts.Domain.Factory
{
    public class IdFactory<T>
    {
        readonly Func<int, T> create;
        int nextValue;

        public IdFactory(Func<int, T> create, int initialValue = 1)
        {
            this.create = create;
            nextValue = initialValue;
        }

        public T Create() => create(nextValue++);
    }
}
