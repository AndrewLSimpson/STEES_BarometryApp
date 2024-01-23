using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using System.IO;
using HtmlAgilityPack;
using Ivi.Visa;


namespace Barometry.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
  
                //web scraper for main department barometer on IP 10.115.16.17
                HtmlWeb web = new HtmlWeb();
                //Load the html page as a document
                HtmlDocument document = web.Load("http://10.115.16.17");
                //Scrape the html that includes the div class with the ID pres_mbar
                HtmlNode mBar = document.DocumentNode.SelectSingleNode("//div[@id='pres_mbar']");
                HtmlNode mmHg = document.DocumentNode.SelectSingleNode("//div[@id='pres_mmhg']");
                //Pull the html into the string
                string divContentmBar = mBar.InnerHtml;
                string divContentmmHg = mmHg.InnerHtml;
                //View the Main Department Barometer
                ViewBag.b1mBar = divContentmBar;
                ViewBag.b1mmHg = divContentmmHg;
            //Set viewbags for the Endeavour Unit Barometer by calling the B2EndeavourUnit method
            try
            {
                ViewBag.b2 = B2EndeavourUnitPressuremBar();
                ViewBag.b2mmHg = B2EndeavourUnitPressuremmHg(B2EndeavourUnitPressuremBar());

                ViewBag.b2time = DateTime.Now;
            }
            catch (Exception ex)
            {
                ViewBag.b2 = "Offline";
                ViewBag.b2mmHg = "Offline";
            }
            //Return View
            return View();
        }

        //Calculate the mmHg for the endeavour unit as not sure if the device sends mmHg.
        public double B2EndeavourUnitPressuremmHg(string mBar)
        {
            double mBarToConvert;
            try
            {
                mBarToConvert = Double.Parse(mBar);
            }
            catch (FormatException)
            {
                throw new ArgumentException("Invalid number format - device may be offline");
            }
            double mmHg = mBarToConvert * 0.750061561306;
            return Math.Round(mmHg, 2); // round the result to two decimal places

        }

        public string B2EndeavourUnitPressuremBar()
        {
            // Connect to the druck using an IVisaSession and globalresourcemanager
            using (IVisaSession res = GlobalResourceManager.Open("TCPIP::10.115.46.84::inst0::INSTR", AccessModes.ExclusiveLock, 2000))
            {
                //if the resource is a IMessageBasedSession (which it should be)
                if (res is IMessageBasedSession session)
                {
                    // Ensure termination character is enabled as here in example we use a SOCKET connection.
                    session.TerminationCharacterEnabled = true;
                    // Request information about the druck.
                    session.FormattedIO.WriteLine(":SENSe:PRESsure?");
                    //Capture the information
                    string idn = session.FormattedIO.ReadLine();
                    //Set the index of the payload
                    int index = idn.IndexOf(' ');
                    //only get these specific characters, we're only interested in the numbers.
                    string pressure = idn.Substring(index + 1, 7);
                    //Return the pressure
                    return pressure;
                }
                else
                {
                    return "Offline";
                }
            }
        }

    //Version Page
    public ActionResult ChangeLog()
        {
            return View();
        }

    }
}