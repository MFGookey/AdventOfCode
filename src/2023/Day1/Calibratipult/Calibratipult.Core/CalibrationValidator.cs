using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Calibratipult.Core
{
  public static class CalibrationValidator
  {
    public static IReadOnlyDictionary<string, int> ValidateCalibrationValues(IEnumerable<string> calibrationValues)
    {
      var response = new Dictionary<string, int>
      {
        { "First-Last Digit Sum", ValidateCalibrationValues_First_Last_Digit(calibrationValues) },
        { "First-Last Digit With Text", ValidateCalibrationValues_First_Last_Digit_WithText(calibrationValues) }
      };

      return response;
    }

    public static int ValidateCalibrationValues_First_Last_Digit(IEnumerable<string> calibrationValues)
    {
      int response = 0;
      foreach(var calibrationValue in  calibrationValues)
      {
        response += ValidateCalibrationValue_First_Last_Digit(calibrationValue);
      }

      return response;
    }

    public static int ValidateCalibrationValue_First_Last_Digit(string value)
    {
      var hits = new List<string>();
      int temp;
      foreach (char c in value.ToCharArray())
      {
        if (int.TryParse(c.ToString(), out temp))
        {
          hits.Add(temp.ToString());
        }
      }
      if (hits.Count >= 2)
      {
        return int.Parse($"{hits.First()}{hits.Last()}");
      }

      if (hits.Any())
      {
        return int.Parse($"{hits.First()}");
      }

      return 0;
    }

    public static int ValidateCalibrationValues_First_Last_Digit_WithText(IEnumerable<string> calibrationValues)
    {
      int response = 0;
      foreach (var calibrationValue in calibrationValues)
      {
        response += ValidateCalibrationValue_First_Last_Digit_WithText(calibrationValue);
      }

      return response;
    }

    public static int ValidateCalibrationValue_First_Last_Digit_WithText(string value)
    {
      var hits = new List<string>();
      int temp;
      foreach (var index in Enumerable.Range(0, value.Length))
      {
        // if value[index] is a digit, great!  Otherwise match it with a regex starting here.
        if (int.TryParse(value.Substring(index, 1), out temp))
        {
          hits.Add(temp.ToString());
        }

        var results = Regex.Match(value.Substring(index), $"^(one|two|three|four|five|six|seven|eight|nine|zero)");
        if (results.Success)
        {
          if (ParseTextualNumber(results.Groups[1].Value, out temp))
          {
            hits.Add(temp.ToString());
          }
          
        }
      }

      return int.Parse($"{hits.First()}{hits.Last()}");
    }

    private static bool ParseTextualNumber(string number, out int parsedResult)
    {
      if (int.TryParse(number, out parsedResult))
      {
        return true;
      }

      switch (number)
      {
        case "one":
          parsedResult =  1;
          return true;
        case "two":
          parsedResult =  2;
          return true;
        case "three":
          parsedResult =  3;
          return true;
        case "four":
          parsedResult =  4;
          return true;
        case "five":
          parsedResult =  5;
          return true;
        case "six":
          parsedResult =  6;
          return true;
        case "seven":
          parsedResult =  7;
          return true;
        case "eight":
          parsedResult =  8;
          return true;
        case "nine":
          parsedResult =  9;
          return true;
        case "zero":
          parsedResult =  0;
          return true;
        default:
          return false;

      }
    }
  }
}
