using System.Collections.Generic;
using System.Threading;

namespace FlipIt.Settings
{
    internal class ThreadSafeCache<TKey, TValue> : Dictionary<TKey, TValue>, ICache<TKey, TValue>
    {
        private readonly ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();

        public TValue Get(TKey key)
        {
            cacheLock.EnterReadLock();
            try
            {
                return ContainsKey(key) 
                    ? this[key] 
                    : default(TValue);
            }
            finally
            {
                cacheLock.ExitReadLock();
            }
        }

        public void Store(TKey key, TValue value)
        {
            cacheLock.EnterWriteLock();
            try
            {
                if (ContainsKey(key))
                {
                    this[key] = value;
                }
                else
                {
                    Add(key, value);
                }
            }
            finally
            {
                cacheLock.ExitWriteLock();
            }
        }

        public bool TryGet(TKey key, out TValue value)
        {
            cacheLock.EnterReadLock();
            try
            {
                if (ContainsKey(key))
                {
                    value = this[key];
                    return true;
                }
                value = default(TValue);
                return false;
            }
            finally
            {
                cacheLock.ExitReadLock();
            }
        }
    }
}