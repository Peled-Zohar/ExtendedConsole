# ExtendedConsole
Provides a set of methods to make your life as a programmer easier when writing a Console application.

It has methods for writing simple markup text to help you easily control the color of your output,  
methods for validating and parsing user input to help you easily get type-safe data from your users,  
methods for easily creating menus and methods for easily clearing specific lines in the console's output.

This repository also contains a project called `UsingExtendedConsole` that contains code samples to shows how to use the `ExtendedConsole` project.

The `ExtendedConsole` project is licenced with the MIT licence, meaning you can use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of this project free of charge, on an "AS IS" basis. For more information, read the LICENSE file (Don't worry, it's short and easy to understand). 

`ExtendedConsole` is available on Nuget - https://www.nuget.org/packages/ExtendedConsole

More information is available on the project's Wiki pages.

Release notes for 1.0.1 version:

1. Changed `ArgumentException` thrown from the `ClearLine` and `ClearLastLines` methods to `ArgumentOutOfRangeException`.
2. Added a missing dot at the end of the `Pause` default text (changed from "Press any key to continue" to "Press any key to continue.").
3. Refactored the `ExConsoleMenu` class (with no changes to public API).

