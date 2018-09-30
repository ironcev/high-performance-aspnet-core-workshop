using System.Collections.Generic;
using System.Threading.Tasks;
using GettingThingsDone.Contracts.Dto;

namespace GettingThingsDone.Contracts.Interface
{
    public interface IActionListService
    {
        Task<ServiceResult<ActionListDto>> GetList(int id);
        Task<ServiceResult<List<ActionListDto>>> GetAll();
        Task<ServiceResult<ActionListDto>> CreateOrUpdate(ActionListDto actionListDto);
        Task<ServiceResult<ActionListDto>> CreateFromLegacyExchangeFormat(string legacyExchangeValue);
        Task<ServiceResult<List<ActionDto>>> GetListActions(int id);
        Task<ServiceResult> Delete(int id);
    }
}