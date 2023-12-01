using System;
using Common.Utilities.IO;
using Calibratipult.Core;
using System.Collections.Generic;
using System.Text;

namespace Calibratipult.Cmd
{
  /// <summary>
  /// The entrypoint for Calibratipult.Cmd
  /// </summary>
  public class Program
  {
    /// <summary>
    /// Calibratipult.Cmd entry point
    /// </summary>
    /// <param name="args">Command line arguments (not used)</param>
    static void Main(string[] args)
    {
      var filePath = "./input";
      var reader = new FileReader();
      //Console.WriteLine(reader.ReadFile(filePath).Length);

      var testData = new List<string>
      {
        "1abc2",
        "pqr3stu8vwx",
        "a1b2c3d4e5f",
        "treb7uchet"
      };

      foreach (var test in testData)
      {
        Console.WriteLine($"{test}:\t{CalibrationValidator.ValidateCalibrationValue_First_Last_Digit(test)}");
      }

      Console.WriteLine($"Test Collection:\t{CalibrationValidator.ValidateCalibrationValues_First_Last_Digit(testData)}");

      var testResults = CalibrationValidator.ValidateCalibrationValues(testData);

      Console.WriteLine(FormatDictionary(testResults));

      testData = new List<string>
      {
        "two1nine",
        "eightwothree",
        "abcone2threexyz",
        "xtwone3four",
        "4nineeightseven2",
        "zoneight234",
        "7pqrstsixteen"
      };

      foreach (var test in testData)
      {
        Console.WriteLine($"{test}:\t{CalibrationValidator.ValidateCalibrationValue_First_Last_Digit_WithText(test)}");
      }

      Console.WriteLine($"Test Collection:\t{CalibrationValidator.ValidateCalibrationValues_First_Last_Digit_WithText(testData)}");

      testResults = CalibrationValidator.ValidateCalibrationValues(testData);

      Console.WriteLine(FormatDictionary(testResults));

      var results = CalibrationValidator.ValidateCalibrationValues(reader.ReadFileByLines(filePath));
      Console.WriteLine(FormatDictionary(results));

      _ = Console.ReadLine();
    }

    private static string FormatDictionary(IReadOnlyDictionary<string, int> toFormat)
    {
      StringBuilder sb = new StringBuilder();
      foreach(var kvp in toFormat)
      {
        sb.AppendLine($"{kvp.Key}:\t{kvp.Value}");
      }

      return sb.ToString();
    }
  }
}
