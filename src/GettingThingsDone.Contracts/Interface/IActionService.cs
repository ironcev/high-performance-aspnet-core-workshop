using System.Collections.Generic;
using System.Threading.Tasks;
using GettingThingsDone.Contracts.Dto;

namespace GettingThingsDone.Contracts.Interface
{
    public interface IActionService
    {
        Task<ServiceResult<ActionDto>> GetAction(int id);
        Task<ServiceResult<List<ActionDto>>> GetAll();
        Task<ServiceResult<ActionDto>> CreateOrUpdate(ActionDto actionDto);
        Task<ServiceResult<ActionDto>> CreateFromLegacyExchangeFormat(string legacyExchangeValue);
        Task<ServiceResult> Delete(int id);
        Task<ServiceResult> MoveToList(int id, int listId);
        Task<ServiceResult> AssignToProject(int id, int projectId);
    }
}