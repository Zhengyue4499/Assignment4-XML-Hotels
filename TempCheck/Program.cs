using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using Newtonsoft.Json;
using System.IO;

/**
 * This template file is created for ASU CSE445 Distributed SW Dev Assignment 4.
 * Please do not modify or delete any existing class/variable/method names. However, you can add more variables and functions.
 * Uploading this file directly will not pass the autograder's compilation check, resulting in a grade of 0.
 **/


namespace ConsoleApp1
{
    public class Program
    {
        public static string xmlURL = "https://zhengyue4499.github.io/Assignment4-XML-Hotels/Hotels.xml";
        public static string xmlErrorURL = "https://zhengyue4499.github.io/Assignment4-XML-Hotels/HotelsErrors.xml";
        public static string xsdURL = "https://zhengyue4499.github.io/Assignment4-XML-Hotels/Hotels.xsd";

        public static void Main(string[] args)
        {
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);

            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);

            result = Xml2Json(xmlURL);
            Console.WriteLine(result);
        }

        // Q2.1
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

        // Q2.2
        public static string Xml2Json(string xmlUrl)
        {
            var doc = new XmlDocument();
            doc.Load(xmlUrl);

            // strip out schema hints so they don't pollute the JSON
            var root = doc.DocumentElement;
            if (root != null)
            {
                root.RemoveAttribute("xmlns:xsi");
                root.RemoveAttribute("xsi:noNamespaceSchemaLocation");
            }

            // serialize entire document (including root <Hotels>) to JSON
            string jsonText = JsonConvert.SerializeXmlNode(
                doc,
                Newtonsoft.Json.Formatting.Indented
            );

            return jsonText;
        }
    }
}