using System;
using System.Collections.Generic;
using System.Globalization;
using System.Data;
using System.IO;
using System.Globalization;

namespace Ema.IjoinsChkInOut.Api.Helpers
{
  public static class Utility
  {

    public static DateTime AddBusinessDays(DateTime date, int days)
    {
      if (days < 0)
      {
        throw new ArgumentException("days cannot be negative", "days");
      }

      if (days == 0) return date;

      if (date.DayOfWeek == DayOfWeek.Saturday)
      {
        date = date.AddDays(2);
        days -= 1;
      }
      else if (date.DayOfWeek == DayOfWeek.Sunday)
      {
        date = date.AddDays(1);
        days -= 1;
      }

      date = date.AddDays(days / 5 * 7);
      int extraDays = days % 5;

      if ((int)date.DayOfWeek + extraDays > 5)
      {
        extraDays += 2;
      }

      return date.AddDays(extraDays);

    }

    public static int GetBusinessDays(DateTime start, DateTime end)
    {
      if (start.DayOfWeek == DayOfWeek.Saturday)
      {
        start = start.AddDays(2);
      }
      else if (start.DayOfWeek == DayOfWeek.Sunday)
      {
        start = start.AddDays(1);
      }

      if (end.DayOfWeek == DayOfWeek.Saturday)
      {
        end = end.AddDays(-1);
      }
      else if (end.DayOfWeek == DayOfWeek.Sunday)
      {
        end = end.AddDays(-2);
      }

      int diff = (int)end.Subtract(start).TotalDays;

      int result = diff / 7 * 5 + diff % 7;

      if (end.DayOfWeek < start.DayOfWeek)
      {
        return result - 2;
      }
      else
      {
        return result;
      }
    }


    public static string GetStryyyyMMddNow()
    {
      CultureInfo enUS = new CultureInfo("en-US");
      return DateTime.Now.ToString("yyyyMMdd", enUS);
    }

    public static string GetStrHHmmNow()
    {
      CultureInfo enUS = new CultureInfo("en-US");
      return DateTime.Now.ToString("HHmm", enUS);
    }

    public static DateTime GetStartDay()
    {
      CultureInfo enUS = new CultureInfo("en-US");
      DateTime.TryParseExact(DateTime.Now.ToString("yyyyMMdd") + " " + "01AM", "yyyyMMdd hhtt", enUS, DateTimeStyles.None, out DateTime StartDay);
      return StartDay;
    }

    public static DateTime GetEndDay()
    {
      CultureInfo enUS = new CultureInfo("en-US");
      DateTime.TryParseExact(DateTime.Now.ToString("yyyyMMdd") + " " + "11PM", "yyyyMMdd hhtt", enUS, DateTimeStyles.None, out DateTime EndDay);
      return EndDay;
    }

  }
}
