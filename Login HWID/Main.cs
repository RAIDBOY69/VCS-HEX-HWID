using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;



namespace Login_HWID
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        [DllImport("kernel32")]
        private static extern bool AllocConsole();
        private void Main_Load(object sender, EventArgs e)
        {
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.EnableVerboseLogging = false;
            service.SuppressInitialDiagnosticInformation = true;
            service.HideCommandPromptWindow = true;

            ChromeOptions options = new ChromeOptions();

            options.PageLoadStrategy = PageLoadStrategy.Normal;



            options.AddArgument("--disable-logging");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--log-level=3");
            options.AddArgument("--output=/dev/null");



            IWebDriver driver2 = new ChromeDriver(service, options);
            IWebDriver driver1 = driver2;
            // navigate to URL  

            driver2.Navigate().GoToUrl("https://vocabsize-legacy.xeersoft.co.th/");


            AllocConsole();
            string title = GenerateRandomString();
            Console.Title = title;


            static string GenerateRandomString()
            {
                const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                Random rnd = new Random();
                char[] chars = new char[10];
                for (int i = 0; i < 10; i++)
                {
                    chars[i] = alphabet[rnd.Next(alphabet.Length)];
                }
                return new string(chars);
            }

            IWebElement WaitUntilElementExists(By elementLocator, int timeout = 10)
            {
                try
                {
                    var waitt = new WebDriverWait(driver1, TimeSpan.FromSeconds(timeout));
                    return waitt.Until(ExpectedConditions.ElementExists(elementLocator));
                }
                catch (NoSuchElementException)
                {
                    Console.WriteLine("Element with locator: '" + elementLocator + "' was not found in current context page.");
                    throw;
                }
            }

            //VERSION
            Console.WriteLine("V0.2");

            string usernamer;
            string passwordr;
            Console.WriteLine("\n\n Enter enconcept username: ");
            usernamer = Console.ReadLine();
            Console.WriteLine("\n\n Enter enconcept password: ");
            passwordr = Console.ReadLine();
            IWebElement ele = driver1.FindElement(By.Id("txt_email"));
            ele.SendKeys(usernamer);
            IWebElement ele2 = driver1.FindElement(By.Id("txt_password"));
            ele2.SendKeys(passwordr);
            IWebElement ele3 = driver1.FindElement(By.Id("btn_submit"));
            ele3.Click();
            ///Thread.Sleep(1000);

            WebDriverWait wait = new WebDriverWait(driver1, TimeSpan.FromSeconds(20));
            _ = wait.Until(condition: driver =>
            {
                return driver.FindElement(By.XPath("/html/body/div[2]/div/div/div[1]/div/div/div[1]/div/a/img"));
            });



            driver1.FindElement(By.XPath("/html/body/div[2]/div/div/div[1]/div/div/div[1]/div/a/img")).Click();

            string assignmentnum;
            Console.WriteLine("\n\n Enter Assignment No. ");
            assignmentnum = Console.ReadLine();
            var assignmentnum2 = String.Format("/html/body/div[2]/table/tbody/tr[{0}]/td[7]/a[2]", assignmentnum);

            IWebElement ele5 = driver1.FindElement(By.XPath(assignmentnum2));
            ele5.Click();



            driver1.FindElement(By.Id("btn_skip")).Click();
            driver1.FindElement(By.Id("button1")).Click();

            //word amount

            int word_amount = Convert.ToInt16(driver1.FindElement(By.XPath("/html/body/section/div[1]/div/div/div[1]/div[6]/span")).GetAttribute("innerHTML"));
            Console.WriteLine("Currently doing " + word_amount + " words in this assignment");



            for (int i = 0; i < word_amount; i++)
            {
                int amount = i;
                var wd = String.Format("/html/body/section/div[4]/table/tbody/tr[{0}]/td[3]/span[1]", amount + 1);
                var words = (driver1.FindElement(By.XPath(wd)).GetAttribute("innerHTML"));
                var txtbox = String.Format("/html/body/section/div[4]/table/tbody/tr[{0}]/td[4]/div/input", amount + 1);
                driver1.FindElement(By.XPath(txtbox)).SendKeys(words);

            }
            ///SAVE
            IWebElement ele10 = driver1.FindElement(By.XPath("//*[@id=\"btn_save\"]"));
            ele10.Click();

            Console.WriteLine("Your Assignment is done!");
            Thread.Sleep(2500);
            Console.Write("Now Closing WebDriver.");
            Console.Write(".");
            Thread.Sleep(1000);
            Console.Write(".");
            Thread.Sleep(1000);
            Console.Write(".");
            Thread.Sleep(1000);
            Console.Write(".");
            Thread.Sleep(2500);

            //close the browser

            Console.WriteLine("terminating program..");
            Thread.Sleep(1500);
            Environment.Exit(0);
        }
    }
}