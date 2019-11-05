using System;

namespace ExtendedConsole
{
    /// <summary>
    /// Provides support for using an xml based markup text in console applications. 
    /// Markup contains text and control tags to set the text and background color. 
    /// Color names are limited to members of the ConsoleColor enum, but are case insensitive.
    /// Markup example:
    /// The next test is &lt;c f='Yellow'&gt; yellow &lt;c b='bluE'&gt; on blue. &lt;/c&gt; &lt;c f='BLACK' b='white'&gt; black on white &lt;/c&gt; yellow again &lt;/c&gt; back to normal colors.
    /// All methods that writes to console in this class supports markup.
    /// Since markup is xml based, all write methods might throw a System.Xml.XmlException.
    /// </summary>
    public class ExConsole
    {
        /// <summary>
        /// Writes a string to the console.
        /// </summary>
        /// <param name="markup">markup text to write.</param>
        public void Write(string markup)
        {
            Parser.ParseMarkeup(markup).Write();
        }

        /// <summary>
        /// Writes a string to the console, advances the cursor to the next line.
        /// </summary>
        /// <param name="markup">markup text to write.</param>
        public void WriteLine(string markup)
        {
            Parser.ParseMarkeup(markup).WriteLine();
        }

        /// <summary>
        /// Writes multiple lines to the console. 
        /// </summary>
        /// <param name="lines">lines of markup text to write.</param>
        public void WriteLines(params string[] lines)
        {
            foreach(var line in lines)
            {
                WriteLine(line);
            }
        }

        /// <summary>
        /// Writes a line with the specified colors.
        /// </summary>
        /// <param name="text">Line of text to write.</param>
        /// <param name="foregroundColor">Foreground color.</param>
        /// <param name="backgroundColor">Background color.</param>
        public void WriteLine(string text, ConsoleColor? foregroundColor, ConsoleColor? backgroundColor)
        {
            WriteInColor(() => WriteLine(text), foregroundColor, backgroundColor);
        }

        /// <summary>
        /// Writes the specified text with the specified colors.
        /// </summary>
        /// <param name="text">Text to write.</param>
        /// <param name="foregroundColor">Foreground color.</param>
        /// <param name="backgroundColor">Background color.</param>
        public void Write(string text, ConsoleColor? foregroundColor, ConsoleColor? backgroundColor)
        {
            WriteInColor(() => Write(text), foregroundColor, backgroundColor);
        }

        /// <summary>
        /// Writes the specified text with the specified colors, 
        /// either as a complete line or as a part of one.
        /// </summary>
        /// <param name="action">Action to execute.</param>
        /// <param name="foregroundColor">Foreground color.</param>
        /// <param name="backgroundColor">Background color.</param>
        private void WriteInColor(Action action, ConsoleColor? foregroundColor, ConsoleColor? backgroundColor)
        {
            var (Foreground, Background) = (Console.ForegroundColor, Console.BackgroundColor);
            Console.ForegroundColor = foregroundColor ?? Foreground;
            Console.BackgroundColor = backgroundColor ?? Background;

            action.Invoke();

            Console.ForegroundColor = Foreground;
            Console.BackgroundColor = Background;
        }
    }
}
