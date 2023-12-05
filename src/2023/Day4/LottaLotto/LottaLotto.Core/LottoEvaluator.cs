using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utilities.Collections;

namespace LottaLotto.Core
{
  public class LottoEvaluator
  {
    private IList<Card> _cards;
    public LottoEvaluator(IEnumerable<string> cardsToParse)
    {
      _cards = cardsToParse.Select(s => new Card(s)).ToList();
    }

    public int GetScore()
    {
      return _cards.Select(c => c.ScoreCard()).Sum();
    }

    public int GetCardCount()
    {
      foreach (var card in _cards.OrderBy(c => c.CardId))
      {
        for (var i = 0; i < card.MatchCount; i++)
        {
          _cards[card.CardId + i].InstanceCount += card.InstanceCount;
        }
      }

      return _cards.Select(c => c.InstanceCount).Sum();
    }
  }
}
