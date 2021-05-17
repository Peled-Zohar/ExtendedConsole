using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ExtendedConsole
{

    /// <summary>
    /// Provides extension methods for ExConsole to support multiple select menus.
    /// </summary>
    public static class ExConsoleMultipleSelectMenu
    {
        /// <summary>
        /// Displays a multiple select menu to the user and returns the item(s) the user selected.
        /// </summary>
        /// <typeparam name="T">The type of items for the user to select from.</typeparam>
        /// <param name="self">The current instance of <see cref="ExConsole"/>.</param>
        /// <param name="displayArgs">An instance of the <see cref="MultipleSelectDisplayArgs"/> class holding the display configuration of the menu.</param>
        /// <param name="items">The items of the menu.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> containing the values selected by the user.</returns>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters is null.</exception>
        /// <exception cref="ArgumentException">Thrown when any of the text properties of <paramref name="displayArgs"/> are empty, or when <paramref name="items"/> contain less than two items.</exception>
        /// <exception cref="System.Xml.XmlException">Thrown when any of the text properties of <paramref name="displayArgs"/> aren't properly formatted xml.</exception>
        /// <example>
        /// Show the user a list of dates to choose from, return an <see cref="IEnumerable{T}"/> where T is <see cref="DateTime"/>.
        /// <code>
        /// var dates = exConsole.MultipleSelectMenu(
        ///     new MultipleSelectDisplayArgs("Select at least one date.", focusedItemColor:ConsoleColor.Cyan), 
        ///     DateTime.Now, 
        ///     DateTime.Now.AddHours(1), 
        ///     DateTime.Now.AddHours(2), 
        ///     DateTime.Now.AddHours(3)
        /// );
        /// </code>
        /// After the user selects the desired values, the `dates` variable will contain the user's selection.
        /// </example>
        public static IEnumerable<T> MultipleSelectMenu<T>(
            this ExConsole self,
            MultipleSelectDisplayArgs displayArgs,
            params T[] items
        ) => MultipleSelectMenu<T>(self, displayArgs, s => s.ToString(), items);

        /// <summary>
        /// Displays a multiple select menu to the user and returns the item(s) the user selected.
        /// </summary>
        /// <typeparam name="T">The type of items for the user to select from.</typeparam>
        /// <param name="self">The current instance of <see cref="ExConsole"/>.</param>
        /// <param name="displayArgs">An instance of the <see cref="MultipleSelectDisplayArgs"/> class holding the display configuration of the menu.</param>
        /// <param name="toString">A <see cref="Func{T, TResult}"/> to be used to display the items on the console, TResult is a <see cref="string"/>.</param>
        /// <param name="items">The items of the menu.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> containing the values selected by the user.</returns>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters is null.</exception>
        /// <exception cref="ArgumentException">Thrown when any of the text properties of <paramref name="displayArgs"/> are empty, or when <paramref name="items"/> contain less than two items.</exception>
        /// <exception cref="System.Xml.XmlException">Thrown when any of the text properties of <paramref name="displayArgs"/> aren't properly formatted xml.</exception>
        /// <example>
        /// Show the user a list of dates to choose from, as yyyy-mm-dd, return an <see cref="IEnumerable{T}"/> where T is <see cref="DateTime"/>.
        /// <code>
        /// var dates = exConsole.MultipleSelectMenu(
        ///     new MultipleSelectDisplayArgs("Select at least one date.", focusedItemColor:ConsoleColor.Cyan), 
        ///     d => d.ToString("yyyy-mm-dd"),
        ///     DateTime.Now, 
        ///     DateTime.Now.AddHours(1), 
        ///     DateTime.Now.AddHours(2), 
        ///     DateTime.Now.AddHours(3)
        /// );
        /// </code>
        /// After the user selects the desired values, the `dates` variable will contain the user's selection.
        /// </example>

        public static IEnumerable<T> MultipleSelectMenu<T>(
           this ExConsole self,
           MultipleSelectDisplayArgs displayArgs,
           Func<T, string> toString,
           params T[] items
       )
        { 
            ValidateArguments();

            var menuItems = items.Select(v => new MultipleSelectMenuItem<T>(v, toString)).ToList();
            self.WriteLines(displayArgs.Title, "", displayArgs.PleaseSelectText);

            var menuTop = Console.CursorTop;
            menuItems[0].IsFocused = true;

            ShowMenu();
            SelectItems();

            if (displayArgs.ClearWhenSelected)
            {
                self.ClearLastLines(Console.CursorTop - menuTop);
            }

            return menuItems.Where(i => i.IsSelected).Select(i => i.Value);

            #region local methods

            void ShowMenu()
            {
                var foreground = Console.ForegroundColor;
                foreach (var item in menuItems)
                {
                    Console.ForegroundColor = item.IsFocused ? displayArgs.FocusedItemColor : foreground;
                    self.WriteLine(item.ToString());
                }
                Console.ForegroundColor = foreground;
            }

            void SelectItems()
            {
                var key = ConsoleKey.A;
                var prevKey = key;
                var done = false;
                var index = 0;

                do
                {
                    key = Console.KeyAvailable ? Console.ReadKey(true).Key : ConsoleKey.A;
                    if (key == prevKey) continue;
                    KeyPressed();
                    self.ClearLastLines(Console.CursorTop - menuTop);
                    ShowMenu();
                    prevKey = key;
                } while (!done);

                void KeyPressed()
                {
                    switch (key)
                    {
                        case ConsoleKey.UpArrow:
                            if (index > 0)
                            {
                                menuItems[index].IsFocused = false;
                                index--;
                                menuItems[index].IsFocused = true;
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            if (index < menuItems.Count-1)
                            {
                                menuItems[index].IsFocused = false;
                                index++;
                                menuItems[index].IsFocused = true;
                            }
                            break;
                        case ConsoleKey.Spacebar:
                            menuItems[index].IsSelected = !menuItems[index].IsSelected;
                            break;
                        case ConsoleKey.Enter:
                            if (menuItems.Any(m => m.IsSelected))
                            {
                                done = true;
                            }
                            else
                            {
                                self.Write(displayArgs.RequiredErrorMessage);
                                key = Console.ReadKey(true).Key;
                                Console.WriteLine();
                                self.ClearLastLine();
                                KeyPressed();
                            }
                            break;
                    }
                }
            }

            void ValidateArguments()
            {
                if (self is null) throw new ArgumentNullException(nameof(self));

                if (toString is null) throw new ArgumentNullException(nameof(toString));

                if (items.Length < 2) throw new ArgumentException("Multiple choise menu must include at least two items", nameof(items));

                if (displayArgs.Title is null) throw new ArgumentNullException(nameof(displayArgs.Title));
                if (displayArgs.Title == "") throw new ArgumentException(nameof(displayArgs.Title) + " can't be empty.", nameof(displayArgs.Title));

                if (displayArgs.PleaseSelectText is null) throw new ArgumentNullException(nameof(displayArgs.PleaseSelectText));
                if (displayArgs.PleaseSelectText == "") throw new ArgumentException(nameof(displayArgs.PleaseSelectText) + " can't be empty.", nameof(displayArgs.PleaseSelectText));

                if (displayArgs.RequiredErrorMessage is null) throw new ArgumentNullException(nameof(displayArgs.RequiredErrorMessage));
                if (displayArgs.RequiredErrorMessage == "") throw new ArgumentException(nameof(displayArgs.RequiredErrorMessage) + " can't be empty.", nameof(displayArgs.RequiredErrorMessage));
            }

            #endregion local methods
        }

        private class MultipleSelectMenuItem<T>
        {
            private readonly Func<T, string> _toString;
            public MultipleSelectMenuItem(T value, Func<T, string> toString)
            {
                Value = value;
                _toString = toString;
            }

            public T Value { get; }

            public bool IsFocused { get; set; }

            public bool IsSelected { get; set; }

            public override string ToString()
            {
                var checkbox = IsSelected ? "[*] " : "[ ] ";
                return checkbox + _toString(Value);
            }

        }
    }
}
