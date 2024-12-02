using System;
using System.Collections.Generic;
using System.Linq;

namespace ReactorReporter.Core;

public class ReactorReportEvaluator
{
  private readonly IEnumerable<IEnumerable<int>> _reports;
  public ReactorReportEvaluator(IEnumerable<string> reports) {
    _reports = reports.Select(r => r.Split(' ').Select(s => int.Parse(s)).ToList());
  }

  public int CalculateSafeReports(int minValidDifference, int maxValidDifference){
    return CalculateSafeReports(minValidDifference, maxValidDifference, 0);
  }

  public int CalculateSafeReports(int minValidDifference, int maxValidDifference, int outOfSpecTolerance){
    int safeCount = 0;
    foreach(var report in _reports) {
      bool? isAscending = null;
      int previousValue = report.First();
      bool isSafe = true;
      int outOfSpec = 0;
      foreach(var level in report.Skip(1)){

        if(!isAscending.HasValue) {
          isAscending = level > previousValue;
        }

        if(isAscending.Value) {
          if(level - previousValue < minValidDifference || level - previousValue > maxValidDifference) {
            outOfSpec++;
            if(outOfSpec > outOfSpecTolerance) {
              isSafe = false;
              break;
            }
          } else {
            previousValue = level;
          }
        } else {
          if(previousValue - level < minValidDifference || previousValue - level > maxValidDifference) {
            outOfSpec++;
            if(outOfSpec > outOfSpecTolerance) {
              isSafe = false;
              break;
            }
          } else {
            previousValue = level;
          }
        }
      }

      if(isSafe) {
        safeCount++;
      }
    }

    return safeCount;
  }
}
