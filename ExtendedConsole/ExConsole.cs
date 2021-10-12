using System;

namespace ExtendedConsole
{
    /// <summary>
    /// Provides support for using an xml based markup text in console applications.
    /// <para>
    /// Markup contains text and control tags to set the foreground and background color of the text. 
    /// Color names are limited to members of the ConsoleColor enum, but are case insensitive.
    /// </para>
    /// <para>
    /// Markup example: 
    /// <code>
    /// &lt;c f='Yellow'&gt;yellow text&lt;c b='blue'&gt; on blue background.&lt;/c&gt;&lt;/c&gt;
    /// </code>
    /// </para>
    /// </summary>
    /// <Remarks>
    /// All methods that writes to console in this class supports markup.
    /// Since markup is xml based, all write methods might throw a System.Xml.XmlException.
    /// </Remarks>
    public class ExConsole
    {
        /// <summary>
        /// Writes the specified markup to the console.
        /// </summary>
        /// <param name="markup">Markup text to write.</param>
        /// <exception cref="System.Xml.XmlException">Thrown when markup text isn't properly formatted xml.</exception>
        /// <returns>The current instance of <see cref="ExConsole"/>.</returns>
        /// <example>
        /// Write "Hello World!", cyan on a dark gray background.
        /// <code>
        /// exConsole.Write("&lt;c f='cyan' b='darkgray'&gt;Hello World!&lt;c&gt;");
        /// </code>
        /// </example>
        public ExConsole Write(string markup)
        {
            Parser.ParseMarkeup(markup).Write();
            return this;
        }

        /// <summary>
        /// Writes the specified markup to the console, advances the cursor to the next line.
        /// </summary>
        /// <param name="markup">Markup text to write.</param>
        /// <exception cref="System.Xml.XmlException">Thrown when markup text isn't properly formatted xml.</exception>
        /// <returns>The current instance of <see cref="ExConsole"/>.</returns>
        /// <example>
        /// Write "Hello World!", black on a white background, 
        /// and advance the cursor to the next line.
        /// <code>
        /// exConsole.WriteLine("&lt;c b='white' f='black'&gt;Hello World!&lt;c&gt;");
        /// </code>
        /// </example>
        public ExConsole WriteLine(string markup)
        {
            Parser.ParseMarkeup(markup).WriteLine();
            return this;
        }

        /// <summary>
        /// Writes multiple markup lines to the console. 
        /// </summary>
        /// <param name="lines">Lines of markup text to write.</param>
        /// <exception cref="System.Xml.XmlException">Thrown when markup text isn't properly formatted xml.</exception>
        /// <returns>The current instance of <see cref="ExConsole"/>.</returns>
        /// <example>
        /// Write "Hello World!" in yellow, 
        /// "This is exConsole writing multiple markup lines" in the next line, with exConsole in green, and
        /// "To the Console." in the last line. Advance the cursor to the next line.
        /// <code>
        /// exConsole.WriteLines(
        ///     "&lt;c f='Yellow'&gt;Hello World!&lt;c&gt;",
        ///     "This is &lt;c f='green'&gt;exConsole&lt;c&gt; writing multiple markup lines",
        ///     "To the Console."
        /// );
        /// </code>
        /// </example>
        public ExConsole WriteLines(params string[] lines)
        {
            foreach(var line in lines)
            {
                WriteLine(line);
            }
            return this;
        }

        /// <summary>
        /// Changes console colors to the specified colors, and writes the specified markup to the console. 
        /// Advances the cursor to the next line.
        /// </summary>
        /// <param name="markup">Markup text to write.</param>
        /// <param name="foregroundColor">Foreground color.</param>
        /// <param name="backgroundColor">Background color.</param>
        /// <exception cref="System.Xml.XmlException">Thrown when markup text isn't properly formatted xml.</exception>
        /// <returns>The current instance of <see cref="ExConsole"/>.</returns>
        /// <example>
        /// Write "Hello World!" in magenta, followed by "How are you?" in black on a white background. Advance the cursor to the next line.
        /// <code>
        /// exConsole.WriteLine("Hello world! &lt;c b='white' f='black'&gt;How are you?&lt;c&gt;", ConsoleColor.Magenta, null);
        /// </code>
        /// </example>
        [Obsolete("this method is deprecated and will be removed in future versions.")]
        public ExConsole WriteLine(string markup, ConsoleColor? foregroundColor, ConsoleColor? backgroundColor)
            => WriteInColor(() => WriteLine(markup), foregroundColor, backgroundColor);

        /// <summary>
        /// Changes console colors to the specified colors, and writes the specified markup to the console. 
        /// </summary>
        /// <param name="markup">Markup text to write.</param>
        /// <param name="foregroundColor">Foreground color.</param>
        /// <param name="backgroundColor">Background color.</param>
        /// <exception cref="System.Xml.XmlException">Thrown when markup text isn't properly formatted xml.</exception>
        /// <returns>The current instance of <see cref="ExConsole"/>.</returns>
        /// <example>
        /// Write "Hello World!" in black on a magenta background, followed by "How are you?" in black on a white background.
        /// <code>
        /// exConsole.Write("Hello world! &lt;c b='white' f='black'&gt;How are you?&lt;c&gt;", ConsoleColor.Black, ConsoleColor.Magenta);
        /// </code>
        /// </example>
        [Obsolete("this method is deprecated and will be removed in future versions.")]
        public ExConsole Write(string markup, ConsoleColor? foregroundColor, ConsoleColor? backgroundColor)
            => WriteInColor(() => Write(markup), foregroundColor, backgroundColor);

        // Remove in a future version
        private ExConsole WriteInColor(Action action, ConsoleColor? foregroundColor, ConsoleColor? backgroundColor)
        {
            var (Foreground, Background) = (Console.ForegroundColor, Console.BackgroundColor);
            Console.ForegroundColor = foregroundColor ?? Foreground;
            Console.BackgroundColor = backgroundColor ?? Background;

            action.Invoke();

            Console.ForegroundColor = Foreground;
            Console.BackgroundColor = Background;

            return this;
        }
    }
}
