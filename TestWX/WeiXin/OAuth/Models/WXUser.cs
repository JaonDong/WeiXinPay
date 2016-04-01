using System.Drawing;

namespace TestWX.WeiXin.OAuth.Models
{
    // ReSharper disable once InconsistentNaming
    public class WXUser
    {
        // ReSharper disable once InconsistentNaming
        public string openid { get; set; }
        public string nickname { get; set; }
        public string sex { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string headimgurl { get; set; }
        public string unionid { get; set; }
    }
}