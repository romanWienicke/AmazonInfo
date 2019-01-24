using System.Xml.Linq;

namespace Tests
{
    public static class Extensions
    {
        public static string ValueString(this XElement element)
        {
            return element.Value.ToLower();
        }

        public static string ValueString(this XElement element, string elementName)
        {
            return element.Element(elementName).ValueString();
        }

        public static string ValueString(this XElement element, string elementName, XNamespace ns)
        {
            return element.Element(ns + elementName).ValueString();
        }
    }

}