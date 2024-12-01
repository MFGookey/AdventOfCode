using System;
using System.Collections.Generic;
using System.Linq;

namespace ListoMathic.Core;

public class ListCalculator
{
  private readonly IList<int> _leftList;
  private readonly IList<int> _rightList;

  public ListCalculator(IEnumerable<string> records) {
    var leftList = new List<int>();
    var rightList = new List<int>();

    foreach(var record in records){
      var subRecords = record.Split("   ");
      leftList.Add(int.Parse(subRecords[0]));
      rightList.Add(int.Parse(subRecords[1]));
    }

    _leftList = leftList.Order().ToList();
    _rightList = rightList.Order().ToList();
  }

  public int CalculateSumOfOrderedDifferences(){
    int runningSum = 0;
    for(var index = 0;index < _leftList.Count; index++){
      runningSum += Math.Abs(_leftList[index] - _rightList[index]);
    }

    return runningSum;
  }
}
