using System.IO;
using System.Text.Json;
using System.Threading;

namespace Insurance.Api.Helpers
{
    public sealed class FileStorageHelper
    {
        private readonly ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();
        private readonly string filePath;

        public FileStorageHelper(string filePath)
        {
            this.filePath = filePath;

            cacheLock.ExitReadLock();
        }

        public T Read<T>()
        {
            cacheLock.EnterReadLock();
            T value = default;

            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
                value = JsonSerializer.Deserialize<T>(jsonString);
            }

            cacheLock.ExitReadLock();

            return value;
        }

        public void Save<T>(T obj)
        {
            cacheLock.EnterWriteLock();

            try
            {
                string jsonString = JsonSerializer.Serialize(obj);
                File.WriteAllText(filePath, jsonString);
            }
            finally
            {
                cacheLock.ExitWriteLock();
            }
        }
    }
}