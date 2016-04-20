using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DotNet4.Utilities;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System.Threading;

namespace MACCWlanInfo
{
    class RujieSoftAC : MACCBaseClass
    {
        //构造器
        public RujieSoftAC(string acIP, string acUserName, string acPassword)            
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
            string strLoginUrl = string.Format("http://{0}/admin/login.loginform", this.ACIP);
            string strLoginReferer = string.Format("http://{0}/admin/login", this.ACIP);
            string strLoginPost = string.Format("t:formdata=SHrtD6Ji%2BKxJrfuAMBHQFeEhBOY=:H4sIAAAAAAAAAFvzloEVAN3OqfcEAAAA&username={0}&password={1}&t:zoneid=loginFormZone", this.ACUserName,this.ACPassword);

            HttpItem item = new HttpItem()
            {
                URL = strLoginUrl,//URL     必需项    
                Method = "post",//方法     可选项 默认为Get   
                Referer = strLoginReferer,//来源URL     可选项   
                Postdata = strLoginPost,//Post数据     可选项GET时不需要写   
                ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值   
                Allowautoredirect = false,//是否根据301跳转     可选项                  
            };
            item.Header.Add("x-requested-with", "XMLHttpRequest");  //主要就是这一句，服务器只接受ajax请求。

            HttpHelper http = new HttpHelper();
            HttpResult result = http.GetHtml(item);

            return result.Cookie;
        }
       
        //获取Ap的数量
        public override int GetAPAmount()
        {
            string strGetAmountResult = GetData( 0 , 1);
            
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
            string strAPAmount = jsonObj["iTotalRecords"].ToString();
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

            //aaDatas存放所有AP信息
            JArray aaDatas = new JArray();

            //服务器返回的json数据里，只有aaData是需要的，保存在一个JArray数组里面
            //循环获取AP信息
            for (int i = 0; i <= (iAPAmount / 10); i++)
            {
                //getAP信息
                string strAPResults = GetData(i, (i * 10));

                //将返回结果反序列化
                try
                {
                    //将获取到的AP信息结果转为json对象
                    JObject jsonObjs = JObject.Parse(strAPResults);
                    //里面的aaData是需要的
                    JArray aaData = JArray.Parse(jsonObjs["aaData"].ToString());
                    foreach (JObject jsonObj in aaData)
                    {
                        //operate和checkbox字段无用，移除掉
                        jsonObj.Remove("operate");
                        jsonObj.Remove("checkbox");

                        //增加acbrand(品牌)和acip两个字段
                        jsonObj.Add("acbrand", "锐捷软AC");
                        jsonObj.Add("acip", this.ACIP);

                        aaDatas.Add(jsonObj);  //插入到aaDatas里面
                    }
                    
                }
                catch (JsonException ex)
                {
                    break;
                }

                Thread.Sleep(500);  //每次操作延时                                        
            }

            return aaDatas;
        }

        //获取数据
        private string GetData(int echo, int displaystart)
        {
            string strGetUrl = string.Format("http://{0}/admin/device/maccdevicemanager.devicetable:data?sEcho={1}&iColumns=10&sColumns=&iDisplayStart={2}&iDisplayLength=10&mDataProp_0=checkbox&mDataProp_1=areaName&mDataProp_2=serialNumber&mDataProp_3=mac&mDataProp_4=modelName&mDataProp_5=userNumber&mDataProp_6=statusMessage&mDataProp_7=lastOnlineTimeMessage&_=1458555601899", this.ACIP, echo.ToString(), displaystart.ToString());
            string strGetReferer = string.Format("http://{0}/admin/maccmainlayout", this.ACIP);

            HttpItem item = new HttpItem()
            {
                URL = strGetUrl,//URL     必需项    
                Referer = strGetReferer,//来源URL     可选项                   
                ContentType = "application/json",//返回类型    可选项有默认值   
                Allowautoredirect = false,//是否根据301跳转     可选项    
                Cookie = this.LoginCookie,
                Accept = "application/json, text/javascript, */*; q=0.01",
            };
            item.Header.Add("X-Requested-With", "XMLHttpRequest");

            HttpHelper http = new HttpHelper();
            HttpResult result = http.GetHtml(item);

            return result.Html;
        }
    }
}
