using System;
using System.Collections.Generic;
using System.Text;

namespace AgriMapping.Core
{
  public class Seed
  {
    private IDictionary<string, long> _mappings;

    public long SeedNo
    {
      get;
      private set;
    }

    public Seed(long seedNo)
    {
      this.SeedNo = seedNo;
      _mappings = new Dictionary<string, long> { { "seed", seedNo } };
    }

    public void AddMapping(string key, long value)
    {
      _mappings.Add(key, value);
    }

    public long GetMapping(string key)
    {
      return _mappings[key];
    }
  }
}
