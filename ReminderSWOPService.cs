using System;
using System.ServiceProcess;
using System.IO;
using System.Net.Mail;
//using System.Threading;

namespace ReminderSWOPWindowsService
{
    public partial class ReminderSWOPService : ServiceBase
    {
        object obj = new object();
        System.Timers.Timer T1 = new System.Timers.Timer();
        bool delay = false;

        public ReminderSWOPService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //checkXML(obj);
            //sendMail("");
            //TimerCallback tm = new TimerCallback(checkXML);
            //System.Threading.Timer timer = new System.Threading.Timer(tm, null, 0, 60000);
            System.Timers.Timer T2 = new System.Timers.Timer();
            T2.Interval = 60000;
            T2.AutoReset = true;
            T2.Enabled = true;
            T2.Start();
            T2.Elapsed += new System.Timers.ElapsedEventHandler(T2_Elapsed);

            
            T1.Interval = 180000;
            T1.AutoReset = false;
            T1.Enabled = true;

        }

        private void T2_Elapsed(object sender, EventArgs e)
        {
            checkXML();
        }

        private void T1_Elapsed(object sender, EventArgs e)
        {
            delay = false;
        }

        protected override void OnStop()
        {

        }

        public void RecordEntry(string event_)
        {

            lock (obj)
            {
                //using (StreamWriter writer = new StreamWriter(@"C:\Glotov\templogSWOP.txt", true))
                using (StreamWriter writer = new StreamWriter(@"E:\ReminderSWOP\templog.txt", true))
                {
                    writer.WriteLine(String.Format("{0} {1}",
                        DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), event_));
                    writer.Flush();
                }
            }
        }

        private void checkXML()
        {
            try
            {

                string[] manFileArray = Directory.GetFiles(@"E:\Agregator\ClientUp\FastMandarin", "file*.xml");
                string[] sbcFileArray = Directory.GetFiles(@"E:\Agregator\ClientUp\FastSBC", "*.xml");
                //string[] manFileArray = Directory.GetFiles(@"\\SWOP\Agregator\ClientUp\FastMandarin", "file*.xml");
                //string[] sbcFileArray = Directory.GetFiles(@"\\SWOP\Agregator\ClientUp\FastSBC", "*.xml");
                string fileName = "";

                foreach (string item in manFileArray)
                {
                    FileInfo fileInfo = new FileInfo(item);
                    DateTime creation = fileInfo.CreationTime;
                    DateTime date = DateTime.Now;
                    TimeSpan t = date.Subtract(creation);

                    if (t.TotalMinutes > 1)
                    {
                        fileName += "FastMandarin\r\n";
                        break;
                    }

                }

                foreach (string item in sbcFileArray)
                {
                    FileInfo fileInfo = new FileInfo(item);
                    DateTime creation = fileInfo.CreationTime;
                    DateTime date = DateTime.Now;
                    TimeSpan t = date.Subtract(creation);

                    if (t.TotalMinutes > 1)
                    {
                        fileName += "FastSBC\r\n";
                        break;
                    }

                }

                if (fileName != "" && delay != true) 
                {
                    delay = true;
                    T1.Start();
                    T1.Elapsed += new System.Timers.ElapsedEventHandler(T1_Elapsed);
                    //RecordEntry(fileName);
                    sendMail(fileName);
                }
                    

            }
            catch (Exception ex)
            {
                RecordEntry(ex.ToString());
            }

        }
        private void sendMail(string catalogs)
        {
            try
            {

                FileSettings fileSettings = new FileSettings();

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(fileSettings.smtp);

                mail.From = new MailAddress(fileSettings.from);
                mail.To.Add(fileSettings.to);
                mail.Subject = "ВНИМАНИЕ! ПЛАНИРОВЩИКИ НЕ РАБОТАЮТ";
                mail.Body += "В этих каталогах файлы созданы больше минуты назад:";
                mail.Body += "\r\n";
                mail.Body += "\r\n";
                mail.Body += catalogs;

                SmtpServer.Port = int.Parse(fileSettings.port);
                SmtpServer.Credentials = new System.Net.NetworkCredential(fileSettings.user, fileSettings.pass);

                SmtpServer.EnableSsl = false;
                SmtpServer.Send(mail);

            }
            catch (Exception ex)
            {
                RecordEntry(ex.ToString());
            }

        }
    }
}
