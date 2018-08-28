using System.Collections.Generic;
using System.Linq;
using GettingThingsDone.ApplicationCore.Helpers;
using GettingThingsDone.Contract.Dto;
using GettingThingsDone.Contract.Interface;
using GettingThingsDone.Contract.Model;

namespace GettingThingsDone.ApplicationCore.Services
{
    public class ActionService : IActionService
    {
        private readonly IRepository<Action> _actionRepository;
        private readonly IRepository<ActionList> _listRepository;
        private readonly IRepository<Project> _projectRepository;

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
            var actionItem = _actionRepository.GetById(id);
            ActionDto actionDto = actionItem.TranslateTo<ActionDto>();
            
            actionDto.UserId = 44; // TODO: Replace with real user id once we add support for users.

            return actionDto;
        }

        public List<ActionDto> GetAll()
        {
            return _actionRepository.GetAll().Select(action => action.TranslateTo<ActionDto>()).ToList();
        }

        public ActionDto CreateOrUpdate(int? id, ActionDto actionDto)
        {
            Action action = actionDto.TranslateTo<Action>();
            try
            {
                if (id.HasValue)
                {
                    action.Id = id.Value;
                    _actionRepository.Update(action);
                }
                else
                {
                    _actionRepository.Add(action);
                    actionDto.Id = action.Id;
                }
            } catch { }

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

            _actionRepository.Update(action);

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

            _actionRepository.Update(action);
            
            result.IsSaved = true;

            return result;
        }
    }
}