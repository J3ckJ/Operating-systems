using System;
using System.Collections.Generic;
using System.Threading;

namespace Laba5
{// РАСПРЕДЕЛЕНИЕ ПАМЯТИ РАЗДЕЛАМИ ПЕРЕМЕННОЙ ВЕЛИЧИНЫ
  class Program
  {
    public static int c = 0;
    public const int meme_size = 64000;
    public static Queue<int> meme = new Queue<int>();
    public static Queue<int> queue = new Queue<int>();
    public static Object locker = new Object();




    
    static void QueueInfo()
    {
      Console.WriteLine($"Задач в очереди: {queue.Count}");
    }
    static void Main(string[] args)
    {
      Char choice = ' ';
      Console.WriteLine("1. Состояние памяти");
      Console.WriteLine("2. Состояние очереди");
      Console.WriteLine("3. Добавить задачи");
      for (; ; )
      {
        choice = (char)Console.Read();
        switch (choice)
        {
          case '1':
            MemoryInfo mI = new MemoryInfo();
            mI.MemoInfo();
            break;
          case '2':
              QueueInfo();
            break;
          case '3':
              lock (locker)
              {
              AddProcess addProcess = new AddProcess();
              Thread myThread = new Thread(() => addProcess.ProcessAdd());
                myThread.Start();
              }
            break;
          default:
            break;
        }
      }
    }
  }
}