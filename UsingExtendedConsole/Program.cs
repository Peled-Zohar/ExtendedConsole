using ExtendedConsole;
using System;
using System.Linq;

namespace UsingExtendedConsole
{

    class Program
    {
        private static ExConsole exConsole = new ExConsole();

        static void Main(string[] args)
        {
            Console.Title = "Using Extended Console";
            //ShowLogo(exConsole);

            var menus = new Func<int>[] { StringsMenu, ActionsMenu };
            var index = 0;
            while (true)
            {
                if (menus[index]() == 0) return;
                index = (index + 1) % menus.Length;
                Console.WriteLine();
                exConsole.Pause();
                Console.Clear();
            }
        }

        private static void ShowLogo(ExConsole exConsole)
        {
            exConsole.WriteLines(
                "",
                ".    <c f='white'>E</c><c f='gray'>x</c><c f='red'>t</c><c f='yellow'>e</c><c f='cyan'>n</c><c f='gray'>d</c><c f='darkgray'>e</c><c f='green'>d</c>",
                ".    <c b='blue'>C</c><c b='darkgray'>o</c><c b='darkred'>n</c><c b='darkblue'>s</c><c b='darkgreen'>o</c><c b='darkmagenta'>l</c><c b='darkyellow'>e</c>",
                ""
            );
            Console.ReadKey();
        }

        private static int StringsMenu()
        {
            var result = exConsole.Menu(
                "Demonstraiting ExConsole - strings menu, clearWhenSelected = false",
                false,
                "<c f='red'>Quit</c>",
                "WriteLine",
                "Read DateTime",
                "Read int",
                "Clear lines",
                "Try it yourself"
            );

            switch (result)
            {
                case 1:
                    WriteLineMethods();
                    break;
                case 2:
                    ReadDateTimeMethods();
                    break;
                case 3:
                    ReadIntMethods();
                    break;
                case 4:
                    ClearLines();
                    break;
                case 5:
                    TryItYourself();
                    break;
            }
            return result;
        }

        private static int ActionsMenu()
        {
            return exConsole.Menu(
                "Demonstraiting ExConsole - actions menu, clearWhenSelected = true",
                true,
                ("<c f='red'>Quit</c>", null),
                ("WriteLine", WriteLineMethods),
                ("Read DateTime", ReadDateTimeMethods),
                ("Read int", ReadIntMethods),
                ("Clear lines", ClearLines),
                ("Try it yourself", TryItYourself)
            );
        }

        static void WriteLineMethods()
        {
            exConsole.WriteLine("Hello <c f='magenta'>world!</c>");
            exConsole.WriteLine("Testing some <c b='darkRed'>unrelated</c> <xml att='val' at2='val2'>tags</xml>.");
            exConsole.WriteLine("And another <tag> </tag> with no text in it");
            exConsole.WriteLine("And another <tag>with <c f='yellow'>yellow</c> text in it</tag>.");
        }

        private static void ReadDateTimeMethods()
        {
            var datetime = exConsole.ReadDateTime("Please enter a datetime value.", "Can't parse datetime value under the current culture settings.");
            if (datetime.HasValue)
            {
                exConsole.WriteLine($"Value entered is <c f='green'>{ datetime.Value.ToShortDateString() }</c>.");
            }
            else
            {
                exConsole.WriteLine("User canceled.");
            }
        }

        private static void ReadIntMethods()
        {
            var result = exConsole.ReadInt("Please enter an int value");
            exConsole.WriteLine($"The int you selected is: <c f='green'>{result}</c>");

            result = exConsole.ReadInt("Please enter an int between <c f='green'>0</c> and <c f='yellow'>5</c>, inclusive.", i => i >= 0 && i <= 5);
            exConsole.WriteLine($"The int you selected is: <c f='green'>{result}</c>");
        }

        private static void ClearLines()
        {
            exConsole.WriteLine("Line <c f='white'>1</c>");
            exConsole.WriteLine("Line <c f='gray'>2</c>");
            exConsole.WriteLine("Line <c f='green'>3</c>");
            exConsole.WriteLine("Line <c f='yellow'>4</c>");
            exConsole.WriteLine("Line 5");
            var result = exConsole.ReadInt("enter an int between 1 and 3 to clear lines", i => i >= 1 && i <= 3);
            exConsole.ClearLastLines(result);
            exConsole.Pause("Press a key to delete the last line.");
            exConsole.ClearLastLine();
        }

        private static void TryItYourself()
        {
            do
            {
                exConsole.WriteLine("Enter free text with markup to see how it's displayed:");
                Console.WriteLine();
                var input = Console.ReadLine();
                Console.WriteLine();
                try
                {
                    exConsole.WriteLine(input);
                }
                catch (System.Xml.XmlException ex)
                {
                    Console.WriteLine();
                    exConsole.WriteLine("<c f='magenta'>The content you've entered can't be parsed to a valid xml.</c>");
                    Console.WriteLine();
                    Console.WriteLine(input);
                    Console.WriteLine();
                    exConsole.WriteLine($"<c f='yellow'>Exception message:</c>\n {ex.Message}");
                }
                Console.WriteLine();
            } while (exConsole.ReadBool(ConsoleKey.Y, "Press <c f='green'>Y</c> to go again"));
        }
    }
}