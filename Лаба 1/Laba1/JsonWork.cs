using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
/*
 * + Создать файл формате JSON из редактора
 * + Создать новый объект. Выполнить сериализацию объекта в формате JSON и записать в файл.
 * + Прочитать файл в консоль
 * + Удалить файл
 */
namespace Laba1
{
  class JsonWork
  {
    string path = "user.json";
    class Person
    {
      public string Name { get; set; }
      public int Age { get; set; }
    }
    public void SerializeEmAll()
    {      using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
      {
        Person Jason = new Person { Name = "Jason", Age = 20 };
        JsonSerializer.SerializeAsync<Person>(fs, Jason);
        Console.WriteLine("Сохранено");
      }
    }
    async public Task ReadEmAll()
    {
      
      using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
      {
        Person restoredPerson = await JsonSerializer.DeserializeAsync<Person>(fs);
        Console.WriteLine($"Name: {restoredPerson.Name}  Age: {restoredPerson.Age}");
      }
    }
    public void DeleteIt()
    {
      FileInfo fileInf = new FileInfo(path);
      if (fileInf.Exists)
      {
        File.Delete(path);
      }
    }
  }
}
