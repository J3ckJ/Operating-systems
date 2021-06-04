using System.Threading;

namespace Scheduler
{
  class ProcessQueue
  {
    public ProcessUnit Back { get; set; }
    public ProcessUnit Forward { get; set; }
    public int ProcessCounter { get; set; }
    Mutex mutex = new Mutex();
    public ProcessQueue()
    {
      ProcessUnit watcher = new ProcessUnit();
      Back = watcher;
      Forward = watcher;
      ProcessCounter = 0;
    }
    public bool EmptyQueueCheck()
    {

      mutex.WaitOne();
      if (Back == Forward)
      {
        mutex.ReleaseMutex();
        return true;
      }

      else
      {
        mutex.ReleaseMutex();
        return false;
      }
    }
    public void PutInQueue(Process process)
    {
      mutex.WaitOne();
      ProcessUnit newProcess = new ProcessUnit(process);
      if (!newProcess.Process.WaitTime.IsRunning)
      {
        newProcess.Process.WaitTime.Start();
      }

      if (EmptyQueueCheck())
      {
        Forward.Next = newProcess;
      }

      Back.Next = newProcess;
      Back = newProcess;

      ProcessCounter++;
      mutex.ReleaseMutex();
    }
    public Process UnqueueProcess()
    {
      mutex.WaitOne();
      if (!EmptyQueueCheck())
      {
        ProcessUnit pU = Forward.Next;
        Forward.Next = pU.Next;

        pU.Next = null;

        if (Forward.Next == null)
        {
          Back = Forward;
        }

        ProcessCounter--;

        mutex.ReleaseMutex();
        return pU.Process;
      }
      else
      {
        mutex.ReleaseMutex();
        return null;
      }
    }
    public Process SearchProcess(int index)
    {
      if (!EmptyQueueCheck())
      {
        mutex.WaitOne();
        if (index >= ProcessCounter)
        {
          mutex.ReleaseMutex();
          return null;
        }

        ProcessUnit pU = Forward.Next;
        for (int cont = 0; cont < index; cont++)
        {
          pU = pU.Next;
        }

        mutex.ReleaseMutex();
        return pU.Process;
      }
      return null;
    }

  }
}
