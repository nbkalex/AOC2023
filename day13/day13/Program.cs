var input = File.ReadAllText("in.txt");

var patterns = input.Split("\r\n\r\n").Select(p => p.Split("\r\n")).ToList();

int vcount = 0;
int hcount = 0;
foreach (var pattern in patterns)
{
  // horizontal line
  PrintPattern(pattern);
  int hIndex = GetReflectionPos(pattern);
  
  Console.WriteLine();
  Console.WriteLine();

  var patternV = Rotate(pattern);
  PrintPattern(patternV);
  int vIndex = GetReflectionPos(patternV);
  
  Console.WriteLine();
  Console.WriteLine();

  Console.WriteLine(" horizontal index: " + hIndex);
  Console.WriteLine(" vertical index: " + vIndex);

  int newhIndex = hIndex;
  int newvIndex = vIndex;

  // part2
  int tryIndex = 0;
  while (true)
  {
    var patternc = pattern.ToArray();
    int rowIndex = tryIndex / pattern[0].Length;
    int colIndex = tryIndex % pattern[0].Length;

    char currentVal = patternc[rowIndex][colIndex];
    currentVal = currentVal == '#' ? '.' : '#';

    patternc[rowIndex] = patternc[rowIndex].Remove(colIndex, 1).Insert(colIndex, currentVal.ToString());

    newhIndex = GetReflectionPos(patternc, hIndex - 1);

    var patternc2 = Rotate(patternc);
    newvIndex = GetReflectionPos(patternc2, vIndex - 1);

    tryIndex++;

    if ((newhIndex != 0 && newhIndex != hIndex) || (newvIndex != 0 && newvIndex != vIndex))
    {
      hIndex = hIndex == newhIndex ? 0 : newhIndex;
      vIndex = vIndex == newvIndex ? 0 : newvIndex;

      break;
    }
  }


  hcount += hIndex;
  vcount += vIndex;

  Console.WriteLine("_____________________________________________________");
}

Console.WriteLine();
Console.WriteLine(hcount * 100 + vcount);

// 5829 too low
// 35481 too low

string[] Rotate(string[] pattern)
{
  List<string> lines = new List<string>();
  for (int i = 0; i < pattern[0].Length; i++)
    lines.Add(string.Join("", pattern.Select(l => l[i])));

  return lines.ToArray();
}

int GetReflectionPos(string[] pattern, int skip = -1)
{
  int hIndex = -1;
  for (int i = 0; i < pattern.Length - 1; i++)
  {
    if (pattern[i].SequenceEqual(pattern[i + 1]))
    {
      int left = i - 1;
      int right = i + 2;

      bool reflets = true;
      while (left >= 0 && right < pattern.Length)
      {
        if (!pattern[left].SequenceEqual(pattern[right]))
        { 
          reflets = false;
          break;
        }

        left--;
        right++;
      }

      if (reflets && i != skip)
      {
        hIndex = i;
        break;
      }
    }
  }

  return hIndex + 1;
}

void PrintPattern(string[] pattern)
{
  for (int i = 0; i < pattern.Length; i++)
  {
    for (int j = 0; j < pattern[i].Length; j++)
      Console.Write(pattern[i][j]);
    Console.WriteLine();
  }
}
