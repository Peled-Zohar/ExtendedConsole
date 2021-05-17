using System;
using System.Collections.Generic;
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
        /// <param name="displayArgs">An instance of the <see cref="MenuDisplayArgs"/> class holding the display configuration of the menu.</param>
        /// <param name="items">The items of the menu. 
        /// Each item contains a title and an action to perform, should the user choses this item.
        /// null can be passed in as the action if the item selection should perform no action.</param>
        /// <returns>An integer representing the user's choice.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="self"/> or <paramref name="displayArgs"/> or any of it's properties are null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="displayArgs"/> is empty or when <paramref name="items"/> are not supplied.</exception>
        /// <exception cref="System.Xml.XmlException">Thrown when any of the text properties of <paramref name="displayArgs"/> isn't properly formatted xml.</exception>
        ///<example>
        /// Create and run a menu with the specified title and items, that will disapear after the user selected an item.
        ///<code>
        /// var selection = exConsole.Menu(
        ///     new MenuDisplayArgs("What to do next?"),
        ///     ("&lt;c f='Yellow'&gt;quit&lt;c&gt;", null),
        ///     ("Do this", () => DoThis(3)),
        ///     ("Do that", DoThat)
        /// );
        ///</code>
        ///</example>
        public static int Menu(this ExConsole self, MenuDisplayArgs displayArgs, params (string Title, Action Action)[] items)
        {
            var result = Menu(self, displayArgs, items.Select(i => i.Title).ToArray());
            items[result].Action?.Invoke();
            return result;
        }

        /// <summary>
        /// Displays a menu to the user and invokes the action the user chooses.
        /// </summary>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="displayArgs">An instance of the <see cref="MenuDisplayArgs"/> class holding the display configuration of the menu.</param>
        /// <param name="items">The items of the menu.</param>
        /// <returns>An integer representing the user's choice.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="self"/> or <paramref name="displayArgs"/> or any of it's properties are null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="displayArgs"/> is empty or when <paramref name="items"/> are not supplied.</exception>
        /// <exception cref="System.Xml.XmlException">Thrown when any of the text properties of <paramref name="displayArgs"/> isn't properly formatted xml.</exception>
        ///<example>
        /// Create and run a menu with the specified items, "What to do next?" as a title,  
        /// "Please enter the number of your selection." displayed below the items,
        /// and "Invalid number entered. Please try again.", if the user enters an invalid value.
        /// This menu does not execute anything, it only returns the zero-based index of the item selected by the user
        ///<code>
        /// var selection = exConsole.Menu(
        ///    new MenuDisplayArgs("What to do next?", "Please enter the number of your selection.", invalidSelectionErrorMessage:"Invalid number entered. Please try again." )
        ///    "&lt;c f='Yellow'&gt;quit&lt;c&gt;",
        ///    "Do this",
        ///    "Do that"
        /// );
        ///</code>
        ///</example>
        public static int Menu(this ExConsole self, MenuDisplayArgs displayArgs, params string[] items)
        {
            ValidateArguments();

            var cursorTop = Console.CursorTop;
            self.WriteLine(displayArgs.Title);
            for (var i = 0; i < items.Length; i++)
            {
                self.WriteLine($"{i}. {items[i]}");
            }
            var result = ExConsoleRead.ReadInt(self, displayArgs.PleaseSelectText, displayArgs.InvalidSelectionErrorMessage, i => i > -1 && i < items.Length);
            if (displayArgs.ClearWhenSelected)
            {
                self.ClearLastLines(Console.CursorTop - cursorTop);
            }
            return result;

            void ValidateArguments()
            {
                if (self is null) throw new ArgumentNullException(nameof(self));

                if (displayArgs.Title is null) throw new ArgumentNullException(nameof(displayArgs.Title));
                if (displayArgs.Title == "") throw new ArgumentException(nameof(displayArgs.Title) + " can't be empty.", nameof(displayArgs.Title));

                if (displayArgs.PleaseSelectText is null) throw new ArgumentNullException(nameof(displayArgs.PleaseSelectText));
                if (displayArgs.PleaseSelectText == "") throw new ArgumentException(nameof(displayArgs.PleaseSelectText) + " can't be empty.", nameof(displayArgs.PleaseSelectText));

                if (displayArgs.InvalidSelectionErrorMessage is null) throw new ArgumentNullException(nameof(displayArgs.InvalidSelectionErrorMessage));
                if (displayArgs.InvalidSelectionErrorMessage == "") throw new ArgumentException(nameof(displayArgs.InvalidSelectionErrorMessage) + " can't be empty.", nameof(displayArgs.InvalidSelectionErrorMessage));

                if (items.Length == 0) throw new ArgumentException("A menu must include at least one item", nameof(items));

            }
        }

        /// <summary>
        /// Note: 
        /// <para>
        /// This method is deprecated and will be removed in future versions.
        /// Use the Menu overload that takes an instance of MenuDisplayArgs as an argument instead.
        /// </para>
        /// <para>
        /// Displays a menu to the user and invokes the action the user chooses.
        /// </para>
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
        [Obsolete("this method is deprecated and will be removed in future versions.\r\nUse the Menu overload that takes an instance of MenuDisplayArgs as an argument instead.")]
        public static int Menu(this ExConsole self, string title, bool clearWhenSelected, params (string Title, Action Action)[] items)
        {
            var result = Menu(self, title, clearWhenSelected, items.Select(i => i.Title).ToArray());
            items[result].Action?.Invoke();
            return result;
        }

        /// <summary>
        /// Note: 
        /// <para>
        /// This method is deprecated and will be removed in future versions.
        /// Use the Menu overload that takes an instance of MenuDisplayArgs as an argument instead.
        /// </para>
        /// <para>
        /// Displays a menu to the user and invokes the action the user chooses.
        /// </para>
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
        [Obsolete("this method is deprecated and will be removed in future versions.\r\nUse the Menu overload that takes an instance of MenuDisplayArgs as an argument instead.")]
        public static int Menu(this ExConsole self, string title, string pleaseSelectText, string invalidSelectionText, bool clearWhenSelected, params (string Title, Action Action)[] items)
        {
            var result = Menu(self, title, pleaseSelectText, invalidSelectionText, clearWhenSelected, items.Select(i => i.Title).ToArray());
            items[result].Action?.Invoke();
            return result;
        }

        /// <summary>
        /// Note: 
        /// <para>
        /// This method is deprecated and will be removed in future versions.
        /// Use the Menu overload that takes an instance of MenuDisplayArgs as an argument instead.
        /// </para>
        /// <para>
        /// Displays a menu to the user and returns the index of the item the user chooses.
        /// </para>
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
        [Obsolete("this method is deprecated and will be removed in future versions.\r\nUse the Menu overload that takes an instance of MenuDisplayArgs as an argument instead.")]
        public static int Menu(this ExConsole self, string title, bool clearWhenSelected, params string[] items)
        {
            return Menu(self, title, "Please select an item from the menu.", "Invalid value entered.", clearWhenSelected, items);
        }

        /// <summary>
        /// Note: 
        /// <para>
        /// This method is deprecated and will be removed in future versions.
        /// Use the Menu overload that takes an instance of MenuDisplayArgs as an argument instead.
        /// </para>
        /// <para>
        /// Displays a menu to the user and returns the index of the item the user chooses.
        /// </para>
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
        [Obsolete("this method is deprecated and will be removed in future versions.\r\nUse the Menu overload that takes an instance of MenuDisplayArgs as an argument instead.")]
        public static int Menu(this ExConsole self, string title, string pleaseSelectText, string invalidSelectionText, bool clearWhenSelected, params string[] items)
        {
            return Menu(self, new MenuDisplayArgs(title, pleaseSelectText, clearWhenSelected, invalidSelectionText), items);
        }
    }
}
