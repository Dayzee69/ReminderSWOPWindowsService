using System;
using System.IO;

namespace ReminderSWOPWindowsService
{
    class FileSettings
    {

        public FileSettings()
        {
            try
            {
                StreamReader stream = new StreamReader(@"E:\ReminderSWOP\setting.txt");
                //StreamReader stream = new StreamReader(@"C:\Glotov\Microsoft Visual Studio\Projects\ReminderSWOPWindowsService\bin\Debug\setting.txt");
                string str = stream.ReadLine();
                string[] strArr = str.Split(':');
                smtp = strArr[1];

                str = stream.ReadLine();
                strArr = str.Split(':');
                from = strArr[1];

                str = stream.ReadLine();
                strArr = str.Split(':');
                to = strArr[1];

                str = stream.ReadLine();
                strArr = str.Split(':');
                port = strArr[1];

                str = stream.ReadLine();
                strArr = str.Split(':');
                user = strArr[1];

                str = stream.ReadLine();
                strArr = str.Split(':');
                pass = strArr[1];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        public string smtp { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string port { get; set; }
        public string user { get; set; }
        public string pass { get; set; }
    }
}
