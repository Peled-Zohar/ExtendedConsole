using ExtendedConsole;
using System;

namespace UsingExtendedConsole
{
    class Program
    {
        private static ExConsole exConsole = new ExConsole();

        static void Main(string[] args)
        {
            while (true)
            {
                if(StringsMenu() == 0) return;
                Console.WriteLine();
                if(exConsole.ReadBool(ConsoleKey.Q, "Press <c f='black' b='white'>Q (or q)</c> to quit, or any other key to go again"))
                {
                    return;
                }
                Console.Clear();
            }
        }
        
        private static int StringsMenu()
        {
            var result = exConsole.Menu(
                "Demonstraiting ExConsole",
                false,
                "<c f='red'>Quit</c>",
                "WriteLine",
                "Read DateTime",
                "Read int",
                "Clear lines"
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
            }
            return result;
        }

        private static int ActionsMenu()
        {
            return exConsole.Menu(
                "Demonstraiting ExConsole",
                true,
                ("<c f='red'>Quit</c>", () => { /* a no-op */ }),
                ("WriteLine", WriteLineMethods),
                ("Read DateTime", ReadDateTimeMethods),
                ("Read int", ReadIntMethods),
                ("Clear lines", ClearLines)
            );
        }

        private static void ClearLines()
        {
            exConsole.WriteLine("Line 1");
            exConsole.WriteLine("Line 2");
            exConsole.WriteLine("Line 3");
            exConsole.WriteLine("Line 4");
            exConsole.WriteLine("Line 5");
            var result = exConsole.ReadInt("enter an int between 1 and 3 to clear lines", i => i >= 1 && i <= 3);
            exConsole.ClearLastLines(result);
            exConsole.Pause("Press a key to delete the last line.");
            exConsole.ClearLastLine();
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

        static void WriteLineMethods()
        {
            exConsole.WriteLine("Hello <c f='magenta'>world!</c>");

            exConsole.WriteLine("Testing some <c b='darkRed'>unrelated</c> <xml att='val' at2='val2'>tags</xml>.");

            exConsole.WriteLine("And another <tag></tag> with no text in it");

            exConsole.WriteLine("And another <tag>with <c f='yellow'>yellow</c> text in it</tag>.");
        }
    }
}
