using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TestWX.WeiXin.OAuth;
using TestWX.WeiXin.Pay;
using TestWX.WeiXin.Pay.Com;
using WxPayAPI;

namespace TestWX.Controllers
{
    public class HomeController : Controller
    {
        static string log = "";
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Redirect(string code = "", string state = "")
        {

            if (code == "")
            {
                return View();
            }
            var token = ComMethods.GetToken(code);
            var user = ComMethods.GetUserInfo(token);
            Session["user"] = user;
            log = "code:" + code + "|token:" + token.access_token + "|user:" + user.nickname;


            return RedirectToAction("Product");
        }

        public ActionResult Product()
        {
            var model = Session["user"];

            return View(model);
        }

        public ActionResult Loger()
        {
            var model = log;

            return View(model);
        }

        public ActionResult Pay(string openId,string total)
        {
            Log.Info(this.GetType().ToString(), "page load");
            var wxJsApiParam = "";
            //检测是否给当前页面传递了相关参数
            if (string.IsNullOrEmpty(openId) || string.IsNullOrEmpty(total))
            {
                Log.Error(this.GetType().ToString(), "This page have not get params, cannot be inited, exit...");
                Response.Write("<span style='color:#FF0000;font-size:20px'>" + "页面传参出错,请返回重试" + "</span>");
            }
            else
            {
                //若传递了相关参数，则调统一下单接口，获得后续相关接口的入口参数
                JsApiPay jsApiPay = new JsApiPay();
                jsApiPay.openid = openId;
                jsApiPay.total_fee = int.Parse(total);
                Log.Info("jsapi","try上面");
                //JSAPI支付预处理
                try
                {
                    WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult();
                    Log.Info("jsapi", "unifiedOrderResult下面");
                    wxJsApiParam = jsApiPay.GetJsApiParameters();//获取H5调起JS API参数                    
                    Log.Debug(this.GetType().ToString(), "wxJsApiParam : " + wxJsApiParam);
                    //在页面上显示订单信息
                    Response.Write("<span style='color:#00CD00;font-size:20px'>订单详情：</span><br/>");
                    Response.Write("<span style='color:#00CD00;font-size:20px'>" + unifiedOrderResult.ToPrintStr() + "</span>");
                    ViewBag.jsApiPay = jsApiPay;
                }
                catch (Exception ex)
                {
                    Response.Write("<span style='color:#FF0000;font-size:20px'>" + "下单失败，请返回重试" + "</span>");
                }
            }
           
            ViewBag.wxJsApiParam =  wxJsApiParam;
            return View();

        }

        public ActionResult Result()
        {
            var re=new ResultNotify(this.HttpContext);
            re.ProcessNotify();
            return View();
        }
    }
}