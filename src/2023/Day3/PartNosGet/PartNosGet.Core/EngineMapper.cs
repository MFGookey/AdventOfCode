using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utilities.TwoD;

namespace PartNosGet.Core
{
  public class EngineMapper
  {
    private DataPoint[,] _mappedSchematic;

    public EngineMapper(IList<string> schematic)
    {
      _mappedSchematic = new DataPoint[schematic.Count,schematic.First().Length];

      string line;

      var neighborMap = MatrixHelper.GenerateNeighborMaps(_mappedSchematic.GetLength(0), _mappedSchematic.GetLength(1));

      Point pointer;

      for(var i = 0; i < schematic.Count; i++)
      {
        line = schematic[i];
        for(var j = 0; j < line.Length; j++)
        {
          pointer = new Point(i, j);
          _mappedSchematic[i, j] = new DataPoint(i, j, line[j], neighborMap[pointer]);
        }
      }
    }

    public string PrintDataPoints()
    {
      var sb = new StringBuilder();
      sb.AppendLine("Row v Col >");
      sb.Append("\t");
      for(var j = 0; j < _mappedSchematic.GetLength(1); j++)
      {
        sb.Append($"{j}\t");
      }

      sb.AppendLine();

      for (var i = 0; i < _mappedSchematic.GetLength(0); i++)
      {
        sb.Append($"[{i}:\t");
        for (var j = 0; j < _mappedSchematic.GetLength(1); j++)
        {
          sb.Append(_mappedSchematic[i, j].Datum);
          sb.Append("\t");
        }
        sb.AppendLine("]");
      }

      return sb.ToString();
    }

    public IEnumerable<DataPoint> GetSpecialPoints()
    {
      return _mappedSchematic.Cast<DataPoint>().Where(p => p.IsSpecial);
    }

    public IEnumerable<int> FindPartNumbers()
    {
      // part numbers are adjacent to special points
      var specials = GetSpecialPoints();
      var candidates = new List<DataPoint>();
      var results = new List<int>();
      var startingPoints = new List<DataPoint>();

      foreach (var point  in specials)
      {
        foreach (var neighbor in point.Neighbors)
        {
          if (_mappedSchematic[neighbor.Row, neighbor.Column].Digit.HasValue)
          {
            candidates.Add(_mappedSchematic[neighbor.Row, neighbor.Column]);
          }
        }
      }

      // for each candidate, find the full number's starting point and ending point
      int index;
      StringBuilder sb = new StringBuilder();

      foreach (var point in candidates)
      {
        sb.Clear();
        // start at the point and search leftward first
        index = point.Column - 1;
        while (index >= 0)
        {
          if (_mappedSchematic[point.Row, index].Digit.HasValue)
          {
            index--;
          }
          else
          {
            index++;
            break;
          }
        }

        if (index < 0)
        {
          index = 0;
        }

        if (startingPoints.Contains(_mappedSchematic[point.Row, index]) == false)
        {
          startingPoints.Add(_mappedSchematic[point.Row, index]);


          // Number starts at point.Row, index at this point.
          while (index < _mappedSchematic.GetLength(0) && _mappedSchematic[point.Row, index].Digit.HasValue)
          {
            sb.Append(_mappedSchematic[point.Row, index].Digit.Value);
            index++;
          }

          var rawResult = sb.ToString();
          if (!string.IsNullOrEmpty(rawResult))
          {
            results.Add(int.Parse(sb.ToString()));
          }
        }
      }

      return results;
    }
  }
}
