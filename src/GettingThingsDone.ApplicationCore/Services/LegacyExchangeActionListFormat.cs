using System;
using GettingThingsDone.Contracts.Dto;
using GettingThingsDone.Contracts.Interface;
using GettingThingsDone.Contracts.Model;
using static GettingThingsDone.Contracts.Interface.ServiceResult;

namespace GettingThingsDone.ApplicationCore.Services
{
    /// <summary>
    /// The legacy exchange action list format is a fixed width format specified as follows:
    /// Title:        100 characters (<see cref="TitleWidth"/>)
    /// Description: 1000 characters (<see cref="DescriptionWidth"/>
    /// In addition, an exchange value can have an arbitrary number of leading and trailing whitespaces.
    /// </summary>
    internal static class LegacyExchangeActionListFormat
    {
        private const int TitleWidth = 100;
        private const int DescriptionWidth = 1000;
        private const int TotalWidth = TitleWidth + DescriptionWidth;

        private const int TitleStart = 0;
 
        public static ServiceResult<ActionListDto> ToActionListDto(string legacyExchangeValue)
        {
            var value = legacyExchangeValue.AsSpan().Trim();
            if (value.Length != TotalWidth)
                return InvalidOperation<ActionListDto>("Invalid legacy exchange value.");

            var title = value.Slice(TitleStart, TitleWidth).Trim();

            return Ok(new ActionListDto
            {
                Name = new string(title)
            });
        }

        public static string ToLegacyFormat(this ActionList actionList)
        {
            return string.Create(TotalWidth, actionList, (span, state) =>
            {
                span.Fill(' ');

                var titleSpan = span.Slice(TitleStart, TitleWidth);

                state.Name?.AsSpan(0, Math.Min(TitleWidth, state.Name.Length)).CopyTo(titleSpan);
            });
        }
    }
}