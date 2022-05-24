using System;

namespace Business.Services
{
    public class LogProgramError
    {
        public static void WriteError(string text)
        {
            using var file = new System.IO.StreamWriter(@"Resources\ProgramErrors.txt", true);
            file.Write("Log at " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + " -> " + text);
            file.WriteLine("");
        }
    }
}
