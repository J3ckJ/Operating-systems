namespace Scheduler
{
  class ProcessUnit
  {
    public Process Process { get; set; }
    public ProcessUnit Next { get; set; }
    public ProcessUnit(Process processs)
    {
      Process = processs;
    }

    public ProcessUnit()
    {

    }
  }
}
