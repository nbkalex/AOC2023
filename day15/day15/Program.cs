var hashes = File.ReadAllText("in.txt").Split(",");
var boxes = hashes.Select(l => (l, l.Aggregate(0, (s, c) => (s + c) * 17 % 256)));

var focals = new List<(string, int)>[256];
for (int i = 0; i < 256; i++)
  focals[i] = new List<(string, int)>();

foreach(var box in boxes)
{
  string[] minusTokens = box.Item1.Split("-");
  string[] equalsTokens = box.Item1.Split("=");

  string label = minusTokens.Length > 1 ? minusTokens[0] : equalsTokens[0];

  var currentBox = focals[label.Aggregate(0, (s, c) => (s + c) * 17 % 256)];

  if (minusTokens.Length > 1)
    currentBox.RemoveAll(b => b.Item1 == minusTokens[0]);
  if(equalsTokens.Length > 1)
  {
    int focal = int.Parse(equalsTokens[1]);
    
    int indexToChange = currentBox.IndexOf(currentBox.FirstOrDefault(b => b.Item1 == label));
    if(indexToChange >= 0)
      currentBox[indexToChange] = (label, focal);
    else
      currentBox.Add((label, focal));
  }
}

int sum = 0;
for(int i = 0; i < 256; i++)
  if (focals[i].Count > 0)
    sum+= focals[i].Select((b,i2) => (i+1) * (i2+1) * b.Item2).Sum();

Console.WriteLine(boxes.Sum(b => b.Item2));

Console.WriteLine(sum);
