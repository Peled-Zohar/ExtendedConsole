using System;
using System.Xml.Linq;
using System.Linq;

namespace ExtendedConsole
{
    internal static class Parser
    {
        /// <summary>
        /// Parse markup and returns an instance of the TextBuilder class,
        /// containing actions to write the text to the Console.
        /// </summary>
        /// <param name="markup">A string containing the markup to write to Console.</param>
        /// <returns>An instance of the TextBuilder class ready to write the text to the Console.</returns>
        internal static TextBuilder ParseMarkeup(string markup)
        {
            var textBuilder = new TextBuilder();

            markup = $"<ExtendedConsoleMarkup>{markup}</ExtendedConsoleMarkup>";
            var element = XElement.Parse(markup);
            if (element.FirstNode != null)
            {
                ParseNode(textBuilder, element.FirstNode);
            }
            return textBuilder;
        }

        /// <summary>
        /// Recursively parses XML nodes in the markup.
        /// </summary>
        /// <param name="textBuilder">The instance of TextBuilder to hold the parsed information.</param>
        /// <param name="node">The current XML node to parse.</param>
        private static void ParseNode(TextBuilder textBuilder, XNode node)
        {
            if (node is null)
            {
                return;
            }

            var element = node as XElement;
            if (element is null)
            {
                textBuilder.AddText(node.ToString(SaveOptions.DisableFormatting));
            }
            else
            {
                ParseElement(textBuilder, element);
            }

            ParseNode(textBuilder, node.NextNode);

        }

        /// <summary>
        /// Parse an XML element into the text builder.
        /// If the element has children, Pass them to the ParseNode method.
        /// </summary>
        /// <param name="textBuilder">The instance of TextBuilder to hold the parsed information.</param>
        /// <param name="element">The current XML element to parse.</param>
        private static void ParseElement(TextBuilder textBuilder, XElement element)
        {
            ConsoleColor? foregroundColor = null, backgroundColor = null;
            var isColorElement = element.Name.LocalName == "c" && 
                element.Attributes().Any(a => a.Name == "f" || a.Name == "b");

            if (isColorElement)
            {
                SetColor(ref foregroundColor, "f", textBuilder.SetForegroundColor);
                SetColor(ref backgroundColor, "b", textBuilder.SetBackgroundColor);
            }
            else
            {
                WriteElement(textBuilder, element);
            }

            if (element.FirstNode != null)
            {
                ParseNode(textBuilder, element.FirstNode);
            }


            if (isColorElement)
            {
                textBuilder.ResetColors(foregroundColor, backgroundColor);
            }
            else if (!(element.FirstNode is null))
            {
                textBuilder.AddText($"</{element.Name}>");
            }

            void SetColor(ref ConsoleColor? color, string attributeName, Func<ConsoleColor, ConsoleColor?> setColor)
            {
                var attribute = element.Attribute(attributeName);
                if (attribute != null &&
                    Enum.TryParse(attribute.Value, true, out ConsoleColor newColor))
                {
                    color = setColor(newColor);
                }
            }
        }

        /// <summary>
        /// Adds the XML element to the TextBuilder.
        /// </summary>
        /// <param name="textBuilder">The instance of TextBuilder to hold the parsed information.</param>
        /// <param name="element">The current XML element to add to the TextBuilder.</param>
        private static void WriteElement(TextBuilder textBuilder, XElement element)
        {

            textBuilder.AddText($"<{element.Name}");
            if (element.HasAttributes)
            {
                WriteAttributes(textBuilder, element.FirstAttribute);
            }
            if (element.FirstNode is null)
            {
                textBuilder.AddText(" /");
            }
            textBuilder.AddText(">");
        }

        /// <summary>
        /// Adds the attributes of an XML element to the TextBuilder.
        /// </summary>
        /// <param name="textBuilder">The instance of TextBuilder to hold the parsed information.</param>
        /// <param name="attribute">The current attribute to add to the TextBuilder.</param>
        private static void WriteAttributes(TextBuilder textBuilder, XAttribute attribute)
        {
            textBuilder.AddText($" {attribute}");
            var nextAttribute = attribute.NextAttribute;
            if (nextAttribute != null)
            {
                WriteAttributes(textBuilder, nextAttribute);
            }
        }
    }


}
