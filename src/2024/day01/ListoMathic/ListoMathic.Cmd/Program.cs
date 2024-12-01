using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using Common.Utilities.Formatter;
using Common.Utilities.IO;
using ListoMathic.Core;
//using ListoMathic.Core;

namespace ListoMathic.Cmd
{
  /// <summary>
  /// The entrypoint for ListoMathic.Cmd
  /// </summary>
  public class Program
  {
    /// <summary>
    /// ListoMathic.Cmd entry point
    /// </summary>
    /// <param name="args">Command line arguments (not used)</param>
    static void Main(string[] args)
    {
      var filePath = "/input";
      var reader = new FileReader();
      Console.WriteLine(reader.ReadFile(Path.Join(AppDomain.CurrentDomain.BaseDirectory, filePath)).Length);
      var formatter = new RecordFormatter(reader);
      IEnumerable<string> records = @"3   4
4   3
2   5
1   3
3   9
3   3".Split('\n');
      var calculator = new ListCalculator(records);
      Console.WriteLine(calculator.CalculateSumOfOrderedDifferences());
      Console.WriteLine(calculator.CalculateSimilarityScore());
      records = formatter.FormatFile(Path.Join(AppDomain.CurrentDomain.BaseDirectory, filePath), "\n", true);
      calculator = new ListCalculator(records);
      Console.WriteLine(calculator.CalculateSumOfOrderedDifferences());
      Console.WriteLine(calculator.CalculateSimilarityScore());
      _ = Console.ReadLine();
    }
  }
}
