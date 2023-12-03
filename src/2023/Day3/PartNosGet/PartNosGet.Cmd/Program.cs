using System;
using System.Linq;
using Common.Utilities.IO;
using PartNosGet.Core;

namespace PartNosGet.Cmd
{
  /// <summary>
  /// The entrypoint for PartNosGet.Cmd
  /// </summary>
  public class Program
  {
    /// <summary>
    /// PartNosGet.Cmd entry point
    /// </summary>
    /// <param name="args">Command line arguments (not used)</param>
    static void Main(string[] args)
    {
      var filePath = "./input";
      var reader = new FileReader();
      Console.WriteLine(reader.ReadFile(filePath).Length);

      var testData = new string[]
      {
        "467..114..",
        "...*......",
        "..35..633.",
        "......#...",
        "617*......",
        ".....+.58.",
        "..592.....",
        "......755.",
        "...$.*....",
        ".664.598.."
      };

      var testMapper = new EngineMapper(testData);

      Console.WriteLine(testMapper.PrintDataPoints());

      foreach (var point in testMapper.GetSpecialPoints())
      {
        Console.WriteLine(point);
      }

      var numbers = testMapper.FindPartNumbers();
      Console.WriteLine(numbers.Sum());

      var mapper = new EngineMapper(reader.ReadFileByLines(filePath));
      Console.WriteLine(mapper.FindPartNumbers().Sum());

      _ = Console.ReadLine();
    }
  }
}
