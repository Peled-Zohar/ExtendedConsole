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
        ///<example>
        /// Create and run a menu with the specified title and items, that will disapear after the user selected an item.
        ///<code>
        /// var selection = exConsole.Menu(
        ///     "What to do next?",
        ///     true,
        ///     ("&lt;c f='Yellow'&gt;quit&lt;c&gt;", null),
        ///     ("Do this", () => DoThis(3)),
        ///     ("Do that", DoThat)
        /// );
        ///</code>
        ///</example>
        public static int Menu(this ExConsole self, string title, bool clearWhenSelected, params (string Title, Action Action)[] items)
        {
            var result = Menu(self, title, clearWhenSelected, items.Select(i => i.Title).ToArray());
            items[result].Action?.Invoke();
            return result;
        }

        /// <summary>
        /// Displays a menu to the user and invokes the action the user chooses.
        /// </summary>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="title">The title of the menu.</param>
        /// <param name="pleaseSelectText">The text to show below the menu.</param>
        /// <param name="invalidSelectionText">The text to show when the user enters an invalid value.</param>
        /// <param name="clearWhenSelected">A boolean value to determine 
        /// whether the menu should still be displayed after the user have chosen an option.</param>
        /// <param name="items">The items of the menu. 
        /// Each item contains a title and an action to perform, should the user choses this item.
        /// null can be passed in as the action if the item selection should perform no action.</param>
        /// <returns>An integer representing the user's choice.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="self"/>, <paramref name="title"/>, <paramref name="pleaseSelectText"/> or <paramref name="invalidSelectionText"/> are null.</exception>
        /// <exception cref="ArgumentException">Thrown when any of <paramref name="title"/>, <paramref name="pleaseSelectText"/> or <paramref name="invalidSelectionText"/> is empty or when <paramref name="items"/> are not supplied.</exception>
        /// <exception cref="System.Xml.XmlException">Thrown when <paramref name="title"/>, <paramref name="pleaseSelectText"/> or <paramref name="invalidSelectionText"/> are not properly formatted xml.</exception>
        ///<example>
        /// Create and run a menu with the specified items, "What to do next?" as a title,  
        /// "Please select an item from the menu." displayed below the items,
        /// and "Invalid value entered.", if the user enters an invalid value.
        /// This menu will still be visible after the user selects an item.
        ///<code>
        ///var selection = exConsole.Menu(
        ///    "What to do next?",
        ///    "Please select an item from the menu.",
        ///    "Invalid value entered.",
        ///    false,
        ///    ("&lt;c f='Yellow'&gt;quit&lt;c&gt;", null),
        ///    ("Do this", () => DoThis(3)),
        ///    ("Do that", DoThat)
        ///);
        ///</code>
        ///</example>
        public static int Menu(this ExConsole self, string title, string pleaseSelectText, string invalidSelectionText, bool clearWhenSelected, params (string Title, Action Action)[] items)
        {
            var result = Menu(self, title, pleaseSelectText, invalidSelectionText, clearWhenSelected, items.Select(i => i.Title).ToArray());
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
        ///<example>
        /// Create and run a menu with the specified title and items.
        /// This menu does not execute anything, it only returns the zero-based index of the item selected by the user
        ///<code>
        ///var selection = exConsole.Menu(
        ///    "What to do next?",
        ///    false,
        ///    "&lt;c f='Yellow'&gt;quit&lt;c&gt;",
        ///    "Do this",
        ///    "Do that"
        ///);
        /// // Do something with the selection here.
        ///</code>
        ///</example>
        public static int Menu(this ExConsole self, string title, bool clearWhenSelected, params string[] items)
        {
            return Menu(self, title, "Please select an item from the menu.", "Invalid value entered.", clearWhenSelected, items);
        }

        /// <summary>
        /// Displays a menu to the user and returns the index of the item the user chooses.
        /// </summary>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="title">The title of the menu.</param>
        /// <param name="pleaseSelectText">The text to show below the menu.</param>
        /// <param name="invalidSelectionText">The text to show when the user enters an invalid value.</param>
        /// <param name="clearWhenSelected">A boolean value to determine 
        /// whether the menu should still be displayed after the user have chosen an option.</param>
        /// <param name="items">The items of the menu.</param>
        /// <returns>An integer representing the user's choice.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="self"/>, <paramref name="title"/>, <paramref name="pleaseSelectText"/> or <paramref name="invalidSelectionText"/> are null.</exception>
        /// <exception cref="ArgumentException">Thrown when any of <paramref name="title"/>, <paramref name="pleaseSelectText"/> or <paramref name="invalidSelectionText"/> is empty or when <paramref name="items"/> are not supplied.</exception>
        /// <exception cref="System.Xml.XmlException">Thrown when <paramref name="title"/>, <paramref name="pleaseSelectText"/> or <paramref name="invalidSelectionText"/> are not properly formatted xml.</exception>
        ///<example>
        /// Create and run a menu with the specified items, "What to do next?" as a title,  
        /// "Please select an item from the menu." displayed below the items,
        /// and "Invalid value entered.", if the user enters an invalid value.
        /// This menu does not execute anything, it only returns the zero-based index of the item selected by the user
        ///<code>
        ///var selection = exConsole.Menu(
        ///    "What to do next?",
        ///    "Please select an item from the menu.",
        ///    "Invalid value entered.",
        ///    false,
        ///    "&lt;c f='Yellow'&gt;quit&lt;c&gt;",
        ///    "Do this",
        ///    "Do that"
        ///);
        /// // Do something with the selection here.
        ///</code>
        ///</example>
        public static int Menu(this ExConsole self, string title, string pleaseSelectText, string invalidSelectionText, bool clearWhenSelected, params string[] items)
        {
            return ShowMenu(self, title, pleaseSelectText, invalidSelectionText, clearWhenSelected, items);
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
        /// <example>
        /// Display the names of the members of the ConsoleColor enum,
        /// asking the user to choose a color from the list. 
        /// the color variable is of type ConsoleColor.
        /// <code>
        /// var color = exConsole.ChooseFromEnum&lt;ConsoleColor&gt;(
        ///    "Choose a foreground color",
        ///    true
        /// );
        /// </code>
        /// </example>
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
        /// <param name="pleaseSelectText">The text to show below the menu.</param>
        /// <param name="invalidSelectionText">The text to show when the user enters an invalid value.</param>
        /// <param name="clearWhenSelected">A boolean value to determine 
        /// whether the menu should still be displayed after the user have chosen an option.</param>
        /// <returns>The member of the enum the user selected.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="self"/>, <paramref name="title"/>, <paramref name="pleaseSelectText"/> or <paramref name="invalidSelectionText"/> are null.</exception>
        /// <exception cref="ArgumentException">Thrown when any of <paramref name="title"/>, <paramref name="pleaseSelectText"/> or <paramref name="invalidSelectionText"/> is empty or when <paramref name="items"/> are not supplied.</exception>
        /// <exception cref="System.Xml.XmlException">Thrown when <paramref name="title"/>, <paramref name="pleaseSelectText"/> or <paramref name="invalidSelectionText"/> are not properly formatted xml.</exception>
        /// <example>
        /// Display the names of the members of the ConsoleColor enum,
        /// asking the user to choose a color from the list. 
        /// the color variable is of type ConsoleColor.
        /// <code>
        /// var color = exConsole.ChooseFromEnum&lt;ConsoleColor&gt;(
        ///    "Choose a foreground color",
        ///    "Please choose a color.",
        ///    "Please choose from the above list.",
        ///    true
        /// );
        /// </code>
        /// </example>
        public static T ChooseFromEnum<T>(this ExConsole self, string title, string pleaseSelectText, string invalidSelectionText, bool clearWhenSelected) where T : Enum
        {
            var names = Enum.GetNames(typeof(T));
            var result = Menu(self, title, pleaseSelectText, invalidSelectionText, clearWhenSelected, names);
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
        /// <example>
        /// Display the names of the members of the ConsoleColor enum,
        /// asking the user to choose a color from the list, or "none".
        /// the color variable is of type Nullable&lt;ConsoleColor&gt; and will be null if the user choose "none".
        /// <code>
        /// var color = exConsole.ChooseFromEnum&lt;ConsoleColor&gt;(
        ///    "Choose a foreground color, or &lt;c f='red&gt;none&lt;/c&gt; to keep current color.",
        ///    "&lt;c f='red'&gt;none&lt;/c&gt;",
        ///    true
        /// );
        /// </code>
        /// </example>
        public static T? ChooseFromEnum<T>(this ExConsole self, string title, string quitText, bool clearWhenSelected) where T : struct, Enum
        {
            var names = Enum.GetNames(typeof(T)).ToList();
            names.Insert(0, quitText);
            var result = Menu(self, title, clearWhenSelected, names.ToArray());
            if (result == 0) return null;

            return (T?)Enum.Parse(typeof(T), names[result]);
        }

        /// <summary>
        /// Displays enum members as a menu for the user to choose from.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="title">The title of the menu.</param>
        /// <param name="pleaseSelectText">The text to show below the menu.</param>
        /// <param name="invalidSelectionText">The text to show when the user enters an invalid value.</param>
        /// <param name="quitText">The title of a menu item that isn't a member of the enum, to enable the user to return without choosing.</param>
        /// <param name="clearWhenSelected">A boolean value to determine 
        /// whether the menu should still be displayed after the user have chosen an option.</param>
        /// <returns>The member of the enum the user selected.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="self"/>, <paramref name="title"/>, <paramref name="pleaseSelectText"/>, <paramref name="invalidSelectionText"/> or <paramref name="quitText"/> are null.</exception>
        /// <exception cref="ArgumentException">Thrown when any of <paramref name="title"/>, <paramref name="pleaseSelectText"/>, <paramref name="invalidSelectionText"/> or <paramref name="quitText"/> is empty or when <paramref name="items"/> are not supplied.</exception>
        /// <exception cref="System.Xml.XmlException">Thrown when <paramref name="title"/>, <paramref name="pleaseSelectText"/>, <paramref name="invalidSelectionText"/> or <paramref name="quitText"/> are not properly formatted xml.</exception>
        /// <example>
        /// Display the names of the members of the ConsoleColor enum,
        /// asking the user to choose a color from the list, or "none".
        /// the color variable is of type Nullable&lt;ConsoleColor&gt; and will be null if the user choose "none".
        /// <code>
        /// var color = exConsole.ChooseFromEnum&lt;ConsoleColor&gt;(
        ///    "Choose a foreground color",
        ///    "Please choose a color or &lt;c f='red&gt;none&lt;/c&gt; to keep current color.",
        ///    "Please choose from the above list.",
        ///    "&lt;c f='red'&gt;none&lt;/c&gt;",
        ///    true
        /// );
        /// </code>
        /// </example>
        public static T? ChooseFromEnum<T>(this ExConsole self, string title, string pleaseSelectText, string invalidSelectionText, string quitText, bool clearWhenSelected) where T : struct, Enum
        {
            if (quitText is null) throw new ArgumentNullException(nameof(quitText));
            if (quitText == "") throw new ArgumentException(nameof(quitText) + " can't be empty.", nameof(quitText));

            var names = Enum.GetNames(typeof(T)).ToList();
            names.Insert(0, quitText);
            var result = Menu(self, title, pleaseSelectText, invalidSelectionText, clearWhenSelected, names.ToArray());
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
        // <exception cref="ArgumentNullException">Thrown when <paramref name="self"/>, <paramref name="title"/>, <paramref name="pleaseSelectText"/> or <paramref name="invalidSelectionText"/> are null.</exception>
        // <exception cref="ArgumentException">Thrown when any of <paramref name="title"/>, <paramref name="pleaseSelectText"/> or <paramref name="invalidSelectionText"/> is empty or when <paramref name="items"/> are not supplied.</exception>
        // <exception cref="System.Xml.XmlException">Thrown when <paramref name="title"/>, <paramref name="pleaseSelectText"/> or <paramref name="invalidSelectionText"/> are not properly formatted xml.</exception>
        private static int ShowMenu(this ExConsole self, string title, string pleaseSelectText, string invalidSelectionText, bool clearWhenSelected, params string[] items)
        {
            if (self is null) throw new ArgumentNullException(nameof(self));

            if (title is null) throw new ArgumentNullException(nameof(title));
            if (title == "") throw new ArgumentException(nameof(title) + " can't be empty.", nameof(title));

            if (pleaseSelectText is null) throw new ArgumentNullException(nameof(pleaseSelectText));
            if (pleaseSelectText == "") throw new ArgumentException(nameof(pleaseSelectText) + " can't be empty.", nameof(pleaseSelectText));

            if (invalidSelectionText is null) throw new ArgumentNullException(nameof(invalidSelectionText));
            if (invalidSelectionText == "") throw new ArgumentException(nameof(invalidSelectionText) + " can't be empty.", nameof(invalidSelectionText));

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
