using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using Newtonsoft.Json;
using JsonFmt = Newtonsoft.Json.Formatting;

namespace ConsoleApp1
{
    public class Program
    {
        // ← make sure there are no leading or trailing spaces in these URLs:
        public static string xmlURL = "https://zhengyue4499.github.io/Assignment4-XML-Hotels/Hotels.xml";
        public static string xmlErrorURL = "https://zhengyue4499.github.io/Assignment4-XML-Hotels/HotelsErrors.xml";
        public static string xsdURL = "https://zhengyue4499.github.io/Assignment4-XML-Hotels/Hotels.xsd";

        public static void Main(string[] args)
        {
            // 1. valid XML
            Console.WriteLine(Verification(xmlURL, xsdURL));
            // 2. invalid XML
            Console.WriteLine(Verification(xmlErrorURL, xsdURL));
            // 3. XML → JSON
            Console.WriteLine(Xml2Json(xmlURL));
        }

        // Q2.1: validate xmlUrl against xsdUrl
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            var errors = new List<string>();
            var settings = new XmlReaderSettings();
            settings.Schemas.Add(null, xsdUrl);
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationEventHandler += (sender, e) =>
            {
                var ex = e.Exception;
                errors.Add($"Line {ex.LineNumber}, pos {ex.LinePosition}: {e.Message}");
            };

            using (var reader = XmlReader.Create(xmlUrl, settings))
            {
                try
                {
                    while (reader.Read()) { }
                }
                catch (XmlException xe)
                {
                    errors.Add($"XML Exception: {xe.Message}");
                }
            }

            return errors.Count == 0
                ? "No Error"
                : string.Join(Environment.NewLine, errors);
        }

        // Q2.2: convert a valid XML to JSON text
        public static string Xml2Json(string xmlUrl)
        {
            var doc = new XmlDocument();
            doc.Load(xmlUrl);

            // Disambiguated Formatting via alias JsonFmt:
            string jsonText = JsonConvert.SerializeXmlNode(
                doc,
                JsonFmt.Indented
            );

            return jsonText;
        }
    }
}
