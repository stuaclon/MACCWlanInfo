using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DotNet4.Utilities;
using System.IO;
using System.Web;
using MongoDB.Bson;

namespace MACCWlanInfo
{
    class MACCClass
    {
        public static string GetACInfoForRuijieSoftAC(string ipaddress, string username, string password)
        {
            string strLoginUrl = string.Format("http://{0}/admin/login.loginform", ipaddress);
            string strLoginPost = string.Format("t:formdata=SHrtD6Ji%2BKxJrfuAMBHQFeEhBOY=:H4sIAAAAAAAAAFvzloEVAN3OqfcEAAAA&username={0}&password={1}&t:zoneid=loginFormZone", username, password);

            string strGetDataUrl = string.Format("http://{0}/admin/device/maccindex", ipaddress);

            HttpHelper http = new HttpHelper();
            HttpItem item;
            HttpResult result;

            item = new HttpItem()
            {
                URL = strLoginUrl,//URL     必需项    
                Method = "post",//URL     可选项 默认为Get   
                Referer = string.Format("http://{0}/admin/login", ipaddress),//来源URL     可选项   
                Postdata = strLoginPost,//Post数据     可选项GET时不需要写   
                ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值   
                Allowautoredirect = false,//是否根据301跳转     可选项                  
            };
            item.Header.Add("x-requested-with", "XMLHttpRequest");  //主要就是这一句，服务器只接受ajax请求。
            result = http.GetHtml(item);

            item = new HttpItem()
            {
                URL = strGetDataUrl,//URL     必需项    
                Referer = string.Format("http://{0}/admin/maccmainlayout", ipaddress),//来源URL     可选项                   
                ContentType = "application/json",//返回类型    可选项有默认值   
                Allowautoredirect = false,//是否根据301跳转     可选项    
                Cookie = result.Cookie,
                Accept = "application/json, text/javascript, */*; q=0.01",
            };
            item.Header.Add("X-Requested-With", "XMLHttpRequest");
            result = http.GetHtml(item);

            return result.Html;
        }

        //获取锐捷软AC的AP信息
        public static string GetAPInfoForRuijieSoftAC(string ipaddress,string username,string password,string echo,string displaystart)
        {           
            string strLoginUrl = string.Format("http://{0}/admin/login.loginform",ipaddress);
            string strLoginPost = string.Format("t:formdata=SHrtD6Ji%2BKxJrfuAMBHQFeEhBOY=:H4sIAAAAAAAAAFvzloEVAN3OqfcEAAAA&username={0}&password={1}&t:zoneid=loginFormZone",username,password);

            string strGetDataUrl = string.Format("http://{0}/admin/device/maccdevicemanager.devicetable:data?sEcho={1}&iColumns=10&sColumns=&iDisplayStart={2}&iDisplayLength=10&mDataProp_0=checkbox&mDataProp_1=areaName&mDataProp_2=serialNumber&mDataProp_3=mac&mDataProp_4=modelName&mDataProp_5=userNumber&mDataProp_6=statusMessage&mDataProp_7=lastOnlineTimeMessage&_=1458555601899",ipaddress,echo,displaystart);

            HttpHelper http = new HttpHelper() ;
            HttpItem item;
            HttpResult result;

            item = new HttpItem()
            {
                URL = strLoginUrl,//URL     必需项    
                Method = "post",//URL     可选项 默认为Get   
                Referer = string.Format("http://{0}/admin/login",ipaddress),//来源URL     可选项   
                Postdata = strLoginPost,//Post数据     可选项GET时不需要写   
                ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值   
                Allowautoredirect = false,//是否根据301跳转     可选项                  
            };
            item.Header.Add("x-requested-with", "XMLHttpRequest");  //主要就是这一句，服务器只接受ajax请求。
            result = http.GetHtml(item);

            item = new HttpItem()
            {
                URL = strGetDataUrl,//URL     必需项    
                Referer = string.Format("http://{0}/admin/device/MaccDeviceManager",ipaddress),//来源URL     可选项                   
                ContentType = "application/json",//返回类型    可选项有默认值   
                Allowautoredirect = false,//是否根据301跳转     可选项    
                Cookie = result.Cookie,
                Accept = "application/json, text/javascript, */*; q=0.01",
            };
            item.Header.Add("X-Requested-With", "XMLHttpRequest");
            result = http.GetHtml(item);

            return result.Html;
        }

        //获取康凯AC的AP信息
        public static string GetAPInfoForKangkaiAC(string ipaddress, string username, string password)
        {
            string strLoginUrl = string.Format("http://{0}/nms/login/login", ipaddress);
            string strLoginPost = string.Format("userName={0}&password={1}&verify=", username, password);

            string strGetDataUrl = string.Format("http://{0}/nms/monitor/device/list", ipaddress);

            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = strLoginUrl,//URL     必需项    
                Method = "post",//URL     可选项 默认为Get   
                Referer = string.Format("http://{0}/nms/login.jsp",ipaddress),//来源URL     可选项   
                Postdata = strLoginPost,//Post数据     可选项GET时不需要写   
                ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值   
                Allowautoredirect = false,//是否根据301跳转     可选项   
            };
            HttpResult result = http.GetHtml(item);

            item = new HttpItem()
            {
                URL = strGetDataUrl,//URL必需项     
                Cookie = result.Cookie,
                Referer = string.Format("http://{0}/nms/login.jsp", ipaddress),//来源URL 
                Accept = "application/json, text/javascript, */*; q=0.01"
            };
            result = http.GetHtml(item);
            return  result.Html; //这里就是你要的Html
        }

        //获取锐捷硬AC的AP信息
        public static string GetAPInfoForRuijieHardAC(string ipaddress, string auth, string apnumber)
        {
            string strLoginUrl = string.Format("http://{0}:10080/login.do", ipaddress);
            string strLoginPost = string.Format("auth={0}",auth);  //帐号密码加过密

            string strPostDataUrl = string.Format("http://{0}:10080/web_config.do", ipaddress);
            string strPostData = string.Format("command=show+ap-config+debug+detail+start+1+end+{0}&mode_url=exec", apnumber);

            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = strLoginUrl,//URL     必需项    
                Method = "post",//URL     可选项 默认为Get   
                Referer = string.Format("http://{0}:10080/index.htm", ipaddress),//来源URL     可选项   
                Postdata = strLoginPost,//Post数据     可选项GET时不需要写   
                ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值   
                Allowautoredirect = false,//是否根据301跳转     可选项   
            };
            HttpResult result = http.GetHtml(item);

            //获取数据
            item = new HttpItem()
            {
                URL = strPostDataUrl,//URL     必需项    
                Method = "post",//URL     可选项 默认为Get   
                Referer = string.Format("http://{0}:10080/index.htm", ipaddress),//来源URL     可选项   
                Postdata = strPostData,//Post数据     可选项GET时不需要写   
                ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值   
                Allowautoredirect = false,//是否根据301跳转     可选项    
                Cookie = result.Cookie,
            };
            item.Header.Add("X-Requested-With", "XMLHttpRequest");
            result = http.GetHtml(item);

            return result.Html;

        }

        //获取信锐AC的AP信息
        public static string GetAPInfoForXinruiAC(string ipaddress, string username, string password, string certpath, string displaystart, string displaynumber)
        {
            string strLoginUrl = string.Format("https://{0}/index.php/welcome/login", ipaddress);
            string strLoginPost = string.Format("username={0}&password={1}&times=0", username,password);

            string strPostDataUrl = string.Format("https://{0}/index.php/sys_runstat", ipaddress);
            //string strPostData = string.Format("{\"opr\":\"listap\",\"data\":{\"search\":null,\"start\":{0},\"limit\":{1},\"sort\":\"joinTime\",\"direction\":\"DESC\"}}", displaystart,displaynumber);
            string strPostData = "{\"opr\":\"listap\",\"data\":{\"search\":null,\"start\":" + displaystart +",\"limit\":" + displaynumber + ",\"sort\":\"joinTime\",\"direction\":\"DESC\"}}";

            //登录
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = strLoginUrl,//URL     必需项    
                Method = "post",//URL     可选项 默认为Get   
                Referer = string.Format("https://{0}/index.php/welcome/login",ipaddress),//来源URL     可选项   
                Postdata = strLoginPost,//Post数据     可选项GET时不需要写   
                ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值   
                //Allowautoredirect = false,//是否根据301跳转     可选项  
                CerPath = certpath,
                Expect100Continue = false,
            };
            HttpResult result = http.GetHtml(item);

            //获取数据
            item = new HttpItem()
            {
                URL = strPostDataUrl,//URL     必需项    
                Method = "post",//URL     可选项 默认为Get   
                Referer = string.Format("https://{0}/index.php/welcome/login", ipaddress),//来源URL     可选项                                   
                Postdata = strPostData,
                ContentType = "application/json",//返回类型    可选项有默认值    
                CerPath = certpath,
                Cookie = result.Cookie.Replace("; path=/; secure; HttpOnly", ""),
                KeepAlive = true,
                Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8",
            };
            item.Header.Add("X-Requested-With", "XMLHttpRequest");
            result = http.GetHtml(item);

            return result.Html;

        }

        //将锐捷硬AC的xml格式AP信息转成MongoDB的BsonDocument格式
        public static List<BsonDocument> XmlToBson(string xml,string acbrand,string acip)
        {
            string Xml = xml.Replace("-", "");  //去掉所有-字符
            int start = Xml.IndexOf("State");  //定位State字符
            int end = Xml.IndexOf("Ap number");  //定位Ap number字符
            Xml = Xml.Substring(start + 5, end - start - 5);  //截取字符


            string[] ss = Xml.Split(); //以空格为界生成数组
            ss = ss.Where(s => !string.IsNullOrEmpty(s)).ToArray(); //使用lambda表达式过滤掉空字符串

            List<BsonDocument> documents = new List<BsonDocument>();

            for (int i = 0; i < ss.Length; i += 11)
            {
                BsonDocument document = new BsonDocument();
                document.Add("apname", BsonValue.Create(ss[i]));
                document.Add("groupname", BsonValue.Create(ss[i + 1]));
                document.Add("cpurate", BsonValue.Create(ss[i + 2]));
                document.Add("memoryrate", BsonValue.Create(ss[i + 3]));
                document.Add("flow", BsonValue.Create(ss[i + 4]));
                document.Add("onlineuser", BsonValue.Create(ss[i + 5]));
                document.Add("ipv4", BsonValue.Create(ss[i + 6]));
                document.Add("ipaddress", BsonValue.Create(ss[i + 7]));
                document.Add("mac", BsonValue.Create(ss[i + 8]));
                document.Add("location", BsonValue.Create(ss[i + 9]));
                document.Add("state", BsonValue.Create(ss[i + 10]));
                //增加品牌和IP两个字段
                document.Add("acbrand", BsonValue.Create(acbrand));
                document.Add("acip", BsonValue.Create(acip));
                documents.Add(document);
            }

            return documents;
        }
    }

}
