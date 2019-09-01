using System;
using System.Collections.Generic;

namespace ExtendedConsole
{
    internal class TextBuilder 
    {
        private readonly List<Action> _actions;
        private ConsoleColor _foreground, _background;

        internal TextBuilder()
        {
            _actions = new List<Action>();
            _foreground = Console.ForegroundColor;
            _background = Console.BackgroundColor;
        }

        internal void AddText(string text)
        {
            _actions.Add(() => Console.Write(text));
        }

        internal ConsoleColor? SetForegroundColor(ConsoleColor color)
        {
            var returnValue = _foreground;
            _foreground = color;
            _actions.Add(() => Console.ForegroundColor = color);
            return returnValue;
        }

        internal ConsoleColor? SetBackgroundColor(ConsoleColor color)
        {
            var returnValue = _background;
            _background = color;
            _actions.Add(() => Console.BackgroundColor = color);
            return returnValue;
        }

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

        internal void Write()
        {
            foreach (var action in _actions)
            {
                action();
            }
            _actions.Clear();
        }

        internal void WriteLine()
        {
            _actions.Add(() => Console.WriteLine());
            foreach (var action in _actions)
            {
                action();
            }
            _actions.Clear();
        }

    }
}
