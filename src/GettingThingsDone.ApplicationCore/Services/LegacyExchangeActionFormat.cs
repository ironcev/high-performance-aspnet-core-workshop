using System;
using System.Globalization;
using GettingThingsDone.Contracts.Dto;
using GettingThingsDone.Contracts.Interface;
using static GettingThingsDone.Contracts.Interface.ServiceResult;

namespace GettingThingsDone.ApplicationCore.Services
{
    /// <summary>
    /// The legacy exchange action format is a fixed width format specified as follows:
    /// Title:        100 characters (<see cref="TitleWidth"/>)
    /// Description: 1000 characters (<see cref="DescriptionWidth"/>
    /// Due Date:      10 characters in format yyyy.MM.dd (<see cref="DueDateWidth"/>)
    /// In addition, an exchange value can have an arbitrary number of leading and trailing whitespaces.
    /// </summary>
    internal class LegacyExchangeActionFormat
    {
        private const int TitleWidth = 100;
        private const int DescriptionWidth = 1000;
        private const int DueDateWidth = 10;
        private const int TotalWidth = TitleWidth + DescriptionWidth + DueDateWidth;

        private const int TitleStart = 0;
        private const int DueDateStart = TitleWidth + DescriptionWidth - 1;

        private const string DueDateFormat = "yyyy.MM.dd";

        public static ServiceResult<ActionDto> ToActionDto(string legacyExchangeValue)
        {
            var value = legacyExchangeValue.Trim();
            if (value.Length != TotalWidth)
                return InvalidOperation<ActionDto>("Invalid legacy exchange value.");

            var title = value.Substring(TitleStart, TitleWidth).Trim();
            var dueDateAsText = value.Substring(DueDateStart, DueDateWidth);

            if (!DateTimeOffset.TryParseExact(
                    dueDateAsText,
                    DueDateFormat,
                    DateTimeFormatInfo.InvariantInfo,
                    DateTimeStyles.None,
                    out var dueDate))
                return InvalidOperation<ActionDto>("Invalid due date.");

            return Ok(new ActionDto
            {
                Title = title,
                DueDate = dueDate
            });
        }
    }
}
