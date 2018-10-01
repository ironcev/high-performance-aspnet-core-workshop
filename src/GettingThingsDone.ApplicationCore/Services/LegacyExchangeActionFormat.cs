using System;
using System.Buffers;
using System.Globalization;
using GettingThingsDone.Contracts.Dto;
using GettingThingsDone.Contracts.Interface;
using static GettingThingsDone.Contracts.Interface.ServiceResult;
using Action = GettingThingsDone.Contracts.Model.Action;

namespace GettingThingsDone.ApplicationCore.Services
{
    /// <summary>
    /// The legacy exchange action format is a fixed width format specified as follows:
    /// Title:        100 characters (<see cref="TitleWidth"/>)
    /// Description: 1000 characters (<see cref="DescriptionWidth"/>
    /// Due Date:      10 characters in format yyyy.MM.dd (<see cref="DueDateWidth"/>)
    /// In addition, an exchange value can have an arbitrary number of leading and trailing whitespaces.
    /// </summary>
    internal static class LegacyExchangeActionFormat
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
            var value = legacyExchangeValue.AsSpan().Trim();
            if (value.Length != TotalWidth)
                return InvalidOperation<ActionDto>("Invalid legacy exchange value.");

            var title = value.Slice(TitleStart, TitleWidth).Trim();
            var dueDateAsText = value.Slice(DueDateStart, DueDateWidth).Trim();

            var dueDate = DateTimeOffset.MinValue;
            if (dueDateAsText.Length > 0)
            {
                if (!DateTimeOffset.TryParseExact(
                    dueDateAsText,
                    DueDateFormat,
                    DateTimeFormatInfo.InvariantInfo,
                    DateTimeStyles.None,
                    out dueDate))
                        return InvalidOperation<ActionDto>("Invalid due date.");
            }

            return Ok(new ActionDto
            {
                Title = new string(title),
                DueDate = dueDate == DateTimeOffset.MinValue ? (DateTimeOffset?)null : dueDate
            });
        }

        public static string ToLegacyFormat(this Action action)
        {            
            var buffer = ArrayPool<char>.Shared.Rent(TotalWidth);

            Array.Fill(buffer, ' ');

            action.Title?.CopyTo(0, buffer, TitleStart, Math.Min(TitleWidth, action.Title.Length));

            action.DueDate?.ToString(DueDateFormat).CopyTo(0, buffer, DueDateStart, DueDateWidth);

            var result = new string(buffer);

            ArrayPool<char>.Shared.Return(buffer);

            return result;
        }
    }
}
