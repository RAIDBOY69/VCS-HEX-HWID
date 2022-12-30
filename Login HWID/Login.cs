/* change isPremium for user to "PREMIUM" for he can use application
 * change isPremium for user to "BANNED" if you want ban user
 * change isPremium for user to "FREE" (DEFAULT) if he don't have purchase application for get access
 * 
 * If you want reset HWID for a user, change table whitelist to: RESET (default) when he will login on his account it's will add his new HWID
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Login_HWID
{
    public partial class Login : Form
    {

        public static Point newpoint = new Point();
        public static int x;
        public static int y;


        public Login()
        {
            InitializeComponent();
        }

        #region "Button Login"
        private void LoginBTN_Click(object sender, EventArgs e) //<== Button for check if username + password match and check if player is PREMIUM / BANNED / FREE
        {
            LoginBTN.Text = "Loading..."; //<== Loading Text Btn

            try
            {
                if (Execute("accessAccount", "username=" + Username.Text + "&password=" + Password.Text) == 1)
                {
                    Username.Text = Username.Text;

                    WebClient fetchInfo = new WebClient();
                    string premiumState = fetchInfo.DownloadString("https://vcshex.000webhostapp.com/API/execute.php?action=isPremium&username=" + Username.Text); //<== API for check if player is PREMIUM / BANNED / FREE

                    if (premiumState == "PREMIUM") //<== If detect player is PREMIUM (PLAYER ALLOWED)
                    {
                        HWIDReset(); //<== Check if HWID need to be reset or not
                        HWIDAllowed(); //<== Check if HWID match or not
                        GETIP(); //<== Get IP and add it on the database
                    }
                    else if (premiumState == "BANNED") //<== If detect player is BANNED
                    {
                        MessageBox.Show("You are banned.", "VCS-Hex", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        LoginBTN.Text = "Login"; //<== Reset Text Btn
                    }
                    else if (premiumState == "FREE") //<== If detect player is FREE
                    {
                        MessageBox.Show("You are a Free Member, you need purchase it", "VCS-Hex", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        LoginBTN.Text = "Login"; //<== Reset Text Btn
                    }
                    else
                    {
                        MessageBox.Show("??????", "VCS-Hex", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("API is offline, contact Dev or wait.", "SERVER ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoginBTN.Text = "Login"; //<== Reset Text Btn
            }
        }

        #endregion


        //HWID + Encryption for PC
        #region "HWID + Encryptions"

        private static string para3() //<= static sting for get HWID of your PC
        {
            string str = "";
            ManagementObjectCollection.ManagementObjectEnumerator objA = new ManagementObjectSearcher("Select * From Win32_processor").Get().GetEnumerator();
            try
            {
                while (true)
                {
                    if (!objA.MoveNext())
                    {
                        break;
                    }
                    ManagementObject current = (ManagementObject)objA.Current;
                    str = current["ProcessorID"].ToString();
                }
            }
            finally
            {
                if (!ReferenceEquals(objA, null))
                {
                    objA.Dispose();
                }
            }
            ManagementObject obj3 = new ManagementObject("win32_logicaldisk.deviceid=\"c:\"");
            obj3.Get();
            return (str + obj3["VolumeSerialNumber"].ToString());
        }

        //Encrypt Strings
        public static string StringToHex(string hexstring)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char t in hexstring)
            {
                //Note: X for upper, x for lower case letters
                sb.Append(Convert.ToInt32(t).ToString("x"));
            }
            return sb.ToString();
        }

        #endregion 


        //HWID Reset if table player of user is to "RESET"
        #region "HWID Reset"

        void HWIDReset()
        {
            try
            {
                string MDR = ("https://vcshex.000webhostapp.com/API/execute.php?action=sendhwid&username=" + Username.Text + "&hwid=" + para3()); //<== Reset HWID send the new HWID on the database
                WebClient Check = new WebClient();
                string mdr = Check.DownloadString("https://vcshex.000webhostapp.com/API/execute.php?action=hwid&username=" + Username.Text);

                if (mdr == "RESET") //<== If table of the player HWID is RESET, HWID can be reset
                {
                    WebClient LOL = new WebClient();
                    LOL.DownloadString(MDR);

                    MessageBox.Show("" + Username.Text + " HWID SAVED, you can now use for this PC !", "Usermode", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Impossible to get / change HWID, contact ADMIN Dev !");
                Application.Exit();
            }
        }
        #endregion


        //Check if HWID match with account
        #region "Void HWID Is Allowed"

        void HWIDAllowed()
        {
            string HWID1 = para3();
            try
            {
                WebClient HWID = new WebClient();
                string secure = HWID.DownloadString("https://vcshex.000webhostapp.com/API/execute.php?action=hwid&username=" + Username.Text); //<== Check if HWID Match

                if (secure == HWID1)
                {
                    AllowAccess(); //If it's match
                }
                else
                {
                    MessageBox.Show("This account is registered to a another PC !", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("API is offline, contact ADMIN Dev or wait.", "SERVER ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion


        //Get last ip and it's send it on the database
        #region "Get Last IP + Send New"

        void GETIP() //<== Send ip of player in the database
        {
            try
            {
                string externalip = new WebClient().DownloadString("http://ipinfo.io/ip");

                string GET = new WebClient().DownloadString("https://vcshex.000webhostapp.com/API/execute.php?action=GETIP&username=" + Username.Text + ""); //<== API  for get IP
                string SEND = new WebClient().DownloadString("https://vcshex.000webhostapp.com/API/execute.php?action=IP&username=" + Username.Text + "&ip=" + externalip + ""); //<== API for send ip in the table
            }
            catch (Exception)
            {
                MessageBox.Show("Impossible to get / change IP, contact VCSHEX Dev !");
                Application.Exit();
            }
        }

        #endregion


        //Some api don't touch change only link api for your website
        #region "Some APIs"

        //The main function
        public static int Execute(string action, string args)
        {
            WebClient requests = new WebClient();
            string url = "https://vcshex.000webhostapp.com/API/execute.php"; //<== Link of your website with (https://vcshex.000webhostapp.com/APIPremium/execute.php)
            string urlaction = "?action=" + action;
            string urlargs = "&" + args;
            string buildurl = url + urlaction + urlargs;

            string response = requests.DownloadString(buildurl);
            if (response == null)
            {
                return 0;
            }

            if (!response.StartsWith("LOGIN_GOOD")) //START WORDS FOR WORKING LOGIN ON API
            {
                CheckError(response);
                return 0;
            }

            return 1;
        }

        public static void RaiseError(string error)
        {
            MessageBox.Show(error, "Oops..", MessageBoxButtons.OK, MessageBoxIcon.Error); ///messages errors
        }

        public static int CheckError(string error)
        {
            Dictionary<string, string> Errors = new Dictionary<string, string>();
            Errors.Add("MISSING_PARAMETERS", "Missing parameters"); //If something is missing
            Errors.Add("INVALID_KEY", "The registration key is not valid"); //If registration key is not valid
            Errors.Add("USERNAME_TOO_SHORT", "Your username is too short"); //if username is too short
            Errors.Add("PASSWORD_TOO_SHORT", "Your password is too short"); //if password is too short
            Errors.Add("USERNAME_TAKEN", "The username you choose is already taken"); //if username is already taken
            Errors.Add("PASSWORDS_NOT_MATCH", "Passwords do not match"); //if password do not match
            Errors.Add("USER_BANNED", "You are banned."); //if you are banned
            Errors.Add("NO_ACTION", "No action"); //No action.
            Errors.Add("NOT_ENOUGH_PRIVILEGES", "You do not have enough privileges"); //if you don't have privileges
            Errors.Add("INVALID_CREDENTIALS", "Invalid Username or Password."); //if username or password is not good

            if (!error.StartsWith("API")) //START ERROR LOGS
            {
                RaiseError(error);
                return 0;
            }

            string message = "Undefined error";
            string[] array = error.Split(':');
            if (array.Length == 2 && Errors.ContainsKey(array[1]))
            {
                string key = array[1];
                message = Errors[key];
            }

            RaiseError(message);
            return 1;
        }

        private void textBoxUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                LoginBTN_Click(null, null);
        }

        private void textBoxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                LoginBTN_Click(null, null);
        }

        #endregion


        //If all match and all is good, user can access to Main Form
        #region "AllowAccess"

        void AllowAccess() //<== If player is allowed !!!
        {
            //if all is good you can access to Main Form !

            MessageBox.Show("Welcome " + Username.Text + " you can use vocabsize cheat !!");
            Main ALLOWED = new Main();
            this.Hide();
            ALLOWED.Show();
            
        }


        #endregion






        //For move application
        #region "System Move Title Panel"
        private void xMouseDown(object sender, MouseEventArgs e)
        {
            x = Control.MousePosition.X - base.Location.X;
            y = Control.MousePosition.Y - base.Location.Y;
        }
        private void xMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                newpoint = Control.MousePosition;
                newpoint.X -= x;
                newpoint.Y -= y;
                base.Location = newpoint;
            }
        }
        #endregion 

        private void Login_Load(object sender, EventArgs e)
        {
            this.label1.MouseDown += this.xMouseDown; //For Move Form
            this.label1.MouseMove += this.xMouseMove; //For Move Form
            this.label1.MouseDown += this.xMouseDown; //For Move Form
            this.label1.MouseMove += this.xMouseMove; //For Move Form
        }

        private void Username_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://discord.gg/NNhzW5X94m");
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
