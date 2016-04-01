using System;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using TestWX.WeiXin.OAuth.Models;

namespace TestWX.WeiXin.OAuth
{
    public static class ComMethods
    {
        /// <summary>
        /// 模拟http请求并返回结果
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string HttpRequest(string url)
        {
            var wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            wc.Encoding = Encoding.UTF8;

            var returnText = wc.DownloadString(url);

            if (returnText.Contains("errcode"))
            {
                //可能发生错误  
            }

            return returnText;
        }
        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static Token GetToken(string code)
        {
            var url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + WxConfig.Appid + "&secret=" + WxConfig.AppSecret + "&code=" + code + "&grant_type=authorization_code";
            var result = HttpRequest(url);
            var token = JsonConvert.DeserializeObject<Token>(result);

            return token;
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="useRefresh"></param>
        /// <returns></returns>
        public static WXUser GetUserInfo(Token token,bool useRefresh=false)
        {
            if (token == null)
            {
                throw new Exception("token is null");
            }
            var tokenNum = useRefresh ? token.refresh_token : token.access_token;

            var url = "https://api.weixin.qq.com/sns/userinfo?access_token=" + tokenNum + "&openid=" + token.openid;
            var result = HttpRequest(url);
            var userInfo = JsonConvert.DeserializeObject<WXUser>(result);

            return userInfo;
        }
        /// <summary>
        /// 微信登录引导url
        /// </summary>
        /// <param name="redirectUrl"></param>
        /// <returns></returns>
        public static string GetLoginUrl(string redirectUrl)
        {
            redirectUrl = HttpUtility.UrlEncode(redirectUrl);
            var url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid="+WxConfig.Appid+"&redirect_uri="+redirectUrl+"&response_type="+WxConfig.ResponseType+"&scope="+WxConfig.Scope+"&state="+WxConfig.State+"#wechat_redirect";

            return url;
        }
    }
}