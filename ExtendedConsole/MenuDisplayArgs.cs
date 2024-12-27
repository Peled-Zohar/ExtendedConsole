using System;

namespace ExtendedConsole
{
    /// <summary>
    /// Stores display arguments for menu.
    /// </summary>
    public class MenuDisplayArgs
    {
        /// <summary>
        /// Creates a new instance of the <see cref="MenuDisplayArgs"/> class with the specified parmeters.
        /// </summary>
        /// <param name="title">The title of the menu.</param>
        /// <param name="pleaseSelectText">The text to be displayed below the menu.</param>
        /// <param name="clearWhenSelected">A boolean value to determine 
        /// whether the menu should still be displayed after the user have chosen an option.</param>
        /// <param name="invalidSelectionErrorMessage">The error message to show if the user entered an invalid selection.</param>
        [Obsolete("This constructor is deprecated and will be removed in future versions.")]
        public MenuDisplayArgs(
            string title,
            string pleaseSelectText = "Please select an item from the menu.",
            bool clearWhenSelected = true,
            string invalidSelectionErrorMessage = "Invalid value entered."
        ) : this(title, pleaseSelectText, clearWhenSelected, invalidSelectionErrorMessage, false) { }

        /// <summary>
        /// Creates a new instance of the <see cref="MenuDisplayArgs"/> class with the specified parmeters.
        /// </summary>
        /// <param name="title">The title of the menu.</param>
        /// <param name="pleaseSelectText">The text to be displayed below the menu.</param>
        /// <param name="clearWhenSelected">A boolean value to determine 
        /// whether the menu should still be displayed after the user have chosen an option.</param>
        /// <param name="invalidSelectionErrorMessage">The error message to show if the user entered an invalid selection.</param>
        /// <param name="showSelectedItem">A boolean value to determine whether to show the title of the item selected by the user after selection</param>
        public MenuDisplayArgs(
            string title,
            string pleaseSelectText = "Please select an item from the menu.",
            bool clearWhenSelected = true,
            string invalidSelectionErrorMessage = "Invalid value entered.",
            bool showSelectedItem = false
        )
        {
            Title = title;
            PleaseSelectText = pleaseSelectText;
            ClearWhenSelected = clearWhenSelected;
            InvalidSelectionErrorMessage = invalidSelectionErrorMessage;
            ShowSelectedItem = showSelectedItem;
        }

        /// <summary>
        /// Gets the title of the multiple select menu.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets the text to be displayed between the title and the menu, instructing the user how to use the menu.
        /// </summary>
        public string PleaseSelectText { get; }

        /// <summary>
        /// Gets a boolean value to determine 
        /// whether the menu should still be displayed after the user have chosen an option.
        /// </summary>
        public bool ClearWhenSelected { get; }

        /// <summary>
        /// Gets the error message to display if the user  enters an invalid value.
        /// </summary>
        public string InvalidSelectionErrorMessage { get; }

        /// <summary>
        /// Gets a boolean value to determine whether the selected item title should be displayed.
        /// </summary>
        public bool ShowSelectedItem { get; }
    }
}
