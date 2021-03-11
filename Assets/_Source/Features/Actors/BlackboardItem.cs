namespace _Source.Features.Actors
{
    public class BlackboardItem<T>
    {
        private T _item;

        public bool HasValue { get; private set; } = false;

        public void Store(T item)
        {
            HasValue = true;
            _item = item;
        }

        public T View()
        {
            return _item;
        }

        public T Consume()
        {
            HasValue = false;

            var takenItem = _item;
            _item = default(T);
            
            return takenItem;
        }
    }
}
