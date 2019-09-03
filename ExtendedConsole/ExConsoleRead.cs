using System;

namespace ExtendedConsole
{
    /// <summary>
    /// Provides extension methods for ExConsole for easy reading and parsing inputs from the user.
    /// </summary>
    public static class ExConsoleRead
    {

        /// <summary>
        /// Pause the console application until the user press a key.
        /// </summary>
        /// <param name="self">The current instance of ExConsole.</param>
        public static void Pause(this ExConsole self)
        {
            self.Pause("Press any key to continue");
        }

        /// <summary>
        /// Pause the console application until the user press a key.
        /// </summary>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="title">The title to show the user before asking for input.</param>
        public static void Pause(this ExConsole self, string title)
        {
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
        public static T ReadUntilConverted<T>(this ExConsole self, string title, string errorMessage, Func<string, (bool Success, T Value)> converter)
        {
            var (Success, Value) = (false, default(T));
            do
            {
                (Success, Value) = self.Read<T>(
                title,
                errorMessage,
                converter);
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
        public static T? ReadStruct<T>(this ExConsole self, string title, string errorMessage, Func<string, (bool Success, T Value)> converter) where T : struct
        {
            var (Success, Value) = self.Read(
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
        public static T ReadClass<T>(this ExConsole self, string title, string errorMessage, Func<string, (bool Success, T Value)> converter) where T : class
        {
            var (Success, Value) = self.Read(
                title,
                errorMessage,
                converter
            );
            return Success ? Value : null;
        }

        /// <summary>
        /// Covnverts the user input to a boolean value.
        /// </summary>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="keyForTrue">The key the user should press to return true.</param>
        /// <param name="title">The title to show on the console.</param>
        /// <returns>True if the user pressed the key specified in keyForTrue, false otherwise.</returns>
        public static bool ReadBool(this ExConsole self, ConsoleKey keyForTrue, string title)
        {
            self.Write(title + " ");

            var key = Console.ReadKey().Key;
            Console.WriteLine();
            self.ClearLastLine();
            return key == keyForTrue;
        }

        /// <summary>
        /// Covnverts the user input to an integer value.
        /// </summary>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="title">The title to show on the console.</param>
        /// <returns>The integer value the user entered.</returns>
        public static int ReadInt(this ExConsole self, string title)
        {
            return self.ReadInt(title, "Please enter an integer value.", i => true);
        }

        /// <summary>
        /// Covnverts the user input to an integer value that meets a specific condition.
        /// </summary>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="title">The title to show on the console.</param>
        /// <param name="condition">The condition the integer value must meet.</param>
        /// <returns>The integer value the user entered.</returns>
        public static int ReadInt(this ExConsole self, string title, Func<int, bool> condition)
        {
            return self.ReadInt(title, title, condition);
        }

        /// <summary>
        /// Covnverts the user input to an integer value that meets a specific condition.
        /// </summary>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="title">The title to show on the console.</param>
        /// <param name="errorMessage">The error message to show the user in case the input value is invalid.</param>
        /// <param name="condition">The condition the integer value must meet.</param>
        /// <returns>The integer value the user entered.</returns>
        public static int ReadInt(this ExConsole self, string title, string errorMessage, Func<int, bool> condition)
        {
            return self.ReadUntilConverted(
                title,
                errorMessage,
                str => (int.TryParse(str, out int res) && condition(res), res)
                );
        }

        /// <summary>
        /// Converts the user input into an instance of the DateTime struct.
        /// </summary>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="title">The title to show on the console.</param>
        /// <param name="errorMessage">The error message to show the user in case the input value is invalid.</param>
        /// <returns></returns>
        public static DateTime? ReadDateTime(this ExConsole self, string title, string errorMessage)
        {
            return self.ReadStruct(
                title,
                errorMessage,
                str => (DateTime.TryParse(str, out DateTime res), res)
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
        public static DateTime? ReadDateTime(this ExConsole self, string title, string errorMessage, string format, IFormatProvider formatProvider, System.Globalization.DateTimeStyles dateTimeStyles)
        {
            return self.ReadStruct(
                title,
                errorMessage,
                str => (DateTime.TryParseExact(str, format, formatProvider, dateTimeStyles, out DateTime res), res)
            );
        }

        #region private methods

        /// <summary>
        /// Reads an input line from the user and attempt to convert it to T.
        /// Repeats until conversion succeeds or the user enters ^Z.
        /// </summary>
        /// <typeparam name="T">The target type of the conversion.</typeparam>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="title">The title to show the user before asking for input.</param>
        /// <param name="errorMessage">The error message to show the user if the conversion failed.</param>
        /// <param name="converter">A function that takes in a string and returns a value tuple of bool success and T value.</param>
        /// <returns>
        /// A value tuple of bool Success and T Value.
        /// If the user enters the quit value, Success will be false and Value will be default(T).
        /// Otherwise, Success will be true and Value will contain the T value converted from the input string.
        /// </returns>
        private static (bool Success, T Value) Read<T>(this ExConsole self, string title, string errorMessage, Func<string, (bool Success, T Value)> converter)
        {
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

        #endregion private methods
    }
}
