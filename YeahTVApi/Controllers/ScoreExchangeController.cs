using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using YeahTVApi.DomainModel.Models.PointsModels;
using YeahTVApi.DomainModel.Models.ScoreStoreModels;
using YeahTVApi.Common;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Models.DomainModels;
using Newtonsoft.Json;
using YeahTVApiLibrary.Infrastructure.ManagerInterface;
using YeahTVApi.Entity;
using YeahTVApiLibrary.Service.HotelMemberInfoManager;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models.DataModel;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApiLibrary.Filter;
using YeahTVApiLibrary.Infrastructure.FactoryInterface;

namespace YeahTVApi.Controllers
{
    [TvApiErrorFilter]
    [TvApiActionFilter]
    [RoutePrefix("api/ScoreExchange")]
    public class ScoreExchangeController : ApiController
    {
        private readonly IVODOrderManager _vOdOrderManager;
        private readonly IGlobalConfigManager _globalConfigManager;
        private readonly IOpenApiManager _iOpenApiManager;
        private readonly IHotelTypeFactory _hotelTypeFactory;
        private readonly IScoreExchangManager _scoreExchangManager;
        private readonly IConstantSystemConfigManager _constantSystemConfigManager;
        private readonly IOrderQRCodeRecordManager _orderQRCodeRecordManager;

        public ScoreExchangeController(IVODOrderManager vOdOrderManager,
            IConstantSystemConfigManager constantSystemConfigManager,
            IOpenApiManager iOpenApiManager,
            IGlobalConfigManager globalConfigManager,
            IHotelTypeFactory hotelTypeFactory,
            IScoreExchangManager scoreExchangManager,
            IOrderQRCodeRecordManager orderQRCodeRecordManager
            )
        {
            this._vOdOrderManager = vOdOrderManager;
            this._constantSystemConfigManager = constantSystemConfigManager;
            this._iOpenApiManager = iOpenApiManager;
            this._globalConfigManager = globalConfigManager;
            this._hotelTypeFactory = hotelTypeFactory;
            this._scoreExchangManager = scoreExchangManager;
            this._orderQRCodeRecordManager = orderQRCodeRecordManager;

        }

        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetOrder")]
        public ResponseApiData<ResponseScore> GetOrder(PostScore request)
        {
            ResponseScore score = new ResponseScore() { Name = "" };
            ResponseApiData<ResponseScore> response = new ResponseApiData<ResponseScore>() { Data = score };
            try
            {
                if (string.IsNullOrEmpty(request.Memberid) || string.IsNullOrEmpty(request.Orderid) || string.IsNullOrEmpty(request.Token))
                {
                    response.Code = (int)ApiErrorType.ParameterInvalid;
                    response.Message = "参数异常！";
                    return response;
                }

                var order = _vOdOrderManager.GetScoreOrderInfo(request.Orderid);

                if (order == null || order.State != (int)OrderState.Paying)
                {
                    response.Code = (int)ApiErrorType.OrderStateError;
                    response.Message = "订单不存在或状态异常！";
                }
                //拼接票据
                else
                {
                    string ticket = string.Format("{0}{1}", request.Orderid, request.Memberid);

                    string ticketMD5 = ticket.StringToMd5();

                    var Verification = _iOpenApiManager.VerificationToken(new RequestTokenParameter
                    {

                        Ticket = ticketMD5,
                        Code = _globalConfigManager.GetHotelScoreBusinessCode(order.HotelId),
                        Token = request.Token
                    });

                    if (Verification == false)
                    {
                        response.Code = (int)ApiErrorType.TokenError;
                        response.Message = "Token验证错误！";
                    }

                    else
                    {

                        score.Score = _vOdOrderManager.GetIntScoresbyOderId(order.Id);
                        score.Name = order.GoodsName;

                        response.Data = score;
                        response.Code = (int)ApiErrorType.Success;
                        response.Message = "积分返回成功！";
                    }
                }
            }
            catch (Exception ex)
            {
                response.Code = ApiErrorType.System.ToInt();
                response.Message = ex.Message;
            }

            return response;
        }

        /// <summary>
        /// 扣除积分接口
        /// </summary>
        /// <param name="requestScore"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("EnsureOrder")]
        public ResponseApiData<string> EnsureOrder(RequestScore requestScore)
        {
            IHotelMemberInfoManager hotelMemberInfo;
            ScorePointResult mumberScoreInfo = null;
            VODOrder vodOrder = new VODOrder();
            string code;
            bool isEfictiveToken = false;
            isEfictiveToken = CheckToken(requestScore, out vodOrder, out code);
            if (isEfictiveToken)
            {
                if (!string.IsNullOrWhiteSpace(code))
                {
                    hotelMemberInfo = _hotelTypeFactory.MakeHotelType(code);
                    if (hotelMemberInfo != null)
                    {
                        if (vodOrder != null && vodOrder.State == (int)OrderState.Paying)
                        {
                            mumberScoreInfo = hotelMemberInfo.GetTheUserScore(vodOrder.HotelId, requestScore.UserId);
                            if (mumberScoreInfo.Code.ParseAsEnum<StoreEnum>().Equals(StoreEnum.Success))
                            {
                                decimal userScore = _vOdOrderManager.GetIntScoresbyOderId(vodOrder.Id);
                                if (Convert.ToDecimal(mumberScoreInfo.Value) > userScore)
                                {
                                    var requestResult = hotelMemberInfo.CommitOrderRequest(requestScore, vodOrder, userScore);
                                    var returnMessage = requestResult.Item1;
                                    if (returnMessage != null && returnMessage.Code.ParseAsEnum<StoreEnum>().Equals(StoreEnum.Success))
                                    {

                                        SuccessCallback(requestResult, vodOrder);
                                        return SetResponseApiData(ApiErrorType.Success, "兑换成功");
                                    }
                                    else
                                        return SetResponseApiData((EnumExtensions.ParseAsEnum<ApiErrorType>(returnMessage.Code.ParseAsEnum<StoreEnum>().ToString())), returnMessage.Code.Equals(StoreEnum.System) ? "系统异常"
                                            : (EnumExtensions.ParseAsEnum<ApiErrorType>(returnMessage.Code.ParseAsEnum<StoreEnum>().ToString())).GetDescription()
                                        );
                                }
                                else
                                {
                                    return SetResponseApiData(ApiErrorType.ScoreLack, "积分不足");
                                }
                            }
                            else
                            {
                                if (mumberScoreInfo.Code.ParseAsEnum<StoreEnum>().Equals(StoreEnum.System))
                                    return SetResponseApiData(ApiErrorType.System, "系统异常");
                                else
                                    return SetResponseApiData((EnumExtensions.ParseAsEnum<ApiErrorType>(mumberScoreInfo.Code.ParseAsEnum<StoreEnum>().ToString())), mumberScoreInfo.Code.ParseAsEnum<StoreEnum>().GetDescription());
                            }
                        }
                        else
                            return SetResponseApiData(ApiErrorType.OrderStateError, "订单不存在或状态异常！");
                    }
                    else
                    {
                        return SetResponseApiData(ApiErrorType.System, "不支持该酒店类型,不能实例化此类");
                    }
                }
                else
                    return SetResponseApiData(ApiErrorType.System, "没有对应的Code配置项");
            }
            else
                return SetResponseApiData(ApiErrorType.TokenError, "token验证失败");
        }

        /// <summary>
        /// 检查对方签名并给H5颁发token
        /// </summary>
        /// <param name="validateSignCriteria"></param>
        /// <returns></returns>

        [HttpPost]
        [Route("PromulgateToken")]
        public ResponseApiData<PromulgateTokenResult> PromulgateToken(ValidateSignCriteria validateSignCriteria)
        {
            var ret = new ResponseApiData<PromulgateTokenResult>() { Data = new PromulgateTokenResult() { OrderId = "", Token = "" } };
            ret.Code = ApiErrorType.Parameter.ToInt();
            VODOrder orderInfo = null;
            string scoreBusinessCode = string.Empty;
            string orderId = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(validateSignCriteria.Ticket))
                {
                    ret.Message = "Ticket不能为空!";
                }
                else if (string.IsNullOrEmpty(validateSignCriteria.MemberId))
                {
                    ret.Message = "会员号不能为空!";
                }
                else if (string.IsNullOrEmpty(validateSignCriteria.Score))
                {
                    ret.Message = "Score不能为空!";
                }
                else if (string.IsNullOrEmpty(validateSignCriteria.Sign))
                {
                    ret.Message = "Sign不能为空!";
                    ret.Code = ApiErrorType.SignError.ToInt();
                }
                else if (validateSignCriteria.ExpiredMinus == 0 || string.IsNullOrEmpty(validateSignCriteria.ExpiredMinus.ToString()))
                {
                    ret.Message = "过期时间错误!";
                }
                else if (string.IsNullOrWhiteSpace((orderId = _orderQRCodeRecordManager.GetOrderIdByTicket(validateSignCriteria.Ticket))))
                {
                    ret.Message = "Ticket无效！";
                    ret.Code = ApiErrorType.OrderStateError.ToInt();
                }
                else if ((orderInfo = _vOdOrderManager.GetScoreOrderInfo(orderId)) == null || orderInfo.State != (int)OrderState.Paying)
                {
                    ret.Message = "订单不存在或状态异常！";
                    ret.Code = ApiErrorType.OrderStateError.ToInt();
                }
                else if (!ValidateSign(validateSignCriteria, orderInfo.HotelId))
                {
                    ret.Code = ApiErrorType.SignError.ToInt();
                    ret.Message = "Sign Error!";
                }
                else if (string.IsNullOrWhiteSpace((scoreBusinessCode = _globalConfigManager.GetHotelScoreBusinessCode(orderInfo.HotelId))))
                {
                    ret.Message = "code不存在";
                }

                if (string.IsNullOrEmpty(ret.Message))
                {
                    var scoreOrderId= _vOdOrderManager.GetScoreOrderId(orderInfo.Id, orderInfo.HotelId);

                    var datestr = DateTime.Now.ToString("yyyyMMddHHmmss");
                    var ticket = string.Format("{0}{1}", scoreOrderId, validateSignCriteria.MemberId).StringToMd5();
                    var sign = CreateSign(ticket + datestr);
                    var url = _constantSystemConfigManager.OpenApiAddress + Constant.OpenApiGetToken;

                    var http = new HttpHelper() { ContentType = "application/json" };
                    var result = http.Post(url, JsonConvert.SerializeObject(new { authTicket = ticket, type = 1, sign = sign, code = scoreBusinessCode, signTime = datestr, expiredMinus = validateSignCriteria.ExpiredMinus }));
                    var resultObj = JsonConvert.DeserializeObject<ResponseApiData<string>>(result);
                    if (resultObj.Code == (int)ApiErrorType.Success)
                    {
                        ret.Code = ApiErrorType.Success.ToInt();
                        ret.Message = "";
                        ret.Data.Token = resultObj.Data;
                        ret.Data.OrderId = scoreOrderId;
                    }
                    else
                    {
                        ret.Code = ApiErrorType.TokenError.ToInt();
                        ret.Message = "获取Token失败:" + resultObj.Message;
                    }
                }
            }
            catch (Exception err)
            {
                ret.Code = ApiErrorType.System.ToInt();
                ret.Message = err.Message;
            }
            return ret;
        }

        /// <summary>
        /// 操作成功后更新订单状态并记录积分兑换记录
        /// </summary>
        /// <param name="requestResult"></param>
        /// <param name="vodOrder"></param>
        private void SuccessCallback(Tuple<ScorePointResult, RequestJjData> requestResult, VODOrder vodOrder)
        {
            // 更新订单状态为成功
            _vOdOrderManager.OrderPaySuccess(new PaymentInfo()
            {
                Sign = "",
                Message = requestResult.Item2.ToJsonString(),
                ResultCode = PayMentOrderState.Success.GetValueStr(),
                Data = requestResult.Item1.ToJsonString()

            }, new PaymentCallBackData()
            {
                ReturnInfo = new PaymentCallBackReturnInfo()
                {
                    NotifyTime = DateTime.Now.ToString(""),
                    OrderId = vodOrder.Id
                }
            });

            //记录订单所扣积分
            _scoreExchangManager.AddOrUpdateScoreExchang(new ScoreExchang()
            {
                OrderId = vodOrder.Id,
                OrderType = OrderType.VOD.GetText(),
                Productid = _vOdOrderManager.GetPorductId(vodOrder),
                Remark = requestResult.Item2.ToJsonString(),
                Reqtime = requestResult.Item2.Reqtime,
                RunningNumber = requestResult.Item1.Value,
                Score = Convert.ToInt32(requestResult.Item2.Score),
                ScoreRate = _globalConfigManager.GetHotelScoreRate(vodOrder.HotelId)
            });

        }

        /// <summary>
        /// 检查Token
        /// </summary>
        /// <param name="requestScore"></param>
        /// <param name="vodOrder"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private bool CheckToken(RequestScore requestScore, out VODOrder vodOrder, out string code)
        {
            string ticketStr = string.Format("{0}{1}", requestScore.OrderId, requestScore.UserId).StringToMd5();
            vodOrder = _vOdOrderManager.GetScoreOrderInfo(requestScore.OrderId);
            code = _globalConfigManager.GetHotelScoreBusinessCode(vodOrder.HotelId); //根据orderInfo中的HotelId获取GlobalConfig表中的TypeId=hotelId
            return _iOpenApiManager.VerificationToken(new RequestTokenParameter { Code = code, Ticket = ticketStr, Token = requestScore.Token });
        }


        private bool ValidateSign(ValidateSignCriteria validateSignCriteria, string hotelId)
        {
            bool validate = false;

            if (!string.IsNullOrWhiteSpace(hotelId))
            {
                var text = validateSignCriteria.Ticket + validateSignCriteria.MemberId + validateSignCriteria.Score + _globalConfigManager.GetHotelScoreGateWayFrontSignKey(hotelId);
                if (text.StringToMd5().ToUpper() == validateSignCriteria.Sign.ToUpper())
                {
                    validate = true;
                }
            }

            return validate;
        }

        private string CreateSign(string data)
        {
            return new StringBuilder().Append(data).Append(GetSignKey()).ToString().StringToMd5();
        }

        private string GetSignKey()
        {
            return _constantSystemConfigManager.OpenAPIAuthSignPrivateKey;
        }
        private ResponseApiData<string> SetResponseApiData(ApiErrorType apiErrorType, string message, string data = "")
        {
            return new ResponseApiData<string> { Code = apiErrorType.ToInt(), Message = message, Data = data };
        }
    }
}
