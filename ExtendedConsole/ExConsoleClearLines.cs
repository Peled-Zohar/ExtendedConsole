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
        /// <param name="cursorTop">Line index to clear.</param>
        public static void ClearLine(this ExConsole self, int cursorTop)
        {
            if (cursorTop < 0) throw new ArgumentException($"{nameof(cursorTop)} can't be a negative value.");

            Console.SetCursorPosition(0, cursorTop);
            Console.WriteLine(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, cursorTop);
        }

        /// <summary>
        /// Clears all text from the last line.
        /// </summary>
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
        /// <param name="numberOfLines">The number of lines to clear (count up from last line)</param>
        public static void ClearLastLines(this ExConsole self, int numberOfLines)
        {
            if (numberOfLines < 1) throw new ArgumentException($"{nameof(numberOfLines)} must be positive.");

            for (var i = 0; i < numberOfLines; i++)
            {
                self.ClearLastLine();
            }
        }
    }
}
