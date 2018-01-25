using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace RecordLogin
{
    /// Get Server's Info
    /// </summary>
    public static class ServerInfoUtility
    {
        /// <summary>
        /// Gets the name of the server.
        /// </summary>
        /// <returns>Server name</returns>
        public static string GetServerName()
        {
            return Environment.MachineName;

            // Or can use
            // return System.Net.Dns.GetHostName();
            // return System.Windows.Forms.SystemInformation.ComputerName;
            // return System.Environment.GetEnvironmentVariable("COMPUTERNAME"); 
        }

        /// <summary>
        /// Gets the login account.
        /// </summary>
        /// <returns>Login account</returns>
        public static string GetLoginAccount()
        {
            return System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        }

        /// <summary>
        /// Gets the ip addresses.
        /// </summary>
        /// <returns>ip addresses</returns>
        public static string[] GetIpAddresses()
        {
            string hostName = GetServerName();

            return System.Net.Dns.GetHostAddresses(hostName).Select(i => i.ToString()).ToArray();
        }
    }
    public static class Proc
    {
        public static void PreventMultiExec()
        {
            //取得此process的名稱
            string name = Process.GetCurrentProcess().ProcessName;
            //取得所有與目前process名稱相同的process
            Process[] ps = Process.GetProcessesByName(name);
            //ps.Length > 1 表示此proces以重複執行
            if (ps.Length > 1)
            {
                Environment.Exit(2);
            }
        }
        public static FileAttributes RemoveAttributes(FileAttributes attributes, FileAttributes attributesToRemove)
        {
            return attributes & ~attributesToRemove;
        }
    }
}
