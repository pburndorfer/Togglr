﻿using System;
using System.Linq;
using System.Text;
using Adliance.Togglr.Extensions;

namespace Adliance.Togglr
{
    public static class DayStatistics
    {
        public static void WriteEveryDayInMonth(Configuration configuration, StringBuilder sb, DateTime firstDayOfMonth, CalculationService calculationService)
        {
            sb.AppendLine("<div class=\"container\">");
            sb.AppendLine($"<h2 class=\"title is-5\" style=\"margin: 2rem 0 0 0;\">{firstDayOfMonth:MMMM yyyy}</h2>");
            sb.AppendLine("<table class=\"table is-size-7\" style=\"margin:1rem 0 0 0;\">");
            sb.AppendLine("<thead><tr>");
            sb.AppendLine("<th>Tag</th>");
            sb.AppendLine("<th class=\"has-text-right\">Soll (h)</th>");
            sb.AppendLine("<th class=\"has-text-right\">Ist (h)</th>");
            sb.AppendLine("<th class=\"has-text-right\">Pause (h)</th>");
            sb.AppendLine("<th class=\"has-text-right\">Überst. (h)</th>");
            sb.AppendLine("<th class=\"has-text-right\">Saldo (h)</th>");
            sb.AppendLine("<th>Anmerkungen</th>");
            sb.AppendLine("</tr></thead>");
            sb.AppendLine("<tbody>");

            var loopDate = firstDayOfMonth;
            while (loopDate.Month == firstDayOfMonth.Month)
            {
                if (calculationService.Days.ContainsKey(loopDate.Date))
                {
                    WriteDay(configuration, sb, calculationService.Days[loopDate.Date]);
                }

                loopDate = loopDate.AddDays(1);
            }

            sb.AppendLine("</tbody>");
            sb.AppendLine("</table>");
            sb.AppendLine("</div>");
        }

        private static void WriteDay(Configuration configuration, StringBuilder sb, CalculationService.Day day)
        {
            sb.AppendLine($"<tr class=\"{(day.Date.IsWeekend() ? "has-text-grey-light" : "")}\">");
            sb.AppendLine($"<td>{day.Date:dddd, dd.MM.yyyy}</td>");

            if (!day.Date.IsWeekend())
            {
                sb.AppendLine($"<td class=\"has-text-right\">{day.Expected:N2}</td>");
                sb.AppendLine($"<td class=\"has-text-right\">{day.Total:N2}</td>");
                sb.AppendLine($"<td class=\"has-text-right\">{day.Breaks:N2}</td>");
            }
            else
            {
                sb.AppendLine("<td></td><td></td><td></td>");
            }

            sb.AppendLine($"<td class=\"has-text-right has-text-success\">{day.Overtime.FormatColor()}</td>");

            sb.AppendLine(!day.Date.IsWeekend()
                ? $"<td class=\"has-text-right has-text-success\">{day.RollingOvertime.FormatColor()}</td>"
                : "<td></td>");

            sb.Append("<td>");
            foreach (var s in day.Specials.Where(x => x.Value > 0))
            {
                sb.Append($"<span class=\"tag is-success\">{s.Key.GetName(configuration)}</span> ");
            }

            foreach (var w in day.Warnings)
            {
                sb.Append($"<span class=\"tag is-danger\">{w}</span> ");
            }

            sb.AppendLine("</td>");

            sb.AppendLine("</tr>");
        }
    }
}