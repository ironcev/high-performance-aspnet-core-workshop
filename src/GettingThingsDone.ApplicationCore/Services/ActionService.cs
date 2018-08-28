using GettingThingsDone.Contract.Dto;
using GettingThingsDone.Contract.Interface;
using GettingThingsDone.ApplicationCore.Helpers;
using System.Collections.Generic;


namespace GettingThingsDone.ApplicationCore.Services
{
    public class ActionService : IActionService
    {
        private readonly IActionRepository _actionRepository;

        public ActionService(
            IRepository<Contract.Model.Action> repository,
            IActionRepository actionRepository
            )
        {
            _actionRepository = actionRepository;
        }

        public ActionDto GetAction(int id)
        {
            var actionItem = _actionRepository.GetById(id);
            ActionDto actionDto = actionItem.TranslateTo<ActionDto>();
            
            actionDto.UserId = 44; // TODO: Replace with real user id once we add support for users.

            return actionDto;
        }

        public ActionDto CreateOrUpdate(int? id, ActionDto actionDto)
        {
            Contract.Model.Action action = actionDto.TranslateTo<Contract.Model.Action>();
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

        public List<ActionDto> GetTop(int count)
        {
            var actions = _actionRepository.GetTop(count);

            List<ActionDto> actionDtos = new List<ActionDto>();
            foreach (var item in actions)
            {
                actionDtos.Add(item.TranslateTo<ActionDto>());
            }

            return actionDtos;            
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
            return _actionRepository.MoveToList(id, listId);
        }

        public DataActionResultDto AssignToProject(int id, int projectId)
        {
            return _actionRepository.AssignToProject(id, projectId);
        }       
    }
}
