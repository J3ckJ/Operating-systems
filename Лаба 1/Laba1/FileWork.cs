using System;
using System.IO;
/*
 * + Создать файл
 * + Записать в файл строку
 * + Прочитать файл в консоль
 * + Удалить файл 
*/
namespace Laba1
{ 
  class FileWork
  {
    string path = "text.txt";
    public void CreateFile()
    {
      string path = "text.txt";
      FileInfo fileInf = new FileInfo(path);
      if (fileInf.Exists)
      {
      }
      else
      {
        using (StreamWriter streamWriter = new StreamWriter(path))
        {
          streamWriter.Close();
        }
      }
      
    }
    public void WriteToFile()
    {
      
      FileInfo fileInfo = new FileInfo(path);
      if (fileInfo.Exists)
      {
        Console.WriteLine("Введите строку для записи в файл:");
        string text = Console.ReadLine();
        using (StreamWriter streamWriter = new StreamWriter(path))
        {
          streamWriter.Write(text);
          streamWriter.Close();
          Console.WriteLine("Текст записан в файл");
        }
      }
    }
    public void ReadFromFile()
    {
      
      FileInfo fileInfo = new FileInfo(path);
      if (fileInfo.Exists)
      {
        using (StreamReader rstream = new StreamReader(path))
        { 
          string textFromFile = rstream.ReadToEnd();
          Console.WriteLine($"Текст из файла: {textFromFile}");
          Console.ReadKey();
        }
      }
    }
    public void DeleteFile()
    {
      
      FileInfo fileInfo = new FileInfo(path);
      if (fileInfo.Exists)
      {

        File.Delete(path);
        Random rnd = new Random();
        int a = rnd.Next(50);
        for (int i = 0; i < a; i++)
        {
          if (i % 4 == 1) { Console.WriteLine("Удаляем."); }
          if (i % 4 == 2) { Console.WriteLine("Удаляем.."); }
          if (i % 4 == 3) { Console.WriteLine("Удаляем..."); }
          System.Threading.Thread.Sleep(10);
          Console.Clear();
        }
        Console.WriteLine("Удалено!");
      }
    }
  }
}