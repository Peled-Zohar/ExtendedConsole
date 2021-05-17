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
        public MenuDisplayArgs (
            string title, 
            string pleaseSelectText = "Please select an item from the menu.",
            bool clearWhenSelected = true, 
            string invalidSelectionErrorMessage = "Invalid value entered."
        )
        {
            Title = title;
            PleaseSelectText = pleaseSelectText;
            ClearWhenSelected = clearWhenSelected;
            InvalidSelectionErrorMessage = invalidSelectionErrorMessage;
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
    }
}
