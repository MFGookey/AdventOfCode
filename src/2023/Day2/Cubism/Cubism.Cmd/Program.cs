using System;
using System.Linq;
using Common.Utilities.IO;
using Cubism.Core;

namespace Cubism.Cmd
{
  /// <summary>
  /// The entrypoint for Cubism.Cmd
  /// </summary>
  public class Program
  {
    /// <summary>
    /// Cubism.Cmd entry point
    /// </summary>
    /// <param name="args">Command line arguments (not used)</param>
    static void Main(string[] args)
    {
      var filePath = "./input";
      var reader = new FileReader();
      Console.WriteLine(reader.ReadFile(filePath).Length);

      var testData = new[] {
        "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green",
        "Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue",
        "Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red",
        "Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red",
        "Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green"
      };

      var testEvaluator = new GameEvaluator(testData);
      Console.WriteLine(testEvaluator.SearchGames(12, 13, 14).Select(game => game.GameId).Sum());

      Console.WriteLine(testEvaluator.GetGames().Select(game => game.GetPower()).Sum());

      var evaluator = new GameEvaluator(reader.ReadFileByLines(filePath));
      Console.WriteLine(evaluator.SearchGames(12, 13, 14).Select(game => game.GameId).Sum());

      Console.WriteLine(evaluator.GetGames().Select(game => game.GetPower()).Sum());

      _ = Console.ReadLine();
    }
  }
}
