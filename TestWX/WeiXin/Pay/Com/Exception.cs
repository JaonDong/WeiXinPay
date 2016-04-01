using System;

namespace TestWX.WeiXin.Pay.Com
{
    public class WxPayException : Exception 
    {
        public WxPayException(string msg) : base(msg) 
        {

        }
     }
}