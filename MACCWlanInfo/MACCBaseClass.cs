using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DotNet4.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core;
using MongoDB.Driver.Linq;


namespace MACCWlanInfo
{
    abstract class MACCBaseClass
    {
        //属性
        public string LoginCookie { get; set; }   //登录后获取的cookie

        public string ACIP { get; set; }  //AC IP
        public string ACUserName { get; set; } //AC帐号
        public string ACPassword { get; set; } //AC密码


        //构造器
        public MACCBaseClass()
        {
        }

        //定义抽象方法
        public abstract string Login();  //登录，获取cookie
        public abstract int GetAPAmount();  //获取AP数量
        public abstract JArray GetAPInfo(); //获取AP信息，返回JSON对象数组

        //将信息存入MongoDB数据库
        public void SaveAPInfo(JArray jsonarray, IMongoCollection<BsonDocument> collection)
        {
            //遍历AP信息数组，将每条AP信息记录都转为Bson格式
            //为避免反复操作数据库，所有数据一次性插入数据库
            List<BsonDocument> documents = new List<BsonDocument>();

            foreach (JObject jsonObj in jsonarray)
            {
                string strBsonDoc = jsonObj.ToString();
                BsonDocument document = BsonDocument.Parse(strBsonDoc);

                documents.Add(document);
            }

            //插入到数据库
            collection.InsertManyAsync(documents);
        }
    }
}
