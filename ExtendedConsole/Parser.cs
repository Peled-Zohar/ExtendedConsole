using System;
using System.Xml.Linq;

namespace ExtendedConsole
{
    internal static class Parser
    {
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

        private static void ParseNode(TextBuilder textBuilder, XNode node)
        {
            var element = node as XElement;
            if (element is null)
            {
                textBuilder.AddText(node.ToString(SaveOptions.DisableFormatting));
            }
            else
            {
                ParseElement(textBuilder, element);
            }

            if (node.NextNode != null)
            {
                ParseNode(textBuilder, node.NextNode);
            }
        }

        private static void ParseElement(TextBuilder textBuilder, XElement element)
        {
            ConsoleColor? foregroundColor = null, backgroundColor = null;
            var isColorElement = element.Name.LocalName == "c";
            if (isColorElement)
            {
                var foreground = element.Attribute("f");
                if (foreground != null)
                {
                    if (Enum.TryParse(foreground.Value, true, out ConsoleColor color))
                    {
                        foregroundColor = textBuilder.SetForegroundColor(color);
                    }
                }

                var background = element.Attribute("b");
                if (background != null)
                {
                    if (Enum.TryParse(background.Value, true, out ConsoleColor color))
                    {
                        backgroundColor = textBuilder.SetBackgroundColor(color);
                    }
                }
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
        }

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

        private static void WriteAttributes(TextBuilder textBuilder, XAttribute attribute)
        {
            textBuilder.AddText($" {attribute}");
            if (attribute.NextAttribute != null)
            {
                WriteAttributes(textBuilder, attribute.NextAttribute);
            }
        }
    }


}
