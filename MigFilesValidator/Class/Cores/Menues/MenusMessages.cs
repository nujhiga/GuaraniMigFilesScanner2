using System;
using System.Linq;

namespace GuaraniMigFilesScanner.Class.Cores.Menues
{
    public static class MenusMessages
    {
        private enum LineAlignment
        {
            None,
            Right,
            Center
        }

        private enum MoveCursorTo
        {
            None,
            Top,
            Down,
            Left,
            Right
        }

        private const string M_SPACE1 = " - ";
        private const string M_SPACE2 = "    ";
        private const string M_SPACE3 = "       ";
        private const string M_SPACE4 = "                                                                               ";

        private const string SEPARATOR = "____________________________________________________________________________________________________________________";

        private const string DESC_LINE1 = "Esta herramienta permite validar los archivos de migración desde sistemas externos";
        private const string DESC_LINE2 = "al sistema de gestión SIU-Guaraní. Así como editar y corregir inconsistencias genéricas.";

        private const string APP_TITLE = "Pasifae - Controles Pre Migración SIU-Guaraní.";
        private const string APP_SIGN_FOOTER = "Desarrollado por el Equipo Técnico de Migración del Proyecto DGPTE/SSTES.";

        private const string MAIN_M_LINE1 = "Controles y Validación Pre-Migración: Sistemas Externos....";
        private const string MAIN_M_LINE2 = "Editar y Corregir Archivos: Inconsistencias genéricas......";

        private const string OPT_EXIT = "             Salir";
        private const string OPT_RESTART = "         Reiniciar";

        private const string KEYS_NUMS = "          [1 - 9]";
        private const string OPT_SELECT = "Seleccionar";
        private const string OPT_NEXT = "Siguiente";
        private const string OPT_PREV = "Anterior";
        private const string OPT_CANCEL = "Cancelar y Volver";

        private const string LBL_AVILFILES = " Archivos 'mig' disponibles.";
        private const string LBL_PAGES = "páginas.";

        private static string Key(ConsoleKey key) => $"[{key}]";
        private static string Key(int key) => $"[{key}]";
        private static string[] GetMainMenuLines()
        {
            return new[] {

                $"{M_SPACE1}{MAIN_M_LINE1}",
                $"{M_SPACE1}{MAIN_M_LINE2}",
                $"{OPT_EXIT}",
                $"{OPT_RESTART}"
            };
        }
        private static string[] GetPageOptionsMenu()
        {
            return new[]
            {
                KEYS_NUMS,
                $" {OPT_SELECT} - ",

                Key(ConsoleKey.LeftArrow),
                $" {OPT_PREV} - ",

                Key(ConsoleKey.RightArrow),
                $" {OPT_NEXT} - ",

                Key(ConsoleKey.Escape),
                $" {OPT_CANCEL}",
            };
        }
        private static int Append(int initialLeft, string text, ConsoleColor textForeground)
        {
            MoveCursorPos(MoveCursorTo.Right);

            int auxLeft = text.Length + initialLeft;

            Console.ForegroundColor = textForeground;

            Console.WriteLine(text);

            if (Console.ForegroundColor != ConsoleColor.White)
                Console.ForegroundColor = ConsoleColor.White;

            Console.SetCursorPosition(auxLeft, Console.CursorTop - 1);

            return Console.CursorLeft;
        }
        private static int AppendLine(int initialLeft, string text, ConsoleColor textForeground)
        {
            var append = Append(initialLeft, text, textForeground);
            MoveCursorPos(MoveCursorTo.Down);
            return append;
        }
        private static int NewAppend(string line, string text, ConsoleColor lineForeground = ConsoleColor.White,
            ConsoleColor textForeground = ConsoleColor.White, LineAlignment alignment = LineAlignment.None,
            MoveCursorTo moveTo = MoveCursorTo.None, int unit = 1)
        {
            if (moveTo != MoveCursorTo.None) MoveCursorPos(moveTo, unit);

            NewLine(line, alignment, lineForeground);

            Console.SetCursorPosition(line.Length, Console.CursorTop - 1);

            if (textForeground != ConsoleColor.White)
            {
                Console.ForegroundColor = textForeground;
                Console.WriteLine(text);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(text);
            }

            return $"{line}{text}".Length;
        }
        private static void NewLine(string line, LineAlignment alignment = LineAlignment.None, ConsoleColor foreground = ConsoleColor.White)
        {
            int width = Console.CursorLeft;

            switch (alignment)
            {
                case LineAlignment.Center:
                    width = (Console.WindowWidth - line.Length) / 2;
                    break;
                case LineAlignment.Right:
                    width = Console.WindowWidth - line.Length - 2;
                    break;
            }

            if (alignment != LineAlignment.None)
                Console.SetCursorPosition(width, Console.CursorTop);

            if (foreground != ConsoleColor.White)
            {
                Console.ForegroundColor = foreground;
                Console.WriteLine(line);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(line);
            }
        }
        private static void MoveCursorPos(MoveCursorTo to, int unit = 1)
        {
            int top = Console.CursorTop;
            int left = Console.CursorLeft;

            switch (to)
            {
                case MoveCursorTo.Down:
                    top += unit;
                    break;
                case MoveCursorTo.Top:
                    top -= unit;
                    break;
                case MoveCursorTo.Left:
                    left -= unit;
                    break;
                case MoveCursorTo.Right:
                    left += unit;
                    break;
            }

            Console.SetCursorPosition(left, top);
        }
        private static void SetHeader(bool addDesc = false)
        {
            Console.Clear();

            MoveCursorPos(MoveCursorTo.Down, 2);

            NewLine(APP_TITLE, LineAlignment.Center, ConsoleColor.DarkCyan);

            MoveCursorPos(MoveCursorTo.Down, 2);

            if (addDesc)
            {
                NewLine(DESC_LINE1, alignment: LineAlignment.Center);

                NewLine(DESC_LINE2, alignment: LineAlignment.Center);

                NewLine(SEPARATOR, alignment: LineAlignment.Center, ConsoleColor.DarkCyan);

                MoveCursorPos(MoveCursorTo.Down, 4);
            }
        }
        private static void SetFooter()
        {
            Console.SetCursorPosition(Console.CursorLeft, 29);
            NewLine(APP_SIGN_FOOTER, alignment: LineAlignment.Right, ConsoleColor.Red);
        }
        public static void ShowMainMenu()
        {
            string[] lines = GetMainMenuLines();

            Console.Title = APP_TITLE;

            SetHeader(true);

            NewAppend(lines[0], Key(ConsoleKey.F1), textForeground: ConsoleColor.Green, moveTo: MoveCursorTo.Top);

            NewAppend(lines[1], Key(ConsoleKey.F2), textForeground: ConsoleColor.Green, moveTo: MoveCursorTo.Down);

            NewAppend(lines[2], $"{M_SPACE4}{Key(ConsoleKey.Escape)}", textForeground: ConsoleColor.Green,
                alignment: LineAlignment.Right, moveTo: MoveCursorTo.Down, unit: 11);

            NewAppend(lines[3], $"{M_SPACE4}{Key(ConsoleKey.Delete)}", textForeground: ConsoleColor.Green,
                alignment: LineAlignment.Right, moveTo: MoveCursorTo.Down);

            SetFooter();
        }
        public static void ShowMaxFileProcessOption()
        {

        }
        public static void ShowFileActionsMenu(string currMigFile)
        {
            SetHeader();

            Console.WriteLine($"{M_SPACE2}Scanner de Inconsistencias - Trabajando con {currMigFile.ToUpper()}");

            Console.WriteLine($"{M_SPACE1}Comenzar.{Key(ConsoleKey.F1)}");

            Console.WriteLine($"{M_SPACE1}Cancelar y Volver.{Key(ConsoleKey.Escape)}");
        }
        private static void SetPageOptionsMenu()
        {
            string[] options = GetPageOptionsMenu();

            int auxAppend = 0;

            for (int i = 0; i < options.Length; i++)
            {
                ConsoleColor clr = i % 2 == 0 ? ConsoleColor.Green : ConsoleColor.White;
                auxAppend = Append(auxAppend, options[i], clr);
            }

            Console.SetCursorPosition(0, Console.CursorTop + 1);
        }
        private static void SetMigFilesSelection(string[] currPageFiles)
        {
            for (int i = 0; i < currPageFiles.Length; i++)
            {
                string file = currPageFiles[i].Split('\\').Last();
                NewAppend($"{M_SPACE1}{Key(i + 1)} ", file, ConsoleColor.Green);
            }
        }
        public static void ShowMigFilesMenu(int allPagesFilesCount, string[] currPageFiles, int currPage, int totalPages)
        {
            SetHeader();

            MoveCursorPos(MoveCursorTo.Down);

            SetPageOptionsMenu();

            NewLine(SEPARATOR, LineAlignment.Center, ConsoleColor.DarkCyan);

            MoveCursorPos(MoveCursorTo.Down, 2);

            NewLine($"{M_SPACE2}{allPagesFilesCount}{LBL_AVILFILES} {currPage}-{totalPages} {LBL_PAGES}");

            MoveCursorPos(MoveCursorTo.Down);

            SetMigFilesSelection(currPageFiles);

            SetFooter();
        }

        public static void ErrorNoFilesInPage()
        {
          

            Console.WriteLine($"¡Error! No hay archivos 'mig' en la página.");
        }

    }
}
