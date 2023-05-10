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
using Microsoft.Win32;
using System.IO;
using System.IO.Compression;
using System.Net;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;



namespace Login_HWID
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }
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

        [DllImport("kernel32")]
        private static extern bool AllocConsole();

        private IWebDriver _webDriver;


        private void Main_Load(object sender, EventArgs e)
        {

            #region Console
            AllocConsole();
            string title = GenerateRandomString();
            Console.Title = title;
            Console.WriteLine("[+]Initializing Driver");
            try
            {
                new DriverManager().SetUpDriver(new ChromeConfig());
                _webDriver = new ChromeDriver();
                Thread.Sleep(3000);
                #endregion


                #region ChromeDriver Settings
                ChromeDriverService service = ChromeDriverService.CreateDefaultService();
                service.EnableVerboseLogging = false;
                service.SuppressInitialDiagnosticInformation = true;
                service.HideCommandPromptWindow = true;
                ChromeOptions options = new ChromeOptions();
                //options.PageLoadStrategy = PageLoadStrategy.Normal;
                options.AddArgument("--disable-logging");
                options.AddArgument("--disable-dev-shm-usage");
                options.AddArgument("--log-level=3");
                options.AddArgument("--output=/dev/null");
                #endregion
                #region ChromeDriver Launch Check
                IWebDriver driver2 = _webDriver;
                IWebDriver driver1 = driver2;
                Console.Clear();
                driver2.Navigate().GoToUrl("https://vocabsize-legacy.xeersoft.co.th/");

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
                Console.Write(".");
                Console.Clear();
                Console.WriteLine("OK!");
                Thread.Sleep(1000);
                Console.Clear();
                #endregion
                #region VCSBOT


                string usernamer;
                string passwordr;
                Console.WriteLine("\n \nEnter enconcept username: ");
                usernamer = Console.ReadLine();
                Console.Clear();
                Console.WriteLine("\n \nEnter enconcept password: ");
                passwordr = Console.ReadLine();
                driver1.FindElement(By.Id("txt_email")).SendKeys(usernamer);
                driver1.FindElement(By.Id("txt_password")).SendKeys(passwordr);
                driver1.FindElement(By.Id("btn_submit")).Click();


                WebDriverWait wait = new WebDriverWait(driver1, TimeSpan.FromSeconds(20));
                _ = wait.Until(condition: driver =>
                {
                    return driver.FindElement(By.XPath("/html/body/div[2]/div/div/div[1]/div/div/div[1]/div/a/img"));
                });

                driver1.FindElement(By.XPath("/html/body/div[2]/div/div/div[1]/div/div/div[1]/div/a/img")).Click();

                string assignmentnum;
                Console.Clear();
                Console.WriteLine("\nEnter TEST No.");
                assignmentnum = Console.ReadLine();
                var assignmentnum2 = String.Format("/html/body/div[2]/table/tbody/tr[{0}]/td[7]", assignmentnum); ///3 row
                                                                                                                  ///html/body/div[2]/table/tbody/tr[{0}]/td[7]
                //var practicenum = String.Format("/html/body/div[2]/table/tbody/tr[{0}]/td[7]/a[1]", assignmentnum);


                string xpath = assignmentnum2 + "//a[contains(text(), 'Vocab Test')]";
                driver1.FindElement(By.XPath(xpath)).Click();

                //*[@id="result_list"]/table/tbody/tr[1]/td[7]/a[2]


                driver1.FindElement(By.Id("btn_skip")).Click();
                driver1.FindElement(By.Id("button1")).Click();

                //word amount
                Console.Clear();
                int word_amount = Convert.ToInt16(driver1.FindElement(By.XPath("/html/body/section/div[1]/div/div/div[1]/div[6]/span")).GetAttribute("innerHTML"));
                Console.WriteLine("\n[*]Total " + word_amount + " Words");



                for (int i = 0; i < word_amount; i++)
                {
                    int amount = i;
                    var wd = String.Format("/html/body/section/div[4]/table/tbody/tr[{0}]/td[3]/span[1]", amount + 1); // 2 row
                    var words = (driver1.FindElement(By.XPath(wd)).GetAttribute("innerHTML"));
                    var txtbox = String.Format("/html/body/section/div[4]/table/tbody/tr[{0}]/td[4]/div/input", amount + 1);
                    driver1.FindElement(By.XPath(txtbox)).SendKeys(words);
                }
                ///SAVE
                IWebElement ele10 = driver1.FindElement(By.XPath("//*[@id=\"btn_save\"]"));
                ele10.Click();

                Console.WriteLine("[+]Assignment Complete");
                Thread.Sleep(1500);
                Console.WriteLine("[+]Closing WebDriver.");
                Thread.Sleep(1500);
                //close the browser
                _webDriver.Quit();
                Console.WriteLine("[+]Terminating Program..");
                Thread.Sleep(1500);
                Environment.Exit(0);
                #endregion
            
            }
            catch(Exception ex)
            {
                Console.Clear();
                Console.WriteLine("[X]Error: " + ex.Message + " Please contact ADMIN @ezvocabsize.");
            }
            }

        private void LoginBTN_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.instagram.com/ezvocabsize/");
        }
    }
}