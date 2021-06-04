using System;
using System.Collections.Generic;
using System.Text;

namespace Laba5
{
  public class MemoryInfo
  {
    public void MemoInfo()
    {
      int occupiedMemory = 0;
      foreach (int m in Program.meme)
      {
        occupiedMemory += m;
      }
      Console.WriteLine($"Задач: {Program.meme.Count} | Свободно: {Program.meme_size - occupiedMemory} | Занято: {occupiedMemory}");
    }
  }
}
