using Microsoft.Win32;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace RecordLogin
{
    public class Configure
    {
        private string configFileName;
        private string configDir;
        private string exeFileName;
        private string _startupDir;
        private string exeRegisterDir;
        private string regAppName = "RecLog";
        public string ConfigFileName
        {
            get
            {
                if (string.IsNullOrEmpty(configFileName))
                    configFileName = "Record.json";
                return configFileName;
            }
            set
            {
                configFileName = value;
            }
        }
        public string ConfigDir
        {
            get
            {
                if (string.IsNullOrEmpty(configDir))
                    configDir = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
                return configDir;
            }
            set
            {
                configDir = value;
            }
        }
        public string ConfigFilePath
        {
            get
            {
                return Path.Combine(ConfigDir, ConfigFileName);
            }
        }
        public string ExeFileName
        {
            get
            {
                if (string.IsNullOrEmpty(exeFileName))
                    exeFileName = Path.GetFileName(Assembly.GetExecutingAssembly().CodeBase);
                return exeFileName;
            }
        }
        public string ExeFilePath
        {
            get
            {
                return Path.Combine(StartupDir, ExeFileName);
            }
        }
        [DllImport("shell32.dll")]
        static extern bool SHGetSpecialFolderPath(IntPtr hwndOwner, [Out] StringBuilder lpszPath, int nFolder, bool fCreate);
        const int CSIDL_COMMON_STARTMENU = 0x16;  // All Users\Start Menu
        public string StartupDir
        {
            get
            {
                if (string.IsNullOrEmpty(_startupDir))
                {
                    StringBuilder path = new StringBuilder(260);
                    SHGetSpecialFolderPath(IntPtr.Zero, path, CSIDL_COMMON_STARTMENU, false);
                    _startupDir = path.ToString() + @"\Programs\Startup";
                }
                return _startupDir;
            }
        }
        public string ExeRegisterDir
        {
            get
            {
                if (string.IsNullOrEmpty(exeRegisterDir))
                    exeRegisterDir = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                return exeRegisterDir;
            }
            set
            {
                exeRegisterDir = value;
            }
        }
        public string ExeRegisterPath
        {
            get
            {
                return Path.Combine(ExeRegisterDir, ExeFileName);
            }
        }
        public void SetExecAtStartUp()
        {
            if (SetRegister())
                return;
            if (SetStartup())
                return;
            Console.WriteLine("setup failed");
        }
        private bool SetRegister()
        {
            try
            {
                RegistryKey rkApp = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if (rkApp.GetValue(regAppName) == null)
                {
                    rkApp.SetValue(regAppName, ExeRegisterPath);
                    // stop trying register if failed
                    if (rkApp.GetValue(regAppName) == null)
                        return false;
                }
                rkApp.Close();
                if (File.Exists(ExeRegisterPath))
                    return true;
                else
                {
                    if (SelfCopy(ExeRegisterPath))
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.ToString());
                return false;
            }
        }
        private bool SetStartup()
        {
            try
            {
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.ToString());
                return false;
            }
        }
        public bool SelfCopy(string targetPath)
        {
            try
            {
                if (File.Exists(targetPath))
                    return true;
                else
                {
                    string currentPath = Assembly.GetEntryAssembly().Location;
                    File.Copy(currentPath, targetPath);

                    FileInfo fileInfo = new FileInfo(currentPath);
                    fileInfo.Attributes = FileAttributes.Hidden;
                    fileInfo = new FileInfo(targetPath);
                    fileInfo.Attributes = FileAttributes.Hidden;

                    if (File.Exists(targetPath))
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.ToString());
                return false;
            }
        }
    }
    public class LoginData
    {
        public LoginData() { }
        public LoginData(string hostName, string loginAccount, string[] ipAddress)
        {
            HostName = hostName;
            LoginAccount = loginAccount;
            IpAddress = ipAddress;
            LoginDateTime = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
        }
        public string HostName { get; set; }
        public string LoginAccount { get; set; }
        public string[] IpAddress { get; set; }
        public string LoginDateTime { get; set; }
    }
}
