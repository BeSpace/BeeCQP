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
            
            var text = (string)HttpUtil.getRequest("http://192.168.1.222:8000/index.php?m=api&f=getSessionID&t=json", "utf-8","utf-8");
            // 嘤嘤嘤，换行，重复消息
            _mahuaApi.SendPrivateMessage(context.FromQq)
                .Text("嘤嘤嘤："+ text)
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
                    var tex2t = (string)HttpUtil.getRequest("http://192.168.1.222:8000/index.php?m=api&f=getSessionID&t=json", "utf-8", "utf-8");
                    Task.Delay(5000).Wait();
                    api.SendPrivateMessage(context.FromQq, tex2t+"开门");
                    //api.SendPrivateMessage(context.FromQq, text);
                }
            });

            // 不要忘记在MahuaModule中注册
        }
    }
}
