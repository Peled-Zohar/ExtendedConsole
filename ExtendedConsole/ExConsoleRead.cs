using System;
using System.Globalization;

namespace ExtendedConsole
{
    /// <summary>
    /// Provides extension methods for ExConsole for easy reading and parsing inputs from the user.
    /// </summary>
    public static class ExConsoleRead
    {
        #region general

        /// <summary>
        /// Pause the console application until the user press a key.
        /// </summary>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="self"/> is null.</exception>
        public static void Pause(this ExConsole self)
        {
            Pause(self, "Press any key to continue.");
        }

        /// <summary>
        /// Pause the console application until the user press a key.
        /// </summary>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="title">The title to show the user before asking for input.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="self"/> or <paramref name="title"/> are null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="title"/> is empty.</exception>
        /// <exception cref="System.Xml.XmlException">Thrown when <paramref name="title"/> isn't properly formatted xml.</exception>
        public static void Pause(this ExConsole self, string title)
        {
            if (self is null) throw new ArgumentNullException(nameof(self));
            if (title is null) throw new ArgumentNullException(nameof(title));
            if (title == "") throw new ArgumentException(nameof(title) + " can't be empty.", nameof(title));

            self.WriteLine(title);
            Console.ReadKey();
        }

        /// <summary>
        /// Reads an input line from the user and converts it to T.
        /// Repeats until conversion succeeds (even if the user entered ^Z).
        /// Use with caution - 
        /// The user can't exit this method without either providing a convertible value or exiting the entire program!
        /// </summary>
        /// <typeparam name="T">The target type of the conversion.</typeparam>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="title">The title to show the user before asking for input.</param>
        /// <param name="errorMessage">The error message to show the user if the conversion failed.</param>
        /// <param name="converter">A function that takes in a string and returns a value tuple of bool success and T value.</param>
        /// <returns>The T value converted from the input string.</returns>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="title"/> or <paramref name="errorMessage"/> are empty.</exception>        
        /// <exception cref="System.Xml.XmlException">Thrown when <paramref name="title"/> or <paramref name="errorMessage"/> aren't properly formatted xml.</exception>
        public static T ReadUntilConverted<T>(this ExConsole self, string title, string errorMessage, Func<string, (bool Success, T Value)> converter)
        {
            var (Success, Value) = (false, default(T));
            do
            {
                (Success, Value) = Read<T>(
                    self,
                    title,
                    errorMessage,
                    converter
                );
            } while (!Success);
            return Value;
        }

        /// <summary>
        /// Reads an input line from the user and converts it to a nullable T.
        /// </summary>
        /// <typeparam name="T">The underlying type of the nullable to convert to.</typeparam>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="title">The title to show the user before asking for input.</param>
        /// <param name="errorMessage">The error message to show the user if the conversion failed.</param>
        /// <param name="converter">A function that takes in a string and returns a value tuple of bool success and T value.</param>
        /// <returns>An instance of T? that has a value unless the user entered ^Z.</returns>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="title"/> or <paramref name="errorMessage"/> are empty.</exception>        
        /// <exception cref="System.Xml.XmlException">Thrown when <paramref name="title"/> or <paramref name="errorMessage"/> aren't properly formatted xml.</exception>
        public static T? ReadStruct<T>(this ExConsole self, string title, string errorMessage, Func<string, (bool Success, T Value)> converter) where T : struct
        {
            var (Success, Value) = Read(
                self,
                title,
                errorMessage,
                converter
            );
            return Success ? Value : (T?)null;
        }

        /// <summary>
        /// Reads an input line from the user and converts it to T.
        /// </summary>
        /// <typeparam name="T">The target type of the conversion.</typeparam>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="title">The title to show the user before asking for input.</param>
        /// <param name="errorMessage">The error message to show the user if the conversion failed.</param>
        /// <param name="converter">A function that takes in a string and returns a value tuple of bool success and T value.</param>
        /// <returns>The T value converted from the input string, or null if the user entered ^Z.</returns>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="title"/> or <paramref name="errorMessage"/> are empty.</exception>        
        /// <exception cref="System.Xml.XmlException">Thrown when <paramref name="title"/> or <paramref name="errorMessage"/> aren't properly formatted xml.</exception>
        public static T ReadClass<T>(this ExConsole self, string title, string errorMessage, Func<string, (bool Success, T Value)> converter) where T : class
        {
            var (Success, Value) = Read(
                self,
                title,
                errorMessage,
                converter
            );
            return Success ? Value : null;
        }

        // <summary>
        // Reads an input line from the user and attempt to convert it to T.
        // Repeats until conversion succeeds or the user enters ^Z.
        // </summary>
        // <typeparam name="T">The target type of the conversion.</typeparam>
        // <param name="self">The current instance of ExConsole.</param>
        // <param name="title">The title to show the user before asking for input.</param>
        // <param name="errorMessage">The error message to show the user if the conversion failed.</param>
        // <param name="converter">A function that takes in a string and returns a value tuple of bool success and T value.</param>
        // <returns>
        // A value tuple of bool Success and T Value.
        // If the user enters ^Z, Success will be false and Value will be default(T).
        // Otherwise, Success will be true and Value will contain the T value converted from the input string.
        // </returns>
        // <exception cref="System.Xml.XmlException">Thrown when <paramref name="title"/> or <paramref name="errorMessage"/> aren't properly formatted xml.</exception>
        // <exception cref="ArgumentNullException">Thrown when any of the parameters is null.</exception>
        // <exception cref="ArgumentException">Thrown when <paramref name="title"/> or <paramref name="errorMessage"/> are null or empty.</exception>
        // The documentation above is not a three slashes documentaion because the method is private.
        private static (bool Success, T Value) Read<T>(this ExConsole self, string title, string errorMessage, Func<string, (bool Success, T Value)> converter)
        {
            if (self is null) throw new ArgumentNullException(nameof(self));
            if (title is null) throw new ArgumentNullException(nameof(title));
            if (title == "") throw new ArgumentException(nameof(title) + " can't be empty.", nameof(title));
            if (errorMessage is null) throw new ArgumentNullException(nameof(errorMessage));
            if (errorMessage == "") throw new ArgumentException(nameof(errorMessage) + " can't be empty.", nameof(title));

            if (converter is null) throw new ArgumentNullException(nameof(converter));

            self.WriteLine(title);
            string input;
            while ((input = Console.ReadLine()) != null)
            {
                var (Success, Value) = converter(input);
                if (Success)
                {
                    return (true, Value);
                }
                self.WriteLine(errorMessage);
            }
            return (false, default(T));
        }

        #endregion general

        #region bool

        /// <summary>
        /// Covnverts the user input to a boolean value.
        /// </summary>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="keyForTrue">The key the user should press to return true.</param>
        /// <param name="title">The title to show on the console.</param>
        /// <returns>True if the user pressed the key specified in keyForTrue, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="self"/> or <paramref name="title"/> are null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="title"/> is empty.</exception>
        /// <exception cref="System.Xml.XmlException">Thrown when <paramref name="title"/> isn't properly formatted xml.</exception>
        public static bool ReadBool(this ExConsole self, ConsoleKey keyForTrue, string title)
        {
            if (self is null) throw new ArgumentNullException(nameof(self));
            if (title is null) throw new ArgumentNullException(nameof(title));
            if (title == "") throw new ArgumentException(nameof(title) + " can't be empty.", nameof(title));

            self.Write(title + " ");

            var key = Console.ReadKey().Key;
            Console.WriteLine();
            self.ClearLastLine();
            return key == keyForTrue;
        }

        /// <summary>
        /// Covnverts the user input to a boolean value.
        /// Will keep waiting until the user press a correct key.
        /// </summary>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="keyForTrue">The key the user should press to return true.</param>
        /// <param name="keyForFalse">The key the user should press to return false.</param>
        /// <param name="title">The title to show on the console.</param>
        /// <returns>True or false based on the key the user pressed.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="self"/> or <paramref name="title"/> are null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="title"/> is empty.</exception>
        /// <exception cref="System.Xml.XmlException">Thrown when <paramref name="title"/> isn't properly formatted xml.</exception>
        public static bool ReadBool(this ExConsole self, ConsoleKey keyForTrue, ConsoleKey keyForFalse, string title)
        {
            if (self is null) throw new ArgumentNullException(nameof(self));
            if (title is null) throw new ArgumentNullException(nameof(title));
            if (title == "") throw new ArgumentException(nameof(title) + " can't be empty.", nameof(title));

            self.Write(title + " ");
            while (true)
            {
                var key = Console.ReadKey().Key;
                if (key == keyForTrue || key == keyForFalse)
                {
                    Console.WriteLine();
                    self.ClearLastLine();
                    return key == keyForTrue;
                }
                Console.CursorLeft--;
                Console.Write(" ");
                Console.CursorLeft--;
            }
        }

        #endregion bool

        #region int

        /// <summary>
        /// Covnverts the user input to an integer value.
        /// </summary>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="title">The title to show on the console.</param>
        /// <remarks>If the user enters a value that can't be parsed as int, 
        /// the title will show again until the user enters an int value.</remarks>
        /// <returns>The integer value the user entered.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="self"/> or <paramref name="title"/> are null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="title"/> is empty.</exception>
        /// <exception cref="System.Xml.XmlException">Thrown when <paramref name="title"/> isn't properly formatted xml.</exception>
        public static int ReadInt(this ExConsole self, string title)
        {
            return ReadInt(self, title, title, null);
        }

        /// <summary>
        /// Covnverts the user input to an integer value that meets a specific condition.
        /// </summary>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="title">The title to show on the console. 
        /// This will be shown repeatedly until the user enters a valid value.</param>
        /// <param name="condition">The condition the integer value must meet.</param>
        /// <returns>The integer value the user entered.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="self"/> or <paramref name="title"/> are null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="title"/> is empty.</exception>
        /// <exception cref="System.Xml.XmlException">Thrown when <paramref name="title"/> isn't properly formatted xml.</exception>
        public static int ReadInt(this ExConsole self, string title, Func<int, bool> condition)
        {
            return ReadInt(self, title, title, condition);
        }

        /// <summary>
        /// Covnverts the user input to an integer value that meets a specific condition.
        /// </summary>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="title">The title to show on the console.</param>
        /// <param name="errorMessage">The error message to show the user in case the input value is invalid.</param>
        /// <param name="condition">The condition the integer value must meet.</param>
        /// <returns>The integer value the user entered.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="self"/>, <paramref name="title"/> or <paramref name="errorMessage"/> are null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="title"/> or <paramref name="errorMessage"/> are empty.</exception>        
        /// <exception cref="System.Xml.XmlException">Thrown when <paramref name="title"/> or <paramref name="errorMessage"/> aren't properly formatted xml.</exception>
        public static int ReadInt(this ExConsole self, string title, string errorMessage, Func<int, bool> condition)
        {
            return ReadUntilConverted(
                self,
                title,
                errorMessage,
                str => (int.TryParse(str, out var res) && (condition?.Invoke(res) ?? true), res)
                );
        }

        #endregion int

        #region datetime

        /// <summary>
        /// Converts the user input into an instance of the DateTime struct.
        /// </summary>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="title">The title to show on the console.</param>
        /// <param name="errorMessage">The error message to show the user in case the input value is invalid.</param>
        /// <returns>A nullable DateTime instance that will have no value if the user entered ^Z.</returns>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="title"/> is empty.</exception>        
        /// <exception cref="System.Xml.XmlException">Thrown when <paramref name="title"/> or <paramref name="errorMessage"/> aren't properly formatted xml.</exception>

        public static DateTime? ReadDateTime(this ExConsole self, string title, string errorMessage)
        {
            return ReadStruct(
                self,
                title,
                errorMessage,
                str => (DateTime.TryParse(str, out var res), res)
            );
        }

        /// <summary>
        /// Converts the user input to an instance of a nullable DateTime.
        /// </summary>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="title">The title to show on the console.</param>
        /// <param name="errorMessage">The error message to show the user in case the input value is invalid.</param>
        /// <param name="format">The format of the string representation of the datetime value expected.</param>
        /// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
        /// <param name="dateTimeStyles">A bitwise combination of one or more enumeration values that indicate the permitted format.</param>
        /// <returns>A nullable DateTime instance that will have no value if the user entered ^Z.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="self"/>, <paramref name="title"/> or <paramref name="errorMessage"/> are null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="title"/> or <paramref name="errorMessage"/> are empty, or if dateTimeStyles is not a member of the DateTimeStyles enum.</exception>
        /// <exception cref="System.Xml.XmlException">Thrown when <paramref name="title"/> or <paramref name="errorMessage"/> aren't properly formatted xml.</exception>
        public static DateTime? ReadDateTime(this ExConsole self, string title, string errorMessage, string format, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles)
        {
            return ReadStruct(
                self,
                title,
                errorMessage,
                str => (DateTime.TryParseExact(str, format, formatProvider, dateTimeStyles, out var res), res)
            );
        }

        #endregion datetime
    }
}
