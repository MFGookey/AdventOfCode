using System;
using System.IO;
using Common.Utilities.IO;
using PlusMult.Core;

namespace PlusMult.Cmd
{
  /// <summary>
  /// The entrypoint for PlusMult.Cmd
  /// </summary>
  public class Program
  {
    /// <summary>
    /// PlusMult.Cmd entry point
    /// </summary>
    /// <param name="args">Command line arguments (not used)</param>
    static void Main(string[] args)
    {
      var filePath = "/input";
      
      var reader = new FileReader();
      Console.WriteLine(reader.ReadFile(Path.Join(AppDomain.CurrentDomain.BaseDirectory, filePath)).Length);

      var calculator = new MemoryCalculator("xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))");
      Console.WriteLine(calculator.CalculateSumOfProducts());
      
      calculator = new MemoryCalculator("xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))");
      Console.WriteLine(calculator.CalculateSumOfToggledProducts());

      calculator = new MemoryCalculator(reader.ReadFile(Path.Join(AppDomain.CurrentDomain.BaseDirectory, filePath)));
      Console.WriteLine(calculator.CalculateSumOfProducts());
      Console.WriteLine(calculator.CalculateSumOfToggledProducts());
      _ = Console.ReadLine();
    }
  }
}
