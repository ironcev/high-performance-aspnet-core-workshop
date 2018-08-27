using GettingThingsDone.Contract.DTO;
using GettingThingsDone.Contract.Interface;
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

        public GettingThingsDone.Contract.Model.Action GetAction(int id)
        {
            var actionItem = _actionRepository.GetById(id);
            return actionItem;
        }

        public Contract.Model.Action CreateOrUpdate(int? id, Contract.Model.Action action)
        {
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
                }
            } catch
            {

            }
            return action;
        }

        public List<Contract.Model.Action> GetTop(int count)
        {
            return _actionRepository.GetTop(count);
            
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
