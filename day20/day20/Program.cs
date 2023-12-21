using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;

var lines = File.ReadAllLines("in.txt");

BroadcasterModule broadcaster = null;

var modules = lines.Select(l => l.Split(" -> ")).Select((t, Index) =>
{
  BaseModule module = null;
  string name = t[0];
  var connections = t[1].Split(", ").ToList();

  if (name == "broadcaster")
  {
    broadcaster = new BroadcasterModule(name, connections);
    module = broadcaster;
  }
  else
  {
    char type = name[0];
    name = name.Substring(1);
    module = type == '%' ? new FlipFlopModule(name, connections) : new ConjunctionModule(name, connections);
  }

  return (name, module);
}).ToDictionary(k => k.name, v => v.module);

foreach (var module in modules)
{
  if (module.Value is FlipFlopModule)
    Console.Write("%" + module.Key + " ");
  if (module.Value is ConjunctionModule)
    Console.Write("&" + module.Key + " ");
}

Console.WriteLine();

foreach (var module in modules.Values)
  module.Initialize(modules);

const int TIMES = 1000;
List<(BaseModule, (BaseModule, bool))> toVisit = new List<(BaseModule, (BaseModule, bool))>();
for (int i = 0; i < TIMES; i++)
{
  toVisit.Add((null, (broadcaster, false)));
  broadcaster.ReceiveSignal(null, false);
  while (toVisit.Any())
  {
    foreach (var module in modules)
      module.Value.Update();

    List<(BaseModule, (BaseModule, bool))> fromVisit = new List<(BaseModule, (BaseModule, bool))>();
    foreach (var module in toVisit)
    {
      if (module.Item1 != null)
      {
        module.Item1.ReceiveSignal(module.Item2.Item1, module.Item2.Item2);
        module.Item1.Update();
        fromVisit.AddRange(module.Item1.SendSignal());
      }
      else
        fromVisit.AddRange(module.Item2.Item1.SendSignal());
      
    }

    toVisit = fromVisit;
  }
}

Console.WriteLine((BaseModule.LowPulseCount + TIMES) * BaseModule.HighPulseCount);

// Class definitions   ---------------------------------------------

class BaseModule
{
  public static long LowPulseCount = 0;
  public static long HighPulseCount = 0;

  protected bool mSignal = false;
  protected bool mTempSignal = false;
  public string mName;

  public List<string> ConnectionsIds { get; private set; }
  public List<BaseModule> Connections { get; private set; } = new List<BaseModule>();

  public BaseModule(string name, List<string> modules)
  {
    ConnectionsIds = modules;
    mName = name;
  }

  public virtual void Update()
  {
    mSignal = mTempSignal;
  }

  public virtual void Initialize(Dictionary<string, BaseModule> modules)
  {
    foreach (var module in ConnectionsIds)
      Connections.Add(modules.ContainsKey(module) ? modules[module] : new BaseModule(module, new List<string>()));

  }

  public virtual void ReceiveSignal(BaseModule module, bool signal)
  {
    mTempSignal = signal;
  }

  protected virtual bool GetOutSignal()
  {
    return mSignal;
  }

  protected virtual bool CanSend()
  {
    return true;
  }

  public virtual List<(BaseModule, (BaseModule, bool))> SendSignal()
  {

    if (!CanSend())
      return new List<(BaseModule, (BaseModule, bool))>();

    List<(BaseModule, (BaseModule, bool))> sent = new List<(BaseModule, (BaseModule, bool))>();
    foreach (var module in Connections)
    {

      if (GetOutSignal())
        HighPulseCount++;
      else
        LowPulseCount++;

      string signalStr = GetOutSignal() ? "high" : "low";
      if (module != null)
        Console.WriteLine($"{mName} -{signalStr} -> {module.mName}");

      sent.Add((module, (this, GetOutSignal())));
    }

    return sent;
  }
}


class BroadcasterModule : BaseModule
{
  public BroadcasterModule(string name, List<string> modules)
    : base(name, modules)
  {
  }
}

class FlipFlopModule : BaseModule
{
  bool mCanSend = false;
  bool mTempCanSend = false;
  bool mState = false;
  bool mTempState = false;

  public FlipFlopModule(string name, List<string> modules)
    : base(name, modules)
  {
    mSignal = true;
  }

  public override void ReceiveSignal(BaseModule module, bool signal)
  {
    if (signal)
    {
      mTempCanSend = false;
      return;
    }

    mTempCanSend = true;
    mTempState = !mState;
    mTempSignal = mTempState;
  }

  public override void Update()
  {
    base.Update();
    mCanSend = mTempCanSend;
    mState = mTempState;
  }

  protected override bool CanSend()
  {
    return mCanSend;
  }
}

class ConjunctionModule : BaseModule
{
  Dictionary<BaseModule, bool> mSignals;
  Dictionary<BaseModule, bool> mTempSignals;
  public ConjunctionModule(string name, List<string> modules)
    : base(name, modules)
  {
  }

  public override void Initialize(Dictionary<string, BaseModule> modules)
  {
    base.Initialize(modules);
    mTempSignals = modules.Where(m => m.Value.ConnectionsIds.Contains(mName)).ToDictionary(k => k.Value, v => false);
    //mTempSignals = new Dictionary<BaseModule, bool>();
  }

  public override void ReceiveSignal(BaseModule module, bool signal)
  {

    mTempSignals[module] = signal;
  }

  public override void Update()
  {
    base.Update();
    mSignals = mTempSignals;
    mSignal = mSignals.Values.All(s => s) ? false : true;
  }
}


// 941692864 too high
// 931225554