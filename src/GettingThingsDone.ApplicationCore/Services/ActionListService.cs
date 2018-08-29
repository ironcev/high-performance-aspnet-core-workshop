using System.Collections.Generic;
using System.Linq;
using GettingThingsDone.ApplicationCore.Helpers;
using GettingThingsDone.Contracts.Dto;
using GettingThingsDone.Contracts.Interface;
using GettingThingsDone.Contracts.Model;
using static GettingThingsDone.Contracts.Interface.ServiceResult;

namespace GettingThingsDone.ApplicationCore.Services
{
    public class ActionListService : IActionListService
    {
        private readonly IRepository<ActionList> _listRepository;

        public ActionListService(IRepository<ActionList> listRepository)
        {
            _listRepository = listRepository;
        }

        public ServiceResult<ActionListDto> GetList(int id)
        {
            var list = _listRepository.GetById(id);

            if (list == null)
                return EntityNotFound<ActionListDto>();

            return list
                .TranslateTo<ActionListDto>()
                .ToOkServiceResult();
        }

        public ServiceResult<List<ActionListDto>> GetAll()
        {
            return _listRepository
                .GetAll()
                .Select(list => list.TranslateTo<ActionListDto>())
                .ToList()
                .ToOkServiceResult();
        }

        public ServiceResult<ActionListDto> CreateOrUpdate(ActionListDto actionListDto)
        {
            ActionList list = actionListDto.RepresentsNewEntity
                ? actionListDto.TranslateTo<ActionList>()
                : _listRepository.GetById(actionListDto.Id).CopyPropertiesFrom(actionListDto);

            if (list == null)
                return EntityNotFound(actionListDto);

            // TODO: Later on we will do the checks here.
            //       So far we assume everything always works fine.

            list = _listRepository.AddOrUpdate(list);

            actionListDto.Id = list.Id;

            return Ok(actionListDto);
        }

        public ServiceResult<List<ActionDto>> GetListActions(int id)
        {
            var list = _listRepository.GetByIdAndInclude(id, x => x.Actions);
            if (list == null)
                return EntityNotFound<List<ActionDto>>();

            return list.Actions
                .Select(action => action.TranslateTo<ActionDto>())
                .ToList()
                .ToOkServiceResult();
        }

        public ServiceResult Delete(int id)
        {
            var list = _listRepository.GetById(id);

            if (list == null)
                return EntityNotFound();

            _listRepository.Delete(list);

            return Ok();
        }
    }
}