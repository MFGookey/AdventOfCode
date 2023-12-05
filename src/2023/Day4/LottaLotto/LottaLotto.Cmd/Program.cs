using System;
using Common.Utilities.IO;
using LottaLotto.Core;

namespace LottaLotto.Cmd
{
  /// <summary>
  /// The entrypoint for LottaLotto.Cmd
  /// </summary>
  public class Program
  {
    /// <summary>
    /// LottaLotto.Cmd entry point
    /// </summary>
    /// <param name="args">Command line arguments (not used)</param>
    static void Main(string[] args)
    {
      var filePath = "./input";
      var reader = new FileReader();
      Console.WriteLine(reader.ReadFile(filePath).Length);
      var testData = new[]
      {
        "Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53",
        "Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19",
        "Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1",
        "Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83",
        "Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36",
        "Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11"
      };

      var testEval = new LottoEvaluator(testData);
      Console.WriteLine(testEval.GetScore());
      Console.WriteLine(testEval.GetCardCount());
      var eval = new LottoEvaluator(reader.ReadFileByLines(filePath));
      Console.WriteLine(eval.GetScore());
      Console.WriteLine(eval.GetCardCount());
      _ = Console.ReadLine();
    }
  }
}
