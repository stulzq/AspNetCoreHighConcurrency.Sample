namespace RedisLock.AspNetCore.LockProcessor
{
    public class LockProcessorResult
    {
        public bool Result { get; set; }

        public T Data { get; set; }
    }
}