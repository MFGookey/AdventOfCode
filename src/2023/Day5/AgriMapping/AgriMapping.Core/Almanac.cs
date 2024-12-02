using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace AgriMapping.Core
{
  public class Almanac
  {
    private IEnumerable<Seed> _seeds;
    private IDictionary<string, IEnumerable<Range>> _mappings;
    private IEnumerable<Range> _seedRanges;

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
      _seedRanges = this.SetupSeedRanges(regions[0]);
      
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

    private IEnumerable<Range> SetupSeedRanges(string seedData)
    {
      var tokens = seedData.Replace("seeds: ", string.Empty).Split(' ').Select(s => long.Parse(s)).ToList();
      var ranges = new List<Range>();
      for (var i = 0; i < tokens.Count; i += 2)
      {
        ranges.Add(new Range(0, tokens[i], tokens[i + 1]));
      }

      return ranges;
    }

    /*private IEnumerable<Seed> SetupSeedsByRange(string seedData)
    {
      var seedNumbers = seedData.Replace("seeds: ", string.Empty).Split(' ').Select(s => long.Parse(s)).ToList();
      var seeds = new List<Seed>();
      for (var i = 0; i < seedNumbers.Count; i += 2)
      {
        seeds.AddRange(
          this
            .CreateRange(seedNumbers[i], seedNumbers[i + 1])
            .Select(n => new Seed(n))
        );
      }

      return seeds;
    }*/

    private IEnumerable<long> CreateRange(long start, long count)
    {
      var limit = start + count;
      while (start < limit)
      {
        yield return start;
        start++;
      }
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
      foreach(var seed in _seeds)
      {
        MapSeed(seed);
      }
    }

    private void MapSeed(Seed toMap)
    {
      string previousMap = "seed";
      for (var i = 0; i < _mappings.Count; i++)
      {
        var key = _mappings.Keys.Where(k => k.StartsWith($"{previousMap}-to-")).First();
        var ranges = _mappings[key];
        long newValue;
        if (ranges.Any(r => r.IsInRange(toMap.GetMapping(previousMap))))
        {
          newValue = ranges.Where(r => r.IsInRange(toMap.GetMapping(previousMap))).First().GetOutput(toMap.GetMapping(previousMap));
        }
        else
        {
          newValue = toMap.GetMapping(previousMap);
        }

        previousMap = key.Replace($"{previousMap}-to-", string.Empty);
        toMap.AddMapping(previousMap, newValue);
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

    public long GetMinimumLocationByRange()
    {
      // invert all of the ranges?
      // for each seed range
      // find-or-create the top level range such that all seed ranges are covered
      
    }

    private IEnumerable<Range> FillDomainWithRanges(IEnumerable<Range> outerRanges, IEnumerable<Range> innerRanges)
    {
      // for the entire set of outer ranges, ensures that there is an inner range for every possible domain from the outer ranges
      var newInnerRanges = new List<Range>();
      foreach (var outer in outerRanges)
      {
        // is there an inner range that straddles the lower domain of our current outer range?
        var overlappingInnerRanges = innerRanges.Where(inner => inner.MinimumInput <= outer.MaximumOutput && inner.MaximumInput >= outer.MinimumOutput).ToList();
        
        if (overlappingInnerRanges.Any())
        {
          if (!overlappingInnerRanges.Any(inner => inner.MinimumInput <= outer.MinimumOutput && inner.MaximumInput >= outer.MinimumOutput))
          {
            var smallestInner = overlappingInnerRanges.OrderBy(i => i.MinimumInput).First();
            overlappingInnerRanges.Add(new Range(outer.MinimumOutput, smallestInner.MinimumInput - 1, smallestInner.MinimumInput - outer.MinimumOutput)); // possible off by 1?
          }
          foreach (var inner in overlappingInnerRanges.OrderBy(inner => inner.MinimumInput))
          {
            // for this current inner range, does it extend all the way to outerRange.MaximumOutput?
            // if yes, then we're done
            // if not - check if there's another inner range starting at inner.MaximumInput+1
            // if yes, then great let's continue
            // if not, then make a new trivial range mapping from inner.MaximumInput+1 to nextInner.MinimumInput-1 or outer.MaximumOutput if nextInner DNE
            if (inner.MaximumInput >= outer.MaximumOutput)
            {
              break;
            }
            
            if (overlappingInnerRanges.Any(i => i.MinimumInput == inner.MaximumInput + 1))
            {
              continue;
            }
            Range nextInner;
            if (overlappingInnerRanges.Any(i => i.MinimumInput > inner.MaximumInput))
            {
              nextInner = overlappingInnerRanges.Where(i => i.MinimumInput > inner.MaximumInput).OrderBy(i => i.MinimumInput).First();
              overlappingInnerRanges.Add(new Range(inner.MaximumInput + 1, inner.MaximumInput + 1, nextInner.MinimumInput - inner.MaximumInput - 1));
            }
            else
            {
              overlappingInnerRanges.Add(new Range(inner.MaximumInput + 1, inner.MaximumInput + 1, outer.MaximumOutput - inner.MaximumInput - 1));
            } 
            
            
          }
        }
        else {
          overlappingInnerRanges.Add(new Range(outer.MinimumOutput, outer.MaximumOutput, outer.MaximumOutput - outer.MinimumOutput + 1));
        }

        // spool overlappingInnerRanges into newInnerRanges
        newInnerRanges.AddRange(overlappingInnerRanges.Where(candidate => !newInnerRanges.Any(newInner => newInner.MinimumInput == candidate.MinimumInput && newInner.MaximumInput == candidate.MaximumInput)));
      }
    }
  }
}
