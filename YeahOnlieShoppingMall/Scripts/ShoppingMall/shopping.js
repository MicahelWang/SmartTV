;

Number.prototype.toFixed = function (len) {
    var tempNum = 0;
    var s, temp;
    var s1 = this + "";
    var start = s1.indexOf(".");

    //截取小数点后,0之后的数字，判断是否大于5，如果大于5这入为1  

    if (s1.substr(start + len + 1, 1) >= 5)
        tempNum = 1;

    //计算10的len次方,把原数字扩大它要保留的小数位数的倍数  
    var temp = Math.pow(10, len);
    //求最接近this * temp的最小数字  
    //floor() 方法执行的是向下取整计算，它返回的是小于或等于函数参数，并且与之最接近的整数  
    s = Math.floor(this * temp) + tempNum;
    return s / temp;
}
var baseStorage = window.localStorage;
var storageCar = baseStorage.Car;
var deviceSeries = "";
var isOrder = false;
function getCategorys() {
    var requestData = {
        "sign": "签名",
        "data": {
            "DeviceSeries": "设备号"
        }
    };
    var categorys = {
        "sign": "签名",
        "data": [
            {
                "id": "分类Id",
                "name": "分类名称",
                "index": "排序",
                "selectImage": "选中状态图片",
                "unselectImage": "未选中状态图片"
            },
            {
                "id": "分类Id",
                "name": "分类名称",
                "index": "排序",
                "selectImage": "选中状态图片",
                "unselectImage": "未选中状态图片"
            }
        ]
    };
    $.ajax({
        async: true,
        type: 'Get',
        url: "/ShoppingMall/GetCategorys",
        data: { "DeviceSeries": deviceSeries },
        dataType: "json",
        success: function (resultResponse) {
            if (resultResponse == "") {
                tipinfo("分类加载失败！");
                return false;
            }
            var result = JSON.parse(resultResponse);
            if (result.data == undefined) {
                tipinfo("分类数据加载失败！");
                return false;
            }
            if (result.data == null) {
                tipinfo("分类数据加载失败！");
                return false;
            }
            if (result != "") {
                if (result.data.sentRoom == false) {
                    $("#nav .sendtype").text("注意：完成支付后，请到前台领取你购买的商品")
                }
                if (result.data.categorys != "") {
                    $("#leftMenu").html("");
                    $.each(result.data.categorys, function (i, v) {
                        $("#leftMenu").append("<li role='presentation'  onfocus='this.blur()'><a href='#GoodsListDiv' categoryid='" + v.id + "' data-toggle='tab' onfocus='this.blur()'><span class='blue'><img src='" + v.selectImage + "' /></span><span class='red'><img src='" + v.unselectImage + "' /></span></a></li>")
                    });
                    $("#leftMenu").find("li").eq(0).addClass("active");
                    getGoodList();
                }
            }
        },
        error: function (a) {
            console.log(a);
            tipinfo("分类加载异常！");
        }
    });
}
function getGoodList(isdown) {
    var requestData = {
        "DeviceSeries": "",
        "CategoryId": $("li.active").find("a").attr("categoryid"),
        "PageIndex": $("._page").attr("pageindex"),
        "PageSize": $("._page").attr("pagesize")
    };
    //
    var goodsList = {
        "sign": "签名",
        "data": {
            "pageTotal": "总页数",
            "pageindex": "当前页码",
            "pagesize": "分页大小",
            "products": [
                  {
                      "id": "商品ID",
                      "number": "商品编号",
                      "name": "商品名称",
                      "price": "商品价格",
                      "on_sale": "是否上架出售 1-上架 0-下架",
                      "brand": "商品品牌",
                      "unit": "单位",
                      "specification": "商品规格",
                      "quantity": "商品数量",
                      "image_url": "图片URL",
                      "description": "商品介绍",
                      "stock_taking_time": "最后盘点时间"
                  }//,
            ]
        }
    };
    $.ajax({
        async: isdown == undefined ? true : false,
        type: 'Get',
        url: "/ShoppingMall/GetGoodList",
        data: requestData,
        dataType: "json",
        success: function (resultResponse) {
            if (resultResponse == "") {
                tipinfo("商品加载失败！");
                return false;
            }
            result = JSON.parse(resultResponse);
            if (result.data == undefined) {
                tipinfo("商品加载失败！");
                return false;
            }
            if (result.data == null) {
                tipinfo("商品数据加载失败！");
                return false;
            }
            if (result.data.products != "") {
                Addlist(result.data.products);
                if (result.data.pageindex * 1 == result.data.pageTotal * 1) {
                    $("._page").attr("isLast", "true");
                }
                if (result.data.pageindex * 1 < result.data.pageTotal * 1) {
                    $("._page").attr("pageindex", result.data.pageindex * 1 + 1);
                    $("._page").show();
                }
                cacheGoods();
                showOrHidePagerImg();
                if (isdown && isdown == true) {
                    down();
                }
            }
            else {                
                $("._page").attr("isLast", "true").hide();
                cacheGoods();
            }

        }, error: function (a) {
            console.log(a);
            tipinfo("商品加载异常！");
        }
    });
    //if ($("._page").attr("isLast") == "false" || $("div.goods", "#GoodsListDiv").length > 9) {
    //    $(".down").css("display", "block");
    //}
    //else {
    //    $(".down").css("display", "none");
    //}
}
//缓存分类-商品列表
function cacheGoods() {
    var catergry = $("li.active").find("a").attr("categoryid");
    var temp = $(".tab-content").clone();
    temp.find("div.changeBG").removeClass("changeBG").end()
        .find("#GoodsListDiv img").attr("src", "/content/img/loading.png").end()
    .find("._page").hide();
    baseStorage.setItem("category_" + catergry, temp.html());

}
function readCacheGoods() {
    var catergry = $("li.active").find("a").attr("categoryid");
    var data = baseStorage.getItem("category_" + catergry);
    if (data == null) {
        return false;
    } else {        
        $(".tab-content").html(data);
        showOrHidePagerImg();
        iniData();
        //setTimeout('iniData()', 100);
        return true;
    }
}

function iniData() {
    $("#GoodsListDiv").find("img").lazyload({
        failure_limit: 10,
        effect: "fadeIn",
        data_attribute: "url"
    });
}
function clearShoppingCar() {
    baseStorage.removeItem("Order");
    baseStorage.removeItem("cacheGood");
    baseStorage.clear();
}
function getGoodsCar() {
    var carArrary = [];
    var goods = $("div.shopping");
    goods.each(function (i, v) {
        var good = {};
        good.id = v.id;
        good.name = v.name;
        good.quantity = v.quantity;
        good.image_url = v.image_url;
        good.price = v.price;
        carArrary.push(good);
    });
    baseStorage.Car = JSON.stringify(carArrary);
}
function tipinfo(a, b) {
    $("#myModal").find("img").attr("src", "/Content/img/erro1.png");
    $("#myModal").find(".ts1").text(a);
    $("#myModal").find(".ts2").text(b || "");
    $("#coverDiv").addClass("in");
    $("#myModal").show();
    $('span.timeClosing').text(5);
    wait = 5;
    timeOut();
    isModal = true;
}
function pay() {
    if (isOrder == true) {
        return false;
    }
    var sum = $("#sum").text() * 1;
    if (sum == 0) {
        tipinfo("您未选择任何商品", "请先选择商品后，再继续操作 :)");
        return false;
    }
    isOrder = true;
    $("#OK").text("下 单 中......");
    var carArrary = {
        "data": [],
        "roomNo": "",
        "payQT": false,
        "sendRoom": true,
        "orderNo": "",
        "total": 0
    };
    var carOrder = {
        "data": {
            "products": [
                {
                    "id": "商品1 ID",
                    "quantity": "购买数量"
                }
            ]
        }
    };
    var goods = $("div.goods", "#Buycar");
    carOrder.data.products = [];
    goods.each(function (i, val) {
        var good = {};
        var v = $(val);
        good.id = v.attr("productId");
        good.quantity = v.find(".snum").text() * 1;
        carOrder.data.products.push(good);
        good.name = v.find("p.sname").text();
        good.image_url = v.find("img").attr("src");
        good.price = v.find("span.sprice").text() * 1;
        carArrary.data.push(good);
       // carArrary.total += (good.quantity * good.price);
    });
     
    carArrary.total = $("#sum").text();     
    //baseStorage.Order = JSON.stringify(carArrary);
    //onPaySuccessCallBack("abc");
    //window.location.href = "ShoppingMall/OrderDetail";
    //return false;

    $.ajax({
        type: 'POST',
        url: "/ShoppingMall/CreateOrder",
        data: {
            "data": carOrder.data
        },
        dataType: 'json',
        async: true,
        success: function (result) {
            isOrder = false;
            if (result == "") {
                tipinfo("下单异常！", "请尝试重新下单！");
                $("#OK").text("马上支付");
                return false;
            }
            var ret = JSON.parse(result);
            if (ret.data == undefined || ret.data == null) {
                tipinfo("下单异常！！！", result);
                $("#OK").text("马上支付");
                return false;
            }
            if (ret.data.resultCode == 2) {//下单成功
                baseStorage.removeItem("Order");
                carArrary.orderNo = ret.data.message;
                carArrary.roomNo = ret.data.roomNo;
                carArrary.sendRoom = ret.data.sendRoom;//true, 送到房间
                baseStorage.Order = JSON.stringify(carArrary);
                startPay(carArrary.orderNo, carArrary.total);
            }
            if (ret.data.resultCode == 1) {//下单异常
                tipinfo("下单异常！");
            }
            if (ret.data.resultCode == 0) {//库存不足,商品ID|商品数量;商品ID|商品数量;商品ID|商品数量
                //tipinfo(ret.data.message);
                var msg = ret.data.message;
                var carGoods = msg.split(';')[0];
                var id = carGoods.split('|')[0];
                var totals = carGoods.split('|')[1];
                var obj = $("#Buycar").find(".goods[productId=" + id + "]");
                var objs = {
                };
                objs.name = obj.find(".sname").text();
                objs.total = totals;
                objs.src = obj.find("img").attr("src");
                objs.num = obj.find(".snum").text();
                message(objs);
                $("#OK").text("马上支付");
            }
            if (ret.data.resultCode == 3) {//商品下架,商品ID|商品图片url|商品名称;商品ID|商品图片url|商品名称
                var msg = ret.data.message;
                var carGoods = msg.split(';')[0].split('|');
                var objs = {
                };
                objs.name = carGoods[2];
                objs.src = carGoods[1];       
                $.each(msg.split(';'), function () {
                    var idcur = this.split('|')[0];
                    $(".goods[productId='" + idcur + "']").remove();
                });
                if (window.localStorage) {
                    window.localStorage.clear();
                }
                getCarTotal();//重新计算购物车总价
                tipinfo("部分商品信息已被修改，请重新下单！");
                $("#OK").text("马上支付");
            }
            if (ret.data.resultCode == 4) {//商品删除,商品ID|商品ID|商品ID
                var msg = ret.data.message;                
                var id = msg.split('|')[0];
                var obj = $("#Buycar").find(".goods[productId=" + id + "]");                
                var name = obj.find(".sname").text(); 
                $.each(msg.split('|'), function () {
                    $(".goods[productId='" +this + "']").remove();
                });
                if (window.localStorage) {
                    window.localStorage.clear();
                    }
                tipinfo(name+" 已删除，请重新下单！");
                $("#OK").text("马上支付");
            }
            window.onfocus();
        },
        error: function (a) {
            tipinfo("下单异常！！！", "请尝试重新下单！");
            isOrder = false;
            $("#OK").text("马上支付");
            window.onfocus();
        }
    });

}

function startPay(oderno, total) {
    //alert("StartPay");
    window.Android.onLaunchPayActivity(oderno, total);
}
function onPaySuccessCallBack(_orderNO, paymathod) {
    var orderObj = JSON.parse(baseStorage.Order);
    orderObj.orderNo = _orderNO;
    orderObj.payQT = paymathod;
    baseStorage.Order = JSON.stringify(orderObj);
    window.location.href = "ShoppingMall/OrderDetail";
    //window.Android.loadWebviewUrl("ShoppingMall/OrderDetail");
}
function Success(result) {
    result = {
        "sign": "签名",
        "data": {
            "resultcode": "返回状态，大于等于0标识成功，小于0标识出错",
            "message": "错误时异常信息，成功时返回订单号"
        }
    };
    if (result.data.resultcode >= 0) {
        alert("下单成功!");
    } else {
        alert(result.data.message);
    }
}
function backKeyCallback() {
    isOrder = false;
    $("#OK").text("马上支付");
    //location.reload();
}

//选择事件
function left() {
    if (col == 0) {
        return false;
        $("#nav").find("li").eq(col1row).removeClass("menuchangeBG");
        $("#GoodsListDiv").find(".goods").removeClass("changeBG");
    }
    else if (col == 1) {
        curIndex = $("div.goods", "#GoodsListDiv").index($("div.changeBG"));
        var rowIndex = $("div.changeBG", "#GoodsListDiv").attr("postionindex");
        var obj = $("#GoodsListDiv").find("div.changeBG").removeClass("changeBG");
        if (rowIndex == 0) {
            $("#nav").find("li.active").removeClass("menuchangeBG");
            col = 0;
        }
        if (rowIndex == 1) {
            obj.prev().addClass("changeBG");
        }
        if (rowIndex == 2) {
            obj.prev().addClass("changeBG");
        }
    }
    else if (col == 2) {
        $("#Showgoods").find(".OKBG,.CarBG").removeClass("OKBG").removeClass("CarBG");
        $("#GoodsListDiv").find(".goods").eq(curIndex).addClass("changeBG");
        col = 1;
        changeTip();
    }
}
function right() {
    if (col == 0) {
        if ($("div.goods", "#GoodsListDiv").length > 9) {
            $(".down").css("display", "block");
        }
        $("#GoodsListDiv").find(".goods").eq(0).addClass("changeBG");
        $("#nav").find("li.active").addClass("menuchangeBG");
        col = 1;
        setScroll("up");
    }
    else if (col == 1) {
        var rowIndex = $("div.goods.changeBG", "#GoodsListDiv").attr("postionindex");
        curIndex = $("div.goods", "#GoodsListDiv").index($("div.changeBG"));
        if (rowIndex < 2) {
            if ($("#GoodsListDiv").find("div.changeBG").next().length > 0) {
                $("#GoodsListDiv").find("div.changeBG").removeClass("changeBG").next().addClass("changeBG");
            } else {
                rowIndex = 2;
            }
        }
        if (rowIndex == 2) {
            $("#GoodsListDiv").find(".goods.changeBG").removeClass("changeBG");
            $("#Showgoods").find(".goods").eq(0).addClass("CarBG");
            if ($("#Showgoods").find(".goods").length == 1) {
                $("#Showgoods").find(".goods").find("button").addClass("OKBG");
            }
            col = 2;
            changeTip();
            $("#Showgoods").find(".CarBG").click();
        }
    }
    else if (col == 2) {
        return false;
    }

}
function up() {
    if (col == 0) {
        if ($("#nav").find("li").index($("li.active")) > 0) {
            $("#nav").find("li.active").removeClass("active").prev().addClass("active").find("a").click();
        }
        return false;
    }
    else if (col == 1) {
        curIndex = $("div.goods", "#GoodsListDiv").index($("div.changeBG"));
        var rowIndex = $("div.goods.changeBG", "#GoodsListDiv").attr("postionindex");
        if (curIndex <= 2) {
            return false;
        } else {
            $("div.goods.changeBG", "#GoodsListDiv").removeClass("changeBG").prevAll("div[postionindex=" + rowIndex + "]").first().addClass("changeBG");
        }
        if ($("div.goods", "#GoodsListDiv").length >= 9) {
            $(".down").css("display", "none");
        }
    }
    else if (col == 2) {
        if ($("div.goods", "#Showgoods").length == 1) {
            return false;
        } else {
            if ($("#Showgoods").find(".OKBG").length == 0) {
                if ($("#Showgoods").find(".CarBG").prev().length > 0)
                    $("#Showgoods").find(".CarBG").removeClass("CarBG").prev().addClass("CarBG");
            } else {
                if ($(".goods", "#Buycar").length > 0) {
                    $("#POK").removeClass("CarBG").find("button").removeClass("OKBG");
                    $(".goods", "#Buycar").last().addClass("CarBG");
                }
            }
        }
        if ($("div.goods.CarBG", "#Buycar").nextAll("div.goods").length > 0 && $("#Buycar").find("div.goods").length > 4) {
            if ($("#Buycar").find("div.goods").index($(".CarBG")) + 1 == $("#Buycar").find("div.goods").length - 4)
            { $(".card").css("display", "block"); }
        }
        else {
            $(".card").css("display", "none");
        }
    }
    setScroll("up");
}
function showOrHidePagerImg()
{
    if ($("._page").attr("isLast") == "false" || $("div.goods", "#GoodsListDiv").length > 9) {
        $(".down").css("display", "block");
    } else {
        $(".down").css("display", "none");
    }
}
function down() {
    var navBtn = $("#nav");
    if (col == 0) {
        if ($("#nav").find("li").length > 0 && $("#nav").find("li.active").next().length > 0) {
            $("#nav").find("li.active").removeClass("active").next().addClass("active").find("a").click();
        }
    }
    else if (col == 1) {
        curIndex = $("div.goods", "#GoodsListDiv").index($("div.changeBG"));
        var rowIndex = $("div.goods.changeBG", "#GoodsListDiv").attr("postionindex");
        if ($("div.goods", "#GoodsListDiv").length <= 3) {
            return false;
        } else {
            var counter = $("div.goods.changeBG", "#GoodsListDiv").nextAll("div[postionindex=" + rowIndex + "]").length;
            if (counter == 0) {
                rowIndex = 0;
                counter = $("div.goods.changeBG", "#GoodsListDiv").nextAll("div[postionindex=0]").length;
              }
            if (counter > 0) {
                $("div.goods.changeBG", "#GoodsListDiv").removeClass("changeBG").nextAll("div[postionindex=" + rowIndex + "]").first().addClass("changeBG");
            } else {
                if ($("._page").attr("isLast") == "false") {
                    //  alert(0);
                    getGoodList(true);
                    // down();
                    return false;
                }
                else if ($("div.goods.changeBG", "#GoodsListDiv").next().length > 0)
                    $("div.goods.changeBG", "#GoodsListDiv").removeClass("changeBG").next().addClass("changeBG");
            }
        }
        //downpager
        if ($("div.goods", "#GoodsListDiv").length > 9) {
            $(".down").css("display", "block");
        }
        if ($("div.goods.changeBG", "#GoodsListDiv").nextAll("div.goods").length < 1) {
            $(".down").css("display", "none");
        }
        if ($("._page").attr("isLast") == "false") {
            $(".down").css("display", "block");
        }

    }
    else if (col == 2) {
        if ($("div.goods", "#Showgoods").length == 1 || $("#Showgoods").find(".OKBG").length > 0) {
            return false;
        } else {
            var carGoods = $("#Showgoods").find(".CarBG").removeClass("CarBG");
            if (carGoods.next().length > 0) {
                carGoods.next().addClass("CarBG");
            } else {
                $("#Showgoods").find(".goods").last().addClass("CarBG").find("button").addClass("OKBG")
            }
        }
        if ($("div.goods.CarBG", "#Buycar").nextAll("div.goods").length > 0 && $("#Buycar").find("div.goods").length > 4) {
            $(".card").css("display", "block");
        }
        else {
            $(".card").css("display", "none");
        }
    }
    setScroll("down");
}
function setScroll(direct) {
    if (col == 1) {
        var goodsContent = $("#GoodsListDiv");
        var postion = $('div.changeBG', goodsContent).position().top;
        var hei = goodsContent.height();
        if (direct == "up") {//up
            if (postion < 200) {
                $('div.changeBG').click();
                return false;
            }
        } else if (postion > hei) {
            var rowIndex = $("div.goods.changeBG", "#GoodsListDiv").attr("postionindex");
            $("div.goods.changeBG", goodsContent).prevAll("div[postionindex=" + rowIndex + "]").eq(1).click();
            // var collum = Math.ceil(($("div.goods", "#GoodsListDiv").index($("div.changeBG")) + 1) / 3) - 3;
            //$('#GoodsListDiv').scrollTop(collum * ($("#GoodsListDiv").height() / 3),500);
        }
    } else if (col == 2) {
        var carContent = $("#Buycar");
        var curent = $('div.CarBG', carContent);
        var hei = carContent.height();
        var prev = $(".CarBG", carContent).prevAll();

        if (direct == "up") {//up
            if (curent.position().top < 0) {
                curent.click();
                return false;
            }
        } else {//down
            if (curent.position().top > ($('div.goods:eq(1)', carContent).position().top - $('div.goods:eq(0)', carContent).position().top) * 3) {
                prev.eq(2).click();
            }
        }
    }
}
function changeTip() {
    if (col == 2) {
        $("p.goodTip").hide();
        $("p.carTip").show();
    } else {
        $("p.goodTip").show();
        $("p.carTip").hide();
    }
}

var wait = 5;
var timeOutHandler;
function timeOut() {
    //wait = 50;
    if (wait == 0) {
        hideModal();

        isModal = false;
        wait = 5;
        window.focus();
    } else {
        clearInterval(timeOutHandler);
        timeOutHandler = setTimeout(function () {
            wait--;
            $('span.timeClosing', "#myModal").replaceWith('<span class="timeClosing" style="color:#fcff21">' + wait + '</span>');
            timeOut();
        }, 1000)
    }
}
function message(objs) {
    $("#myModal").find("img").attr("src", objs.src);
    if (objs.name.length > 9) {
        objs.name = objs.name.substr(0, 8) + "...";
    }
    $("#myModal").find("p.ts1").text("\“" + objs.name + "\”" + "备货不足啦");
    $("#myModal").find("p.ts2").text("共准备了" + objs.total + "件，您已选择了" + objs.num + "件");
    $('span.timeClosing', "#myModal").replaceWith('<span class="timeClosing" style="color:#fcff21">' + 5 + '</span>');
    wait = 5;
    timeOut();
    $("#coverDiv").addClass("in");
    $("#myModal").show();
    isModal = true;
}
function xiajia(objs) {
    $("#myModal").find("img").attr("src", objs.src);
    if (objs.name.length > 9) {
        objs.name = objs.name.substr(0, 8) + "...";
    }
    $("#myModal").find("p.ts1").text("\“" + objs.name + "\”" + " 已下架");
    $("#myModal").find("p.ts2").text("");
    $('span.timeClosing', "#myModal").replaceWith('<span class="timeClosing" style="color:#fcff21">' + 5 + '</span>');
    wait = 5;
    timeOut();
    $("#coverDiv").addClass("in");
    $("#myModal").show();
    isModal = true;
}
function hideModal() {
    $("#coverDiv").removeClass("in");
    $("#myModal").hide();
    isModal = false;
    window.focus();
}