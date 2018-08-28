using System.Collections.Generic;
using System.Linq;
using GettingThingsDone.ApplicationCore.Helpers;
using GettingThingsDone.Contracts.Dto;
using GettingThingsDone.Contracts.Interface;
using GettingThingsDone.Contracts.Model;

namespace GettingThingsDone.ApplicationCore.Services
{
    // This is a sample implementation.
    // As such, it contains detailed comments to emphases the general principle.
    // This comments shouldn't be in the real-life code.
    public class ActionService : IActionService
    {
        private readonly IRepository<Action> _actionRepository;
        private readonly IRepository<ActionList> _listRepository;
        private readonly IRepository<Project> _projectRepository;

        // Inject repositories and/or all the other needed services.
        public ActionService(
            IRepository<Action> actionRepository,
            IRepository<ActionList> listRepository,
            IRepository<Project> projectRepository)
        {
            _actionRepository = actionRepository;
            _listRepository = listRepository;
            _projectRepository = projectRepository;
        }

        public ActionDto GetAction(int id)
        {
            // Get the entity from the repository.
            var actionItem = _actionRepository.GetById(id);

            // Translate it into DTO and return.
            ActionDto actionDto = actionItem.TranslateTo<ActionDto>();

            return actionDto;
        }

        public List<ActionDto> GetAll()
        {
            return _actionRepository
                .GetAll() // Get the entities from the repository.
                .Select(action => action.TranslateTo<ActionDto>()) // Translate them into DTOs.
                .ToList(); // And return.
        }

        // This is the simplest implementation in the CRUD scenarios.
        // In complex scenarios creating a new entity and editing existing
        // once can differ significantly. In that case we recommend to
        // split this method into two or more.
        public ActionDto CreateOrUpdate(ActionDto actionDto)
        {
            // Either create a new entity based on the DTO
            // or change an existing one based on the DTO.
            Action action = actionDto.RepresentsNewEntity
                ? actionDto.TranslateTo<Action>()
                : _actionRepository.GetById(actionDto.Id).CopyPropertiesFrom(actionDto);

            // TODO: Later on we will do the checks here and see how to return the result.
            //       So far we assume everything always works fine.

            // Save changes.
            action = _actionRepository.AddOrUpdate(action);

            // If the DTO was representing a new DTO
            // we need to set the assigned Id.
            // If it already had the Id, it is the same one.
            // So we can simply have an assignment here.

            actionDto.Id = action.Id;

            return actionDto;
        }

        public DataActionResultDto Delete(int id)
        {
            DataActionResultDto result = new DataActionResultDto();
            result.IsSaved = false;

            try
            {
                var action = _actionRepository.GetById(id);
                _actionRepository.Delete(action);
                result.IsSaved = true;
            } catch { }

            return result;
        }

        public DataActionResultDto MoveToList(int id, int listId)
        {
            DataActionResultDto result = new DataActionResultDto();
            result.IsSaved = false;
            var action = _actionRepository.GetById(id);
            if (action == null)
            {
                result.Message = "Action not found.";
                return result;
            }

            var list = _listRepository.GetById(listId);
            if (list == null)
            {
                result.Message = "List not found.";
                return result;
            }

            action.List = list;

            _actionRepository.AddOrUpdate(action);

            result.IsSaved = true;

            return result;
        }

        public DataActionResultDto AssignToProject(int id, int projectId)
        {
            DataActionResultDto result = new DataActionResultDto();
            var action = _actionRepository.GetById(id);
            if (action == null)
            {
                result.Message = "Action not found.";
                return result;
            }

            var project = _projectRepository.GetById(projectId);
            if (project == null)
            {
                result.Message = "Project not found.";
                return result;
            }

            action.Project = project;

            _actionRepository.AddOrUpdate(action);
            
            result.IsSaved = true;

            return result;
        }
    }
}