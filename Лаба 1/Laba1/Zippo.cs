using System;
using System.IO;
using System.IO.Compression;
using System.Text;
/*+ Создать архив в формате zip
* + Добавить файл в архив
* + Разархивировать файл и вывести данные о нем
* + Удалить файл и архив
*/
namespace Laba1
{
  class Zippo
  {
    string sourceFile = @"D:\text\text.txt";
    public void CreateFolder()
    {
      Directory.CreateDirectory(@"D:\text\");
      using (StreamWriter streamWriter = new StreamWriter(sourceFile))
      {
        string text = "a";
        for (int i = 0; i < 10; i++)
        {
          text += text;
        }
          streamWriter.WriteLine(text);
          streamWriter.Close();
      }
    }
      public void CompressUncompress()
      {
        string compressed = @"D:\text.zip";
        using (FileStream sourceStream = new FileStream(sourceFile, FileMode.OpenOrCreate))
        {
          using (FileStream targetStream = File.Create(compressed))
          {
            using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
            {
              sourceStream.CopyTo(compressionStream);
              Console.WriteLine("Сжатие файла {0} завершено. Исходный размер: {1}  сжатый размер: {2}.",
                  sourceFile, sourceStream.Length.ToString(), targetStream.Length.ToString());
            }
          }
        }
      using (FileStream sourceStream = new FileStream(compressed, FileMode.OpenOrCreate))
      {
        using (FileStream targetStream = File.Create(sourceFile))
        {
          using (GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
          {
            decompressionStream.CopyTo(targetStream);
            Console.WriteLine("Восстановлен файл: {0}", sourceFile);
          }
        }
      }
        File.Delete(compressed);
      }
    }
  }