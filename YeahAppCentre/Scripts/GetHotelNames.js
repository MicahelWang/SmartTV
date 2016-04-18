function GetHotelNames(url, textSender, sender)
{
    textSender.click(function () {
   
    });

    textSender.keyup(function () {
        if ($(this).val().length == 0)
        {
            sender.val("");
        }
        else
        {
            textSender.val($.trim(textSender.val()));
        }
    });

    var cutDom = textSender.closest("form");
    cutDom.find(":submit").click(function () {
        if (sender.val().length == 0) {
            textSender.val("");
        }

    });

    textSender.typeahead({
        ajax: {
            url: url,
            timeout: 300,                   // 延时
            method: 'get',
            triggerLength: 1,               // 输入几个字符之后，开始请求
            loadingClass: null,             // 加载数据时, 元素使用的样式类
            preDispatch: function (query) {
                var para = {};

                para.query = query;
                sender.val("");

                return para;
            },　　　　　　　　// 发出请求之前，调用的预处理方法
            preProcess: null,
        },
        display: "HotelName",     // 默认的对象属性名称为 name 属性
        val: "HotelId",           // 默认的标识属性名称为 id 属性
        items: 8,
        itemSelected: function (item, val, text) {      // 当选中一个项目的时候，回调函数
            sender.val(val);

            var hotelName = textSender.val();
            //var index = hotelName.indexOf("(", 0);
            //var index = hotelName.indexOf("）", 0);
            textSender.val(hotelName);

            
        }
    });
}


function GetAppNames(url, textSender, sender) {
    console.log(textSender);
    textSender.click(function () {
       
    });

    textSender.keyup(function () {
        if ($(this).val().length == 0) {
            sender.val("");
        }
        else {
            textSender.val($.trim(textSender.val()));
        }
    });

    var cutDom = textSender.closest("form");
    cutDom.find(":submit").click(function () {
        if (sender.val().length == 0) {
            textSender.val("");
            
        }

    });

    textSender.typeahead({
        ajax: {
            url: url,
            timeout: 300,                   // 延时
            method: 'get',
            triggerLength: 1,               // 输入几个字符之后，开始请求
            loadingClass: null,             // 加载数据时, 元素使用的样式类
            preDispatch: function (query) {
                var para = {};

                para.query = query;
                sender.val("");
                console.log(query);
                return para;
            },　　　　　　　　// 发出请求之前，调用的预处理方法
            preProcess: null,
        },
        display: "AppName",     // 默认的对象属性名称为 name 属性
        val: "AppId",           // 默认的标识属性名称为 id 属性
        items: 8,
        itemSelected: function (item, val, text) {      // 当选中一个项目的时候，回调函数
            sender.val(val);
            var hotelName = textSender.val();
            textSender.val(hotelName);
        }
    });
}

function GetMovieNames(url, textSender, sender) {
    
    textSender.click(function () {
       
    });

    textSender.keyup(function () {
        if ($(this).val().length == 0) {
            sender.val("");
        }
        else {
            textSender.val($.trim(textSender.val()));
        }
    });

    var cutDom = textSender.closest("form");
    cutDom.find(":submit").click(function () {
        if (sender.val().length == 0) {
            textSender.val("");

        }

    });

    textSender.typeahead({
        ajax: {
            url: url,
            timeout: 300,                   // 延时
            method: 'get',
            triggerLength: 1,               // 输入几个字符之后，开始请求
            loadingClass: null,             // 加载数据时, 元素使用的样式类
            preDispatch: function (query) {
                var para = {};
                console.log(query);
                para.query = query;
                sender.val("");
                console.log(query);
                return para;
            },　　　　　　　　// 发出请求之前，调用的预处理方法
            preProcess: null,
        },
        display: "MovieName",     // 默认的对象属性名称为 name 属性
        val: "MovieId",           // 默认的标识属性名称为 id 属性
        items: 8,
        itemSelected: function (item, val, text) {      // 当选中一个项目的时候，回调函数
            sender.val(val);
            var hotelName = textSender.val();
            textSender.val(hotelName);
        }
    });




}

function GetUserNames(url, textSender, sender) {
    console.log(textSender);
    textSender.click(function () {

    });

    textSender.keyup(function () {
        if ($(this).val().length == 0) {
            sender.val("");
        }
        else {
            textSender.val($.trim(textSender.val()));
        }
    });

    var cutDom = textSender.closest("form");
    cutDom.find(":submit").click(function () {
        if (sender.val().length == 0) {
            textSender.val("");

        }

    });

    textSender.typeahead({
        ajax: {
            url: url,
            timeout: 300,                   // 延时
            method: 'get',
            triggerLength: 1,               // 输入几个字符之后，开始请求
            loadingClass: null,             // 加载数据时, 元素使用的样式类
            preDispatch: function (query) {
                var para = {};
                para.query = query;
                sender.val("");
                console.log(query);
                return para;
            },　　　　　　　　// 发出请求之前，调用的预处理方法
            preProcess: null,
        },
        display: "UserName",     // 默认的对象属性名称为 name 属性
        val: "UserId",           // 默认的标识属性名称为 id 属性
        items: 8,
        itemSelected: function (item, val, text) {      // 当选中一个项目的时候，回调函数
            sender.val(val);
            var userName = textSender.val();
            textSender.val(userName);
        }
    });
}
