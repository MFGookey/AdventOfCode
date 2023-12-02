using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cubism.Core
{
  public class GameEvaluator
  {
    private IEnumerable<Game> _games;

    public GameEvaluator(IEnumerable<string> gameRecords)
    {
      _games = gameRecords.Select(gameRecord => new Game(gameRecord)).ToList();
    }

    public IEnumerable<Game> SearchGames(int redCount, int greenCount, int blueCount)
    {
      return _games.Where(
        game => {
          return game.RedCount <= redCount
            && game.GreenCount <= greenCount
            && game.BlueCount <= blueCount;
        });
    }

    public IReadOnlyList<Game> GetGames()
    {
      return _games.ToList();
    }
  }
}
