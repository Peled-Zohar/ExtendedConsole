using System;
using System.Linq;

namespace ExtendedConsole
{

    /// <summary>
    /// Provides extension methods for ExConsole to support simple menus.
    /// </summary>
    public static class ExConsoleMenu
    {
        /// <summary>
        /// Displays a menu to the user and invokes the action the user chooses.
        /// </summary>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="title">The title of the menu.</param>
        /// <param name="clearWhenSelected">A boolean value to determine 
        /// whether the menu should still be displayed after the user have chosen an option.</param>
        /// <param name="items">The items of the menu. 
        /// Each item contains a title and an action to perform, should the user choses this item.
        /// null can be passed in as the action if the item selection should perform no action.</param>
        /// <returns>An integer representing the user's choice.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="self"/> or <paramref name="title"/> are null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="title"/> is empty or when <paramref name="items"/> are not supplied.</exception>
        /// <exception cref="System.Xml.XmlException">Thrown when <paramref name="title"/> isn't properly formatted xml.</exception>
        public static int Menu(this ExConsole self, string title, bool clearWhenSelected, params (string Title, Action Action)[] items)
        {
            var result = Menu(self, title, clearWhenSelected, items.Select(i => i.Title).ToArray());
            items[result].Action?.Invoke();
            return result;
        }

        /// <summary>
        /// Displays a menu to the user and returns the index of the item the user chooses.
        /// </summary>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="title">The title of the menu.</param>
        /// <param name="clearWhenSelected">A boolean value to determine 
        /// whether the menu should still be displayed after the user have chosen an option.</param>
        /// <param name="items">The items of the menu.</param>
        /// <returns>An integer representing the user's choice.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="self"/> or <paramref name="title"/> are null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="title"/> is empty or when <paramref name="items"/> are not supplied.</exception>
        /// <exception cref="System.Xml.XmlException">Thrown when <paramref name="title"/> isn't properly formatted xml.</exception>
        public static int Menu(this ExConsole self, string title, bool clearWhenSelected, params string[] items)
        {
            return ShowMenu(self, title, "Please select an item from the menu.", "Invalid value entered.", clearWhenSelected, items);
        }

        /// <summary>
        /// Displays enum members as a menu for the user to choose from.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="title">The title of the menu.</param>
        /// <param name="clearWhenSelected">A boolean value to determine 
        /// whether the menu should still be displayed after the user have chosen an option.</param>
        /// <returns>The member of the enum the user selected.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="self"/> or <paramref name="title"/> are null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="title"/> is empty.</exception>
        /// <exception cref="System.Xml.XmlException">Thrown when <paramref name="title"/> isn't properly formatted xml.</exception>
        public static T ChooseFromEnum<T>(this ExConsole self, string title, bool clearWhenSelected) where T : Enum
        {
            var names = Enum.GetNames(typeof(T));
            var result = Menu(self, title, clearWhenSelected, names);
            return (T)Enum.Parse(typeof(T), names[result]);
        }

        /// <summary>
        /// Displays enum members as a menu for the user to choose from.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="title">The title of the menu.</param>
        /// <param name="quitText">The title of a menu item that isn't a member of the enum, to enable the user to return without choosing.</param>
        /// <param name="clearWhenSelected">A boolean value to determine 
        /// whether the menu should still be displayed after the user have chosen an option.</param>
        /// <returns>The member of the enum the user selected.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="self"/> or <paramref name="title"/> are null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="title"/> is empty.</exception>
        /// <exception cref="System.Xml.XmlException">Thrown when <paramref name="title"/> isn't properly formatted xml.</exception>
        public static T? ChooseFromEnum<T>(this ExConsole self, string title, string quitText, bool clearWhenSelected) where T : struct, Enum
        {
            var names = Enum.GetNames(typeof(T)).ToList();
            names.Insert(0, quitText);
            var result = Menu(self, title, clearWhenSelected, names.ToArray());
            if (result == 0) return null;

            return (T?)Enum.Parse(typeof(T), names[result]);
        }

        #region private methods

        // <summary>
        // Displays a menu to the user and returns the index of the item the user chooses.
        // </summary>
        // <param name="self">The current instance of ExConsole.</param>
        // <param name="title">The title of the menu.</param>
        // <param name="pleaseSelectText">The text to show below the menu.</param>
        // <param name="invalidSelectionText">The text to show if the user entered an invalid selection.</param>
        // <param name="clearWhenSelected">A boolean value to determine 
        // whether the menu should still be displayed after the user have chosen an option.</param>
        // <param name="itemTitles">The items of the menu.</param>
        // <returns>An integer representing the user's choice.</returns>
        // <exception cref="ArgumentNullException">Thrown when <paramref name="self"/> or <paramref name="title"/> are null.</exception>
        // <exception cref="ArgumentException">Thrown when <paramref name="title"/> is empty or when <paramref name="items"/> are not supplied.</exception>
        // <exception cref="System.Xml.XmlException">Thrown when <paramref name="title"/> isn't properly formatted xml.</exception>
        private static int ShowMenu(this ExConsole self, string title, string pleaseSelectText, string invalidSelectionText, bool clearWhenSelected, params string[] items)
        {
            if (self is null) throw new ArgumentNullException(nameof(self));
            if(title is null) throw new ArgumentNullException(nameof(title));
            if (title == "") throw new ArgumentException(nameof(title) + " can't be empty.", nameof(title));

            if (items.Length == 0) throw new ArgumentException("A menu must include at least one item", nameof(items));

            var cursorTop = Console.CursorTop;
            self.WriteLine(title);
            for (var i = 0; i < items.Length; i++)
            {
                self.WriteLine($"{i}. {items[i]}");
            }
            var result = ExConsoleRead.ReadInt(self, pleaseSelectText, invalidSelectionText, i => i > -1 && i < items.Length);
            if (clearWhenSelected)
            {
                self.ClearLastLines(Console.CursorTop - cursorTop);
            }
            return result;
        }

        #endregion private methods
    }
}
