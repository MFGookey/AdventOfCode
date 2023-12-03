using System;
using System.Collections.Generic;
using System.Text;
using Common.Utilities.TwoD;

namespace PartNosGet.Core
{
  public class DataPoint : Point
  {
    public char Datum
    {
      get; private set;
    }

    public bool IsSpecial
    {
      get; private set;
    }

    public int? Digit
    {
      get; private set;
    }

    public IReadOnlyList<Point> Neighbors
    {
      get; private set;
    }

    public DataPoint(int row, int column, char datum, IReadOnlyList<Point> neighbors) : base(row, column)
    {
      Datum = datum;
      Neighbors = neighbors;

      int parsedValue;
      bool isInt = int.TryParse(datum.ToString(), out parsedValue);
      this.IsSpecial = Datum != '.' && !isInt;

      if (isInt)
      {
        this.Digit = parsedValue;
      }
    }

    public override string ToString()
    {
      return $"[{this.Row}, {this.Column}]: {Datum}";
    }
  }
}
