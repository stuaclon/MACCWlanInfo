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
    class KangKaiAC : MACCBaseClass
    {
        //构造器
        public KangKaiAC(string acIP, string acUserName, string acPassword)            
        {
            //在构造器中就先给属性赋值，以方便拿到cookie，供其他环节使用
            this.ACIP = acIP;
            this.ACUserName = acUserName;
            this.ACPassword = acPassword;
            
            //登录拿cookie
            this.LoginCookie = Login();           
        }

        //实现基类中的抽象方法
        //登录拿Cookie
        public override string Login()
        {
            string strLoginUrl = string.Format("http://{0}/nms/login/login", this.ACIP);
            string strLoginReferer = string.Format("http://{0}/nms/login.jsp", this.ACIP);
            string strLoginPost = string.Format("userName={0}&password={1}&verify=", this.ACUserName, this.ACPassword);
         
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = strLoginUrl,//URL     必需项    
                Method = "post",//URL     可选项 默认为Get   
                Referer = strLoginReferer,//来源URL     可选项   
                Postdata = strLoginPost,//Post数据     可选项GET时不需要写   
                ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值   
                Allowautoredirect = false,//是否根据301跳转     可选项   
            };
            HttpResult result = http.GetHtml(item);

            return result.Cookie;
        }

        //获取Ap的数量
        public override int GetAPAmount()
        {
            string strGetAmountResult = PostData(10);

            //拿到结果先判断是否正确，错误不往下执行
            //错误原因如：网络中断、服务器出错、服务器禁止访问等
            JObject jsonObj = new JObject();
            try
            {
                jsonObj = JObject.Parse(strGetAmountResult); //将返回结果反序列化
            }
            catch (JsonException ex)
            {
                return 0;
            }

            //AP总数
            string strAPAmount = jsonObj["pageCount"].ToString();
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


            //拿到结果先判断是否正确，错误不往下执行
            //错误原因如：网络中断、服务器出错、服务器禁止访问等
            JArray devicesJArray = new JArray();
            JObject jsonObjs = new JObject();
            try
            {
                jsonObjs = JObject.Parse(strAPResults); //将返回结果反序列化
                //AP信息数组，AP信息在devices对象里，每个devices对象又由两个对象(info和monitor)组成
                devicesJArray = JArray.Parse(jsonObjs["devices"].ToString());

                foreach (JObject jsonObj in devicesJArray)
                {
                    //增加acbrand(品牌)和acip两个字段
                    jsonObj.Add("acbrand", "康凯AC");
                    jsonObj.Add("acip", this.ACIP);
                }
            }
            catch (JsonException ex)
            {
                return null;
            }

            return devicesJArray;
        }

        //获取数据
        private string PostData(int pageSize)
        {
            string strPostDataUrl = string.Format("http://{0}/nms/monitor/device/list", this.ACIP);
            string strPostDataReferer = string.Format("http://{0}/nms/index", this.ACIP);
            string strPostData = "pageSize=" + pageSize.ToString() + "&pageIndex=1&sort=device_monitor.id+desc&param={\"status\":\"2\", \"searchType\":\"1\",\"searchText\":\"\",\"tags\":null}";


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
