using GettingThingsDone.Contracts.Dto;
using GettingThingsDone.Contracts.Interface;
using static GettingThingsDone.Contracts.Interface.ServiceResult;

namespace GettingThingsDone.ApplicationCore.Services
{
    /// <summary>
    /// The legacy exchange action list format is a fixed width format specified as follows:
    /// Title:        100 characters (<see cref="TitleWidth"/>)
    /// Description: 1000 characters (<see cref="DescriptionWidth"/>
    /// In addition, an exchange value can have an arbitrary number of leading and trailing whitespaces.
    /// </summary>
    internal class LegacyExchangeActionListFormat
    {
        private const int TitleWidth = 100;
        private const int DescriptionWidth = 1000;
        private const int TotalWidth = TitleWidth + DescriptionWidth;

        private const int TitleStart = 0;
 
        public static ServiceResult<ActionListDto> ToActionListDto(string legacyExchangeValue)
        {
            var value = legacyExchangeValue.Trim();
            if (value.Length != TotalWidth)
                return InvalidOperation<ActionListDto>("Invalid legacy exchange value.");

            var title = value.Substring(TitleStart, TitleWidth).Trim();

            return Ok(new ActionListDto
            {
                Name = title
            });
        }
    }
}