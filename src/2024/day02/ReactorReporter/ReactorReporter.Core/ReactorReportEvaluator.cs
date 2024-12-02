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
      if(CalculateSafeReport(report, minValidDifference, maxValidDifference, outOfSpecTolerance)) {
        safeCount++;
      }
    }

    return safeCount;
  }

  private bool CalculateSafeReport(IEnumerable<int> report, int minValidDifference, int maxValidDifference, int outOfSpecTolerance) {
    if(outOfSpecTolerance < 0) {
      return false;
    }

    if(outOfSpecTolerance == 0) {
      return SafeReportEvaluator(report, minValidDifference, maxValidDifference);
    }

    bool foundSafe = false;

    for(int index = 0; index < report.Count(); index++) {
      if(CalculateSafeReport(report.Where((v, i) => i != index), minValidDifference, maxValidDifference, outOfSpecTolerance - 1)) {
        foundSafe = true;
        break;
      }
    }
    
    return foundSafe;
  }

  private bool SafeReportEvaluator(IEnumerable<int> report, int minValidDifference, int maxValidDifference){
    bool? isAscending = null;
    int previousValue = report.First();
    bool isSafe = true;

    foreach(var level in report.Skip(1)){

      if(!isAscending.HasValue) {
        isAscending = level > previousValue;
      }

      if(isAscending.Value) {
        if(level - previousValue < minValidDifference || level - previousValue > maxValidDifference) {
          isSafe = false;
          break;
        }
      } else {
        if(previousValue - level < minValidDifference || previousValue - level > maxValidDifference) {
          isSafe = false;
          break;
        }
      }

      previousValue = level;
    }

    return isSafe;
  }
}
