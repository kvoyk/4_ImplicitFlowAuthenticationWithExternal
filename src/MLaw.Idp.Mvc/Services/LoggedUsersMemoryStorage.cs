using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using MLaw.Idp.Mvc.Models;
using Newtonsoft.Json;

namespace MLaw.Idp.Mvc.Services
{
    public class LoggedUsersMemoryStorage : ILoggedUsersStorage
    {
        private readonly IMemoryCache _memoryCache;
        private readonly CacheKeyCreator _cacheKeyCreator;

        public LoggedUsersMemoryStorage(IMemoryCache memoryCache, CacheKeyCreator cacheKeyCreator)
        {
            _memoryCache = memoryCache;
            _cacheKeyCreator = cacheKeyCreator;
        }

        public async Task<string> SaveLoginAsync(LoggedinUserModel loggedinUserModel)
        {
            string cacheKey = _cacheKeyCreator.CreateKey();

            await Task.Run(() =>
            {
                var memoryCacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(300));
                _memoryCache.Set(cacheKey, loggedinUserModel, memoryCacheEntryOptions);
            });

            return cacheKey;
        }

        public async Task<LoggedinUserModel> GetLoginAsync(string cacheKey)
        {
            return await Task.Run(() =>
            {
                _memoryCache.TryGetValue(cacheKey, out LoggedinUserModel loggedinUserModel);
                return loggedinUserModel;
            });
        }
    }
}