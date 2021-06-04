using System;
using System.Threading;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Laba5
{
  public class AddProcess
  {
    public void ProccessDo()
    {
      Console.WriteLine("Задача выполняется");
      if (Program.meme.Count != 0)
      {
        Program.meme.Dequeue();
        Console.WriteLine("Задача удалена");
      }
    }
    public void QueueDo()
    {
      int count = 0;
      if (Program.meme.Count == 0 && Program.queue.Count == 0)
        Console.WriteLine("Задач нет");
      foreach (int m in Program.meme)
      {
        count += m;
      }
      if (Program.queue.Count != 0)
      {
        if ((count + Program.queue.Peek()) <= Program.meme_size)
        {
          Program.meme.Enqueue(Program.queue.Dequeue());
        }
      }
    }
      public void ProcessAdd()
    {
      Random random = new Random();
      int sizeOfFile = random.Next(65535);
      int count = 0;
      string fileName = "{COUNT + 1}.txt";
      using (FileStream fWrite = new FileStream(fileName, FileMode.Create, FileAccess.Write))
      {
        fWrite.SetLength(sizeOfFile);
        count++;
      }

      foreach (int m in Program.meme) {
        count += m;
      }
      if (sizeOfFile+count < Program.meme_size) 
      {
        Program.meme.Enqueue(sizeOfFile);
        Console.WriteLine("Задача добавлена!");
      }
      else
      {
        Program.queue.Enqueue(sizeOfFile);
        Console.WriteLine("Не хватает места. Отправлено в очередь");
      }
      var outer = Task.Run(() =>
      {
        ProccessDo();
        Action p = () =>
                   {
                     QueueDo();
                   };
        var inner = Task.Run(p);
      });
      outer.Wait(); 
    }

    }
  }
