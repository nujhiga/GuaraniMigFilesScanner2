using System;
using System.Linq;
using System.Threading.Tasks;
using PasifaeG3Migrations.Class.Extras;

namespace GuaraniMigFilesScanner.Class.Cores.Menues
{
    public static class MenusManager
    {
        public async static Task RunMainMenu(Core core)
        {
            ConsoleKey userInput;

            MenusMessages.ShowMainMenu();

            do
            {
                userInput = Console.ReadKey(true).Key;

                switch (userInput)
                {
                    case ConsoleKey.F1:

                        await core.SelectMigFile();

                        break;

                    case ConsoleKey.F2:

                        await core.LocalFilesPersonsValidation();

                        break;

                    case ConsoleKey.Delete:
                        core.ResetCore();
                        break;

                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        break;
                }

            } while (true);
        }


        public async static Task RunMigFileActionsMenu(Core c)
        {
            ConsoleKey userInput;

            MenusMessages.ShowFileActionsMenu(c.CurrentMigFileName);

            do
            {
                userInput = Console.ReadKey().Key;

                switch (userInput)
                {
                    case ConsoleKey.F1:

                        await c.RunScannerService();

                        break;


                    case ConsoleKey.Escape:

                        if (!c.IsWorking)
                        {
                            c.SemiResetCore();
                            await c.SelectMigFile();
                        }

                        break;

                    case ConsoleKey.F5:

                        break;
                }

            } while (true);
        }

        private static int? GetMaxFilesCount()
        {
            ConsoleKey inKey;
            string inVal;
            bool isValid;
            int maxFiles;

            do
            {
                inKey = Console.ReadKey().Key;

                if (inKey == ConsoleKey.Delete)
                    return null;

                inVal = Console.ReadLine();

                isValid = int.TryParse(inVal, out maxFiles);

                if (maxFiles > 5) maxFiles = 5;
            }
            while (!isValid);

            return maxFiles;
        }

        public static string[] RunMultipleMigFilesMenu(Core c)
        {
            MenusMessages.ShowMaxFileProcessOption();

            int? maxFilesCount = GetMaxFilesCount();

            if (maxFilesCount == null) return null;

            FtpFileDialog.BrowseDialog d = new FtpFileDialog.BrowseDialog();

            return null;
        }

        private static string[] TryGetCurrentPageFiles(Core c, int index)
        {
            string[] currPageFiles = null;

            if (c.PaginatedFilePaths.IsNullOrEmpty() ||
                !c.PaginatedFilePaths.TryGetValue(index, out currPageFiles) ||
                currPageFiles.IsNullOrEmpty())
            {
                MenusMessages.ErrorNoFilesInPage();
            }

            return currPageFiles;
        }

        private static void InitMigFilesMenu(Core c, string[] currPageFiles, out int allPagesFilesCount)
        {
            allPagesFilesCount = 0;

            c.CurrentPageFiles = currPageFiles;

            c.CurrentPagesCount = c.PaginatedFilePaths.Count;

            foreach (var f in c.PaginatedFilePaths.ToList())
                allPagesFilesCount += f.Value.Length;
        }

        private static bool IsValidKeyInput(ConsoleKeyInfo keyInfo, string[] currPageFiles, out int index) =>
            (int.TryParse(keyInfo.KeyChar.ToString(), out index) && index <= currPageFiles.Length &&
            !string.IsNullOrWhiteSpace(currPageFiles[index]));

        

        public static string RunMigFilesMenu(Core c)
        {
            string[] currPageFiles = TryGetCurrentPageFiles(c, 0);

            if (currPageFiles == null) return string.Empty;

            InitMigFilesMenu(c, currPageFiles, out int allPagesFilesCount);

            ConsoleKeyInfo keyInfo;

            int pageIndex = 0;

            string auxFile = string.Empty;
         
            MenusMessages.ShowMigFilesMenu(allPagesFilesCount, currPageFiles, pageIndex + 1, c.PaginatedFilePaths.Count);

            do
            {
                keyInfo = Console.ReadKey(false);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.RightArrow:
                        if (pageIndex < c.CurrentPagesCount) pageIndex++;
                        break;

                    case ConsoleKey.LeftArrow:
                        if (pageIndex > 0) pageIndex--;
                        break;

                    default:
                        if (IsValidKeyInput(keyInfo, currPageFiles, out pageIndex))
                            return currPageFiles[pageIndex];
                        break;
                }

                currPageFiles = TryGetCurrentPageFiles(c, pageIndex);

                MenusMessages.ShowMigFilesMenu(allPagesFilesCount, currPageFiles, pageIndex + 1, c.PaginatedFilePaths.Count);

            } while (string.IsNullOrWhiteSpace(auxFile) && keyInfo.Key != ConsoleKey.Escape);

            return auxFile;
        }



    }
}



//                    case ConsoleKey.NumPad1:
//                        if (!string.IsNullOrWhiteSpace(c.CurrentPageFiles[0]))
//    return c.CurrentPageFiles[0];
//break;
//                    case ConsoleKey.NumPad2:
//                        if (!string.IsNullOrWhiteSpace(c.CurrentPageFiles[1]))
//    return c.CurrentPageFiles[1];
//break;
//                    case ConsoleKey.NumPad3:
//                        if (!string.IsNullOrWhiteSpace(c.CurrentPageFiles[2]))
//    return c.CurrentPageFiles[2];
//break;
//                    case ConsoleKey.NumPad4:
//                        if (!string.IsNullOrWhiteSpace(c.CurrentPageFiles[3]))
//    return c.CurrentPageFiles[3];
//break;
//                    case ConsoleKey.NumPad5:
//                        if (!string.IsNullOrWhiteSpace(c.CurrentPageFiles[4]))
//    return c.CurrentPageFiles[4];
//break;
//                    case ConsoleKey.NumPad6:
//                        if (!string.IsNullOrWhiteSpace(c.CurrentPageFiles[5]))
//    return c.CurrentPageFiles[5];
//break;
//                    case ConsoleKey.NumPad7:
//                        if (!string.IsNullOrWhiteSpace(c.CurrentPageFiles[6]))
//    return c.CurrentPageFiles[6];
//break;
//                    case ConsoleKey.NumPad8:
//                        if (!string.IsNullOrWhiteSpace(c.CurrentPageFiles[7]))
//    return c.CurrentPageFiles[7];
//break;
//                    case ConsoleKey.NumPad9:
//                        if (!string.IsNullOrWhiteSpace(c.CurrentPageFiles[8]))
//    return c.CurrentPageFiles[8];
//break;
