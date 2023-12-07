using System.Collections.Generic;
using System.Security.Cryptography;

const string cards = "J123456789TQKA";

var lines = File.ReadAllLines("in.txt");
var hands = lines.Select(l => (l.Split(" ")[0], int.Parse(l.Split(" ")[1]))).ToArray();
List<(List<(int, int)>, int, string)> processedHands = new List<(List<(int, int)>, int, string)>();
foreach (var hand in hands)
{
  Dictionary<int, int> keyValuePairs = new Dictionary<int, int>();
  foreach(var c in hand.Item1)
  {
    int index = cards.IndexOf(c);
    if (keyValuePairs.ContainsKey(index))
      keyValuePairs[index]++;
    else
      keyValuePairs.Add(index, 1);
  }

  int jIndex = cards.IndexOf('J');
  int jCount = keyValuePairs.ContainsKey(jIndex) ? keyValuePairs[jIndex] : 0;
  if(jCount < 5)
    keyValuePairs.Remove(jIndex);

  var cardsCol = keyValuePairs.Select(kvp => (kvp.Value, kvp.Key)).ToList();
  cardsCol.Sort(new CardComparer());
  if (jCount < 5)
    cardsCol[0] = (cardsCol[0].Value + jCount, cardsCol[0].Key);

  processedHands.Add((cardsCol.ToList(), hand.Item2, hand.Item1));
}

var dublicates = processedHands.FirstOrDefault(ph => processedHands.Any(ph2 => ph != ph2 && ph2.Item3 == ph.Item3));

processedHands.Sort(new MyComparer());

foreach(var hand in processedHands)
  Console.Write(hand.Item3 + " ");

Console.WriteLine();
Console.WriteLine(processedHands.Sum(h => (long)h.Item2 * (processedHands.IndexOf(h) + 1)));

class CardComparer : IComparer<(int, int)>
{
  public int Compare((int, int)c1, (int, int)c2)
  {
    int ret = c2.Item1.CompareTo(c1.Item1);
    if(ret != 0)
      return ret;

    return c2.Item2.CompareTo(c1.Item2);
  }
}

class MyComparer : IComparer<(List<(int,int)>,int, string)>
{
  const string cards = "J123456789TJQKA";

  public int Compare((List<(int, int)>, int, string) h1, (List<(int, int)>, int, string) h2)
  {
    int cmp = h2.Item1.Count.CompareTo(h1.Item1.Count);
    if(cmp != 0) 
      return cmp;


    for (int i = 0; i < h1.Item1.Count; i++)
    {
      cmp = h1.Item1[i].Item1.CompareTo(h2.Item1[i].Item1);
      if(cmp != 0) 
        return cmp;
    }

    for (int i = 0; i < h1.Item3.Length; i++)
    {
      cmp = cards.IndexOf(h1.Item3[i]).CompareTo(cards.IndexOf(h2.Item3[i]));
      if (cmp != 0)
        return cmp;
    }

    return 0;
  }
}