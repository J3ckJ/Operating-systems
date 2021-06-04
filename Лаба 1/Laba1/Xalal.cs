using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
/*
* +Создать файл формате XML из редактора
* + Записать в файл новые данные из консоли .
* + Прочитать файл в консоль.
* + Удалить файл.
*/
namespace Laba1
{
  public class Xalal
  {
    string path = "users.xml";

    public void WriteData()
    {
      Console.WriteLine("\nName Company Price");
      string name = Console.ReadLine();
      string company = Console.ReadLine();
      string price = Console.ReadLine();
      XDocument xdoc = new XDocument(new XElement("phones",
          new XElement("phone",
              new XAttribute("name", "iPhone 6"),
              new XElement("company", "Apple"),
              new XElement("price", "40000")),
          new XElement("phone",
              new XAttribute("name", "Samsung Galaxy S5"),
              new XElement("company", "Samsung"),
              new XElement("price", "33000")),
          new XElement("phone",
              new XAttribute("name", name),
              new XElement("company", company),
              new XElement("price", price))));
      xdoc.Save(path);
    }
    public void ShowData()
    {
      XmlDocument xDoc = new XmlDocument();
      xDoc.Load(path);
      XmlElement xRoot = xDoc.DocumentElement;

      foreach (XmlNode xnode in xRoot)
      {

        if (xnode.Attributes.Count > 0)
        {
          XmlNode attr = xnode.Attributes.GetNamedItem("name");
          if (attr != null)
            Console.WriteLine(attr.Value);
        }
        foreach (XmlNode childnode in xnode.ChildNodes)
        {
          if (childnode.Name == "name")
          {
            Console.WriteLine($"Название: {childnode.InnerText}");
          }
          if (childnode.Name == "company")
          {
            Console.WriteLine($"Компания: {childnode.InnerText}");
          }
          if (childnode.Name == "price")
          {
            Console.WriteLine($"Цена: {childnode.InnerText}");
          }
        }
        Console.WriteLine();
      }
      Console.Read();
    }
    public void DeleteXML()
    {
      File.Delete(path);
    }
  }
}