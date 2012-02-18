namespace FlipIt.Settings
{
    internal interface ICache<TKey, TValue>
    {
        TValue Get(TKey key);
        void Store(TKey key, TValue value);
        bool TryGet(TKey key, out TValue value);
    }
}