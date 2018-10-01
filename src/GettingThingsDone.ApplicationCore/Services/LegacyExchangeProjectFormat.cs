using System;
using System.Buffers;
using GettingThingsDone.Contracts.Dto;
using GettingThingsDone.Contracts.Interface;
using GettingThingsDone.Contracts.Model;
using static GettingThingsDone.Contracts.Interface.ServiceResult;

namespace GettingThingsDone.ApplicationCore.Services
{
    /// <summary>
    /// The legacy exchange project format is a fixed width format specified as follows:
    /// Title:        100 characters (<see cref="TitleWidth"/>)
    /// Description: 1000 characters (<see cref="DescriptionWidth"/>
    /// In addition, an exchange value can have an arbitrary number of leading and trailing whitespaces.
    /// </summary>
    internal static class LegacyExchangeProjectFormat
    {
        private const int TitleWidth = 100;
        private const int DescriptionWidth = 1000;
        private const int TotalWidth = TitleWidth + DescriptionWidth;

        private const int TitleStart = 0;
 
        public static ServiceResult<ProjectDto> ToProjectDto(string legacyExchangeValue)
        {
            var value = legacyExchangeValue.AsSpan().Trim();
            if (value.Length != TotalWidth)
                return InvalidOperation<ProjectDto>("Invalid legacy exchange value.");

            var title = value.Slice(TitleStart, TitleWidth).Trim();

            return Ok(new ProjectDto
            {
                Name = new string(title)
            });
        }

        public static string ToLegacyFormat(this Project project)
        {
            var buffer = ArrayPool<char>.Shared.Rent(TotalWidth);

            string result;
            try
            {
                Array.Fill(buffer, ' ');

                project.Name?.CopyTo(0, buffer, TitleStart, Math.Min(TitleWidth, project.Name.Length));

                result = new string(buffer);
            }
            finally 
            {
                ArrayPool<char>.Shared.Return(buffer);
            }

            return result;
        }
    }
}