using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;

namespace AgriMapping.Core
{
  public class Almanac
  {
    private IEnumerable<Seed> _seeds;
    private IDictionary<string, IEnumerable<Range>> _mappings;

    public Almanac(string data)
    {
      // Data is of the form:
      /*
        seeds: 79 14 55 13

        seed-to-soil map:
        50 98 2
        52 50 48

        soil-to-fertilizer map:
        0 15 37
        37 52 2
        39 0 15

        fertilizer-to-water map:
        49 53 8
        0 11 42
        42 0 7
        57 7 4

        water-to-light map:
        88 18 7
        18 25 70

        light-to-temperature map:
        45 77 23
        81 45 19
        68 64 13

        temperature-to-humidity map:
        0 69 1
        1 0 69

        humidity-to-location map:
        60 56 37
        56 93 4
      */
      _mappings = new Dictionary<string, IEnumerable<Range>>();
      var regions = Regex.Split(data.Replace("\r\n", "\n"), "\n\n");
      _seeds = this.SetupSeeds(regions[0]);
      for (var i = 1; i < regions.Length; i++)
      {
        _mappings.Add(this.ParseMap(regions[i]));
      }

      this.MapSeeds();
    }

    private IEnumerable<Seed> SetupSeeds(string seedData)
    {
      // seedData is of the form
      // seeds: 79 14 55 13
      return seedData.Replace("seeds: ", string.Empty).Split(' ').Select(s => new Seed(long.Parse(s))).ToList();
    }

    private KeyValuePair<string, IEnumerable<Range>> ParseMap(string mapData)
    {
      // mapData is of the form
      /*
        seed-to-soil map:
        50 98 2
        52 50 48
      */

      var lines = mapData
        .Split('\n')
        .Where(l => !string.IsNullOrEmpty(l))
        .ToList();

      var ranges = lines
        .Where((line, index) => index > 0)
        .Select(line =>
        {
          var tokens = line
            .Replace("  ", " ")
            .Split(" ")
            .Where(s => !string.IsNullOrEmpty(s))
            .Select(token => long.Parse(token))
            .ToList();
          return new Range(tokens[0], tokens[1], tokens[2]);
        })
        .ToList();

      return new KeyValuePair<string, IEnumerable<Range>>(lines[0].Replace(" map:", string.Empty), ranges);
    }

    private void MapSeeds()
    {
      string previousMap;
      foreach(var seed in _seeds)
      {
        previousMap = "seed";
        for (var i = 0; i < _mappings.Count; i++)
        {
          var key = _mappings.Keys.Where(k => k.StartsWith($"{previousMap}-to-")).First();
          var ranges = _mappings[key];
          long newValue;
          if (ranges.Any(r => r.IsInRange(seed.GetMapping(previousMap))))
          {
            newValue = ranges.Where(r => r.IsInRange(seed.GetMapping(previousMap))).First().GetOutput(seed.GetMapping(previousMap));
          }
          else {
            newValue = seed.GetMapping(previousMap);
          }

          previousMap = key.Replace($"{previousMap}-to-", string.Empty);
          seed.AddMapping(previousMap, newValue);
        }
      }
    }

    public long GetMinimumLocation()
    {
      var lowest = _seeds.First().GetMapping("location");
      foreach(var seed in _seeds)
      {
        if (seed.GetMapping("location") < lowest)
        {
          lowest = seed.GetMapping("location");
        }
      }

      return lowest;
    }
  }
}
