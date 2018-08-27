using GettingThingsDone.Contract.DTO;
using GettingThingsDone.Contract.Interface;
using GettingThingsDone.ApplicationCore.Helpers;
using System;
using System.Collections.Generic;
using System.Text;


namespace GettingThingsDone.ApplicationCore.Services
{
    public class ActionService : IActionService
    {
        private readonly IActionRepository _actionRepository;

        public ActionService(IRepository<Contract.Model.Action> repository,
            IActionRepository actionRepository
            )
        {
            _actionRepository = actionRepository;
        }

        public ActionDTO GetAction(int id)
        {
            var actionItem = _actionRepository.GetById(id);
            ActionDTO actionDto = actionItem.TranslateTo<ActionDTO>();
            //additional metadata exp
            actionDto.UserId = 44;

            return actionDto;
        }

        public ActionDTO CreateOrUpdate(int? id, ActionDTO actionDto)
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
            } catch
            {

            }
            return actionDto;
        }

        public List<ActionDTO> GetTop(int count)
        {
            var actions = _actionRepository.GetTop(count);

            List<ActionDTO> actionDtos = new List<ActionDTO>();
            foreach (var item in actions)
            {
                actionDtos.Add(item.TranslateTo<ActionDTO>());
            }

            return actionDtos;
            
        }

        public DataActionResultDTO Delete(int id)
        {
            DataActionResultDTO result = new DataActionResultDTO();
            result.IsSaved = false;
            try
            {
                var action = _actionRepository.GetById(id);
                _actionRepository.Delete(action);
                result.IsSaved = true;
            } catch { }

            return result;
        }

        public DataActionResultDTO MoveToList(int id, int listId)
        {
            return _actionRepository.MoveToList(id, listId);
        }

        public DataActionResultDTO AssignToProject(int id, int projectId)
        {
            return _actionRepository.AssignToProject(id, projectId);
        }

       
    }
}
