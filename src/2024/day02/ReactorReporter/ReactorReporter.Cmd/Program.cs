﻿using System;
using System.IO;
using System.Linq;
using Common.Utilities.Formatter;
using Common.Utilities.IO;
using ReactorReporter.Core;

namespace ReactorReporter.Cmd
{
  /// <summary>
  /// The entrypoint for ReactorReporter.Cmd
  /// </summary>
  public class Program
  {
    /// <summary>
    /// ReactorReporter.Cmd entry point
    /// </summary>
    /// <param name="args">Command line arguments (not used)</param>
    static void Main(string[] args)
    {
      var filePath = "/input";
      var reader = new FileReader();
      var reports = @"7 6 4 2 1
1 2 7 8 9
9 7 6 2 1
1 3 2 4 5
8 6 4 4 1
1 3 6 7 9".Split('\n').AsEnumerable();
      Console.WriteLine(reports.ToList().Count);
      var reactorReportEvaluator = new ReactorReportEvaluator(reports);
      Console.WriteLine(reactorReportEvaluator.CalculateSafeReports(1, 3));
      Console.WriteLine(reactorReportEvaluator.CalculateSafeReports(1, 3, 1));

      var formatter = new RecordFormatter(reader);
      reports = formatter.FormatFile(Path.Join(AppDomain.CurrentDomain.BaseDirectory, filePath), "\n", true);
      reactorReportEvaluator = new ReactorReportEvaluator(reports);
      Console.WriteLine(reactorReportEvaluator.CalculateSafeReports(1, 3));
      Console.WriteLine(reactorReportEvaluator.CalculateSafeReports(1, 3, 1));
      _ = Console.ReadLine();
    }
  }
}
