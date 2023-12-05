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

    public int InstanceCount
    {
      get;
      set;
    }

    public int MatchCount
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
      this.MatchCount = this.WinningNumbers.Join(DrawnNumbers, w => w, d => d, (w, d) => w).Count();
      this.InstanceCount = 1;
    }

    public int ScoreCard()
    {
      if (this.MatchCount > 0)
      {
        return (int)Math.Pow(2, this.MatchCount - 1);
      }
      return 0;
    }
  }
}
