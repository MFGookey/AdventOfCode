using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AgriMapping.Core
{
  public class Range
  {
    private long _minimumInput;
    private long _maximumInput;
    private long _minimumOutput;

    public Range(long outputMin, long inputMin, long rangeLength)
    {
      _minimumInput = inputMin;
      _maximumInput = inputMin + rangeLength - 1;
      _minimumOutput = outputMin;
    }

    public bool IsInRange(long sample)
    {
      return sample >= _minimumInput && sample <= _maximumInput;
    }

    public long GetOutput(long sample)
    {
      return _minimumOutput + (sample - _minimumInput);
    }
  }
}
