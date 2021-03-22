namespace _Source.Features.Tokens
{
    public class IdGenerator : IIdGenerator
    {
        private readonly string _baseId;

        private int _autoId;
        private int AutoId => _autoId++;

        public IdGenerator(string baseId = "Id")
        {
            _baseId = baseId;
        }

        public string Generate()
        {
            return $"{_baseId}_{AutoId}";
        }
    }
}
