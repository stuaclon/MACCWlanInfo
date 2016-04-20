using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DotNet4.Utilities;
using System.IO;
using System.Web;
using System.Threading;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core;
using MongoDB.Driver.Linq;

using System.Text.RegularExpressions;
using System.Configuration;

using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace MACCWlanInfo
{
    public partial class Form1 : Form
    {
        private BackgroundWorker bgwRuijieSoftAC;  //锐捷软AC
        private BackgroundWorker bgwRuijieHardAC;  //锐捷硬AC
        private BackgroundWorker bgwKangkaiAC;  //康凯AC
        private BackgroundWorker bgwXinruiAC;  //信锐AC

        //数据库相关信息
        private string strDatabaseAddress = ConfigurationManager.AppSettings["DatabaseAddress"]; //地址
        private string strDatabaseName = ConfigurationManager.AppSettings["DatabaseName"]; //名称
        //数据库里面存放AP信息的Collection名称
        private string strRuijieSoftAP = ConfigurationManager.AppSettings["RuijieSoftAP"]; //锐捷软AC
        private string strRuijieHardAP = ConfigurationManager.AppSettings["RuijieHardAP"]; //锐捷硬AC
        private string strKangkaiAP = ConfigurationManager.AppSettings["KangkaiAP"]; //康凯AC
        private string strXinruiAP = ConfigurationManager.AppSettings["XinruiAP"]; //信锐AC

        //定时器
        System.Timers.Timer timer = new System.Timers.Timer();

        public Form1()
        {
            InitializeComponent();

            //初始化获取锐捷软AC信息的线程
            bgwRuijieSoftAC = new BackgroundWorker();
            bgwRuijieSoftAC.DoWork += bgwRuijieSoftAC_DoWork;
            bgwRuijieSoftAC.RunWorkerCompleted += bgwRuijieSoftAC_RunWorkerCompleted;
            //允许取消
            bgwRuijieSoftAC.WorkerSupportsCancellation = true;

            //初始化获取锐捷硬AC信息的线程
            bgwRuijieHardAC = new BackgroundWorker();
            bgwRuijieHardAC.DoWork += bgwRuijieHardAC_DoWork;
            bgwRuijieHardAC.RunWorkerCompleted += bgwRuijieHardAC_RunWorkerCompleted;
            //允许取消
            bgwRuijieHardAC.WorkerSupportsCancellation = true;

            //初始化获取康凯AC信息的线程
            bgwKangkaiAC = new BackgroundWorker();
            bgwKangkaiAC.DoWork += bgwKangkaiAC_DoWork;
            bgwKangkaiAC.RunWorkerCompleted += bgwKangkaiAC_RunWorkerCompleted;
            //允许取消
            bgwKangkaiAC.WorkerSupportsCancellation = true;

            //初始化获取信锐AC信息的线程
            bgwXinruiAC = new BackgroundWorker();
            bgwXinruiAC.DoWork += bgwXinruiAC_DoWork;
            bgwXinruiAC.RunWorkerCompleted += bgwXinruiAC_RunWorkerCompleted;
            //允许取消
            bgwXinruiAC.WorkerSupportsCancellation = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {        
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            //开机自动运行
            string strAutoRun = ConfigurationManager.AppSettings["AutoRun"];
            cbAutoRun.Checked = Convert.ToBoolean(strAutoRun);
            Util.SetAutoRun(cbAutoRun.Checked);

            //自动抓取
            string strAutoGet = ConfigurationManager.AppSettings["AutoGetData"];
            cbAutoGet.Checked = Convert.ToBoolean(strAutoGet);
            if (cbAutoGet.Checked)
            {
                btnStartAuto_Click(sender, e); 
            }

            //自动抓取间隔
            string strGetDataInterval = ConfigurationManager.AppSettings["GetDataInterval"];
            tbGetDataInterval.Text = strGetDataInterval;

            //运行后最小化
            string strMinimize = ConfigurationManager.AppSettings["Minimize"];
            cbMinimize.Checked = Convert.ToBoolean(strMinimize);
            if (cbMinimize.Checked)
            {
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
                this.Hide();
            }
        } 

        private void bgwRuijieSoftAC_DoWork(object sender, DoWorkEventArgs e)
        {
            string strACIP = "119.145.135.155";
            string strUsername = "cirrus";
            string strPassword = "0757$ctel";

            //首先请求一次，获取AP总数
            this.Invoke((MethodInvoker)delegate
            {
                tbResultOut.AppendText(DateTime.Now.ToString() + "------开始锐捷软AC抓取任务。------" + "\r\n");
                tbResultOut.AppendText(DateTime.Now.ToString() + "正在连接数据库..." + "\r\n");
            });

            //连接MongoDB数据库
            var client = new MongoClient(strDatabaseAddress);

            //发送一个列表请求看数据库是否连接成功
            try
            {
                client.ListDatabases();
            }
            catch (TimeoutException ex)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    tbResultOut.AppendText(DateTime.Now.ToString() + "数据库连接失败。\r\n");
                });
                return;
            }

            //获取数据库和collections
            var database = client.GetDatabase(strDatabaseName);
            var Collection = database.GetCollection<BsonDocument>(strRuijieSoftAP);

            this.Invoke((MethodInvoker)delegate
            {
                tbResultOut.AppendText(DateTime.Now.ToString() + "数据库连接成功。\r\n");
                tbResultOut.AppendText(DateTime.Now.ToString() + "正在获取AP数量……\r\n");
            });

            RujieSoftAC rujieSoftAC = new RujieSoftAC(strACIP, strUsername, strPassword);
            int iAPAmount = rujieSoftAC.GetAPAmount();
 
            this.Invoke((MethodInvoker)delegate
            {
                tbResultOut.AppendText(DateTime.Now.ToString() + "AP总数为" + iAPAmount.ToString()+ "个\r\n");
                tbResultOut.AppendText(DateTime.Now.ToString() + "正在获取AP信息，请稍候...\r\n");
            });

            JArray aparray = rujieSoftAC.GetAPInfo();
            try 
            { 
                rujieSoftAC.SaveAPInfo(aparray, Collection); 
            }
            catch(NullReferenceException ex)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    tbResultOut.AppendText(DateTime.Now.ToString() + "AP信息获取失败。\r\n");
                });
            }
                    

            this.Invoke((MethodInvoker)delegate
            {
                tbResultOut.AppendText(DateTime.Now.ToString() + "------锐捷软AC任务完成。------\r\n");
            });
        }

        private void bgwRuijieSoftAC_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bgwRuijieHardAC.RunWorkerAsync();
        }


        private void bgwRuijieHardAC_DoWork(object sender, DoWorkEventArgs e)
        {
            string strACIP = "61.142.245.163";
            string strAuth = "Y2lycnVzOjA3NTdjdGVs";

            this.Invoke((MethodInvoker)delegate
            {
                tbResultOut.AppendText(DateTime.Now.ToString() + "★★★★★开始锐捷硬AC抓取任务。★★★★★" + "\r\n");
                tbResultOut.AppendText(DateTime.Now.ToString() + "正在连接数据库……\r\n");
            });

            //连接数据库
            var client = new MongoClient(strDatabaseAddress);

            //发送一个列表请求看数据库是否连接成功
            try
            {
                client.ListDatabases();
            }
            catch (TimeoutException ex)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    tbResultOut.AppendText(DateTime.Now.ToString() + "数据库连接失败。\r\n");
                });
                return;
            }

            //获取数据库和collections
            var database = client.GetDatabase(strDatabaseName);
            var Collection = database.GetCollection<BsonDocument>(strRuijieHardAP);

            this.Invoke((MethodInvoker)delegate
            {
                tbResultOut.AppendText(DateTime.Now.ToString() + "数据库连接成功。\r\n");
                tbResultOut.AppendText(DateTime.Now.ToString() + "正在获取AP数量……\r\n");
            });

            RujieHardAC rujieHardAC = new RujieHardAC(strACIP, strAuth);
            int iAPAmount = rujieHardAC.GetAPAmount();

            this.Invoke((MethodInvoker)delegate
            {
                tbResultOut.AppendText(DateTime.Now.ToString() + "AP总数为" + iAPAmount.ToString() + "个\r\n");
                tbResultOut.AppendText(DateTime.Now.ToString() + "正在获取AP信息，请稍候...\r\n");
            });

            JArray aparray = rujieHardAC.GetAPInfo();
            try
            {
                rujieHardAC.SaveAPInfo(aparray, Collection);
            }
            catch (NullReferenceException ex)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    tbResultOut.AppendText(DateTime.Now.ToString() + "AP信息获取失败。\r\n");
                });
            }


            this.Invoke((MethodInvoker)delegate
            {
                tbResultOut.AppendText(DateTime.Now.ToString() + "★★★★★锐捷硬AC任务完成。★★★★★\r\n");
            });
        }

        private void bgwRuijieHardAC_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bgwKangkaiAC.RunWorkerAsync();
        }


        private void bgwKangkaiAC_DoWork(object sender, DoWorkEventArgs e)
        {
            string strACIP = "119.145.135.152";
            string strUsername = "xfs@gdcirrus.com";
            string strPassword = "commsky";
                       
            this.Invoke((MethodInvoker)delegate
            {
                tbResultOut.AppendText(DateTime.Now.ToString() + "☆☆☆☆☆开始康凯AC抓取任务。☆☆☆☆☆" + "\r\n");
                tbResultOut.AppendText(DateTime.Now.ToString() + "正在连接数据库……\r\n");
            });

            //连接数据库
            var client = new MongoClient(strDatabaseAddress);

            //发送一个列表请求看数据库是否连接成功
            try
            {
                client.ListDatabases();
            }
            catch (TimeoutException ex)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    tbResultOut.AppendText(DateTime.Now.ToString() + "数据库连接失败。\r\n");
                });
                return;
            }

            //获取数据库和collections
            var database = client.GetDatabase(strDatabaseName);
            var Collection = database.GetCollection<BsonDocument>(strKangkaiAP);

            this.Invoke((MethodInvoker)delegate
            {
                tbResultOut.AppendText(DateTime.Now.ToString() + "数据库连接成功。\r\n");
                tbResultOut.AppendText(DateTime.Now.ToString() + "正在获取AP数量……\r\n");
            });

            KangKaiAC kangKaiAC = new KangKaiAC(strACIP, strUsername, strPassword);
            int iAPAmount = kangKaiAC.GetAPAmount();

            this.Invoke((MethodInvoker)delegate
            {
                tbResultOut.AppendText(DateTime.Now.ToString() + "AP总数为" + iAPAmount.ToString() + "个\r\n");
                tbResultOut.AppendText(DateTime.Now.ToString() + "正在获取AP信息，请稍候...\r\n");
            });

            JArray aparray = kangKaiAC.GetAPInfo();
            try
            {
                kangKaiAC.SaveAPInfo(aparray, Collection);
            }
            catch (NullReferenceException ex)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    tbResultOut.AppendText(DateTime.Now.ToString() + "AP信息获取失败。\r\n");
                });
            }


            this.Invoke((MethodInvoker)delegate
            {
                tbResultOut.AppendText(DateTime.Now.ToString() + "☆☆☆☆☆康凯AC任务完成。☆☆☆☆☆\r\n");
            });
        }

        private void bgwKangkaiAC_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bgwXinruiAC.RunWorkerAsync();
        }


        private void bgwXinruiAC_DoWork(object sender, DoWorkEventArgs e)
        {
            string strACIP = "119.145.135.151";
            string strUsername = "fssn";
            string strPassword = "fsggwifi@123";
            string strCerPath = System.Environment.CurrentDirectory + "\\cert\\xinruiac.cer";   

            this.Invoke((MethodInvoker)delegate
            {
                tbResultOut.AppendText(DateTime.Now.ToString() + "◆◆◆◆◆开始信锐AC抓取任务。◆◆◆◆◆" + "\r\n");
                tbResultOut.AppendText(DateTime.Now.ToString() + "正在连接数据库……\r\n");
            });

            //连接数据库
            var client = new MongoClient(strDatabaseAddress);

            //发送一个列表请求看数据库是否连接成功
            try
            {
                client.ListDatabases();
            }
            catch (TimeoutException ex)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    tbResultOut.AppendText(DateTime.Now.ToString() + "数据库连接失败。\r\n");
                });
                return;
            }

            //获取数据库和collections
            var database = client.GetDatabase(strDatabaseName);
            var Collection = database.GetCollection<BsonDocument>(strXinruiAP);

            this.Invoke((MethodInvoker)delegate
            {
                tbResultOut.AppendText(DateTime.Now.ToString() + "数据库连接成功。\r\n");
                tbResultOut.AppendText(DateTime.Now.ToString() + "正在获取AP数量……\r\n");
            });

            XinRuiAC xinRuiAC = new XinRuiAC(strACIP, strUsername,strPassword,strCerPath);          
            int iAPAmount = xinRuiAC.GetAPAmount();

            this.Invoke((MethodInvoker)delegate
            {
                tbResultOut.AppendText(DateTime.Now.ToString() + "AP总数为" + iAPAmount.ToString() + "个\r\n");
                tbResultOut.AppendText(DateTime.Now.ToString() + "正在获取AP信息，请稍候...\r\n");
            });

            JArray aparray = xinRuiAC.GetAPInfo();
            try
            {
                xinRuiAC.SaveAPInfo(aparray, Collection);
            }
            catch (NullReferenceException ex)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    tbResultOut.AppendText(DateTime.Now.ToString() + "AP信息获取失败。\r\n");
                });
            }


            this.Invoke((MethodInvoker)delegate
            {
                tbResultOut.AppendText(DateTime.Now.ToString() + "◆◆◆◆◆信锐AC任务完成。◆◆◆◆◆\r\n");
            });
        }

        private void bgwXinruiAC_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }      

        //开始自动获取
        private void btnStartAuto_Click(object sender, EventArgs e)
        {
            timer.Interval = (Convert.ToInt32(tbGetDataInterval.Text))*60*1000;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer_Elapsed(null, null);
            timer.Start();

            btnStopAuto.Enabled = true;
            btnStartAuto.Enabled = false;           
        }

        public void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            bgwRuijieSoftAC.RunWorkerAsync();
        }

        //停止自动获取
        private void btnStopAuto_Click(object sender, EventArgs e)
        {
            bgwRuijieSoftAC.CancelAsync();
            bgwRuijieHardAC.CancelAsync();
            bgwKangkaiAC.CancelAsync();
            bgwXinruiAC.CancelAsync();
            timer.Stop();
            btnStopAuto.Enabled = false;
            btnStartAuto.Enabled = true; 
        }

        //最小化到任务栏
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                this.Hide();               
            }
        }

        //截获点击窗口关闭按钮的动作，不让窗口关闭
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        // 重写OnClosing使点击关闭按键时窗体能够缩进托盘
        protected override void OnClosing(CancelEventArgs e)
        {
            this.ShowInTaskbar = false;
            this.WindowState = FormWindowState.Minimized;
            e.Cancel = true;
        }

        //任务栏图标右键退出菜单
        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        //任务栏图标右键关于菜单
        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAbout formAbout = new FormAbout();
            formAbout.ShowDialog();
        }

        //点击任务栏图标弹出程序
        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            this.ShowInTaskbar = true;
            this.WindowState = FormWindowState.Normal;
        }

        //自启动勾选改变时
        private void cbAutoRun_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAutoRun.Checked)
            {
                Util.SaveConfig("AutoRun", "true");
                Util.SetAutoRun(true);
            }
            else
            {
                Util.SaveConfig("AutoRun", "false");
                Util.SetAutoRun(false);
            }
        }

        //自动抓取勾选改变时
        private void cbAutoGet_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAutoGet.Checked)
            {
                Util.SaveConfig("AutoGetData", "true");
            }
            else
            {
                Util.SaveConfig("AutoGetData", "false");
            }
        }

        //启动后最小化改变时
        private void cbMinimize_CheckedChanged(object sender, EventArgs e)
        {
            if (cbMinimize.Checked)
            {
                Util.SaveConfig("Minimize", "true");
            }
            else
            {
                Util.SaveConfig("Minimize", "false");
            }
            
        }

        //抓取间隔改变时
        private void tbGetDataInterval_TextChanged(object sender, EventArgs e)
        {
            //通过正则判断是否正整数
            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[0-9]\d*$");
            if (!reg1.IsMatch(tbGetDataInterval.Text))
            {
                MessageBox.Show("输入错误。");
                return;
            }

            Util.SaveConfig("GetDataInterval", tbGetDataInterval.Text);
        }          
        
    }
}
