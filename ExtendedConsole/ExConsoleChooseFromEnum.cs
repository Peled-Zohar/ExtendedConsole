using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtendedConsole
{
    /// <summary>
    /// Provides extension methods for ExConsole to support simple menus based on enum values.
    /// </summary>
    public static class ExConsoleChooseFromEnum
    {
        /// <summary>
        /// Displays enum members as a menu for the user to choose from.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="quitText">The title of a menu item that isn't a member of the enum, to enable the user to return without choosing.</param>
        /// <param name="displayArgs">An instance of the <see cref="MenuDisplayArgs"/> class holding the display configuration of the enum based menu.</param>
        /// <returns>The member of the enum the user selected, or null if the user selected the "quit" menu item.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="self"/>, <paramref name="displayArgs"/> or any of its properties, or <paramref name="quitText"/> are null.</exception>
        /// <exception cref="ArgumentException">Thrown when any of the string properties of <paramref name="displayArgs"/> or <paramref name="quitText"/> are empty.</exception>
        /// <exception cref="System.Xml.XmlException">Thrown when any of the string properties of <paramref name="displayArgs"/> or <paramref name="quitText"/> are not properly formatted xml.</exception>
        /// <example>
        /// Display the names of the members of the ConsoleColor enum,
        /// asking the user to choose a color from the list, or "none".
        /// the color variable is of type Nullable&lt;ConsoleColor&gt; and will be null if the user choose "none".
        /// <code>
        /// var color = exConsole.ChooseFromEnum&lt;ConsoleColor&gt;(
        ///    "&lt;c f='red'&gt;none&lt;/c&gt;",
        ///    new MenuDisplayArgs(
        ///        "Choose a foreground color", 
        ///        "Please choose a color or &lt;c f='red&gt;none&lt;/c&gt; to keep current color.", 
        ///        invalidSelectionErrorMessage:"Please choose from the above list.")
        /// );
        /// </code>
        /// </example>
        public static T? ChooseFromEnum<T>(this ExConsole self, string quitText, MenuDisplayArgs displayArgs) where T : struct, Enum
        {
            if (quitText is null) throw new ArgumentNullException(nameof(quitText));
            if (quitText == "") throw new ArgumentException(nameof(quitText) + " can't be empty.", nameof(quitText));

            var names = Enum.GetNames(typeof(T)).ToList();
            names.Insert(0, quitText);
            var result = ExConsoleMenu.Menu(self, displayArgs, names.ToArray());
            if (result == 0) return null;

            return (T?)Enum.Parse(typeof(T), names[result]);
        }

        /// <summary>
        /// Displays enum members as a menu for the user to choose from.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="displayArgs">An instance of the <see cref="MenuDisplayArgs"/> class holding the display configuration of the enum based menu.</param>
        /// <returns>The member of the enum the user selected.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="self"/>, <paramref name="displayArgs"/> or any of its properties are null.</exception>
        /// <exception cref="ArgumentException">Thrown when any of the string properties of <paramref name="displayArgs"/> are empty.</exception>
        /// <exception cref="System.Xml.XmlException">Thrown when any of the string properties of <paramref name="displayArgs"/> are not properly formatted xml.</exception>
        /// <example>
        /// Display the names of the members of the ConsoleColor enum,
        /// asking the user to choose a color from the list. 
        /// the color variable is of type ConsoleColor.
        /// <code>
        /// var color = exConsole.ChooseFromEnum&lt;ConsoleColor&gt;(
        ///    new MenuDisplayArgs(
        ///        "Choose a foreground color",
        ///        "Please choose a color.",
        ///        false, 
        ///        "Please choose from the above list.")
        /// );
        /// </code>
        /// </example>
        public static T ChooseFromEnum<T>(this ExConsole self, MenuDisplayArgs displayArgs) where T : Enum
        {
            var names = Enum.GetNames(typeof(T));
            var result = ExConsoleMenu.Menu(self, displayArgs, names);
            return (T)Enum.Parse(typeof(T), names[result]);
        }
    }
}
