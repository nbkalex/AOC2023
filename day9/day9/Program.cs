var report = File.ReadAllLines("in.txt").Select(l => l.Split(" ").Select(n => int.Parse(n)).ToList()).ToList();

var added = report.Select(history =>
{
  List<List<int>> processedHistory = new List<List<int>>(){ history };

  while (processedHistory.Last().Any(c => c != 0))
    processedHistory.Add(processedHistory.Last().Skip(1).Zip(processedHistory.Last())
                                                .Select(c => c.First - c.Second).ToList());
  processedHistory.Last().Insert(0, 0);
  processedHistory.Reverse();
  foreach(var ph in processedHistory.Skip(1).Zip(processedHistory))
    ph.First.Insert(0, ph.First.First() - ph.Second.First());

  return processedHistory.Last().First();
});

Console.WriteLine(added.Sum());
