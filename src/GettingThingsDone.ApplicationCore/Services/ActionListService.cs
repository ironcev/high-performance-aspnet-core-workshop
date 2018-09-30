using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GettingThingsDone.ApplicationCore.Helpers;
using GettingThingsDone.Contracts.Dto;
using GettingThingsDone.Contracts.Interface;
using GettingThingsDone.Contracts.Model;
using static GettingThingsDone.Contracts.Interface.ServiceResult;

namespace GettingThingsDone.ApplicationCore.Services
{
    public class ActionListService : IActionListService
    {
        private readonly IAsyncRepository<ActionList> _listRepository;

        public ActionListService(IAsyncRepository<ActionList> listRepository)
        {
            _listRepository = listRepository;
        }

        public async Task<ServiceResult<ActionListDto>> GetList(int id)
        {
            var list = await _listRepository.GetById(id);

            if (list == null)
                return EntityNotFound<ActionListDto>();

            return list
                .TranslateTo<ActionListDto>()
                .ToOkServiceResult();
        }

        public async Task<ServiceResult<List<ActionListDto>>> GetAll()
        {
            return (await _listRepository
                .GetAll(TrackingOption.WithoutTracking))
                .Select(list => list.TranslateTo<ActionListDto>())
                .ToList()
                .ToOkServiceResult();
        }

        public async Task<ServiceResult<ActionListDto>> CreateOrUpdate(ActionListDto actionListDto)
        {
            ActionList list = actionListDto.RepresentsNewEntity
                ? actionListDto.TranslateTo<ActionList>()
                : (await _listRepository.GetById(actionListDto.Id)).CopyPropertiesFrom(actionListDto);

            if (list == null)
                return EntityNotFound(actionListDto);

            // TODO: Later on we will do the checks here.
            //       So far we assume everything always works fine.

            list = await _listRepository.AddOrUpdate(list);

            actionListDto.Id = list.Id;

            return Ok(actionListDto);
        }

        public Task<ServiceResult<ActionListDto>> CreateFromLegacyExchangeFormat(string legacyExchangeValue)
        {
            var result = LegacyExchangeActionListFormat.ToActionListDto(legacyExchangeValue);

            if (!result.IsOk()) return Task.FromResult(result);

            return CreateOrUpdate(result.Value);
        }

        public async Task<ServiceResult<List<ActionDto>>> GetListActions(int id)
        {
            var list = await _listRepository.GetByIdAndInclude(id, x => x.Actions, TrackingOption.WithoutTracking);
            if (list == null)
                return EntityNotFound<List<ActionDto>>();

            return list.Actions
                .Select(action => action.TranslateTo<ActionDto>())
                .ToList()
                .ToOkServiceResult();
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var list = await _listRepository.GetById(id);

            if (list == null)
                return EntityNotFound();

            await _listRepository.Delete(list);

            return Ok();
        }
    }
}