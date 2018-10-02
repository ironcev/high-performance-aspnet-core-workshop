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
    // Get data from cache or from store
    public class CacheHelper
    {
        private readonly IAsyncRepository<Action> _actionRepository;
        private readonly IMemoryCache _memoryCache;

        public CacheHelper( IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public CacheHelper(IAsyncRepository<Action> actionRepository, IMemoryCache memoryCache)
        {
            _actionRepository = actionRepository;
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Get Actions from the DB or from the Cache if exists.
        /// </summary>
        /// <returns></returns>
        public async Task<List<ActionDto>> GetActions()
        {
            string cacheKey = CacheKeys.ActionAll;

            List<ActionDto> actionsAll;
            // Try to get from cache and convert, null if not allready cached.
            //actionsAll = _memoryCache.Get(cacheKey) as List<ActionDto>;

            // TryGet returns true if the cache entry was found.
            // Othervise Set it into the cache
            if (!_memoryCache.TryGetValue(cacheKey, out actionsAll))
            {
                // Get data from the store, DB.
                actionsAll = (await _actionRepository
                .GetAll(TrackingOption.WithoutTracking)) // Get the entities from the repository.
                .Select(action => action.TranslateTo<ActionDto>()) // Translate them into DTOs.
                .ToList();

                // If there is any Set to cache
                if (actionsAll.Count() > 0)
                {
                    // Store object into the cache
                    _memoryCache.Set(cacheKey, actionsAll,
                        new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(15)) // After last call.
                                                                        //.SetAbsoluteExpiration(TimeSpan.FromSeconds(20)) // Absolute cache life duration.
                                                                        //.SetPriority(CacheItemPriority.High) // If memory is low cache will be cleaned, with priority we can set in which order.
                        );
                }
            }

            return actionsAll;
        }

        public void RemoveActions()
        {
            _memoryCache.Remove(CacheKeys.ActionAll);
        }
    }
}
