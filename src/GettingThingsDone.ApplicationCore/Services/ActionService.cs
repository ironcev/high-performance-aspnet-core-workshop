using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GettingThingsDone.ApplicationCore.Helpers;
using GettingThingsDone.Contracts.Dto;
using GettingThingsDone.Contracts.Interface;
using GettingThingsDone.Contracts.Model;
using Microsoft.Extensions.Caching.Memory;
using static GettingThingsDone.Contracts.Interface.ServiceResult;
using Action = GettingThingsDone.Contracts.Model.Action;

namespace GettingThingsDone.ApplicationCore.Services
{
    // This is a sample implementation.
    // As such, it contains detailed comments to emphases the general principle.
    // These kind of comments shouldn't be in the real-life code.
    public class ActionService : IActionService
    {
        private readonly IAsyncRepository<Action> _actionRepository;
        private readonly IAsyncRepository<ActionList> _listRepository;
        private readonly IAsyncRepository<Project> _projectRepository;
        private readonly IMemoryCache _memoryCache;
        // Inject repositories and/or all the other needed services.
        public ActionService(
            IAsyncRepository<Action> actionRepository,
            IAsyncRepository<ActionList> listRepository,
            IAsyncRepository<Project> projectRepository,
            IMemoryCache memoryCache
            )
        {
            _actionRepository = actionRepository;
            _listRepository = listRepository;
            _projectRepository = projectRepository;
            _memoryCache = memoryCache;
        }

        public async Task<ServiceResult<ActionDto>> GetAction(int id)
        {
            // Get the entity from the repository.
            var action = await _actionRepository.GetById(id);

            // Return the appropriate service result if the
            // requested entity does not exist.
            if (action == null)
                return EntityNotFound<ActionDto>();

            // Translate it into DTO and return.
            return action
                .TranslateTo<ActionDto>()
                .ToOkServiceResult();
        }

        public async Task<ServiceResult<List<ActionDto>>> GetAll()
        {
            CacheHelper cacheHelper = new CacheHelper(_actionRepository, _memoryCache);
           
            return (await cacheHelper.GetActions())
                .ToOkServiceResult(); // And return.
        }

        // This is the simplest implementation usual in CRUD scenarios
        // or CRUD scenarios with additional validation.
        // In complex scenarios creating a new entity and editing existing
        // once can differ significantly. In that case we recommend to
        // split this method into two or more.
        public async Task<ServiceResult<ActionDto>> CreateOrUpdate(ActionDto actionDto)
        {
            // Either create a new entity based on the DTO
            // or change an existing one based on the DTO.
            Action action = actionDto.RepresentsNewEntity
                ? actionDto.TranslateTo<Action>()
                : (await _actionRepository.GetById(actionDto.Id)).CopyPropertiesFrom(actionDto);

            // Check if the entity exists (if it was an update).
            if (action == null)
                return EntityNotFound(actionDto);

            // TODO: Later on we will do the checks here.
            //       So far we assume everything always works fine.

            // Save changes.
            action = await _actionRepository.AddOrUpdate(action);

            // If the DTO was representing a new DTO
            // we need to set the assigned Id.
            // If it already had the Id, it is the same one.
            // So we can simply have an assignment here.

            actionDto.Id = action.Id;

            // Remove ActionsAll from cache, next time getall call will fill the cache.
            CacheHelper cacheHelper = new CacheHelper(_memoryCache);
            cacheHelper.RemoveActions();

            return Ok(actionDto);
        }

        public Task<ServiceResult<ActionDto>> CreateFromLegacyExchangeFormat(string legacyExchangeValue)
        {
            var result = LegacyExchangeActionFormat.ToActionDto(legacyExchangeValue);

            if (!result.IsOk()) return Task.FromResult(result);

            return CreateOrUpdate(result.Value);
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var action = await _actionRepository.GetById(id);

            if (action == null)
                return EntityNotFound();

            await _actionRepository.Delete(action);

            return Ok();
        }

        public async Task<ServiceResult> MoveToList(int id, int listId)
        {
            var action = await _actionRepository.GetById(id);
            if (action == null)
                return EntityNotFound("Action not found.");

            var list = await _listRepository.GetById(listId);
            if (list == null)
                return EntityNotFound("List not found.");

            action.List = list;

            await _actionRepository.AddOrUpdate(action);

            return Ok();
        }

        public async Task<ServiceResult> AssignToProject(int id, int projectId)
        {
            var action = await _actionRepository.GetById(id);
            if (action == null)
                return EntityNotFound("Action not found.");

            var project = await _projectRepository.GetById(projectId);
            if (project == null)
                return EntityNotFound("Project not found.");

            action.Project = project;

            await _actionRepository.AddOrUpdate(action);

            return Ok();
        }
    }
}