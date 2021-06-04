using System;
using System.IO;
using System.Windows;

namespace Scheduler
{
  class GenerateFile
  {
    StreamReader reader;
    string fileName = "asdad.txt";
    public GenerateFile()
    {
      if (File.Exists(fileName))
      {
        File.Delete(fileName);
        using (StreamWriter streamWriter = new StreamWriter(fileName, true, System.Text.Encoding.Default))
        {
          streamWriter.WriteLine("1;p1;1;100");
          streamWriter.WriteLine("2;p2;8;100");
          streamWriter.WriteLine("3;p3;15;100");
          streamWriter.Close();
        }
      }
      else
      {
        using (StreamWriter streamWriter = new StreamWriter(fileName, true, System.Text.Encoding.Default))
        {
          streamWriter.WriteLine("1;p1;1;100");
          streamWriter.WriteLine("2;p2;8;100");
          streamWriter.WriteLine("3;p3;15;100");
          streamWriter.Close();
        }
      }
      reader = new StreamReader(fileName);
    }
    private Process CreateNewProcess()
    {
      string line = reader.ReadLine();

      string[] cell = line.Split(';', ',');

      int PID = 0, priority = 0, numberOfCycles = 0;
      string name = "EMPTY";


      PID = int.Parse(cell[0]);
      name = cell[1];
      priority = int.Parse(cell[2]);
      numberOfCycles = int.Parse(cell[3]);

      Process p = new Process(PID, name, priority, numberOfCycles);

      return p;
    }

    public void FillQueue(ProcessQueue[] f)
    {
      if (File.Exists(fileName))
      {
        reader = new StreamReader(fileName);
        while (!reader.EndOfStream)
        {
          Process process = CreateNewProcess();
          int priority = process.Priority;

          try
          {
            f[priority].PutInQueue(process);
          }

          catch (IndexOutOfRangeException)
          {
            MessageBox.Show("OutOfRangeExeption");
          }
        }
        reader.Close();
      }
    }
  }
}
