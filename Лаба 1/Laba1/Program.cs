using System;
using System.Threading.Tasks;

namespace Laba1
{
  class Program
  {
    async static Task Main(string[] args)
    {

      string sw;
      for (; ; )
      {
      Start:
        Console.WriteLine("Введите цифру для выполнения соответсвующего задания:\n" +
          "1. Вывести информацию в консоль о логических дисках, именах, метке тома, размере типе файловой системы.\n" +
          "2. Работа с файлами.\n" +
          "3. Работа с форматом JSON.\n" +
          "4. Работа с форматом XML.\n" +
          "5. Создание zip архива, добавление туда файла, определение размера архива.\n");
        sw = "a";
        while (sw != null)
        {
          Console.WriteLine("Ввод: ");
          sw = Console.ReadLine();
          switch (sw)
          {
            case "1":
              var disk = new Disk();
              disk.ShowDisksInfo();
              Routine();
              break;
            case "2":
              var file = new FileWork();
              file.CreateFile();
              file.WriteToFile();
              file.ReadFromFile();
              file.DeleteFile();
              Routine();
              break;
            case "3":
              var jason = new JsonWork();
              jason.SerializeEmAll();
              await jason.ReadEmAll();
              jason.DeleteIt();
              Routine();
              break;
            case "4":
              var xalal = new Xalal();
              xalal.WriteData();
              xalal.ShowData();
              Routine();
              break;
            case "5":
              var zippo = new Zippo();
              zippo.CreateFolder();
              zippo.CompressUncompress();
              Routine();
              break;
            default:
              Console.WriteLine("Попробуй ещё раз\n");
              goto Start;
          }
        }
      }
      void Routine()
      {
        Console.WriteLine("Нажмите любую клавишу для продолжения...");
        Console.ReadKey();
        Console.Clear();
      }
    }
  }
}


