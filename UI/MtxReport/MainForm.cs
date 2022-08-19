using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Outlook;

namespace MtxReport
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private string fixStatus(string s, string fromStr, string toStr)
        {
            string res=s;
            var i = s.LastIndexOf(fromStr);
            if (i != -1)
            {
                string s0 = s;
                s0 = s.Substring(0, i);
                s0 = s0.Trim();
                if (s0[s0.Length - 1] == '.') s0 = s0.Remove(s0.Length - 1);
                res = s0.Trim() + toStr;
            }

            return res;
        }

        private string renderTasks(string title, IDictionary<string, string> d, bool addBr = true)
        {
            string res="";
            if (d.Count > 0)
            {
                if(addBr) res += "<p class=MsoNormal><br></p>";
                res += "<p class=MsoNormal>" + title + "</p>\n";
                res += "<ul>\n";

                foreach (var item in d)
                {
                    string k = item.Key;
                    string v = item.Value;

                    res += "<li class=MsoListParagraph style='margin-left:0in'>" + k + ". " + v + "</li>\n";
                }

                res += "</ul>\n";                
            }

            return res;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Outlook.Application oApp = new Microsoft.Office.Interop.Outlook.Application();
            Microsoft.Office.Interop.Outlook.MailItem oMsg = (Microsoft.Office.Interop.Outlook.MailItem)oApp.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
            var mailItemInspector = oMsg.GetInspector;

            DayOfWeek currentDay = DateTime.Now.DayOfWeek;
            int daysTillCurrentDay = (currentDay == DayOfWeek.Sunday ? 7 : (int)currentDay) - (int)DayOfWeek.Monday;
            DateTime weekStartDate = DateTime.Now.AddDays(-daysTillCurrentDay);
            DateTime weekEndDate = weekStartDate.AddDays(4);

            Regex regex = new Regex("^[a-zA-Z]+-[0-9]+ - .+$", RegexOptions.IgnoreCase);
            
            List<string> lines = new List<string>();
            for (int i = 0; i < tasksBox.Lines.Length; i++)
            {
                string s = tasksBox.Lines[i];
                Match m = regex.Match(s);
                if(m.Success) 
                {
                    lines.Add(s.TrimEnd());
                }
            }

            SortedDictionary<string, string> d = new SortedDictionary<string, string>();

            foreach(string l in lines) 
            {
                string[] rsp = Regex.Split(l, "^[a-zA-Z]+-[0-9]+ - ");
                string[] lsp = Regex.Split(l, " - ");

                rsp[1] = fixStatus(rsp[1], " OPEN",                     ". (In progress)");
                rsp[1] = fixStatus(rsp[1], " REOPENED",                 ". (In progress)");
                rsp[1] = fixStatus(rsp[1], " IN PROGRESS",              ". (In progress)");
                rsp[1] = fixStatus(rsp[1], " CODE DONE",                ". (In progress)");
                rsp[1] = fixStatus(rsp[1], " READY FOR IMPLEMENTATION", ". (In progress)");
                rsp[1] = fixStatus(rsp[1], " RESOLVED",                 ". (Done)");
                rsp[1] = fixStatus(rsp[1], " CLOSED",                   ". (Done)");

                d[lsp[0]] = rsp[1];
            }

            SortedDictionary<string, string> ducsx = new SortedDictionary<string, string>();
            SortedDictionary<string, string> ducs = new SortedDictionary<string, string>();
            SortedDictionary<string, string> dciwd = new SortedDictionary<string, string>();
            SortedDictionary<string, string> dnexus = new SortedDictionary<string, string>();
            SortedDictionary<string, string> dgps = new SortedDictionary<string, string>();
            SortedDictionary<string, string> dgpsbl = new SortedDictionary<string, string>();
            SortedDictionary<string, string> dothers = new SortedDictionary<string, string>();
  
            foreach(var item in d)
            {
                string k = item.Key;
                string v = item.Value;

                if(Regex.Match(k, "^UCSX-[0-9]+").Success || Regex.Match(k, "^GAPI-[0-9]+").Success)
                    ducsx[k]=v;
                else if(Regex.Match(k, "^ESR-[0-9]+").Success || Regex.Match(k, "^CM-[0-9]+").Success)
                    ducs[k]=v;
                else if(Regex.Match(k, "^CIWD-[0-9]+").Success)
                    dciwd[k]=v;
                else if(Regex.Match(k, "^NEXUS-[0-9]+").Success)
                    dnexus[k]=v;
                else if(Regex.Match(k, "^GPS-[0-9]+").Success)
                    dgps[k]=v;
                else if(Regex.Match(k, "^GPSBL-[0-9]+").Success)
                    dgpsbl[k]=v;
                else
                    dothers[k]=v;
            }

            string msgHeader = System.IO.File.ReadAllText("header.html");
            string msgFooter = System.IO.File.ReadAllText("footer.html");
            string msgBody = "";

            msgBody += renderTasks("UCS-X", ducsx, false);
            msgBody += renderTasks("UCS", ducs);
            msgBody += renderTasks("Cloud iWD", dciwd);
            msgBody += renderTasks("Nexus", dnexus);
            msgBody += renderTasks("G+ SAP Adapter", dgps);
            msgBody += renderTasks("G+ Siebel Adapter", dgpsbl);
            msgBody += renderTasks("Others", dothers);

            //oMsg.To = "serhiy.osinniy@genesys.com";
            oMsg.To = "olena.logvyna@genesys.com";
            oMsg.Subject = string.Format("Weekly report ({0} - {1}, {2})", weekStartDate.ToString("MMMM d"), weekEndDate.ToString("MMMM d"), weekStartDate.Year.ToString());
            oMsg.BodyFormat = OlBodyFormat.olFormatHTML;
            oMsg.HTMLBody = msgHeader + msgBody + msgFooter;
            oMsg.Display(false); //In order to display it in modal inspector change the argument to true
            mailItemInspector.Activate(); // Bring the editor to the foreground.
        }
    }
}
