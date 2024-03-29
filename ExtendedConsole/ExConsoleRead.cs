﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

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
        /// <returns>The current instance of <see cref="ExConsole"/>.</returns>
        /// <example>
        /// Write "Press any key to continue." to the console, 
        /// and wait for the user to press a key. Advance the cursor to the next line.
        /// <code>
        /// exConsole.Pause();
        /// </code>
        /// </example>
        public static ExConsole Pause(this ExConsole self)
        {
            return Pause(self, "Press any key to continue.");
        }

        /// <summary>
        /// Pause the console application until the user press a key.
        /// </summary>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="title">The title to show the user before asking for input.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="self"/> or <paramref name="title"/> are null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="title"/> is empty.</exception>
        /// <exception cref="System.Xml.XmlException">Thrown when <paramref name="title"/> isn't properly formatted xml.</exception>
        /// <returns>The current instance of <see cref="ExConsole"/>.</returns>
        /// <example>
        /// Write "Press any key to continue." to the console, where "any key" is in yellow,
        /// and wait for the user to press a key. Advance the cursor to the next line.
        /// <code>
        /// exConsole.Pause("Press &lt;c f='yellow'&gt;any key&lt;/c&gt; to continue.");
        /// </code>
        /// </example>
        public static ExConsole Pause(this ExConsole self, string title)
        {
            if (self is null) throw new ArgumentNullException(nameof(self));
            if (title is null) throw new ArgumentNullException(nameof(title));
            if (title == "") throw new ArgumentException(nameof(title) + " can't be empty.", nameof(title));

            self.WriteLine(title);
            Console.ReadKey(true);
            return self;
        }

        /// <summary>
        /// Writes the title to the console, and read a key.
        /// </summary>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="title">The title to show the user before asking for input.</param>
        /// <param name="intercept">Determines whether to display the pressed key in the console window. true to not display the pressed key; otherwise, false.</param>
        /// <returns>An instance of the <see cref="ConsoleKeyInfo"/> struct that describes the console key the user pressed.</returns>
        public static ConsoleKeyInfo ReadKey(this ExConsole self, string title, bool intercept)
        {
            if (self is null) throw new ArgumentNullException(nameof(self));
            if (title is null) throw new ArgumentNullException(nameof(title));
            if (title == "") throw new ArgumentException(nameof(title) + " can't be empty.", nameof(title));
            self.WriteLine(title);
            return Console.ReadKey(intercept);
        }

        /// <summary>
        /// Writes the title to the console, and read a line of text.
        /// </summary>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="title">The title to show the user before asking for input.</param>
        /// <returns>The string the user entered.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="self"/> or <paramref name="title"/> are null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="title"/> is empty.</exception>
        /// <exception cref="System.Xml.XmlException">Thrown when <paramref name="title"/> isn't properly formatted xml.</exception>
        /// <example>
        /// Write "Please enter your first name:" to the console, where "first" is in green,
        /// waits for the user to enter a lime, and returns the user's input.
        /// <code>
        /// var firstName = exConsole.ReadLine("Please enter your &lt;c f='green'&gt;first&lt;/c&gt; name:");
        /// </code>
        /// </example>
        public static string ReadLine(this ExConsole self, string title)
        {
            if (self is null) throw new ArgumentNullException(nameof(self));
            if (title is null) throw new ArgumentNullException(nameof(title));
            if (title == "") throw new ArgumentException(nameof(title) + " can't be empty.", nameof(title));

            self.WriteLine(title);
            return Console.ReadLine();
        }

        /// <summary>
        /// Writes the title to the console, reads a line of text, 
        /// and returns the input only if it passed the validataion.
        /// </summary>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="title">The title to show the user before asking for input.</param>
        /// <param name="errorMessage">The error message to show the user if the validation failed.</param>
        /// <param name="validator">The <see cref="Predicate{T}"/> to validate the input with.</param>
        /// <returns>The string the user entered.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="self"/> or <paramref name="title"/> are null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="title"/> is empty.</exception>
        /// <exception cref="System.Xml.XmlException">Thrown when <paramref name="title"/> isn't properly formatted xml.</exception>
        /// <example>
        /// Write "Please enter your first name:" to the console, where "first" is in green,
        /// waits for the user to enter a lime, and returns the user's input.
        /// <code>
        /// var firstName = exConsole.ReadLine("Please enter your &lt;c f='green'&gt;first&lt;/c&gt; name:", "&lt;c f='green'&gt;first&lt;/c&gt; name must start with a capital letter", s => char.IsUpper(s[0]));
        /// </code>
        /// </example>
        public static string ReadLine(this ExConsole self, string title, string errorMessage, Predicate<string> validator)
        {
            if (self is null) throw new ArgumentNullException(nameof(self));
            if (title is null) throw new ArgumentNullException(nameof(title));
            if (title == "") throw new ArgumentException(nameof(title) + " can't be empty.", nameof(title));
            if (errorMessage is null) throw new ArgumentNullException(nameof(errorMessage));
            if (errorMessage == "") throw new ArgumentException(nameof(errorMessage) + " can't be empty.", nameof(errorMessage));
            if (validator is null) throw new ArgumentNullException(nameof(title));

            self.WriteLine(title);
            while (true)
            {
                var input = Console.ReadLine();
                if (validator(input))
                {
                    return input;
                }
                self.WriteLine(errorMessage);
            }
        }

        /// <summary>
        /// Reads an input line from the user and converts it to T.
        /// Repeats until conversion succeeds (even if the user entered ^Z).
        /// <para>
        /// Use with caution!  
        /// The user can't exit this method without either providing a convertible value or exiting the entire program!
        /// </para>
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
        /// <example>
        /// Parse and get an instance of the `TimeSpan` struct from user input.
        /// <code>
        /// var duration = exConsole.ReadUntilConverted(
        ///     "Please enter estimated time (HH:mm:ss)",
        ///     "Invalid value entered",
        ///     str => (TimeSpan.TryParse(str, out var result), result)
        /// );
        /// </code>
        /// </example>
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
        /// Reads an input line from the user and converts it to a nullable <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The underlying type of the nullable to convert to.</typeparam>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="title">The title to show the user before asking for input.</param>
        /// <param name="errorMessage">The error message to show the user if the conversion failed.</param>
        /// <param name="converter">A function that takes in a string and returns a <see cref="ValueTuple"/> of <see cref="bool"/> success and <typeparamref name="T"/> value.</param>
        /// <returns>An instance of <typeparamref name="T"/>? that has a value unless the user entered ^Z.</returns>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="title"/> or <paramref name="errorMessage"/> are empty.</exception>        
        /// <exception cref="System.Xml.XmlException">Thrown when <paramref name="title"/> or <paramref name="errorMessage"/> aren't properly formatted xml.</exception>
        /// <example>
        /// Parse and get an instance of the `TimeSpan` struct from user input.
        /// The duration variable is of type `Nullable&lt;TimeSpan&gt;`, and will be null if the user entered ctrl+Z.
        /// <code>
        /// var duration = exConsole.ReadStruct(
        ///     "Please enter estimated time (HH:mm:ss)",
        ///     "Invalid value entered",
        ///     str => (TimeSpan.TryParse(str, out var result), result)
        /// );
        /// </code>
        /// </example>
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
        /// Reads an input line from the user and converts it to <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The target type of the conversion.</typeparam>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="title">The title to show the user before asking for input.</param>
        /// <param name="errorMessage">The error message to show the user if the conversion failed.</param>
        /// <param name="converter">A function that takes in a string and returns a <see cref="ValueTuple"/> of <see cref="bool"/> success and <typeparamref name="T"/> value.</param>
        /// <returns>The <typeparamref name="T"/> value converted from the input string, or null if the user entered ^Z.</returns>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="title"/> or <paramref name="errorMessage"/> are empty.</exception>        
        /// <exception cref="System.Xml.XmlException">Thrown when <paramref name="title"/> or <paramref name="errorMessage"/> aren't properly formatted xml.</exception>
        /// <example>
        /// Parse and get an array of int values from comma-separated integers entered by the user.
        /// <code>
        /// var ints = exConsole.ReadClass(
        ///     "Please enter a list of comma-separated integers",
        ///     "Invalid value entered",
        ///     str => {
        ///         var arr = str.Split(',');
        ///         if (arr.All(s => int.TryParse(s, out var i)))
        ///         {
        ///             return (true, arr.Select(s => int.Parse(s)).ToArray());
        ///         }
        ///         return (false, default(int[]));
        ///     }
        /// );
        /// </code>
        /// </example>
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

        /// <summary>
        /// Reads multiple values from the user, converts and returns them as an <see cref="IEnumerable{T}"/>.
        /// Repeats until the user enters the quit text.
        /// </summary>
        /// <typeparam name="T">The type of values to convert to.</typeparam>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="title">The title to show the user before asking for input.</param>
        /// <param name="errorMessage">The error message to show the user if the conversion failed.</param>
        /// <param name="quit">A string the user should enter when they are done entering values.</param>
        /// <param name="converter">A function that takes in a string and returns a value tuple of bool success and T value.</param>        
        /// <returns>An <see cref="IEnumerable{T}"/> containing values conveted from the user input.</returns>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="title"/>, <paramref name="errorMessage"/> or <paramref name="quit"/> are empty.</exception>        
        /// <exception cref="System.Xml.XmlException">Thrown when <paramref name="title"/> or <paramref name="errorMessage"/> aren't properly formatted xml.</exception>
        /// <example>
        /// Read a collection of int values from the user.
        /// The integers vairable is of type <see cref="IEnumerable{T}"/>, and will never be null.
        /// If the user enters "done" before entering any valid integers, the retun value is an empty <see cref="IEnumerable{T}"/>.
        /// <code>
        /// var integers = _exConsole.ReadValues(
        ///     "Please enter integer values, or &lt;c f='red'&gt;done&lt;/c&gt; to quit.",
        ///     "failed to parse input as int.",
        ///     "done",
        ///     str => (int.TryParse(str, out var val), val)
        /// );
        /// </code>
        /// </example>
        public static IEnumerable<T> ReadValues<T>(this ExConsole self, string title, string errorMessage, string quit, Func<string, (bool Success, T Value)> converter)
        {
            if (self is null) throw new ArgumentNullException(nameof(self));
            if (title is null) throw new ArgumentNullException(nameof(title));
            if (title == "") throw new ArgumentException(nameof(title) + " can't be empty.", nameof(title));
            if (quit is null) throw new ArgumentNullException(nameof(quit));
            if (quit == "") throw new ArgumentException(nameof(quit) + " can't be empty.", nameof(quit));

            var returnValue = new List<T>();
            string input;
            self.WriteLine(title);
            do
            {
                var (success, value, entered) = Read(self, errorMessage, converter, quit);
                if (success)
                {
                    returnValue.Add(value);
                }
                input = entered;
            } while (input != quit);

            return returnValue;
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

            self.WriteLine(title);

            var (Success, Value, input) = Read(self, errorMessage, converter, null);
            return (Success, Value);
        }

        // <summary>
        // Reads an input line from the user and attempt to convert it to T.
        // Repeats until conversion succeeds or the user enters ^Z or the quit value.
        // </summary>
        // <typeparam name="T">The target type of the conversion.</typeparam>
        // <param name="self">The current instance of ExConsole.</param>
        // <param name="title">The title to show the user before asking for input.</param>
        // <param name="errorMessage">The error message to show the user if the conversion failed.</param>
        // <param name="converter">A function that takes in a string and returns a value tuple of bool success and T value.</param>
        // <param name="quit>The text the user should enter to quit.</param>
        // <returns>
        // A value tuple of bool Success, T Value and string input.
        // If the user enters ^Z or the quit value, Success will be false and Value will be default(T).
        // Otherwise, Success will be true and Value will contain the T value converted from the input string.
        // </returns>
        // <exception cref="System.Xml.XmlException">Thrown when <paramref name="title"/> or <paramref name="errorMessage"/> aren't properly formatted xml.</exception>
        // <exception cref="ArgumentNullException">Thrown when any of the parameters is null.</exception>
        // <exception cref="ArgumentException">Thrown when <paramref name="title"/> or <paramref name="errorMessage"/> are null or empty.</exception>
        // The documentation above is not a three slashes documentaion because the method is private.
        private static (bool Success, T Value, string Input) Read<T>(this ExConsole self, string errorMessage, Func<string, (bool Success, T Value)> converter, string quit)
        {
            if (self is null) throw new ArgumentNullException(nameof(self));
            if (errorMessage is null) throw new ArgumentNullException(nameof(errorMessage));
            if (errorMessage == "") throw new ArgumentException(nameof(errorMessage) + " can't be empty.", nameof(errorMessage));
            if (converter is null) throw new ArgumentNullException(nameof(converter));

            while (true)
            {
                var input = Console.ReadLine();
                if (input is null || input == quit)
                {
                    return (false, default(T), input);
                }

                var (Success, Value) = converter(input);
                if (Success)
                {
                    return (true, Value, input);
                }
                self.WriteLine(errorMessage);
            }

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
        /// <example>
        /// Askes the user to press "q" to quit, returns true if the user pressed "q", false otherwise:
        /// <code>
        /// var result = exConsole.ReadBool(ConsoleKey.Q, "Press 'Q' to quit");
        /// </code>
        /// </example>
        public static bool ReadBool(this ExConsole self, ConsoleKey keyForTrue, string title)
        {
            if (self is null) throw new ArgumentNullException(nameof(self));
            if (title is null) throw new ArgumentNullException(nameof(title));
            if (title == "") throw new ArgumentException(nameof(title) + " can't be empty.", nameof(title));
            self.Write(title + " ");
            var key = Console.ReadKey(true).Key;
            self.ClearCurrentLine();
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
        /// <example>
        /// Askes the user a yes/no question, returns true if they pressed "Y" or false if they press "N".
        /// Repeats until the user press either "Y" or "N".
        /// <code>
        /// var result = exConsole.ReadBool(
        ///     ConsoleKey.Y,
        ///     ConsoleKey.N,
        ///     "Are you happy (y/n)?"
        /// );
        /// </code>
        /// </example>
        public static bool ReadBool(this ExConsole self, ConsoleKey keyForTrue, ConsoleKey keyForFalse, string title)
        {
            if (self is null) throw new ArgumentNullException(nameof(self));
            if (title is null) throw new ArgumentNullException(nameof(title));
            if (title == "") throw new ArgumentException(nameof(title) + " can't be empty.", nameof(title));

            self.Write(title + " ");
            while (true)
            {
                var key = Console.ReadKey(true).Key;
                if (key == keyForTrue || key == keyForFalse)
                {
                    self.ClearCurrentLine();
                    return key == keyForTrue;
                }
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
        /// <example>
        /// Parse and return an int from the user input.
        /// Repeats until the user enters a value that can be parsed as int.
        /// <code>
        /// var result = exConsole.ReadInt("Please enter an integer value.");
        /// </code>
        /// </example>
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
        /// <example>
        /// Parse and return an int from the user input. 
        /// Repeats until the value meets the specified condition.
        /// <code>
        /// var result = exConsole.ReadInt("Please enter a positive integer value.", i => i > 0);
        /// </code>
        /// </example>
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
        /// <example>
        /// Parse and return an int from the user input. 
        /// If the user enters a value that can't be parsed or doesn't meet the specified condition,
        /// display "Invalid input." 
        /// Repeats until the value meets the specified condition.
        /// <code>
        /// var result = exConsole.ReadInt("Please enter a positive integer value.", "Invalid input.", i => i > 0);
        /// </code>
        /// </example>
        public static int ReadInt(this ExConsole self, string title, string errorMessage, Func<int, bool> condition)
        {
            return ReadUntilConverted(
                self,
                title,
                errorMessage,
                str => (int.TryParse(str, out var result) && (condition?.Invoke(result) ?? true), result)
                );
        }


        #endregion int

        #region datetime

        /// <summary>
        /// Converts the user input into an instance of Nullable&lt;DateTime&gt;.
        /// </summary>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="title">The title to show on the console.</param>
        /// <param name="errorMessage">The error message to show the user in case the input value is invalid.</param>
        /// <returns>A nullable DateTime instance that will have no value if the user entered ^Z.</returns>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="title"/> is empty.</exception>        
        /// <exception cref="System.Xml.XmlException">Thrown when <paramref name="title"/> or <paramref name="errorMessage"/> aren't properly formatted xml.</exception>
        /// <example>
        /// Reads the user input and tries to parse it as a DateTime based on the current thread culture.
        /// Returns an instance of Nullalble&lt;DateTime&gt; which is null if the user entered ctrl+Z.
        /// Repeats untill successful conversion or user abort.
        /// <code>
        /// var result = exConsole.ReadDateTime("Please enter a date.", "failed to convert to date.");
        /// </code>
        /// </example>
        public static DateTime? ReadDateTime(this ExConsole self, string title, string errorMessage)
        {
            return ReadStruct(
                self,
                title,
                errorMessage,
                str => (DateTime.TryParse(str, out var result), result)
            );
        }

        /// <summary>
        /// Converts the user input into an instance of Nullable&lt;DateTime&gt;.
        /// </summary>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="title">The title to show on the console.</param>
        /// <param name="errorMessage">The error message to show the user in case the input value is invalid.</param>
        /// <param name="formatProvider">An object that supplies culture-specific formatting information about the string expected from the user's input.</param>
        /// <param name="dateTimeStyles">A bitwise combination of members of the <see cref="DateTimeStyles"/> enum.</param>
        /// <returns>A nullable DateTime instance that will have no value if the user entered ^Z.</returns>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="title"/> is empty.</exception>        
        /// <exception cref="System.Xml.XmlException">Thrown when <paramref name="title"/> or <paramref name="errorMessage"/> aren't properly formatted xml.</exception>
        /// <example>
        /// Reads the user input and tries to parse it as a DateTime based on the "en-US" culture.
        /// Returns an instance of Nullalble&lt;DateTime&gt; which is null if the user entered ctrl+Z.
        /// Repeats untill successful conversion or user abort.
        /// <code>
        /// var result = exConsole.ReadDateTime(
        ///     "Please enter a date.", 
        ///     "failed to convert to date.",
        ///     CultureInfo.GetCultureInfo("en-US"),
        ///     DateTimeStyles.None);
        /// </code>
        /// </example>
        public static DateTime? ReadDateTime(this ExConsole self, string title, string errorMessage, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles)
        {
            return ReadStruct(
                self,
                title,
                errorMessage,
                str => (DateTime.TryParse(str, formatProvider, dateTimeStyles, out var result), result)
            );
        }

        /// <summary>
        /// Converts the user input to an instance of Nullable&lt;DateTime&gt;.
        /// </summary>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="title">The title to show on the console.</param>
        /// <param name="errorMessage">The error message to show the user in case the input value is invalid.</param>
        /// <param name="format">The format of the string representation of the datetime value expected.</param>
        /// <param name="formatProvider">An object that supplies culture-specific formatting information about the string expected from the user's input.</param>
        /// <param name="dateTimeStyles">A bitwise combination of members of the <see cref="DateTimeStyles"/> enum.</param>
        /// <returns>A nullable DateTime instance that will have no value if the user entered ^Z.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="self"/>, <paramref name="title"/> or <paramref name="errorMessage"/> are null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="title"/> or <paramref name="errorMessage"/> are empty, or if dateTimeStyles is not a member of the DateTimeStyles enum.</exception>
        /// <exception cref="System.Xml.XmlException">Thrown when <paramref name="title"/> or <paramref name="errorMessage"/> aren't properly formatted xml.</exception>
        /// <example>
        /// Reads the user input and tries to parse it as DateTime based on the specified parameters.
        /// Returns an instance of Nullalble&lt;DateTime&gt; which is null if the user entered ctrl+Z.
        /// Repeats untill successful conversion or user abort.
        /// <code>
        /// var result = exConsole.ReadDateTime(
        ///     "Please enter a date (dd/MM/yyyy).",
        ///     "failed to convert to date.",
        ///     "dd/MM/yyyy",
        ///     CultureInfo.InvariantCulture,
        ///     DateTimeStyles.AssumeLocal
        /// );
        /// </code>
        /// </example>
        public static DateTime? ReadDateTime(this ExConsole self, string title, string errorMessage, string format, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles)
        {
            return ReadStruct(
                self,
                title,
                errorMessage,
                str => (DateTime.TryParseExact(str, format, formatProvider, dateTimeStyles, out var result), result)
            );
        }

        /// <summary>
        /// Converts the user input to an instance of Nullable&lt;DateTime&gt;"/>.
        /// </summary>
        /// <param name="self">The current instance of ExConsole.</param>
        /// <param name="title">The title to show on the console.</param>
        /// <param name="errorMessage">The error message to show the user in case the input value is invalid.</param>
        /// <param name="formats">An array containing the acceptable formats of the string representation of the datetime value expected.</param>
        /// <param name="formatProvider">An object that supplies culture-specific formatting information about the string expected from the user's input.</param>
        /// <param name="dateTimeStyles">A bitwise combination of members of the <see cref="DateTimeStyles"/> enum.</param>
        /// <returns>A nullable DateTime instance that will have no value if the user entered ^Z.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="self"/>, <paramref name="title"/> or <paramref name="errorMessage"/> are null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="title"/> or <paramref name="errorMessage"/> are empty, or if dateTimeStyles is not a member of the DateTimeStyles enum.</exception>
        /// <exception cref="System.Xml.XmlException">Thrown when <paramref name="title"/> or <paramref name="errorMessage"/> aren't properly formatted xml.</exception>
        /// <example>
        /// Reads the user input and tries to parse it as DateTime based on the specified parameters.
        /// Returns an instance of Nullalble&lt;DateTime&gt; which is null if the user entered ctrl+Z.
        /// Repeats untill successful conversion or user abort.
        /// <code>
        /// var result = exConsole.ReadDateTime(
        ///     "Please enter a date. Acceptable formats are dd/MM/yyyy, MM-dd-yyyy or yyyy-MM-dd.",
        ///     "failed to convert to date.",
        ///     new string[] {"dd/MM/yyyy", "MM-dd-yyyy", "yyyy-MM-dd"},
        ///     CultureInfo.InvariantCulture,
        ///     DateTimeStyles.AssumeLocal
        /// );
        /// </code>
        /// </example>
        public static DateTime? ReadDateTime(this ExConsole self, string title, string errorMessage, string[] formats, IFormatProvider formatProvider, DateTimeStyles dateTimeStyles)
        {
            return ReadStruct(
                self,
                title,
                errorMessage,
                str => (DateTime.TryParseExact(str, formats, formatProvider, dateTimeStyles, out var result), result)
            );
        }

        #endregion datetime
    }
}
