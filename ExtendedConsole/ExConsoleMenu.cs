﻿using System;
using System.Linq;

namespace ExtendedConsole
{

    /// <summary>
    /// Provides extension methods for ExConsole to support simple menus.
    /// </summary>
    public static class ExConsoleMenu
    {

        /// <summary>
        /// Displays a menu to the user and returns the index of the item the user chooses.
        /// </summary>
        /// <param name="title">The title of the menu.</param>
        /// <param name="clearWhenSelected">A boolean value to determine 
        /// whether the menu should still be displayed after the user have chosen an option.</param>
        /// <param name="items">The items of the menu.</param>
        /// <returns>An integer representing the user's choice.</returns>
        public static int Menu(this ExConsole self, string title, bool clearWhenSelected, params string[] items)
        {
            return Math.Max(self.ShowMenu(title, clearWhenSelected, items), 0);
        }

        /// <summary>
        /// Displays a menu to the user and invokes the action the user chooses.
        /// </summary>
        /// <param name="title">The title of the menu.</param>
        /// <param name="items">The items of the menu. 
        /// Each item contains a title and an action to perform, should the user choses this item.</param>
        /// <returns>An integer representing the user's choice.</returns>
        public static int Menu(this ExConsole self, string title, bool clearWhenSelected, params (string Title, Action Action)[] items)
        {
            var result = self.ShowMenu(title, clearWhenSelected, items.Select(i => i.Title).ToArray());
            if (result >= 0)
            {
                items[result].Action.Invoke();
            }
            return result;
        }

        #region private methods

        /// <summary>
        /// Displays a menu to the user and returns the index of the item the user chooses.
        /// </summary>
        /// <param name="title">The title of the menu.</param>
        /// <param name="clearWhenSelected">A boolean value to determine 
        /// whether the menu should still be displayed after the user have chosen an option.</param>
        /// <param name="itemTitles">The items of the menu.</param>
        /// <returns>An integer representing the user's choice.</returns>
        private static int ShowMenu(this ExConsole self, string title, bool clearWhenSelected, params string[] itemTitles)
        {
            if (itemTitles.Length == 0)
            {
                return -1;
            }
            var cursorTop = Console.CursorTop;
            self.WriteLine(title);
            for (var i = 0; i < itemTitles.Length; i++)
            {
                self.WriteLine($"{i}. {itemTitles[i]}");
            }
            var result = self.ReadInt("Please select an item from the menu.", "Invalid value entered.", i => i > -1 && i < itemTitles.Length);
            if (clearWhenSelected)
            {
                self.ClearLastLines(Console.CursorTop - cursorTop);
            }
            return result;
        }

        #endregion private methods
    }
}
