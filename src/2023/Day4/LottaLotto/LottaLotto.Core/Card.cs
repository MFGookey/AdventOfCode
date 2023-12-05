using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LottaLotto.Core
{
  public class Card
  {
    public int CardId
    {
      get; private set;
    }

    public IEnumerable<int> WinningNumbers
    {
      get; private set;
    }

    public IEnumerable<int> DrawnNumbers
    {
      get;
      private set;
    }

    public Card(string toParse)
    {
      var temp = toParse.Split(':');
      this.CardId = int.Parse(temp[0].Replace("Card ", string.Empty));
      temp = temp[1].Split('|');
      this.WinningNumbers = temp[0].Trim().Replace("  ", " ").Split(" ").Select(n => int.Parse(n.Trim())).ToList();
      this.DrawnNumbers = temp[1].Trim().Replace("  ", " ").Split(" ").Select(n => int.Parse(n.Trim())).ToList();
    }

    public int ScoreCard()
    {
      var matches = this.WinningNumbers.Join(DrawnNumbers, w => w, d => d, (w, d) => w);
      if (matches.Any())
      {
        return (int)Math.Pow(2, matches.Count() - 1);
      }
      return 0;
    }
  }
}
