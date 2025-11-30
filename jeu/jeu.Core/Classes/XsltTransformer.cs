using System;
using System.Xml.Xsl;

namespace jeu.Core.Classes;

public static class XsltTransformer
{
	public static void Transform(string xmlPath, string xslPath, string outputPath)
	{
		try
		{
			XslCompiledTransform xslt = new();
			xslt.Load(xslPath);
			xslt.Transform(xmlPath, outputPath);
		}
		catch (Exception ex)
		{
			Console.WriteLine("Erreur XSLT : " + ex.Message);
		}
	}
}