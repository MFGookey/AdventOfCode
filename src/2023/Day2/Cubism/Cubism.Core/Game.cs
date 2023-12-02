using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cubism.Core
{
  public class Game
  {
    public int GameId
    {
      get; private set;
    }

    public int RedCount
    {
      get; private set;
    }

    public int GreenCount
    {
      get; private set;
    }

    public int BlueCount
    {
      get; private set;
    }

    public int Rounds
    {
      get; private set;
    }

    public Game(string gameRecord)
    {
      // Game record format:
      // "Game #: # color, # color, ...; # color, # color, ..."
      this.RedCount = 0;
      this.GreenCount = 0;
      this.BlueCount = 0;

      var topLevelTokens = gameRecord.Split(':');
      this.GameId = ParseGameId(topLevelTokens[0]);
      var rounds = topLevelTokens[1].Split(';');
      this.Rounds = rounds.Length;

      foreach (var round in rounds)
      {
        var draws = round.Split(',');
        int temp;

        temp = ParseDraws(draws, "red");
        if (temp > this.RedCount)
        {
          this.RedCount = temp;
        }

        temp = ParseDraws(draws, "green");
        if (temp > this.GreenCount)
        {
          this.GreenCount = temp;
        }

        temp = ParseDraws(draws, "blue");
        if (temp > this.BlueCount)
        {
          this.BlueCount = temp;
        }
      }
    }

    private int ParseGameId(string token)
    {
      return int.Parse(token.Replace("Game ", string.Empty));
    }

    private int ParseDraws(IEnumerable<string> draws, string color)
    {
      if (draws.Where(draw => draw.Contains(color)).Any())
      {
        return draws.Where(draw => draw.Contains(color)).Select(draw => int.Parse(draw.Replace($" {color}", ""))).Max();
      }

      return 0;
    }
  }
}
