/// <reference path="Mc.Base.js" />

!function ($) {

    $(function () {

        // IE10 viewport hack for Surface/desktop Windows 8 bug
        //
        // See Getting Started docs for more information
        if (navigator.userAgent.match(/IEMobile\/10\.0/)) {
            var msViewportStyle = document.createElement("style");
            msViewportStyle.appendChild(
                document.createTextNode(
                    "@-ms-viewport{width:auto!important}"
                )
            );
            document.getElementsByTagName("head")[0].
                appendChild(msViewportStyle);
        }


        var $window = $(window)
        var $body = $(document.body);

        var navHeight = $('.navbar').outerHeight(true) + 10

        $body.scrollspy({
            target: '.bs-sidebar',
            offset: navHeight
        });

        $window.on('load', function () {
            $body.scrollspy('refresh');
        });

        $('.bs-docs-container [href=#]').click(function (e) {
            e.preventDefault()
        })
        //$(".Edit").delegate("click", function () {


        // back to top
        setTimeout(function () {
            var $sideBar = $('.bs-sidebar');

            $sideBar.affix({
                offset: {
                    top: function () {
                        var offsetTop = $sideBar.offset().top
                        var sideBarMargin = parseInt($sideBar.children(0).css('margin-top'), 10)
                        var navOuterHeight = $('.bs-docs-nav').height()

                        return (this.top = offsetTop - navOuterHeight - sideBarMargin)
                    },
                    bottom: function () {
                        return (this.bottom = $('.bs-footer').outerHeight(true))
                    }
                }
            });
        }, 100);

        setTimeout(function () {
            $('.bs-top').affix();
        }, 100);

        // tooltip demo
        $('body').tooltip({
            selector: "[data-toggle=tooltip]",
            container: "body"
        })

        $('.tooltip-test').tooltip()
        $('.popover-test').popover()

        $('.bs-docs-navbar').tooltip({
            selector: "a[data-toggle=tooltip]",
            container: ".bs-docs-navbar .nav"
        })

        // popover demo
        $("[data-toggle=popover]")
            .popover()

        // button state demo
        $('#fat-btn')
            .click(function () {
                var btn = $(this)
                btn.button('loading')
                setTimeout(function () {
                    btn.button('reset')
                }, 3000)
            })
    });

}(jQuery);


var openDialog = function (opt) {
    var url = opt.url;
    var title = opt.title;
    var option = opt.option;
    var divId = opt.divId;
    var e = opt.saveclick;
    var btns = [

    {
        text: "取消",
        'class': "btn-default",
        click: function () {
            /*your login handler*/

            $(this).dialog("destroy");
        }
    }
    ];
    var cancelBtn = {
        text: "提交",
        'class': "btn-success",
        click: function() {
            var rtn = e(event, this);
            if (rtn != false) {
                $(this).dialog("destroy");
            }
            event.stopPropagation();
            return false;
        }
    };
    if (typeof (e) == "function") {
        btns.push(cancelBtn);
    }
    $.get(url, function (html) {
        var $panle = $(html);
        $panle.dialog({
            title: title
        , onClose: function () {
            // your handler
            $(this).dialog("destroy");
        }, buttons:
            btns.reverse()
        });
    });
}

function customAlert(options) {
    var params = {
        title: "提示",
        content: ""
    };     
    if (typeof (options) == "string") {
        params.content = options;
    }
    var panleFormat = '<div class="alert alert-danger alert-dismissible fade in navbar-fixed-top" role="alert" style="z-index:999999">'
        + '<button type="button" class="close" data-dismiss="alert"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>'
        //+ '<h4>{0}</h4>'
        + '<p>{1}</p>'
        + '</div>';
    var html = panleFormat.format(params.title, params.content);
    var panle = $(html);
    panle.appendTo($("body"));
    panle.alert();

}
$.ajaxSetup({
    statusCode: {
        500: function (data) {
            var errorObj = $.parseJSON(data.responseText);
            $.gritter.add({
                title: '服务器错误',
                text: '请联系管理员，错误ID:' + errorObj.ErrorId,
                class_name: 'gritter-error'
                //                sticky: true,
                //                time: ''
            });

        },
        400: function (data) { //自定义HttpCode 服务端验证出错

            alert("表单验证出错. " + data.responseText);
        },
        499: function (data) { //自定义HttpCode 频繁提交请求

            alert(data.responseText);
        },
        498: function (data) { //自定义HttpCode 498 处理AjaxSession过期

            alert("用户连接超时，请重新登录系统。");
            window.location.href = "/Login";
        }
    }
});



mask = function () {
    return {
        show: function () {
            $("#maskBodyBg").css({ height: $(document).height() + "px" }).show();
        },
        hide: function () {
            $("#maskBodyBg").hide();
        }
    };
}();

//*显示居中对话框+遮罩(id,[true,false])*/
function setDialogCenter(divId, IsMask) {
    if (IsMask) {
        mask.show();
    }

    var divLeft = document.documentElement.clientWidth / 2 - $("#" + divId).width() / 2;
    var divTop = document.documentElement.clientHeight / 2 - $("#" + divId).height() / 2;
    var divScrollTop = document.documentElement.scrollTop + divTop; //当前浏览器可见元素的TOP
    var divScrollLeft = divLeft - document.documentElement.scrollLeft / 2;
    $("#" + divId).animate({ top: divScrollTop + "px", left: divScrollLeft + "px" }, 10)
                  .fadeIn(200);
}

////override dialog's title function to allow for HTML titles
//if ($.widget) {
//    $.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
//        _title: function (title) {
//            var $title = this.options.title || '&nbsp;'
//            if (("title_html" in this.options) && this.options.title_html == true)
//                title.html($title);
//            else title.text($title);
//        }
//    }));
//}


var loadListView = function () {
    var postData = {};
    $.postJson(url, postData, function (data) {
        alert(data);
    });

    var panle = $(' <div class="panel panel-primary"></div>');
    var panelHeading = $('<div class="panel-heading">Pane</div>');
    var panleBody = $('<div class="panel-body"><p>...</p></div>');
    var table = $('<table class="table"></table>');
    var thead = $('<thead><tr></tr></thead>');
    var tboy = $('<tbody></tbody>');
    var tr = $("<tr></tr>");
    var edit = $('<a href="javascript:void(0)" data-href="/User/Edit/1" data-toggle="tooltip" title="" data-original-title="Edit" class="Edit">Edit</a>');
}

function ShowModelDiaLog(html) {
    mask.show();
    var obj = $("<div class='JqDialog' style='position: absolute;display:none'>" + html + "</div>");
    var div = obj.find("div.ui-dialog");
    div.draggable();
    var divLeft = document.documentElement.clientWidth / 2 - div.width() / 2;
    var divTop = document.documentElement.clientHeight / 2 - div.height() / 2;
    var divScrollTop = document.documentElement.scrollTop + divTop; //当前浏览器可见元素的TOP
    var divScrollLeft = divLeft - document.documentElement.scrollLeft / 2;
    $("body").append(obj);
    obj.css("top", divScrollTop + "px");
    obj.css("left", divScrollLeft + "px");
    obj.fadeIn(200);
}


function addComma(nStr) {

    nStr += '';
    x = nStr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;


}

function showLoad() {
    var img = $("#progressImgage");
    img.show().css({
        "position": "fixed",
        "top": "50%",
        "left": "50%",
        "margin-top": function () { return -1 * img.height() / 2; },
        "margin-left": function () { return -1 * img.width() / 2; }
    });
    mask.show();
}

function closeLoad() {
    var img = $("#progressImgage");
    img.hide();
    mask.hide();
}

// showpop
(function ($) {
    // 插件的定义     
    $.fn.showpop = function (options) {
        // build main options before element iteration     
        var opts = $.extend({}, $.fn.showpop.defaults, options);
        // iterate and reformat each matched element     
        return this.each(function () {
            $this = $(this);

            var fun = $this.attr("init");
            var data = options.data;
            if (fun != "") {
                setTimeout(function () {
                    eval(fun + "(data)");
                    setDialogCenter($this.attr("id"), true);
                }, 0);

            }
        });
    };

})(jQuery);


// 修复FormClone 不复制Select
(function ($) {
    // 插件的定义     
    $.fn.cloneForm = function () {

        var oldForm = $(this);
        var newForm = oldForm.clone();

        var $origSelects = $('select', oldForm);
        var $clonedSelects = $('select', newForm);

        $origSelects.each(function (i) {
            $clonedSelects.eq(i).val($(this).val());
        });
        return newForm;
    };

})(jQuery);



jQuery.extend({
    cloneArray: function (originalArray) {
        var clonedArray = $.map(originalArray, function (obj) {
            return $.extend({}, obj);
        });
        return clonedArray;
    },
    cloneArrayDeep: function (originalArray) {
        var clonedArray = $.map(true, originalArray, function (obj) {
            return $.extend({}, obj);
        });
        return clonedArray;
    },
});

(function ($) {
    var re = /([^&=]+)=?([^&]*)/g;
    var decode = function (str) {
        return decodeURIComponent(str.replace(/\+/g, ' '));
    };
    $.parseParams = function (query) {
        var params = {}, e;
        if (query) {
            if (query.substr(0, 1) == '?') {
                query = query.substr(1);
            }

            while (e = re.exec(query)) {
                var k = decode(e[1]);
                var v = decode(e[2]);
                if (params[k] !== undefined) {
                    if (!$.isArray(params[k])) {
                        params[k] = [params[k]];
                    }
                    params[k].push(v);
                } else {
                    params[k] = v;
                }
            }
        }
        return params;
    };
})(jQuery);

// 对Date的扩展，将 Date 转化为指定格式的String    
// 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符，    
// 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字)    
// 例子：    
// (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423    
// (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18    
Date.prototype.Format = function (fmt) { //author: meizz    
    var o = {
        "M+": this.getMonth() + 1,                 //月份    
        "d+": this.getDate(),                    //日    
        "h+": this.getHours(),                   //小时    
        "m+": this.getMinutes(),                 //分    
        "s+": this.getSeconds(),                 //秒    
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度    
        "S": this.getMilliseconds()             //毫秒    
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
};

Date.prototype.addHours = function (h) {
    var copiedDate = new Date(this.getTime());
    copiedDate.setHours(copiedDate.getHours() + h);
    return copiedDate;
};
$.ajaxSetup({
    cache: false
});

function QueryString() {
    var name, value, i;
    var str = location.href;
    var num = str.indexOf("?");
    str = str.substr(num + 1);
    var arrtmp = str.split("&");
    for (i = 0; i < arrtmp.length; i++) {
        num = arrtmp[i].indexOf("=");
        if (num > 0) {
            name = arrtmp[i].substring(0, num);
            value = arrtmp[i].substr(num + 1);
            this[name] = value;
        }
    }
}

function setValidator() {
    if (jQuery.validator) {
        jQuery.validator.setDefaults({
            ignore: '',
            showErrors: function (errorMap, errorList) {

                if (errorList.length > 0) {
                    var data = "";
                    $.each(errorList, function () {
                        //data += this.message + "<br/>";
                        data += this.message + "\n";
                    });
                    alert(data);
                    //Mc.Error(data);
                    //alert(errorList[0].message);
                    return false;
                }
                return true;
            },
            onfocusout: false, onclick: false
        });
    }
}

//Array.prototype.contains = function (obj) {
//    var i = this.length;
//    while (i--) {
//        if (this[i] === obj) {
//            return true;
//        }
//    }
//    return false;
//};


function gd(year, month, day) {
    return new Date(year, month - 1, day).getTime();
}

function convertMonthDay(nS) {
    var date = new Date(nS);
    return (date.getMonth() + 1) + "月" + date.getDate() + "日";
}

function convertYearMonth(nS) {
    var date = new Date(nS);
    return date.getFullYear() + "年" + (date.getMonth() + 1) + "月";
}

function showTooltip(x, y, contents) {
    $("<div id='linetooltip'>" + contents + "</div>").css({
        position: "absolute",
        display: "none",
        top: y + 5,
        left: x + 5,
        border: "1px solid #fdd",
        padding: "2px",
        "background-color": "#fee",
        opacity: 0.80
    }).appendTo("body").fadeIn(200);
}

String.prototype.replaceAll = function (s1, s2) {

    var r = new RegExp(s1.replace(/([\(\)\[\]\{\}\^\$\+\-\*\?\.\"\'\|\/\\])/g, "\\$1"), "ig");
    return this.replace(r, s2);
};

String.prototype.format = function () {
    var args = arguments;
    return this.replace(/\{(\d+)\}/g,
        function (m, i) {
            return args[i];
        });
};

String.prototype.ltrim = function (p1) {

    var j;
    for (j = 0; this.charAt(j) == p1; j++) {
    }
    return this.substring(j, this.length);
};


$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name] !== undefined) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};



function GetDateDifference(start, end) {
    // Parse the entries
    var startDate = Date.parse(start.replace('-', '/').replace('-', '/'));
    var endDate = Date.parse(end.replace('-', '/').replace('-', '/'));

    // Check the date range, 86400000 is the number of milliseconds in one day
    var difference = (endDate - startDate) / (86400000);

    return difference;
}

setValidator();

$(function () {

    lrFixFooter("footer");	//调用方法：lrFixFooter("div.footerwarp"); 传入底部的类名或者ID名

    function lrFixFooter(obj) {
        var footer = $(obj), doc = $(document);
        function fixFooter() {
            if (doc.height() - 4 <= $(window).height()) {
                footer.css({
                    width: "100%",
                    position: "absolute",
                    left: 0,
                    bottom: 0
                });
            } else {
                footer.css({
                    position: "static"
                });
            }
        }
        fixFooter();
        $(window).on('resize.footer', function () {
            fixFooter();
        });
        $(window).on('scroll.footer', function () {
            fixFooter();
        });

    }

})