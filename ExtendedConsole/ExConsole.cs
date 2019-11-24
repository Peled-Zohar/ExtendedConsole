using System;

namespace ExtendedConsole
{
    /// <summary>
    /// Provides support for using an xml based markup text in console applications. 
    /// Markup contains text and control tags to set the text and background color. 
    /// Color names are limited to members of the ConsoleColor enum, but are case insensitive.
    /// </summary>
    /// <Remarks>
    /// All methods that writes to console in this class supports markup.
    /// Since markup is xml based, all write methods might throw a System.Xml.XmlException.
    /// <para>Markup example: &lt;c f='Yellow'&gt;yellow text&lt;c b='blue'&gt; on blue background.&lt;/c&gt;&lt;/c&gt;</para>
    /// </Remarks>
    public class ExConsole
    {
        /// <summary>
        /// Writes the specified markup to the console.
        /// <para>Markup example: &lt;c f='Yellow'&gt;yellow text&lt;c b='blue'&gt; on blue background.&lt;/c&gt;&lt;/c&gt;</para>
        /// </summary>
        /// <param name="markup">Markup text to write.</param>
        /// <exception cref="System.Xml.XmlException">Thrown when markup text isn't properly formatted xml.</exception>
        /// <example>
        /// <code>
        /// exConsole.Write("&lt;c f='cyan' b='darkgray'&gt;Hello World!&lt;c&gt;");
        /// </code>
        /// </example>
        public void Write(string markup)
        {
            Parser.ParseMarkeup(markup).Write();
        }

        /// <summary>
        /// Writes the specified markup to the console, advances the cursor to the next line.
        /// <para>Markup example: &lt;c f='Yellow'&gt;yellow text&lt;c b='blue'&gt; on blue background.&lt;/c&gt;&lt;/c&gt;</para>
        /// </summary>
        /// <param name="markup">Markup text to write.</param>
        /// <exception cref="System.Xml.XmlException">Thrown when markup text isn't properly formatted xml.</exception>
        /// <example>
        /// <code>
        /// exConsole.WriteLine("&lt;c b='white' f='black'&gt;Hello World!&lt;c&gt;");
        /// </code>
        /// </example>
        public void WriteLine(string markup)
        {
            Parser.ParseMarkeup(markup).WriteLine();
        }

        /// <summary>
        /// Writes multiple markup lines to the console. 
        /// <para>Markup example: &lt;c f='Yellow'&gt;yellow text&lt;c b='blue'&gt; on blue background.&lt;/c&gt;&lt;/c&gt;</para>
        /// </summary>
        /// <param name="lines">Lines of markup text to write.</param>
        /// <exception cref="System.Xml.XmlException">Thrown when markup text isn't properly formatted xml.</exception>
        /// <example>
        /// <code>
        /// exConsole.WriteLines(
        ///     "&lt;c f='Yellow'&gt;Hello World!&lt;c&gt;",
        ///     "This is &lt;c f='green'&gt;exConsole&lt;c&gt; writing multiple markup lines",
        ///     "To the Console."
        /// );
        /// </code>
        /// </example>
        public void WriteLines(params string[] lines)
        {
            foreach(var line in lines)
            {
                WriteLine(line);
            }
        }

        /// <summary>
        /// Changes console colors to the specified colors, and writes the specified markup to the console. 
        /// Advances the cursor to the next line.
        /// </summary>
        /// <param name="markup">Markup text to write.</param>
        /// <param name="foregroundColor">Foreground color.</param>
        /// <param name="backgroundColor">Background color.</param>
        /// <exception cref="System.Xml.XmlException">Thrown when markup text isn't properly formatted xml.</exception>
        /// <example>
        /// <code>
        /// exConsole.WriteLine("Hello world! &lt;c b='white' f='black'&gt;How are you?&lt;c&gt;", ConsoleColor.Magenta, null);
        /// </code>
        /// </example>
        public void WriteLine(string markup, ConsoleColor? foregroundColor, ConsoleColor? backgroundColor)
        {
            WriteInColor(() => WriteLine(markup), foregroundColor, backgroundColor);
        }

        /// <summary>
        /// Changes console colors to the specified colors, and writes the specified markup to the console. 
        /// </summary>
        /// <param name="markup">Markup text to write.</param>
        /// <param name="foregroundColor">Foreground color.</param>
        /// <param name="backgroundColor">Background color.</param>
        /// <exception cref="System.Xml.XmlException">Thrown when markup text isn't properly formatted xml.</exception>
        /// <example>
        /// <code>
        /// exConsole.Write("Hello world! &lt;c b='white' f='black'&gt;How are you?&lt;c&gt;", ConsoleColor.Black, ConsoleColor.Magenta);
        /// </code>
        /// </example>
        public void Write(string markup, ConsoleColor? foregroundColor, ConsoleColor? backgroundColor)
        {
            
            WriteInColor(() => Write(markup), foregroundColor, backgroundColor);
        }

        // <summary>
        // Writes the specified text with the specified colors, 
        // either as a complete line or as a part of one.
        // </summary>
        // <param name="action">Action to execute.</param>
        // <param name="foregroundColor">Foreground color.</param>
        // <param name="backgroundColor">Background color.</param>
        // <exception cref="System.Xml.XmlException">Thrown when markup text isn't properly formatted xml.</exception>
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
