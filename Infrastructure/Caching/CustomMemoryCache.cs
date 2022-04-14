using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Caching
{
    public class CustomMemoryCache
    {
        public MemoryCache Cache { get; } = new MemoryCache(
            new MemoryCacheOptions());
    }
}
