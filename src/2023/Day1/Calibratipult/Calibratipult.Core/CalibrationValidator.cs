using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calibratipult.Core
{
  public static class CalibrationValidator
  {
    public static IReadOnlyDictionary<string, int> ValidateCalibrationValues(IEnumerable<string> calibrationValues)
    {
      var response = new Dictionary<string, int>
      {
        { "First-Last Digit Sum", ValidateCalibrationValues_First_Last_Digit(calibrationValues) }
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

      return int.Parse($"{hits.First()}{hits.Last()}");
    }
  }
}
