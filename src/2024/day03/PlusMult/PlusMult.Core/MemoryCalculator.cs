using System.Text.RegularExpressions;

namespace PlusMult.Core;

public class MemoryCalculator
{
  private readonly string _memoryDump;
  private readonly Regex _instructionFinder;
  private readonly Regex _blockSplitter;
  public MemoryCalculator(string memoryDump) {
    _memoryDump = memoryDump;
    _instructionFinder = new Regex(@"mul\((\d+),(\d+)\)");
    _blockSplitter = new Regex(@"(?=do(?:n't)?\(\))");
  }

  public int CalculateSumOfProducts() {
    return CalculateSumOfProducts(_memoryDump);
  }

  private int CalculateSumOfProducts(string memoryDump){
    var matches = _instructionFinder.Matches(memoryDump);
    int sum = 0;
    for(var i = 0; i < matches.Count; i++) {
      sum += ParseAndMultiply(matches[i]);
    }

    return sum;
  }

  public int CalculateSumOfToggledProducts() {
    var blocks = _blockSplitter.Split(_memoryDump);
    int sum = 0;
    foreach(var block in blocks){
      if(!block.StartsWith("don't()")) {
        sum += CalculateSumOfProducts(block);
      }
    }
    return sum;
  }

  private int ParseAndMultiply(Match match){
    return int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);
  }
}
