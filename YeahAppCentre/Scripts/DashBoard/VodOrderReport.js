$(document).ready(init);

function init() {

    var dayNames = ["日", "一", "二", "三", "四", "五", "六"];
    var monthNames = ["1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月"];

    $('#CompleteTimeRange').daterangepicker({
        timePicker: true,
        timePickerIncrement: 10,
        format: 'YYYY/MM/DD',
        timePicker: false,
        locale: {
            applyLabel: '确定',
            cancelLabel: '取消',
            fromLabel: '从',
            toLabel: '到',
            customRangeLabel: '自定义范围',
            daysOfWeek: dayNames,
            monthNames: monthNames
        },
        ranges: {
            '今天': [moment(), moment()],
            '昨天': [moment().subtract('days', 1), moment().subtract('days', 1)],
            '最近7天': [moment().subtract('days', 6), moment()],
            '最近30天': [moment().subtract('days', 29), moment()],
            '当月': [moment().startOf('month'), moment().endOf('month')],
            '上月': [moment().subtract('month', 1).startOf('month'), moment().subtract('month', 1).endOf('month')]
        },
        separator: ' - ',
        opens: 'right',
        showDropdowns: true,
        buttonClasses: ['btn btn-default'],
        applyClass: 'btn-small btn-primary',
        cancelClass: 'btn-small'
    });

    $('#CompleteTimeRange input').html(moment().startOf('month').format('YYYY/MM/DD') + ' - ' + moment().endOf('month').format('YYYY/MM/DD'));

    // 获取Request值的工具方法
    QueryString = {
        data: {},
        Initial: function () {
            var aPairs, aTmp;
            var queryString = new String(window.location.search);
            queryString = queryString.substr(1, queryString.length); //remove   "?"     
            aPairs = queryString.split("&");
            for (var i = 0; i < aPairs.length; i++) {
                aTmp = aPairs[i].split("=");
                this.data[aTmp[0]] = aTmp[1];
            }
        },
        GetValue: function (key) {
            return this.data[key];
        }
    }

    QueryString.Initial();
    $('#HotelId').attr("value", QueryString.GetValue('hotelId'));

    //$('a[data-optype=edit]').click(
    //    function () {
    //        openDialog(GetDefaultOpt('@OpType.Update.GetValueStr()', GetKey($(this)), QueryString.GetValue('hotelId')));
    //    });

    $(document).delegate("tr [data-optype=edit]", "click",
    function () {
        openDialog(GetDefaultOpt('@OpType.Update.GetValueStr()', GetKey($(this)), QueryString.GetValue('hotelId')));
    });
}

function SetKeyReadonly() {
    $("div.modal-dialog input,select").attr("disabled", "disabled");
}

function GetDefaultOpt(type, key, hotelId) {
    var map = {
        '@OpType.View.GetValueStr()': '@OpType.View.GetDescription()',
        '@OpType.Add.GetValueStr()': '@OpType.Add.GetDescription()',
        '@OpType.Update.GetValueStr()': "@OpType.Update.GetDescription()",
        '@OpType.Delete.GetValueStr()': "@OpType.Delete.GetDescription()"
    };

    var opt = {};
    //opt.title = "VOD支付订单查询 -> " + map[type];
    opt.title = "VOD支付订单查询 -> " + "更新";
    opt.url = $("#PopUrl").val().replace('_id_', key).replace('_type_', 3).replace('_hotelId_', hotelId);

    //if (type != '@OpType.View.GetValueStr()') {
    if (type != 1) {
        opt.saveclick = function () {
            $("div.modal-dialog form").submit();
            event.preventDefault();
            return false;
        };
    }
    return opt;
}

function GetKey(jqObj) {
    var tagName = jqObj[0].tagName;
    if (tagName == "A") {
        var tr = jqObj.closest("tr");
        return tr.find("input.key").val();
    } else if (tagName == "TR") {
        return jqObj.find("input.key").val();
    }
    return '';
}

function Success(data) {
    if (data == "Success") {
        Mc.Msg("保存成功！", function () {
            location.href = location.href;// + "?hotelId=" + $('#HotelId').val();
        });

    } else {
        Mc.Error(data);
    }
}

