using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DotNet4.Utilities;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MACCWlanInfo
{
    class RujieHardAC : MACCBaseClass
    {
        //属性
        public string ACLoginAuth { get; set; }   //锐捷硬AC的登录帐号密码加过密

        //构造器
        public RujieHardAC(string acIP, string acAuth)            
        {
            //在构造器中就先给属性赋值，以方便拿到cookie，供其他环节使用
            this.ACIP = acIP;
            this.ACLoginAuth = acAuth;          
            
            //登录拿cookie
            this.LoginCookie = Login();           
        }

        //实现基类中的抽象方法
        //登录拿Cookie
        public override string Login()
        {
            string strLoginUrl = string.Format("http://{0}:10080/login.do", this.ACIP);
            string strLoginReferer = string.Format("http://{0}:10080/index.htm", this.ACIP);
            string strLoginPost = string.Format("auth={0}", this.ACLoginAuth);  //帐号密码加过密
           
            HttpItem item = new HttpItem()
            {
                URL = strLoginUrl,//URL     必需项    
                Method = "post",//URL     可选项 默认为Get   
                Referer = strLoginReferer,//来源URL     可选项   
                Postdata = strLoginPost,//Post数据     可选项GET时不需要写   
                ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值   
                Allowautoredirect = false,//是否根据301跳转     可选项   
            };

            HttpHelper http = new HttpHelper();
            HttpResult result = http.GetHtml(item);

            return result.Cookie;
        }

        //获取Ap的数量
        public override int GetAPAmount()
        {
            string strGetAmountResult = PostData(10);

            //拿到结果先判断是否正确，错误不往下执行
            //错误原因如：网络中断、服务器出错、服务器禁止访问等
            //判断依据是返回的字符里是否含有Success标记
            if (strGetAmountResult.IndexOf("<return-desc>Success</return-desc>") == -1)
            {
                return 0;
            }

            //AP总数
            string strAPAmount;
            try
            {
                int start = strGetAmountResult.IndexOf("Ap number:");  //定位Ap number:字符
                int end = strGetAmountResult.IndexOf("]]></content>");  //定位]]></content>字符
                strAPAmount = strGetAmountResult.Substring(start + 10, end - start - 10).Replace(" ", "").Replace("\r\n", "");  //截取字符
            }
            catch
            {
                return 0;
            }
            
            int iTotalRecords = Convert.ToInt32(strAPAmount);

            return iTotalRecords;
        }

        //获取AP信息
        public override JArray GetAPInfo()
        {
            //获取AP数量
            int iAPAmount = GetAPAmount();
            if (iAPAmount == 0)
            {
                return null;
            }

            string strAPResults = PostData(iAPAmount); //获取所有AP信息

            //获取到的数据为xml格式，先对字符串进行处理
            //处理完的ss数组格式如下：
            /*
            [0]	"DLJYAP01"	apname
		    [1]	"DLJY"	groupname
		    [2]	"11%"	cpurate
		    [3]	"33%"	memoryrate
		    [4]	"279"	flow
		    [5]	"22"	onlineuser
		    [6]	"32"	ipv4
		    [7]	"192.168.101.2"	ipaddress
		    [8]	"1414.4b54.4da6"	mac
		    [9]	"NA"	location
		    [10]	"Run"	state
             */
            string[] ss;
            try
            {
                strAPResults = strAPResults.Replace("-", "");  //去掉所有-字符
                int start = strAPResults.IndexOf("State");  //定位State字符
                int end = strAPResults.IndexOf("Ap number");  //定位Ap number字符
                strAPResults = strAPResults.Substring(start + 5, end - start - 5);  //截取字符
                ss = strAPResults.Split(); //以空格为界生成数组
                ss = ss.Where(s => !string.IsNullOrEmpty(s)).ToArray(); //使用lambda表达式过滤掉空字符串
            }
            catch 
            {
                return null;
            }

            JArray jsonArray = new JArray();

            for (int i = 0; i < ss.Length; i += 11)
            {
                JObject jsonObj = new JObject();
                jsonObj.Add("apname", ss[i]);
                jsonObj.Add("groupname", ss[i + 1]);
                jsonObj.Add("cpurate", ss[i + 2]);
                jsonObj.Add("memoryrate", ss[i + 3]);
                jsonObj.Add("flow", ss[i + 4]);
                jsonObj.Add("onlineuser", ss[i + 5]);
                jsonObj.Add("ipv4", ss[i + 6]);
                jsonObj.Add("ipaddress", ss[i + 7]);
                jsonObj.Add("mac", ss[i + 8]);
                jsonObj.Add("location", ss[i + 9]);
                jsonObj.Add("state", ss[i + 10]);
                //增加品牌和IP两个字段
                jsonObj.Add("acbrand", "锐捷硬Ac");
                jsonObj.Add("acip", this.ACIP);
                jsonArray.Add(jsonObj);
            }

            return jsonArray;
        }


        //获取数据
        private string PostData(int apnumber)
        {
            string strPostDataUrl = string.Format("http://{0}:10080/web_config.do", this.ACIP);
            string strPostDataReferer = string.Format("http://{0}:10080/index.htm", this.ACIP);
            string strPostData = string.Format("command=show+ap-config+debug+detail+start+1+end+{0}&mode_url=exec", apnumber);

            //获取数据
            HttpItem item = new HttpItem()
            {
                URL = strPostDataUrl,//URL     必需项    
                Method = "post",//URL     可选项 默认为Get   
                Referer = strPostDataReferer,//来源URL     可选项   
                Postdata = strPostData,//Post数据     可选项GET时不需要写   
                ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值   
                Allowautoredirect = false,//是否根据301跳转     可选项    
                Cookie = this.LoginCookie
            };
            item.Header.Add("X-Requested-With", "XMLHttpRequest");
            HttpHelper http = new HttpHelper();
            HttpResult result = http.GetHtml(item);

            return result.Html;
        }
    }
}
