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
    class XinRuiAC : MACCBaseClass
    {
        //属性
        public string ACCertPath { get; set; }   //信锐AC是https网站，登录需要证书
        
        //构造器
        public XinRuiAC(string acIP, string acUserName, string acPassword, string acCertPath)            
        {
            //在构造器中就先给属性赋值，以方便拿到cookie，供其他环节使用
            this.ACIP = acIP;
            this.ACUserName = acUserName;
            this.ACPassword = acPassword;
            this.ACCertPath = acCertPath;
            
            //登录拿cookie
            this.LoginCookie = Login();           
        }

        //获取Ap的数量
        public override int GetAPAmount()
        {
            string strGetAmountResult = PostData(0, 10);

            //拿到结果先判断是否正确
            //错误原因如：网络中断、服务器出错、服务器禁止访问等
            JObject jsonObj = new JObject();
            try
            {
                jsonObj = JObject.Parse(strGetAmountResult);
            }
            catch (JsonException ex)
            {
                return 0;
            }

            //AP总数
            string strAPAmount = jsonObj["total"].ToString();
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

            //所有AP信息数组
            JArray apJArrays = new JArray();

            //循环获取AP信息,每次最多只能获取100条记录
            for (int i = 0; i <= iAPAmount / 100; i++)
            {
                try
                {
                    //getAP信息
                    string strAPResults = PostData(i * 100, 100);

                    //将返回结果反序列化
                    JObject jsonObjs = JObject.Parse(strAPResults);
                    //AP信息数组，100个
                    JArray apJArray = JArray.Parse(jsonObjs["data"].ToString());

                    foreach (JObject jsonObj in apJArray)
                    {
                        //增加acbrand(品牌)和acip两个字段
                        jsonObj.Add("acbrand", "信锐AC");
                        jsonObj.Add("acip", this.ACIP);

                        apJArrays.Add(jsonObj);  //插入到所有AP的数组里面
                    }
                }
                catch (JsonException ex )
                {
                    break;
                }               
            }

            return apJArrays;
        }

        //实现基类中的抽象方法
        //登录拿Cookie
        public override string Login()
        {
            string strLoginUrl = string.Format("https://{0}/index.php/welcome/login", this.ACIP);
            string strLoginReferer = string.Format("https://{0}/index.php/welcome/login", this.ACIP);
            string strLoginPost = string.Format("username={0}&password={1}&times=0", this.ACUserName,this.ACPassword);

            //登录
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = strLoginUrl,//URL     必需项    
                Method = "post",//URL     可选项 默认为Get   
                Referer = strLoginReferer,//来源URL     可选项   
                Postdata = strLoginPost,//Post数据     可选项GET时不需要写   
                ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值   
                CerPath = this.ACCertPath,
                Expect100Continue = false,
            };
            HttpResult result = http.GetHtml(item);

            return result.Cookie;
        }

        //获取数据
        private string PostData(int displaystart, int displaynumber)
        {
            string strPostDataUrl = string.Format("https://{0}/index.php/sys_runstat", this.ACIP);
            string strReferer = string.Format("https://{0}/index.php/welcome/login", this.ACIP);
            string strPostData = "{\"opr\":\"listap\",\"data\":{\"search\":null,\"start\":" + displaystart.ToString() + ",\"limit\":" + displaynumber.ToString() + ",\"sort\":\"joinTime\",\"direction\":\"DESC\"}}";

            //获取数据
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = strPostDataUrl,//URL     必需项    
                Method = "post",//URL     可选项 默认为Get   
                Referer = strReferer,//来源URL     可选项                                   
                Postdata = strPostData,
                ContentType = "application/json",//返回类型    可选项有默认值    
                CerPath = this.ACCertPath,
                Cookie = this.LoginCookie.Replace("; path=/; secure; HttpOnly", ""),
                KeepAlive = true,
                Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8",
            };
            item.Header.Add("X-Requested-With", "XMLHttpRequest");
            HttpResult result = http.GetHtml(item);

            return result.Html;
        }
    }
}
