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
        }

        public T Read<T>()
        {
            cacheLock.EnterReadLock();

            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
                cacheLock.ExitReadLock();
                return JsonSerializer.Deserialize<T>(jsonString);
            }

            cacheLock.ExitReadLock();
            return default;
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