using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Configuration;
using Microsoft.Win32;

namespace MACCWlanInfo
{
    abstract class Util
    {
        //保存config文件
        public static  void SaveConfig(string key, string value)
        {
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            cfa.AppSettings.Settings[key].Value = value;
            cfa.Save();
        }

        //设置/取消开机自启动
        public static void SetAutoRun(bool isAutoRun)
        {
            RegistryKey reg = null;
            string path = Application.ExecutablePath;
            try
            {
                reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                if (reg == null)
                    reg = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                if (isAutoRun)
                    reg.SetValue("MACCWlanInfo", path);
                else
                    reg.DeleteValue("MACCWlanInfo", false);
            }
            catch
            {
                //throw new Exception(ex.ToString());  
            }
            finally
            {
                if (reg != null)
                    reg.Close();
            }
        }
    }
}
