using System.Collections.Generic;
using GettingThingsDone.Contracts.Dto;

namespace GettingThingsDone.Contracts.Interface
{
    public interface IActionListService
    {
        ServiceResult<ActionListDto> GetList(int id);
        ServiceResult<List<ActionListDto>> GetAll();
        ServiceResult<ActionListDto> CreateOrUpdate(ActionListDto actionListDto);
        ServiceResult<List<ActionDto>> GetListActions(int id);
        ServiceResult Delete(int id);
    }
}