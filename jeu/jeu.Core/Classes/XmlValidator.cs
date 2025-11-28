using System;
using System.Xml;
using System.Xml.Schema;

namespace jeu.Core.Classes;

public class XmlValidator
{
    public bool Validate(string xmlPath, string xsdPath, out string errorMessage)
    {
        errorMessage = string.Empty;
        try
        {
            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add("", xsdPath);

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Schemas.Add(schemas);
            settings.ValidationType = ValidationType.Schema;

            string errors = string.Empty;

            settings.ValidationEventHandler += (sender, e) =>
            {
                errors += $"{e.Message}\n"; 
            };

            XmlReader reader = XmlReader.Create(xmlPath, settings);
            while (reader.Read()) { } 
            
            errorMessage = errors;
            return string.IsNullOrEmpty(errorMessage);
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
            return false;
        }
    }
}