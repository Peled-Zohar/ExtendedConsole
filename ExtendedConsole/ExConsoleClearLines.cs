using System;

namespace ExtendedConsole
{

    /// <summary>
    /// Provides extension methods for ExConsole for clearing specific console lines.
    /// </summary>
    public static class ExConsoleClearLines
    {

        /// <summary>
        /// Clears all text from a specific line.
        /// </summary>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="lineIndex">Line index to clear.</param>
        public static void ClearLine(this ExConsole self, int lineIndex)
        {
            if (lineIndex < 0) throw new ArgumentOutOfRangeException($"{nameof(lineIndex)} can't be a negative value.");

            Console.SetCursorPosition(0, lineIndex);
            Console.WriteLine(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, lineIndex);
        }

        /// <summary>
        /// Clears all text from the last line.
        /// </summary>
        /// <param name="self">The current instance of ExConsole.</param>
        public static void ClearLastLine(this ExConsole self)
        {
            if (Console.CursorTop > 0)
            {
                self.ClearLine(Console.CursorTop - 1);
            }
        }

        /// <summary>
        /// Clears all text from the last lines.
        /// </summary>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="numberOfLines">The number of lines to clear (count up from last line)</param>
        public static void ClearLastLines(this ExConsole self, int numberOfLines)
        {
            if (numberOfLines < 1) throw new ArgumentOutOfRangeException($"{nameof(numberOfLines)} must be a positive value.");

            for (var i = 0; i < numberOfLines; i++)
            {
                self.ClearLastLine();
            }
        }
    }
}
