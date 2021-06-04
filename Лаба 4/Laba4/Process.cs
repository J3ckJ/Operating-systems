using System.Diagnostics;

namespace Scheduler
{
  public class Process
  {
    public int PID { get; set; }
    public string Name { get; set; }
    public int Priority { get; set; }
    public int NumberOfCycles { get; set; }
    public int ET { get; set; }
    public string Status { get; set; }
    public Stopwatch ExecTime { get; set; }
    public Stopwatch WaitTime { get; set; }
    public Process(int PID, string Name, int Priority, int NumberOfCycles)
    {
      this.PID = PID;
      this.Name = Name;
      this.Priority = Priority;
      this.NumberOfCycles = NumberOfCycles;
      Status = "EARLY";
      ExecTime = new Stopwatch();
      WaitTime = new Stopwatch();
    }
    public void CycleComplete()
    {
      Status = "WAITING";
      ExecTime.Reset();
      ExecTime.Stop();
      if (ET <= 0)
      {
        ET = 0;
        if (NumberOfCycles > 0)
        {
          NumberOfCycles--;
          WaitTime.Start();
        }
      }

      Status = "EARLY";
    }

    public void StartCycle()
    {
      Status = "RUNNING";
      WaitTime.Reset();
      WaitTime.Stop();
      ExecTime.Start();
    }
    public int IncreasePriority()
    {
      if (Priority < 32)
      {
        Priority++;
      }
      return Priority;
    }

    public int ReducePriority()
    {
      if (Priority > 1)
      {
        Priority--;
      }
      return Priority;
    }

  }
}
