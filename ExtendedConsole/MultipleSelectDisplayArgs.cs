using System;

namespace ExtendedConsole
{
    /// <summary>
    /// Stores display arguments for multiple select menu.
    /// </summary>
    public class MultipleSelectDisplayArgs
    {
        /// <summary>
        /// Creates a new instance of the <see cref="MultipleSelectDisplayArgs"/> class with the specified parmeters.
        /// </summary>
        /// <param name="title">The title of the multiple select menu.</param>
        /// <param name="pleaseSelectText">The text to be displayed between the title and the menu, instructing the user how to use the menu.</param>
        /// <param name="clearWhenSelected">A boolean value to determine 
        /// whether the menu should still be displayed after the user have completed his selection.</param>
        /// <param name="focusedItemColor">The forecolor to use for the currently focused item of the menu.</param>
        /// <param name="requiredErrorMessage">The error message to display if the user did not select anything from the menu.</param>
        /// <param name="selectedItemColor">The forecolor to use for the currently selected item(s) of the menu.
        /// Wnen left null, selected items will have the same color as other items.
        /// </param>
        public MultipleSelectDisplayArgs(
            string title, 
            string pleaseSelectText = "Use arrow keys (up/down) to navigate, spece bar to select, and enter to submit selection.", 
            bool clearWhenSelected = true, 
            ConsoleColor focusedItemColor = ConsoleColor.Magenta, 
            string requiredErrorMessage = "You must select at least one item.",
            ConsoleColor? selectedItemColor = null
        )
        {
            Title = title;
            PleaseSelectText = pleaseSelectText;
            ClearWhenSelected = clearWhenSelected;
            FocusedItemColor = focusedItemColor;
            RequiredErrorMessage = requiredErrorMessage;
            SelectedItemColor = selectedItemColor;
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
        /// whether the menu should still be displayed after the user have completed his selection.
        /// </summary>
        public bool ClearWhenSelected { get; }

        /// <summary>
        /// Gets the forecolor to use for the currently focused item of the menu.
        /// </summary>
        public ConsoleColor FocusedItemColor { get; }

        /// <summary>
        /// Gets the error message to display if the user did not select anything from the menu.
        /// </summary>
        public string RequiredErrorMessage { get; }

        /// <summary>
        /// Gets the forecolor to use for the currently selected item(s) of the menu.
        /// </summary>
        public ConsoleColor? SelectedItemColor { get; }
    }
}
