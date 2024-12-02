using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AgriMapping.Core
{
  public class Range
  {
    public long MinimumInput
    {
      get;
      private set;
    }

    public long MaximumInput
    {
      get;
      private set;
    }

    public long MinimumOutput
    {
      get;
      private set;
    }

    public long MaximumOutput
    {
      get;
      private set;
    }

    public Range(long outputMin, long inputMin, long rangeLength)
    {
      this.MinimumInput = inputMin;
      this.MaximumInput = inputMin + rangeLength - 1;
      this.MinimumOutput = outputMin;
      this.MaximumOutput = outputMin + rangeLength - 1;
    }

    public bool IsInRange(long sample)
    {
      return sample >= this.MinimumInput && sample <= this.MaximumInput;
    }

    public bool IsInDomain(long sample)
    {
      return sample >= this.MinimumOutput && sample <= this.MaximumOutput;
    }

    public long GetOutput(long sample)
    {
      return this.MinimumOutput + (sample - this.MinimumInput);
    }

    public long InvertOutput(long sample)
    {
      return this.MinimumInput + (sample - this.MinimumOutput);
    }

    public Tuple<Range, Range> SplitByRange(long splitAfter)
    {
      if (!this.IsInRange(splitAfter) || !this.IsInRange(splitAfter + 1))
      {
        throw new ArgumentOutOfRangeException($"SplitByRange value {splitAfter} must be between {this.MinimumInput} and {this.MaximumInput - 1} inclusive.", nameof(splitAfter));
      }

      return new Tuple<Range, Range>(
        new Range(
          this.MinimumOutput,
          this.MinimumInput,
          (splitAfter - this.MinimumInput + 1)
        ),
        new Range(
          this.GetOutput(splitAfter + 1),
          splitAfter + 1,
          this.MaximumInput - splitAfter
        )
      );
    }

    public Tuple<Range, Range> SplitByDomain(long splitAfter)
    {
      if (!this.IsInDomain(splitAfter) || !this.IsInDomain(splitAfter + 1))
      {
        throw new ArgumentOutOfRangeException($"SplitByDomain value {splitAfter} must be between {this.MinimumOutput} and {this.MaximumOutput - 1} inclusive.", nameof(splitAfter));
      }

      return new Tuple<Range, Range>(
        new Range(
          this.MinimumOutput,
          this.MinimumInput,
          (splitAfter - this.MinimumOutput + 1)
        ),
        new Range(
          splitAfter + 1,
          this.InvertOutput(splitAfter + 1),
          this.MaximumOutput - splitAfter
        )
      );
    }

    public override string ToString()
    {
      return $"Range: ({this.MinimumInput}-{this.MaximumInput})\tDomain: ({this.MinimumOutput}-{this.MaximumOutput})";
    }
  }
}
