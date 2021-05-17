using System;

namespace ExtendedConsole
{

    /// <summary>
    /// Provides extension methods for ExConsole for clearing specific console lines.
    /// </summary>
    public static class ExConsoleClearLines
    {
        /// <summary>
        /// Clears all text from the current line.
        /// </summary>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <returns>The current instance of <see cref="ExConsole"/>.</returns>
        /// <example>
        /// Clears the current line of the console.
        /// <code>
        /// exConsole.ClearCurrentLine();
        /// </code>
        /// </example>
        public static ExConsole ClearCurrentLine(this ExConsole self)
        {
            return ClearLine(self, Console.CursorTop);
        }

        /// <summary>
        /// Clears all text from a specific line.
        /// </summary>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="lineIndex">Line index to clear.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="lineIndex"/> is less than zero.</exception>
        /// <returns>The current instance of <see cref="ExConsole"/>.</returns>
        /// <example>
        /// Clear the second line from the top.
        /// <code>
        /// exConsole.ClearLine(1);
        /// </code>
        /// </example>
        public static ExConsole ClearLine(this ExConsole self, int lineIndex)
        {
            if (lineIndex < 0) throw new ArgumentOutOfRangeException($"{nameof(lineIndex)} can't be a negative value.");

            Console.SetCursorPosition(0, lineIndex);
            Console.WriteLine(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, lineIndex);

            return self;
        }


        /// <summary>
        /// Clears all text from the last line.
        /// </summary>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <returns>The current instance of <see cref="ExConsole"/>.</returns>
        /// <example>
        /// Clear the last line.
        /// <code>
        /// exConsole.ClearLastLine();
        /// </code>
        /// </example>
        public static ExConsole ClearLastLine(this ExConsole self)
        {
            if (Console.CursorTop > 0)
            {
                ClearLine(self, Console.CursorTop - 1);
            }
            return self;
        }

        /// <summary>
        /// Clears all text from the last lines.
        /// </summary>
        /// <param name="self">The current instance of <see cref="ExConsole"/>.</param>
        /// <param name="numberOfLines">The number of lines to clear (count up from last line)</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="numberOfLines"/> is less than one.</exception>
        /// <returns>The current instance of <see cref="ExConsole"/>.</returns>
        /// <example>
        /// Clear the last three lines.
        /// <code>
        /// exConsole.ClearLastLines(3);
        /// </code>
        /// </example>
        public static ExConsole ClearLastLines(this ExConsole self, int numberOfLines)
        {
            if (numberOfLines < 1) throw new ArgumentOutOfRangeException($"{nameof(numberOfLines)} must be a positive value.");

            for (var i = 0; i < numberOfLines; i++)
            {
                ClearLastLine(self);
            }
            return self;
        }

        /// <summary>
        /// Clears all the text on the console, and returns the current instance of ExConsole.
        /// </summary>
        /// <param name="self">The current instance of <see cref="ExConsole"/>.</param>
        /// <returns>The current instance of <see cref="ExConsole"/>.</returns>
        /// <example>
        /// Pause, clear all lines, and ask the user to enter an integer value:
        /// <code>
        /// exConsole
        ///     .Pause("Press any key to continue")
        ///     .ClearAllLines()
        ///     .ReadInt("Please enter a number:");
        /// </code>
        /// </example>
        public static ExConsole ClearAllLines(this ExConsole self)
        {
            Console.Clear();
            return self;
        }
    }
}
