namespace _Source.Features.ActorBehaviours
{
    public class BlackboardItem<T>
    {
        private T _item;

        public void Store(T item)
        {
            _item = item;
        }

        public T View()
        {
            return _item;
        }

        public T Consume()
        {
            var takenItem = _item;
            _item = default(T);

            return takenItem;
        }
    }
}
