using GettingThingsDone.Contracts.Dto;
using GettingThingsDone.Contracts.Interface;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Action = GettingThingsDone.Contracts.Model.Action;

namespace GettingThingsDone.ApplicationCore.Helpers
{
    public class CacheHelper
    {
        private readonly IAsyncRepository<Action> _actionRepository;
        private readonly IMemoryCache _memoryCache;
        public CacheHelper(IAsyncRepository<Action> actionRepository, IMemoryCache memoryCache)
        {
            _actionRepository = actionRepository;
            _memoryCache = memoryCache; 
        }
        public async Task<List<ActionDto>> GetActions()
        {
            string cacheKey = "Actions-GetAll";
            List<ActionDto> actionsAll;
            // Try to get from cache and convert, null if not allready cached.
            actionsAll = _memoryCache.Get(cacheKey) as List<ActionDto>;

            // TryGet returns true if the cache entry was found.
            // Othervise Set it into the cache
            if (!_memoryCache.TryGetValue(cacheKey, out actionsAll))
            {
                // Get data from the store, DB.
                actionsAll = (await _actionRepository
                .GetAll(TrackingOption.WithoutTracking)) // Get the entities from the repository.
                .Select(action => action.TranslateTo<ActionDto>()) // Translate them into DTOs.
                .ToList();

                // Store object into the cache
                _memoryCache.Set(cacheKey, actionsAll,
                    new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(20)));
            }
            return actionsAll;
        }
    }
}
