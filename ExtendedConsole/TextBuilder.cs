using System;
using System.Collections.Generic;

namespace ExtendedConsole
{
    /// <summary>
    /// Builds the text and colors to write to Console.
    /// </summary>
    internal class TextBuilder 
    {
        private readonly List<Action> _actions;
        private ConsoleColor _foreground, _background;

        /// <summary>
        /// Initialize a new instance of the TextBuilder class.
        /// </summary>
        internal TextBuilder()
        {
            _actions = new List<Action>();
            _foreground = Console.ForegroundColor;
            _background = Console.BackgroundColor;
        }

        /// <summary>
        /// Adds text to write to the console.
        /// </summary>
        /// <param name="text">Text to write to the console.</param>
        internal void AddText(string text)
        {
            _actions.Add(() => Console.Write(text));
        }

        /// <summary>
        /// Adds a foreground color change, and returns the previous foreground color.
        /// </summary>
        /// <param name="color">A member of the ConsoleColor enum to set the foreground color to.</param>
        /// <returns>A nullable member of the ConsoleColor enum.</returns>
        internal ConsoleColor? SetForegroundColor(ConsoleColor color)
        {
            var returnValue = _foreground;
            _foreground = color;
            _actions.Add(() => Console.ForegroundColor = color);
            return returnValue;
        }

        /// <summary>
        /// Adds a background color change, and returns the previous background color.
        /// </summary>
        /// <param name="color">A member of the ConsoleColor enum to set the background color to.</param>
        /// <returns>A nullable member of the ConsoleColor enum.</returns>
        internal ConsoleColor? SetBackgroundColor(ConsoleColor color)
        {
            var returnValue = _background;
            _background = color;
            _actions.Add(() => Console.BackgroundColor = color);
            return returnValue;
        }

        /// <summary>
        /// Adds a foreground / background color change. If an argument is null, it has no effect.
        /// </summary>
        /// <param name="foreground">A nullable member of the ConsoleColor enum to set as foreground.</param>
        /// <param name="background">A nullable member of the ConsoleColor enum to set as background.</param>
        internal void ResetColors(ConsoleColor? foreground, ConsoleColor? background)
        {
            if (foreground.HasValue)
            {
                _actions.Add(() => Console.ForegroundColor = foreground.Value);
                _foreground = foreground.Value;
            }
            if (background.HasValue)
            {
                _actions.Add(() => Console.BackgroundColor = background.Value);
                _background = background.Value;
            }
        }

        /// <summary>
        /// Writes the content of the TextBuilder to the Console and clears it.
        /// </summary>
        internal void Write()
        {
            foreach (var action in _actions)
            {
                action();
            }
            _actions.Clear();
        }

        /// <summary>
        /// Writes the content of the TextBuilder to the Console, clears it and adds a new line.
        /// </summary>
        internal void WriteLine()
        {
            _actions.Add(() => Console.WriteLine());
            Write();
        }

    }
}
