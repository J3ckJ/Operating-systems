using System;
using System.Threading;

namespace Scheduler
{
  class ProcessProcessor
  {
    public int Quant { get; set; }
    public Process Execution { get; set; }
    public int ProcessesFinished { get; set; }
    public int ExecTime { get; set; }
    public ProcessProcessor(int quant, int ExecTime)
    {
      this.Quant = quant;
      this.ExecTime = ExecTime;
    }
    public static bool EmptyProcessQueue(ProcessQueue[] processQueue)
    {
      for (int i = 0; i < processQueue.Length; i++)
      {
        if (!processQueue[i].EmptyQueueCheck())
        {
          return false;
        }
      }

      return true;
    }

    public void ExecProcess(ProcessQueue[] processQueue)
    {
      int priority = 0;
      int mt = 0;
      do
      {
        for (int i = (processQueue.Length - 1); i >= 0; i--)
        {
          if (!processQueue[i].EmptyQueueCheck())
          {
            priority = i;
            i = -1;
          }
          else
          {
            mt++;
          }
        }
        if (mt == processQueue.Length)
        {
          priority = -1;
        }
        if (priority != -1)
        {
          Exec(processQueue, priority);
        }
      } while (priority != -1);
    }

    private void Exec(ProcessQueue[] F, int queuePriority)
    {
      if (!F[queuePriority].EmptyQueueCheck())
      {
        Execution = F[queuePriority].UnqueueProcess();
        if (Execution == null)
          return;

        double TimeReserved1 = 0.01 * Quant;
        int TimeReserved2 = Convert.ToInt32(TimeReserved1);
        int timeUse = 0;

        Execution.StartCycle();
        if (Execution.ET > 0)
        {
          if (Execution.ET > ExecTime)
          {
            Thread.Sleep(ExecTime);
            Execution.ET -= ExecTime;
            TimeReserved2 = 0;
          }
          else
          {
            Thread.Sleep(Execution.ET);
            TimeReserved2 -= Execution.ET;
            timeUse = Execution.ET;
            Execution.NumberOfCycles--;
            Execution.ET = 0;
          }
        }
        if (TimeReserved2 >= ExecTime)
        {
          Thread.Sleep(ExecTime);
          queuePriority = Execution.ReducePriority() - 1;
          Execution.ET = TimeReserved2 - ExecTime;
        }
        else
        {
          int timeRemain = TimeReserved2 - timeUse;

          if (timeRemain > 0)
          {
            Thread.Sleep(timeRemain);
            Execution.ET = TimeReserved2;
          }
        }
        Execution.CycleComplete();
        if (Execution.NumberOfCycles > 0)
        {
          F[queuePriority].PutInQueue(Execution);
        }

        else
        {
          ProcessesFinished++;
        }
      }
    }
    public static void ControlPriority(ProcessQueue[] processQueue, int MaximumWaitTime)
    {
      while (true)
      {
        Thread.Sleep(MaximumWaitTime);
        ProcessQueue[] pQ = new ProcessQueue[32];
        for (int p = 0; p < pQ.Length; p++)
          pQ[p] = new ProcessQueue();

        Monitor.Enter(processQueue);
        for (int x = 0; x < processQueue.Length - 1; x++)
        {
          int nOfUses = processQueue[x].ProcessCounter;
          int pQprior = x;

          for (int u = 0; u < nOfUses; u++)
          {
            Process processAnalysis = processQueue[pQprior].UnqueueProcess();
            if (processAnalysis.WaitTime.ElapsedMilliseconds > MaximumWaitTime)
            {
              pQprior = processAnalysis.IncreasePriority() - 1;
              pQ[pQprior - 1].PutInQueue(processAnalysis);
            }

            else
            {
              processQueue[pQprior].PutInQueue(processAnalysis);
            }
            pQprior = x;
          }
        }

        for (int i = 1; i < processQueue.Length; i++)
        {
          while (!pQ[i - 1].EmptyQueueCheck())
            processQueue[i].PutInQueue(pQ[i - 1].UnqueueProcess());
        }

        Monitor.Exit(processQueue);

      }
    }
  }
}
