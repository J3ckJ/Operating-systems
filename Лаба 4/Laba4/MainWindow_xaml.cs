using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Scheduler
{
  public partial class MainWindow : Window
  {
    ManualResetEvent run = new ManualResetEvent(false);
    ProcessProcessor[] processGenerators;
    ProcessQueue[] processQueues { get; set; } = new ProcessQueue[32];
    int totalProcesses;
    int numThreadsDisplay;
    int refreshRate;
    Thread[] ThreadsExec;
    Thread PriorityWait;
    Thread ThreadInterface;
    GenerateFile generateFile = new GenerateFile();

    public MainWindow()
    {
      InitiateQueue();
      InitializeComponent();
    }
    private void UpdateInterface()
    {
      while (!ProcessProcessor.EmptyProcessQueue(processQueues))
      {
        try
        {
          Dispatcher.Invoke(
          new Action(() =>
          {
            numThreadsDisplay = ComboBoxDisplay.SelectedIndex;
          }));
          UpdateValues(numThreadsDisplay);
          Thread.Sleep(refreshRate);
        }
        catch (TaskCanceledException)
        {
          return;
        }
      }

      Dispatcher.Invoke(
          new Action(() =>
          {
            DataGridGenerator.Items.Clear();
            MessageBox.Show("All processes have been completed", CompletedProcessTotal().ToString(), MessageBoxButton.OK, MessageBoxImage.Information);
            UpdateButtons(true);

            LabelName.Content = "";
            LabelPID.Content = "";
            LabelPriority.Content = "";
            LabelExecTime.Content = "";

            InterruptThreads();
          }));
    }
    private void InitiateQueue()
    {
      for (int i = 0; i < processQueues.Length; i++)
        processQueues[i] = new ProcessQueue();
    }
    private void Btn_Simulate_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        int q = int.Parse(TxtQuant.Text);
        int threads = (Convert.ToInt32(ComboBoxDisplay.SelectedIndex + 1));
        refreshRate = int.Parse(TxtTimeInterface.Text);
        int timeCheck = int.Parse(TxtTimeCheck.Text);
        int waitTime = int.Parse(TxtWaitingTime.Text);
        totalProcesses = 0;
        if (q <= 0)
        {
          MessageBox.Show("Invalid quant value. The quant must be greater than 0. ");
        }

        else if (refreshRate <= 0)
        {
          MessageBox.Show("Invalid interface update time. The time must be greater than 0. ");
        }

        else if (timeCheck <= 0)
        {
          MessageBox.Show("Invalid priority check time. The time must be greater than 0. ");
        }

        else if (threads <= 0)
        {
          MessageBox.Show("Specify a number of threads. ");
        }
        else
        {
          processGenerators = new ProcessProcessor[threads];
          ThreadsExec = new Thread[threads];
          generateFile.FillQueue(processQueues);
          foreach (ProcessQueue queue in processQueues)
            totalProcesses += queue.ProcessCounter;
          for (int ax = 0; ax < threads; ax++)
          {
            processGenerators[ax] = new ProcessProcessor(q, timeCheck);
          }

          int a = 0;
          foreach (ProcessProcessor g in processGenerators)
          {
            ThreadsExec[a] = new Thread(() => g.ExecProcess(processQueues));
            ThreadsExec[a].Start();
            a++;
          }

          PriorityWait = new Thread(() => ProcessProcessor.ControlPriority(processQueues, waitTime));
          PriorityWait.Start();

          ThreadInterface = new Thread(UpdateInterface);
          for (int ab = 0; ab < threads; ab++)
            ComboBoxDisplay.Items.Add(ab + 1);

          ComboBoxDisplay.SelectedIndex = 0;
          ThreadInterface.Start();
          UpdateButtons(false);
        }
      }

      catch (FormatException ex)
      {
        MessageBox.Show(ex.Message);
      }
    }
    private void UpdateButtons(bool value)
    {
      Btn_Simulate.IsEnabled = value;
      TxtQuant.IsEnabled = value;
      TxtTimeInterface.IsEnabled = value;
      TxtTimeCheck.IsEnabled = value;
      ComboBoxThreadsNumber.IsEnabled = value;
      TxtWaitingTime.IsEnabled = value;
      ComboBoxDisplay.IsEnabled = value;
      Btn_Interrupt.IsEnabled = !value;
    }

    private int CompletedProcessTotal()
    {
      int total = 0;
      foreach (ProcessProcessor g in processGenerators)
        total += g.ProcessesFinished;

      return total;
    }
    private void UpdateValues(int index)
    {
      Dispatcher.Invoke(
          new Action(() =>
          {
            DataGridGenerator.Items.Clear();
            for (int i = (processQueues.Length - 1); i >= 0; i--)
            {
              Monitor.Enter(processQueues);
              for (int u = 0; u < processQueues[i].ProcessCounter; u++)
              {
                DataGridGenerator.Items.Add(processQueues[i].SearchProcess(u));
              }
              Monitor.Exit(processQueues);

              Process process = processGenerators[index].Execution;

              LabelName.Content = process.Name;
              LabelPID.Content = process.PID;
              LabelPriority.Content = process.Priority;
              LabelExecTime.Content = process.ExecTime.ElapsedMilliseconds + "ms";
              CycleLabel.Content = process.NumberOfCycles;
            }
          }));

    }




    private void InterruptThreads()
    {
      if (ThreadsExec != null)
      {
        foreach (Thread t in ThreadsExec)
        {
          try
          {
            if (t.ThreadState == System.Threading.ThreadState.WaitSleepJoin)
            {
              run.Set(); 
            }
            else
            {
              run.Reset();
            }

          }

          catch (ThreadStateException ex)
          {
            MessageBox.Show(ex.Message);
          }
        }
      }

      if (ThreadInterface != null)
      {
        try
        {
          run.Reset();
        }

        catch (ThreadStateException ex)
        {
          MessageBox.Show(ex.Message);
        }
      }

      if (PriorityWait != null)
      {
        try
        {
          run.Reset();
        }

        catch (ThreadStateException ex)
        {
          MessageBox.Show(ex.Message);
        }
      }
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
      InterruptThreads();
    }

    private void Btn_Interrupt_Click(object sender, RoutedEventArgs e)
    {
      InterruptThreads();
      DataGridGenerator.Items.Clear();
      UpdateButtons(true);
      InitiateQueue();
      LabelName.Content = "";
      LabelPID.Content = "";
      LabelPriority.Content = "";
      LabelExecTime.Content = "";
    }
  }
}
