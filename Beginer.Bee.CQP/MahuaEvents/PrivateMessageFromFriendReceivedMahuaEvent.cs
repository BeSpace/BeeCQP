using Newbe.Mahua;
using Newbe.Mahua.MahuaEvents;
using System.Threading.Tasks;
using Beginer.Bee.CQP.BeeUtil;

namespace Beginer.Bee.CQP.MahuaEvents
{
    /// <summary>
    /// 来自好友的私聊消息接收事件
    /// </summary>
    public class PrivateMessageFromFriendReceivedMahuaEvent
        : IPrivateMessageFromFriendReceivedMahuaEvent
    {
        private readonly IMahuaApi _mahuaApi;

        //private readonly HttpWebSocket _httpWebSocket;

        public PrivateMessageFromFriendReceivedMahuaEvent(
            IMahuaApi mahuaApi)
        {
            _mahuaApi = mahuaApi;
        }

        public void ProcessFriendMessage(PrivateMessageFromFriendReceivedContext context)
        {
            // todo 填充处理逻辑
            // 戳一戳
            _mahuaApi.SendPrivateMessage(context.FromQq)
                .Shake()
                .Done();
            
            var text = (string)HttpUtil.getRequest("https://www.baidu.com/s?ie=utf-8&f=8&rsv_bp=1&rsv_idx=1&tn=baidu&wd=webClient&oq=%25E9%2585%25B7Q&rsv_pq=fc6735c100016728&rsv_t=f98e4FBjAbOPAuiNKru1GtR2JHdAl0PJ1PT8D%2BKxtwA3VR8EP24z1XXo3OU&rqlang=cn&rsv_enter=1&inputT=4494&rsv_sug3=15&rsv_sug1=14&rsv_sug7=100&rsv_n=2&rsv_sug2=0&rsv_sug4=5179", "utf-8","utf-8");
            // 嘤嘤嘤，换行，重复消息
            _mahuaApi.SendPrivateMessage(context.FromQq)
                .Text("我是机器人：\r\n"+ text)
                .Newline()
                .Text(context.Message)
                .Done();


            // 异步发送消息，不能使用 _mahuaApi 实例，需要另外开启Session
            Task.Factory.StartNew(() =>
            {
                using (var robotSession = MahuaRobotManager.Instance.CreateSession())
                {
                    var api = robotSession.MahuaApi;
                    api.SendPrivateMessage(context.FromQq, "芝麻");
                    //var tex2t = (string)HttpUtil.getRequest("http://demo.zentao.net/api-getsessionid.json", "utf-8", "utf-8");
                    Task.Delay(5000).Wait();
                    //api.SendPrivateMessage(context.FromQq, tex2t+"开门");
                    //api.SendPrivateMessage(context.FromQq, text);
                }
            });

            // 不要忘记在MahuaModule中注册
        }
    }
}
