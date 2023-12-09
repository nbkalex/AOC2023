var report = File.ReadAllLines("in.txt").Select(l => l.Split(" ").Select(n => int.Parse(n)).ToList()).ToList();

foreach(var history in report)
{
  List<List<int>> processedHistory = new List<List<int>>(){ history };
  
  while(processedHistory.Last().Any(c => c != 0))
  {
    List<int> newValues = new List<int>();
    for(int i = 0; i < processedHistory.Last().Count-1; i++)
      newValues.Add(processedHistory.Last()[i+1] - processedHistory.Last()[i]);
    processedHistory.Add(newValues);
  }

  processedHistory.Last().Insert(0,0);
  processedHistory.Reverse();
  for (int i = 1; i < processedHistory.Count; i++)
    processedHistory[i].Insert(0, processedHistory[i].First() - processedHistory[i-1].First());

  Console.WriteLine(processedHistory.Last().First());
}

Console.WriteLine(report.Sum(r => r.First()));
