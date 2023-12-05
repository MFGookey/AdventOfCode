using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LottaLotto.Core
{
  public class LottoEvaluator
  {
    private IEnumerable<Card> _cards;
    public LottoEvaluator(IEnumerable<string> cardsToParse)
    {
      _cards = cardsToParse.Select(s => new Card(s)).ToList();
    }

    public int GetScore()
    {
      return _cards.Select(c => c.ScoreCard()).Sum();
    }
  }
}
