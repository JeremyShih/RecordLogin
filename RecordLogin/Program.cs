using System.IO;
using Newtonsoft.Json;

namespace RecordLogin
{
    class Program
    {
        static void Main(string[] args)
        {
            Proc.PreventMultiExec();
            Configure config = new Configure();
            config.SetExecAtStartUp();
#if DEBUG
            config.ConfigDir = @"D:\000000";
#endif
            LoginData info = new LoginData(ServerInfoUtility.GetServerName()
                , ServerInfoUtility.GetLoginAccount(), ServerInfoUtility.GetIpAddresses());
            SubmitInfo(config.ConfigFilePath, info);
        }
        static void SubmitInfo(string filePath, LoginData logindata)
        {
            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine(JsonConvert.SerializeObject(logindata, Formatting.Indented));
            }
            FileInfo fileInfo = new FileInfo(filePath);
#if !DEBUG
            fileInfo.Attributes = FileAttributes.Hidden;
#else
            FileAttributes attributes = File.GetAttributes(filePath);
            attributes = Proc.RemoveAttributes(attributes, FileAttributes.Hidden);
            File.SetAttributes(filePath, attributes);
#endif
        }
    }
}
