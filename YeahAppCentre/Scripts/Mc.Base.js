(function (scope, undefined, anonymous) {
    var anonymous = function () { };

    Mc = scope.Mc || {};

    if (Mc.loaded) return false;  //已载入
    scope.document.window = scope; //设置一个window到document的引用

    /* 版本信息 */
    Mc.name = '5173 Javascript Core';
    Mc.version = '2.0.0';
    Mc.toString = function () { return '[' + this.name + '] ver.' + this.version };

    /*
    Mc命名空间下的本地方法列表
    本地方法是指在子框架页面使用顶层McBase的时候，需要传递子框架页面document对象的方法
    */
    Mc.localMethods = 'paintRow,fixBgCache,open,ready,AJAX,AJAXGet,AJAXPost,loadJs,loadCss'.split(',');

    /*
    主机名
    */
    Mc.Host = 'http://gi.5173.com/';

    /* 
    缓存系统 主要用于绑定事件时使用(修复ie的attachEvent不绑定this和执行顺序不确定的bug)
    虽然也可以作为元素的数据绑定，但是一般不使用
    */
    var Cache = function (obj) {
        var cacheId = '', cacheName = '_Mc_Cache_ID_';
        //window
        if (obj == obj.self && obj.document) cacheId = 'window_' + obj.location.href;
            //document
        else if (obj.window && (obj == obj.window.document)) cacheId = 'document_' + obj.URL;
            //other element or object
        else
            if (obj[cacheName]) cacheId = obj[cacheName];
            else cacheId = obj[cacheName] = (Math.random() + '').substr(2);
        this.cacheId = cacheId;
    }

    Cache.prototype = (function () {
        var CacheBase = {};
        return {
            cacheId: null,
            prototype: Cache,
            set: function (key, value) {
                var cb;
                if (!(cb = CacheBase[this.cacheId])) cb = CacheBase[this.cacheId] = {};
                cb[key] = value;
                return this;
            },
            get: function (key) {
                var cb;
                if (!(cb = CacheBase[this.cacheId])) return null;
                return cb[key];
            },
            remove: function (key) {
                var cb;
                if (!(cb = CacheBase[this.cacheId])) return null;
                cb[key] = null;
                delete cb[key];
                return this;
            },
            clear: function () {
                if (!(CacheBase[this.cacheId])) return null;
                CacheBase[this.cacheId] = null;
                delete CacheBase[this.cacheId];
                return this;
            }
        }
    })();

    /* 修复ie6的css背景不缓存bug */
    Mc.fixBgCache = function (docRef) {
        try {
            docRef.execCommand("BackgroundImageCache", false, true);
        } catch (e) { }
    }

    /* 简易版的document.onready */
    Mc.ready = function (fn, docRef) {
        var doc = Mc.DOM.getDocument(docRef);
        var oldLoad = doc.window.onload;
        doc.window.onload = function () {
            oldLoad && oldLoad();
            fn();
        }
    }

    /* Cache的快捷访问方式（不使用new） */
    Mc.Cache = function (obj) { return new Cache(obj); }

    /*
    浏览器类型检测  	
    使用 if( Mc.Browser.msie ){ 来检测是否是ie }
    */
    Mc.Browser = (function () {
        var userAgent = navigator.userAgent.toLowerCase();
        return {
            version: (userAgent.match(/.+(?:rv|it|ra|ie)[\/: ]([\d.]+)/) || [0, '0'])[1],
            safari: /webkit/.test(userAgent),
            opera: /opera/.test(userAgent),
            msie: /msie/.test(userAgent) && !/opera/.test(userAgent),
            msie10: /msie 10/.test(userAgent) && !/opera/.test(userAgent),
            msie6: !window.XMLHttpRequest,
            mozilla: /mozilla/.test(userAgent) && !/(compatible|webkit)/.test(userAgent)
        }
    })();

    /*
    Function 常用方法
    */
    Mc.Function =
	{
	    /*
	    创建一个this为obj的function并返回
	    用例:
	    var F =  Mc.Function;
	    var me = this;
	    el.onclick = F.bind( me,function()
	    {
	    alert( me );
	    });
	      
	    */
	    bind: function (obj/* Object */, func/* Function */) { return function () { func.apply(obj, arguments); } },

	    /*
	    创建一个绑定this和参数的函数，类似于bind,只不过多绑定了一个参数
	    */
	    createFunc: function (fn/* Function */, argus/* Array */, ref/* Object */) { return function () { fn.apply(ref, argus) } }
	}

    /* 
    数字常用方法
    */
    Mc.Number =
	{
	    /*
	    range 方法接受三个数字，进行范围判定
	    如果num 大于max 就返回max
	    如果num 小于min 就返回min
	    如果num在max和min之间，就返回num
	    */
	    range: function (num, max, min) {
	        return Math.min(Math.max(min, num), max);
	    }
	}

    /* 
    字符串常用方法
    */
    Mc.String =
	{
	    /* 空字符串常量 */
	    empty: '',

	    //模拟 c#上中的Format()方法
	    format: function (str) {
	        var args = arguments;
	        return new String(str).replace(/\{(\d+)\}/g,
			function (m, i) {
			    i = parseInt(i);
			    return args[i + 1];
			}).toString();
	    },

	    //替换器用来替换一组跟对象的key相对应的字符串 
	    replacer: function (text, objFormat) {
	        return text.replace(/\{([^}]+)\}/g, function (match, key) {
	            return (key in objFormat) ? objFormat[key] : key;
	        });
	    },

	    //删除字符串里所有的html标记并返回
	    removeTag: function (str) {
	        return str.replace(/<[^>]+>/ig, '');
	    },

	    //替换全部匹配字符..a替换为b
	    replaceAll: function (str, a, b) { return new String(str).replace(new RegExp(a, 'g'), b).toString(); },

	    //字符串转化成字符数组
	    toArray: function (str) { return str.split(''); },

	    //判断 containedStr 是否在 str 中出现
	    contains: function (str, containedStr) { return new String(str).indexOf(containedStr) > -1; },

	    //去全部空格
	    trimAll: function (str) { return new String(str).replace(/\s/g, '').toString(); },

	    //去前后空格
	    trim: function (str, clearHtmlSpace) {
	        if (clearHtmlSpace) return new String(str).replace(/(^\s+)|\n+|\r+|(\&nbsp;)+|(\s+$)/g, '').toString();
	        return new String(str).replace(/(^\s*)|(\s*$)/g, '').toString();
	    },

	    //删除字符串左右两边出现的 sign(字符串)
	    trimSign: function (str, sign) {
	        var trim = new RegExp('^\\' + sign + '+|\\' + sign + '+$', 'g');
	        return str.replace(trim, '');
	    },

	    //去前空格
	    lTrim: function (str) { return new String(str).replace(/^\s*/g, '').toString(); },

	    //去后空格
	    rTrim: function (str) { return new String(str).replace(/\s*$/g, '').toString(); },

	    //得到一个字符串的字节长度
	    getBit: function (str) { return new String(new String(str).replace(/[^\x00-\xff]/g, '..')).length; },

	    //首字母大写
	    toInitialUpperCase: function (str) { return this.trim(str).toLowerCase().replace(/^\S/, function (s) { return s.toUpperCase() }); },

	    //得到一个清除所有html标记和html空格的字符串（实际用于判断是否有可用数据）
	    getCleanHtmlStr: function (str) {
	        if (Mc.String.hasImgTag(str)) return '0_0';
	        return str.replace(/(^\s+)|\s+|\n+|\r+|(\&nbsp;)+|<.+?>|(\s+$)/g, '');
	    },

	    //判断字符串里是否含有<img>标签
	    hasImgTag: function (str) { return /<img.*?>/ig.test(str); },

	    //将html标签转换成html实体 比如 空格转换成&nbsp;
	    toHTMLEntity: function (htmlFrag) {
	        var c = Mc.DOM.createNode('div');
	        c.appendChild(Mc.DOM.createText(htmlFrag));
	        return c.innerHTML;
	    },

	    //编码html，跟toHTMLEntity效果一致
	    htmlEnCode: function (str) {
	        return Mc.String.toHTMLEntity(str);
	    },

	    //将string转换成字符串( json 用 )
	    stringToObject: function (text) {
	        if (typeof text == 'object') return text;
	        else if (typeof text == 'string') {
	            text = text.replace(/\n/g, '\\n').replace(/\r/g, '\\r');
	            return eval('(' + text + ')');
	        }
	    }
	}

    /*
    Date 常用方法
    */
    Mc.Date =
	{
	    /*
	    计算Date 增加或减少若干 时间，并返回一个被格式化的Date字符串
	    比如
	    var d = new Date();
	    calcDate( d,'+1s','yyyy-MM-dd HH:mm:ss' ); 
	    为d增加一秒 并且返回一个 格式为'yyyy-MM-dd HH:mm:ss'字符串
	    还可以使用
	        
	    s ：+s -s 增加减少秒
	    m ：+m -m 增加减少分钟
	    h ：增加或减少小时
	    d ：增加减少天数
	    M ：增加或减少月份
	    y ：增加或者减少年份
	        
	    */
	    calcDate: function (date/*Date*/, span/*String*/, dateFormatStr/*String*/) {
	        var reg = /^([-|+]?\d+)([s|m|h|d|M|y])$/,
				tArr = typeof span == 'number' ? ['wait~', span, 'd'] : span.match(reg),
				rDate = date instanceof Date ? date : new Date(date.replace(/-/g, '/')),
				t,
				dateFormatStr = dateFormatStr || 'yyyy-MM-dd',
				mList =
				{
				    s: 'Seconds',
				    m: 'Minutes',
				    h: 'Hours',
				    d: 'Date',
				    M: 'Month',
				    y: 'FullYear'
				},
				val = Number(tArr[1]);
	        unit = tArr[2];
	        rDate['set' + mList[unit]](rDate['get' + mList[unit]]() + val);
	        return Mc.Date.dateFormat(rDate, dateFormatStr);
	    },

	    /*
	    格式化Date,类似C# 不过支持的参数少一点
	    */
	    dateFormat: function (date, formatString) {
	        var q =
			{
			    y: 'FullYear',
			    M: 'Month',
			    d: 'Date',
			    H: 'Hours',
			    h: 'Hours',
			    m: 'Minutes',
			    s: 'Seconds',
			    f: 'Milliseconds'
			}
	        var weekCn = '日一二三四五六'.split('');
	        function h(date, offsetVal, formatKey, matchFormat, action) {
	            var returnVal = String(date['get' + q[formatKey]]() + offsetVal);
	            if (!action) return (matchFormat.length > 1 ? (returnVal < 10 ? '0' + returnVal : returnVal) : returnVal);
	            else return action(returnVal, matchFormat);
	        }

	        return formatString.replace(/y+|M+|d+|h+|H+|m+|s+|f+|w+/g, function (matchFormat) {
	            var firstChar = matchFormat.charAt(0);
	            switch (firstChar) {
	                case 'y':
	                    return h(date, 0, firstChar, matchFormat, function (val, m) { return val.substr((m.length > 2 ? 0 : 2)) })
	                case 'M':
	                    return h(date, +1, firstChar, matchFormat);
	                case 'h':
	                    return h(date, -12, firstChar, matchFormat);
	                case 'f':
	                    return h(date, 0, firstChar, matchFormat, function (val, m) { return val; })
	                case 'd':
	                case 'H':
	                case 'm':
	                case 's':
	                    return h(date, 0, firstChar, matchFormat);
	                case 'w':
	                    return weekCn[date.getDay()];
	            }
	            return m;
	        });
	    },

	    /* 用string来创建一个date，可以传递类似'1986-12-11'的字符串来创建 */
	    create: function (date) {
	        if (Mc.Core.isDate(date))
	            return date;
	        else if (typeof date == 'string') {
	            var d = Date.parse(date.replace(/-/g, '/'));
	            if (!d) return null;
	            return Mc.Date.clone(d);
	        }
	        return null;
	    },

	    /* 克隆一个date并且返回 */
	    clone: function (date) {
	        return new Date(+date)
	    },

	    /* 判断dateA和dateB是否年月日相等 */
	    equalsDatePart: function (dateA, dateB) {
	        return dateA.getFullYear() == dateB.getFullYear() &&
		           dateA.getDate() == dateB.getDate() &&
		           dateA.getMonth() == dateB.getMonth();
	    }
	}

    /*
    Array 常用方法
    */
    Mc.Array =
	{
	    //用索引删除一个数组元素
	    remove: function (arr/*Array*/, idx/*Number*/) { arr.splice(idx, 1); return arr; },

	    //遍历一个数组,为每一个数组元素执行callback
	    forEach: function (arr/*Array*/, callBack/*Function*/) {
	        if (!arr) return;
	        var oThis = oThis || window;
	        for (var i = 0, l = arr.length; i < l; i++) callBack.call(oThis, arr[i], i);
	    },

	    //克隆一个数组并返回
	    copy: function (arr) { return arr.concat(); },

	    //清空数组
	    clear: function (arr) { arr.length = 0; },

	    //验证 array元素中 是否包含某个元素
	    contains: function (arr, obj) {
	        for (var i = 0, l = arr.length; i < l; i++) if (arr[i] == obj) return i;
	        return -1;
	    },

	    //删除数组的重复项
	    deleteRepeater: function (arr) {
	        if (arr.length < 2) return arr;
	        var aT = arr.concat();
	        arr.length = 0;
	        for (var i = 0; i < aT.length; i++) {
	            arr.push(aT.splice(i--, 1)[0]);
	            for (var j = 0; j < aT.length; j++) if (aT[j] == arr[arr.length - 1]) aT.splice(j--, 1);
	        }
	        return arr;
	    },

	    //将NodeList转换成数组
	    toArray: function (nodeList) {
	        try {
	            return Array.prototype.slice.call(nodeList);
	        } catch (e) {
	            for (var i = 0, arr = [], l = nodeList.length; i < l; i++) arr.push(nodeList[i]);
	            return arr;
	        }
	    }
	}

    /*
    一些核心方法
    */
    Mc.Core =
	{
	    //判断对象是否是null
	    isNull: function (t) { return (t == null) },
	    //判断对象是否是DOM对象
	    isElement: function (t) { return !!(t && t.nodeType && t.ownerDocument) },
	    //判断是否是NodeList对象
	    isNodeList: function (t) { return t && !Mc.Core.isElement(t) && typeOf(t.length) == 'number' && t.item && !t.push && !t.pop },
	    //判断是否是函数中的arguments对象
	    isArguments: function (t) { return t && typeOf(t.length) == 'number' && t.callee },

	    //extend 接受若干个对象，把除了第一个对象的其他对象的属性逐个覆盖到第一个上面来
	    extend: function () {
	        var target = arguments[0], key;
	        for (var i = 1; i < arguments.length; i++) for (key in arguments[i]) target[key] = arguments[i][key];
	        return target;
	    },

	    //类似extend，但是覆盖时会检查属性是否存在于第一个对象，如果不存在则不覆盖
	    extendProperty: function () {
	        var target = arguments[0], key;
	        for (var i = 1; i < arguments.length; i++)
	            for (key in arguments[i])
	                if (key in target)
	                    target[key] = arguments[i][key];
	        return target;
	    },

	    // 通用分时处理函数
	    timedChunk: function (items, process, context, callback) {
	        if (items.length == 0) return;
	        var todo = items.concat(), delay = 25;

	        setTimeout(function () {
	            var start = +new Date();

	            do {
	                try { process.call(context, todo.shift()) } catch (e) { };
	            } while (todo.length > 0 && (+new Date() - start < 50));

	            if (todo.length > 0) {
	                setTimeout(arguments.callee, delay);
	            } else if (callback) {
	                callback(items);
	            }

	        }, delay);
	    },

	    //OO 创建一个类
	    createClass: function () {
	        return function () {
	            if (!this.initialize) throw new Error('Initialize method is missing.');
	            this.initialize.apply(this, arguments);
	        }
	    },

	    //继承
	    inherit: function (superClass, members) {
	        var subClass = Mc.Core.createClass();
	        if (!superClass) return subClass;

	        var fn = function () { };
	        fn.prototype = superClass.prototype;
	        subClass.prototype = new fn();

	        subClass.Base = superClass.prototype;
	        if (subClass.prototype.constructor == Object.prototype.constructor) subClass.prototype.constructor = superClass;

	        if (members) Mc.Core.extend(subClass.prototype, members);

	        return subClass;
	    },

	    //类似于Array.forEach.能接受所有带length的对象
	    forEach: function (t, f) {
	        if (!t || !f || !Mc.Core.isNumber(t.length)) return;
	        Mc.Array.forEach(t, function (item) {
	            f.call(window, item);
	        })
	    },

	    toArray: Mc.Array.toArray
	}
    var typeOf = function (o) { return Object.prototype.toString.call(o).toLowerCase().match(/\[\w+\s(\w+)\]/)[1]; },
		types = 'string,number,array,object,regexp,function,boolean,date,undefined'.split(',');
    for (var i = 0, l = types.length; i < l; i++) {
        Mc.Core['is' + Mc.String.toInitialUpperCase(types[i])] = (function (tname) {
            return function (t) { return typeOf(t) == tname }
        })(types[i]);
    }

    /* 
    表单Form常用方法 
    */
    Mc.Form =
	{
	    //序列化一个表单内所有有效数据，也就是找到表单内的name和value,组成 name=value&name=value的字符串
	    serialize: function (form, attrs) {
	        var elements = form.elements,
                params = [],
                tempElement = null;

	        for (var i = 0, l = elements.length; i < l; i++) {
	            tempElement = elements[i];

	            //如果没有name就不收集，跳过循环
	            if (!tempElement.name) continue;
	            if (/select-multiple/i.test(tempElement.type)) {
	                for (var n = 0, m = tempElement.options.length; n < m; n++)
	                    if (tempElement.options[n].selected)
	                        params.push(tempElement.name + '=' + encodeURIComponent(tempElement.options[n].value));
	                continue;
	            }

	            //如果表单元素的type是radio|checkbox, 只收集checked的元素值,不是checked就跳过循环
	            if (/radio|checkbox/i.test(tempElement.type) && !tempElement.checked) continue;

	            params.push(tempElement.name + '=' + encodeURIComponent(tempElement.value));
	        }


	        return params.join('&') + '&' + (attrs ? Mc.Utils.toQueryString(attrs) : '');
	    },

	    /*
	    ajax提交表单
	    params = 
	    {
	    form : Mc.$('form1'),
	    callBack : function( data )
	    {
                
	    }
	    }
	    */
	    ajaxForm: function (params) {
	        var form = params.form,
                oldSubmit = form.onsubmit,
                result = true;
	        form.onsubmit = function () {
	            if (oldSubmit) result = oldSubmit();
	            if (!result) return false;
	            if (!params.url && !params.type && !params.onSuccess) {
	                params.url = Mc.Utils.mergePath(form.ownerDocument.window.location.href, form.action);
	                params.onSuccess = params.callBack;
	                params.onError = params.callBack;
	                params.type = form.method;
	            }
	            params.data = Mc.Form.serialize(form);
	            Mc.AJAX(params);
	            return false;
	        }

	        return true;
	    },

	    ajaxSubmit: function (params) {
	        var form = params.form;
	        params.url = Mc.Utils.mergePath(form.ownerDocument.window.location.href, form.action);
	        params.onSuccess = params.callBack;
	        params.onError = params.callBack;
	        params.type = form.method;
	        params.data = Mc.Form.serialize(form);
	        Mc.AJAX(params);
	    }
	}



    Mc.DOM =
	{
	    localMethods: 'getEvent,$,$A,$N,$C,$T,createHTML,createNode,createText,cleanWhitespace,getPageSize,show,hide,addClass,removeClass,visible,hidden,toggle'.split(','),

	    getDocument: function (docRef) {
	        if (docRef && docRef.window && (docRef == docRef.window.document)) return docRef
	        else return document;
	    },

	    getDocByObject: function (obj) {
	        if (!obj) return document;
	        if (obj.ownerDocument) return obj.ownerDocument;
	        if (obj.window && obj.window.document == obj) return obj;
	        if (obj.document && obj.document.window == obj) return obj.document;
	        return document;
	    },

	    getWinByObject: function (obj) {
	        return Mc.DOM.getDocByObject(obj).window;
	    },

	    createHandler: function (obj, type) {
	        return function (e) {
	            var handlersList = Mc.Cache(obj).get(type),
					handler = null,
					D = Mc.DOM;
	            if (!handlersList || handlersList.length == 0) {
	                Mc.Cache(obj).remove(type);
	                D.un(obj, type, arguments.callee);
	                return;
	            }
	            for (var i = 0, l = handlersList.length; i < l; i++) {
	                handler = handlersList[i];
	                if (handler && handler.fn) {
	                    handler.fn.call(handler.ref || obj, (handler.sevt ? D.getEvent(e, D.getDocByObject(obj)) : e), obj);
	                }
	            }
	        }
	    },

	    on: function (obj, type, handler, capture) {
	        if (type == 'DOMMouseScroll' && Mc.Browser.msie) type = 'mousewheel';
	        var fname = obj.addEventListener ? 'addEventListener' : ((type = 'on' + type), 'attachEvent');
	        obj[fname](type, handler, !!capture);
	    },

	    un: function (obj, type, handler, capture) {
	        var fname = obj.addEventListener ? 'removeEventListener' : ((type = 'on' + type), 'detachEvent');
	        obj[fname](type, handler, !!capture);
	    },

	    trigger: function (obj, type) {
	        if (obj.fireEvent)
	            obj.fireEvent('on' + type);
	        else {
	            var o = (!obj.ownerDocument ? obj : obj.ownerDocument);

	            var e = o.createEvent("UIEvents");
	            e.initUIEvent(type, true, true, Mc.DOM.getWinByObject(obj), 1);
	            obj.dispatchEvent(e);
	        }
	    },

	    addEvent: function (obj, type, handler, sevt, ref, ignore) {
	        var cache = Mc.Cache(obj),
				handlers = cache.get(type),
				evt = { 'fn': handler, 'ref': ref, 'sevt': sevt },
				isRepeat = false;
	        if (!handlers) {
	            cache.set(type, (handlers = [evt]));
	            Mc.DOM.on(obj, type, Mc.DOM.createHandler(obj, type));
	            return;
	        }
	        else if (ignore) {
	            Mc.Array.forEach(handlers, function (o) {
	                if (o == handler) {
	                    isRepeat = true;
	                }
	            });
	        }
	        if (!isRepeat) handlers.push(evt);
	    },

	    removeEvent: function (obj, type, handler) {
	        var cache = Mc.Cache(obj),
				handlers = cache.get(type),
				curr;
	        if (!handlers) return;
	        for (var l = handlers.length - 1; l >= 0; l--) {
	            curr = handlers[l];
	            if (curr.fn == handler) Mc.Array.remove(handlers, l);
	        }
	        if (handlers.length == 0) Mc.DOM.trigger(obj, type);
	    },

	    getEvent: function (e, docRef) {
	        docRef = Mc.DOM.getDocument(docRef);
	        var evt = e || docRef.window.event,
			d = docRef.documentElement,
			b = docRef.body || {
			    scrollLeft: 0,
			    scrollTop: 0
			};

	        return {
	            altKey: evt.altKey,
	            altLeft: evt.altLeft,
	            button: evt.which ? evt.which : ((evt.button === 2) ? 3 : 1),
	            cancelBubble: function () {
	                if (evt.stopPropagation) evt.stopPropagation();
	                else evt.cancelBubble = true;
	                return this;
	            },
	            stopPropagation: function () { this.cancelBubble(); return this; },
	            stop: function () { this.cancelBubble(); this.preventDefault(); return this; },
	            preventDefault: function () {
	                if (evt.preventDefault) evt.preventDefault();
	                else evt.returnValue = false;
	                return this;
	            },

	            detail: (evt.wheelDelta ? -(evt.wheelDelta / 120) : evt.detail / 3) || 0,

	            clientX: evt.pageX || evt.clientX,
	            clientY: evt.pageY || evt.clientY,
	            ctrlKey: evt.ctrlKey,
	            ctrlLeft: evt.ctrlLeft,
	            fromElement: evt.fromElement || evt.relatedTarget,
	            keyCode: evt.keyCode || evt.charCode || evt.which,
	            offsetX: evt.offsetX || evt.layerX,
	            offsetY: evt.offsetY || evt.layerY,
	            returnValue: evt.returnValue,
	            screenX: evt.screenX,
	            screenY: evt.screenY,
	            shiftKey: evt.shiftKey,
	            shiftLeft: evt.shiftLeft,
	            srcElement: evt.srcElement || evt.target,
	            target: evt.srcElement || evt.target,
	            toElement: evt.toElement || evt.relatedTarget,
	            type: evt.type,
	            x: evt.pageX || evt.clientX,
	            y: evt.pageY || evt.clientY,
	            pageX: evt.pageX || (evt.clientX + (d.scrollLeft || b.scrollLeft) - (d.clientLeft || 0)),
	            pageY: evt.pageY || (evt.clientY + (d.scrollTop || b.scrollTop) - (d.clientTop || 0)),
	            source: evt
	        }
	    },

	    checkPageEmbed: function (f) {
	        return !!(window.frameElement && window.frameElement.tagName.toLowerCase() == f);
	    },

	    inFrame: function () {
	        return Mc.DOM.checkPageEmbed('frame');
	    },

	    inIframe: function () {
	        return Mc.DOM.checkPageEmbed('iframe');
	    },

	    focusAfter: function (o) {
	        o = Mc.$(o);
	        if (!o || !o.createTextRange) return;
	        o.focus();
	        setTimeout(function () {
	            var range = o.createTextRange();
	            range.moveStart('character', o.value.length);
	            range.collapse(true);
	            range.select();
	        }, 0);
	    },

	    isPosElement: function (t) {
	        var s = Mc.DOM.getStyle(t, 'position');
	        return (s == 'absolute' || s == 'relative');
	    },

	    //判断对象是否可见
	    isVisible: function (obj) {
	        return Mc.DOM.getStyle(obj, 'display') != 'none';
	    },

	    notInDOMTree: function (t) {
	        while (t = t.parentNode) if (t == document) return false;
	        return !t;
	    },

	    $: function () {
	        var returnValue = [],
				A = Mc.Array,
				idList = Mc.Core.toArray(arguments),
				docRef = idList[idList.length - 1];

	        if (docRef && docRef.window && (docRef == docRef.window.document)) A.remove(idList, idList.length - 1);
	        else docRef = document;

	        A.forEach(idList, function (id, idx) {
	            var o = !Mc.Core.isString(id) ? id : docRef.getElementById(id);
	            if (!o) return;
	            returnValue.push(o);
	        })

	        if (returnValue.length == 0) return null;
	        if (returnValue.length == 1) return returnValue[0];
	        return returnValue;
	    },

	    $N: function (value, parent, docRef) {
	        return Mc.DOM.$A('name', value, parent, docRef);
	    },

	    $C: function (value, parent, docRef) {
	        return Mc.DOM.$A('class', value, parent, docRef);
	    },

	    $T: function (value, parent, docRef) {
	        docRef = Mc.DOM.getDocument(docRef);
	        parent = Mc.$(parent, docRef) || docRef;
	        return Mc.Core.toArray(parent.getElementsByTagName(value));
	    },

	    $A: function (attr, value, parent, docRef) {
	        var list = null, arr = [],
				docRef = Mc.DOM.getDocument(docRef);
	        parent = Mc.$(parent, docRef) || docRef;

	        if (parent.querySelectorAll) {
	            if (attr == 'class') return Mc.Core.toArray(parent.querySelectorAll('.' + value));
	            return Mc.Core.toArray(parent.querySelectorAll(Mc.String.format(value ? '*[{0}="{1}"]' : '*[{0}]', attr, value)));
	        }
	        else {
	            if (attr == 'class') {
	                if (parent.getElementsByClassName) return Mc.Core.toArray(parent.getElementsByClassName(value));
	                else list = parent.getElementsByTagName('*');
	                attr == 'className';
	                Mc.Core.forEach(list, function (t) { if (Mc.DOM.hasClass(t, value)) arr.push(t); });
	            }
	            else {
	                list = parent.getElementsByTagName('*');
	                Mc.Core.forEach(list, function (t) { if (t.getAttribute(attr) == value) arr.push(t); });
	            }
	        }
	        return arr;
	    },

	    createHTML: function (str, docRef) {
	        docRef = Mc.DOM.getDocument(docRef);
	        if (Mc.Core.isElement(str)) return str;
	        var oTemp = docRef.createDocumentFragment(), oDiv = docRef.createElement('div');
	        oDiv.innerHTML = str;
	        while (oDiv.firstChild) oTemp.appendChild(oDiv.firstChild);
	        return oTemp;
	    },

	    createNode: function (name, attrs, docRef) {
	        if (arguments.length == 1) attrs = null;
	        if (arguments.length == 2 && attrs == Mc.DOM.getDocument(attrs)) {
	            docRef = attrs;
	            attrs = null;
	        }
	        docRef = Mc.DOM.getDocument(docRef);
	        var n = docRef.createElement(name);
	        Mc.DOM.setAttrs(n, attrs);
	        return n;
	    },

	    createText: function (text, docRef) {
	        docRef = Mc.DOM.getDocument(docRef);
	        return docRef.createTextNode(text);
	    },

	    cleanWhitespace: function (element, docRef) {
	        element = Mc.$(element, docRef);
	        var node = element.firstChild;
	        while (node) {
	            var nextNode = node.nextSibling;
	            if (node.nodeType == 3 && !/\S/.test(node.nodeValue))
	                element.removeChild(node);
	            node = nextNode;
	        }
	        return element;
	    },

	    copyOffset: function (gObj, rObj) {
	        var size = Mc.DOM.getSize(gObj),
		        offset = Mc.DOM.getOffset(gObj);
	        //var _scrollTop = 0;
	        var _scrollHeight = 0;
	        if (document.documentElement && document.documentElement.scrollHeight) {
	            //_scrollTop = document.documentElement.scrollTop || 0;
	            _scrollHeight = document.documentElement.scrollHeight || 0;

	        }
	        else if (document.body) {
	            //_scrollTop = document.body.scrollTop || 0;
	            _scrollHeight = document.body.scrollHeight || 0;
	        }
	        Mc.DOM.setStyle(rObj,
	        {
	            width: '100%',
	            height: '100%',
	            top: offset.top + 'px',
	            left: offset.left + 'px'
	        });
	    },

	    setAttrs: function (obj, attrs) {
	        var ot = 'object', n = Mc.$(obj);
	        if (attrs) {
	            if (attrs.appendTo) attrs.appendTo.appendChild(n);
	            if (attrs.appendChild) n.appendChild(attrs.appendChild);
	            delete attrs.appendTo; delete attrs.appendChild;
	        }
	        (function (n, attrs) { for (var i in attrs) typeof attrs[i] == ot ? arguments.callee(n[i], attrs[i]) : n[i] = attrs[i]; })(n, attrs);
	        return n;
	    },

	    isHideElement: function (o) {
	        return (o.offsetWidth == 0
		        && o.offsetHeight == 0
		        && o.offsetTop == 0
		        && o.offsetLeft == 0
		     );
	    },

	    getStyle: function (o, attr, removeUnit) {
	        var returnValue = '';
	        attr = attr.replace(/-(\w)/g, function (a, f) { return f.toUpperCase(); });
	        if (o.currentStyle) returnValue = o.currentStyle[attr == 'opacity' ? 'filter' : attr];
	        else returnValue = o.ownerDocument.defaultView.getComputedStyle(o, null)[attr];
	        if (removeUnit) return parseInt(returnValue) || 0;
	        return returnValue;
	    },

	    setStyle: function (t, style) {
	        t = Mc.$(t); if (!t) return;
	        if (Mc.Core.isString(style)) t.style.cssText = style;
	        for (var key in style) try { t.style[key] = style[key]; } catch (e) { continue; }
	    },

	    setOpacity: function (node, opacityValue) {
	        //opacityValue 必须是0-100的整数       
	        opacityValue = parseInt(opacityValue);
	        opacityValue = opacityValue > 100 ? 100 : opacityValue < 0 ? 0 : opacityValue;
	        if (!Mc.Browser.msie || Mc.Browser.msie10) node.style.opacity = opacityValue ? opacityValue / 100 : opacityValue == 0 ? 0 : 9 / 10; //支持 css3 浏览器 的方法
	        else {
	            if (opacityValue == 100) node.style.filter = '';
	            else node.style.filter = 'Alpha(Opacity=' + (opacityValue ? opacityValue : opacityValue == 0 ? 0 : 90) + ')';
	        }
	        //xhtml1.0+ ie方法 //缺点，覆盖掉filter属性
	    },

	    //得到对象的透明度
	    getOpacity: function (node) {
	        return getStyle(node, 'opacity')
	    },

	    getOffset: function (o) {

	        var ol = 'offsetLeft', ot = 'offsetTop', r = { left: o[ol], top: o[ot] };
	        while (o = o.offsetParent) r.left += o[ol], r.top += o[ot];
	        return r;
	    },

	    //得到相对于父对象的坐标
	    getPosition: function (obj) {
	        return {
	            left: obj.offsetLeft,
	            top: obj.offsetTop
	        }
	    },

	    getBaseSize: function (o, attr) {
	        var docRef = o.ownerDocument;
	        attr = attr.replace(/^\w/, function (f) { return f.toUpperCase(); });
	        var cl = 'client' + attr, of = 'offset' + attr, sc = 'scroll' + attr, bh = 0
			, h = docRef.documentElement, b = docRef.body;
	        if (o == h) return Math.max(h[cl], h[sc], b[cl], b[sc]);
	        return o[of];
	    },

	    getSize: function (el) {
	        var isHide = this.isHideElement(el)
	        /*if( isHide )
	        {
	        var t = Mc.DOM.createHTML('<div style="left:-5000px;top:-5000px;position:absolute;"></div>').firstChild;
	        var next = el.nextSibling;
	        var par = el.parentNode;
	        var os = Mc.DOM.getStyle( el,'display' );
	        el.style.display = 'block'  
	        el.ownerDocument.body.appendChild( el ); 
	        }*/
	        var box = el.getBoundingClientRect(),
			doc = el.ownerDocument,
			body = doc.body,
			html = doc.documentElement,
			clientTop = html.clientTop || body.clientTop || 0,
			clientLeft = html.clientLeft || body.clientLeft || 0,
			top = box.top + (self.pageYOffset || html.scrollTop || body.scrollTop) - clientTop,
			left = box.left + (self.pageXOffset || html.scrollLeft || body.scrollLeft) - clientLeft;
	        var resultVal = {
	            top: top,
	            left: left,
	            width: el.scrollWidth,
	            height: el.scrollHeight
	        }

	        /*if( isHide )
	        {
	        el.style.display = os;
	        if( next )
	        next.parentNode.insertBefore( el,next );
	        else if( par )
	        par.appendChild( el );
	        }*/
	        return resultVal;
	    },

	    //得到属性值和单位
	    getUnit: function (val) {
	        if (Mc.Core.isNull(val) || Mc.Core.isUndefined(val)) return;
	        var style, t;
	        if (isNumber(val)) style = { value: val, unit: '' };
	        else if ((t = val.match(/(\d+)(px|em|ex|pt|pc|in|mm|cm|%)$/i)) && t.length == 3) style = { value: Number(t[1]) || 0, unit: t[2] }; else style = { value: Number(val) || 0, unit: '' };
	        return style;
	    },

	    //居中一个节点
	    setCenter: function (obj, scrollCenter) {
	        var docRef = obj.ownerDocument,
				isUnShow = !Mc.DOM.isVisible(obj);
	        if (isUnShow) {
	            Mc.DOM.setStyle(obj,
		        {
		            left: '-9999px',
		            top: '-9999px',
		            display: ''
		        });
	        }
	        var size = Mc.DOM.getSize(obj), range = Mc.DOM.getPageSize(docRef), left = range.width / 2 - size.width / 2,
				top = docRef.compatMode == 'CSS1Compat' ?
				docRef.documentElement.scrollTop + (docRef.documentElement.clientHeight - size.height) / 2 :
				docRef.body.scrollTop + (docRef.body.clientHeight - this.height) / 2;
	        //if( pos == 'fixed' )top = ( document.documentElement.clientHeight  - size.height )/2; 
	        Mc.DOM.setStyle(obj, {
	            'position': 'position',
	            'left': left + 'px',
	            'top': top + 'px',
	            'margin': '0'
	        });
	        if (isUnShow) Mc.DOM.hide(obj);
	    },

	    getPageSize: function (docRef) {
	        /*var size = Mc.DOM.getSize( Mc.DOM.getDocument( docRef ).documentElement );
	        var    bodyMargin =
	        {
	        left : Mc.DOM.getStyle( docRef.body,'margin-left',true ),
	        top : Mc.DOM.getStyle( docRef.body,'margin-top',true ),
	        right : Mc.DOM.getStyle( docRef.body,'margin-right',true ),
	        bottom : Mc.DOM.getStyle( docRef.body,'margin-bottom',true )
	        }
	        size.width += bodyMargin.left + bodyMargin.
	        return size;*/
	        var d = Mc.DOM.getDocument(docRef),
				h = d.documentElement,
				b = d.body,
				ch = 'clientHeight',
				sh = 'scrollHeight',
				oh = 'offsetHeight',
				ow = 'offsetWidth',
				sw = 'scrollWidth',
				m = d.compatMode == 'CSS1Compat'
	        t = (m ? h : b);
	        return {

	            width: (t[sw] > b[ow] ? t[sw] : b[ow]) - (m ? 0 : 16),
	            height: t[sh] > t[ch] ? t[sh] : t[ch],

	            viewWidth: b.clientWidth,
	            viewHeight: h[ch] > b[ch] ? b[ch] : b[ch]
	        }
	    },

	    //得到指定的属性值
	    getAttribute: function (obj, attr) { if (attr in obj) return obj[attr]; else return obj.getAttribute(attr); },

	    //设置属性值
	    setAttribute: function (obj, attr, value) { obj[attr] = value; },

	    //移除属性
	    removeAttribute: function (obj, attr) { obj.removeAttribute(attr); },

	    //给对象添加一个css class，不能重复添加
	    addClass: function (obj, cssClass, docRef) {
	        obj = Mc.$(obj, docRef);
	        if (!Mc.DOM.hasClass(obj, cssClass)) {
	            if (!obj.className) obj.className = cssClass;
	            else obj.className = Mc.String.trim(obj.className) + " " + cssClass;
	        }
	    },

	    hasClass: function (obj, cssClass) {
	        return obj.className.match(new RegExp('(\\s|^)' + cssClass + '(\\s|$)'));
	    },

	    //给对象添加一个css class，不能重复添加
	    setClass: function (obj, cssClass) { obj.className = cssClass; },

	    //切换class
	    toggleClass: function (obj, cssClass) {
	        if (Mc.DOM.hasClass(obj, cssClass)) Mc.DOM.removeClass(obj, cssClass);
	        else Mc.DOM.addClass(obj, cssClass);
	    },

	    //移除对象上指定的cssClass
	    removeClass: function (obj, cssClass, docRef) {
	        obj = Mc.$(obj, docRef);
	        var reg = new RegExp('(\\s|^)' + cssClass + '(\\s|$)');
	        if (Mc.DOM.hasClass(obj, cssClass)) obj.className = obj.className.replace(reg, ' ');
	    },

	    clearClass: function (obj) { Mc.DOM.setClass(obj, '') },

	    show: function (obj, docRef) {
	        var list = Mc.$(obj, docRef);
	        if (!list) return;
	        if (!Mc.Core.isArray(list)) list = [list];
	        Mc.Array.forEach(list, function (obj, idx) {
	            Mc.$(obj, docRef).style.display = '';
	        })
	    },

	    hide: function (obj, docRef) {
	        var list = Mc.$(obj, docRef);
	        if (!list) return;
	        if (!Mc.Core.isArray(list)) list = [list];
	        Mc.Array.forEach(list, function (obj, idx) {
	            Mc.$(obj, docRef).style.display = 'none';
	        });
	    },

	    visible: function (obj, docRef) {
	        var docRef = Mc.DOM.getDocument(docRef), list = Mc.$(obj, docRef);
	        if (!list) return;
	        if (!Mc.Core.isArray(list)) list = [list];
	        Mc.Array.forEach(list, function (obj, idx) {
	            obj.style.visibility = 'visible';
	        });
	    },

	    hidden: function (obj, docRef) {
	        var docRef = Mc.DOM.getDocument(docRef), list = Mc.$(obj, docRef);
	        if (!list) return;
	        if (!Mc.Core.isArray(list)) list = [list];
	        Mc.Array.forEach(list, function (obj, idx) {
	            obj.style.visibility = 'hidden';
	        });
	    },

	    toggle: function (o, docRef) {
	        var list = Mc.$(o, docRef);
	        if (!list.length) list = [list];
	        Mc.Array.forEach(list, function (t, idx) {
	            if (Mc.DOM.getStyle(t, 'display') == 'none') Mc.DOM.show(t);
	            else Mc.DOM.hide(t);
	        });
	    },

	    getOuterHTML: function (root, _isDocumentFragment) {
	        var html = "";
	        if (root.outerHTML) return root.outerHTML; //ie
	        else if (Mc.Browser.msie && root.nodeType == 11) {
	            for (var i = 0; i < root.childNodes.length; i++) {
	                html += root.childNodes[i].nodeType == 1 ? root.childNodes[i].outerHTML : root.childNodes[i].data;
	            }
	            return html;
	        }
	        if (window.XMLSerializer) {  //串行化xml dom对象.实现outerHTML
	            var xml = new XMLSerializer();
	            html = xml.serializeToString(root);
	            xml = null;
	            return html;
	        }
	        //不支持outerHTML 又不支持xmlSerializer xml核心方法 则 使用递归+循环遍历+节点类型判断 获取outerHTML
	        var moz_check = /_moz/i;
	        switch (root.nodeType) {
	            case Node.ELEMENT_NODE:
	            case Node.DOCUMENT_FRAGMENT_NODE:
	                var closed;
	                if (!_isDocumentFragment) {
	                    closed = !root.hasChildNodes();
	                    html = '<' + root.tagName.toLowerCase();
	                    var attr = root.attributes;
	                    for (var i = 0; i < attr.length; ++i) {
	                        var a = attr.item(i);
	                        if (!a.specified || a.name.match(moz_check) || a.value.match(moz_check)) {
	                            continue;
	                        }
	                        html += " " + a.name.toLowerCase() + '="' + a.value + '"';
	                    }
	                    html += closed ? " />" : ">";
	                }
	                for (var i = root.firstChild; i; i = i.nextSibling) {
	                    html += getOuterHTML(i);
	                }
	                if (!_isDocumentFragment && !closed) {
	                    html += "</" + root.tagName.toLowerCase() + ">";
	                }
	                break;

	            case Node.TEXT_NODE:
	                html = root.data;
	                break;
	        }
	        return html;
	    },

	    getInnerText: function (obj) {
	        if (obj.hasOwnProperty('innerText')) return obj.innerText;
	        else if (obj.hasOwnProperty('textContent')) return obj.textContent;
	    },

	    setInnerText: function (obj, text) {
	        if (obj.hasOwnProperty('innerText')) obj.innerText = text;
	        else if (obj.hasOwnProperty('textContent')) obj.textContent = text;
	        return obj;
	    },

	    //在目标节点前插入新节点
	    insertBefore: function (targetNode, newNode) {
	        targetNode.parentNode.insertBefore(newNode, targetNode);
	    },

	    //在目标节点后插入新节点
	    insertAfter: function (newNode, targetNode) {
	        if (targetNode.nextSibling) targetNode.parentNode.insertBefore(newNode, targetNode.nextSibling);
	        else targetNode.parentNode.appendChild(newNode);
	    },

	    //替换节点
	    replaceElement: function (newNode, oldNode) {
	        var tempNewNode = Mc.DOM.createHTML(newNode);
	        oldNode.parentNode.replaceChild(tempNewNode, oldNode);
	        return tempNewNode;
	    },

	    //移除一个节点
	    removeElement: function (node) {
	        node.parentNode.removeChild(node);
	        node = null;
	    },

	    //得到对象在父元素中的索引
	    getChildIndex: function (obj) {
	        var count = 0, o = obj;
	        while ((o = o.previousSibling)) count++;
	        return count;
	    }
	}

    Mc.$ = Mc.DOM.$;
    Mc.$C = Mc.DOM.$C;
    Mc.$A = Mc.DOM.$A;
    Mc.$T = Mc.DOM.$T;
    Mc.$N = Mc.DOM.$N;
    Mc.DOM.create = Mc.DOM.createHTML;
    Mc.DOM.getStandardEvent = Mc.DOM.getEvent;

    Mc.Utils =
	{
	    localMethods: 'queryString'.split(','),
	    getRandom: function (start, end) {
	        switch (arguments.length) {
	            case 0: return Math.random();
	            case 1: return Math.random() * start + 1;
	            case 2: return Math.random() * (start - end + 1) + end;
	        }
	    },

	    queryString: function (key, docRef) {
	        var argu = arguments,
		        win = Mc.DOM.getDocument(docRef).window;

	        params = win.location.search;

	        var re = new RegExp("[?&]" + key + "=([^&$]*)", "i");
	        var offset = params.search(re);
	        if (offset == -1) return '';
	        if (RegExp.$1 == '') return '';
	        return decodeURIComponent(RegExp.$1);
	    },

	    encodeStringParams: function (url) {
	        return url.replace(/=([^&]*)/g, function (a, b) {
	            //escape
	            return '=' + encodeURIComponent(b);
	        });
	    },

	    toQueryString: function (obj) {
	        var arr = [];
	        for (var i in obj) arr.push(i + '=' + encodeURIComponent(obj[i]));
	        return arr.join('&');
	    },

	    paramsToObject: function () {
	    },

	    valueSwitch: function (element, switchClass) {
	        Mc.DOM.addEvent(element, 'focus', function () {
	            if (this.value == this.defaultValue) this.value = '';
	        });
	    },

	    getFocusTarget: function () {
	        return document.activeElement || null;
	    },

	    mergePath: function (src, path) {
	        src = src.replace(/\?.*$/, '');
	        var url = src.split('/');
	        if (path.indexOf('http://') != 0 && path.indexOf('/') != 0) {
	            Mc.Array.remove(url, url.length - 1);
	            url = url.join('/');
	            return url + '/' + path;
	        }
	        return path;
	    }
	}

    Mc.rgExp =
	{			//支持负数
	    Int: /^\-?\d{1,9}$/ig,
	    //浮点数
	    Float: /^\-?\d{1,9}(\.\d{1,2})?$/ig,
	    //邮件地址
	    Mail: /^[^\s@:"'<>,&]{2,}@[^\[\.]+\.[^\[]{2,}$/ig,
	    //电话号码
	    Tel: /^\+?[0-9]+\-?[0-9]+$/ig,
	    //手机
	    Mobel: /^0?13[0-9]{9}$/ig,
	    //MSN号码
	    Msn: /^[^\s@:"'<>,&]{2,}@[^\[\.]+\.[^\[]{2,}$/ig,
	    //Http地址
	    Http: /^(http|https):\/\/[A-Za-z0-9%\-_@]+\.[A-Za-z0-9%\-_@]{2,}[A-Za-z0-9\.\/=\?%\-&_~`@[\]:+!;]*$/ig
	};


    Mc.Base =
	{
	    IsNull: function (object, IsObject) {
	        if (object == null) return true;
	        if (IsObject) return (typeof (object) != "object");
	        if (object == "") return true;
	        return false;
	    },

	    //是否正整数
	    IsInt: function (object) {
	        return this.Search(object, Mc.rgExp.Int);
	    },
	    //转化对象为正整数 参数, 默认值, 最小值, 最大值
	    ToInt: function (object, defV, minV, maxV) {
	        if (!this.IsInt(object)) return (!this.IsInt(defV)) ? 0 : defV;
	        var reV = parseInt(object, 10);
	        defV = (!this.IsInt(defV)) ? 0 : parseInt(defV, 10);
	        minV = (!this.IsInt(minV)) ? minV - 1 : parseInt(minV, 10);
	        maxV = (!this.IsInt(maxV)) ? maxV + 1 : parseInt(maxV, 10);
	        if (reV > maxV || reV < minV) return defV;
	        return reV;
	    },
	    //转化对象为浮点数 参数, 默认值, 最小值, 最大值
	    ToFloat: function (object, defV, minV, maxV) {
	        if (!this.IsFloat(object)) return (!this.IsFloat(defV)) ? 0 : defV;
	        var reV = parseFloat(object, 10);
	        defV = (!this.IsFloat(defV)) ? 0 : parseFloat(defV, 10);
	        minV = (!this.IsFloat(minV)) ? minV - 1 : parseFloat(minV, 10);
	        maxV = (!this.IsFloat(maxV)) ? maxV + 1 : parseFloat(maxV, 10);
	        if (reV > maxV || reV < minV) return defV;
	        return reV;
	    },
	    //重新加载并转化数据
	    ReLoadArguments: function (object, sign) {
	        object = Mc.String.trimSign(object, sign);
	        var reg = new RegExp('\\' + sign + '{2,}', 'g');
	        return object.replace(reg, ',');
	    },
	    //正则查找 返回 Boolean
	    Search: function (object, reReg) {
	        if (Mc.Core.isNull(object)) return false;
	        reReg.lastIndex = 0;
	        return reReg.test(object);
	    }
	}



    Mc.Cookie = new function () {
        /**
        * 增加一个cookie。可以指定它的所有可写属性。
        * @param {String} name 存储名称，用于查找对应值，重复设置同名cookie，则前面的值会被覆盖
        * @param {String} value 存储值
        * @param {int} days 存储时间，单位：天。可以为小数，默认为1。
        * @param {String} path 存储路径
        * @param {String} domain 存储域名
        * @param {boolean} secure 是否起用安全属性
        * <p>
        * <b>注意：</b>sPath会影响cookie的读取，不同路径的相同name的cookie值并不会相互影响
        * </p>
        */
        this.setCookie = function (name, value, days, path, domain, secure) {
            days = days || 1;
            value = value || ' ';
            var expTime = new Date;
            expTime.setTime(expTime.getTime() + days * 86400000);
            document.cookie = name + "=" + encodeURIComponent(value) +
                "; expires=" + expTime.toGMTString() +
                (path ? "; path=" + path : "") +
                (domain ? "; domain=" + domain : "") +
                (secure ? "; secure" : "");
        }

        /**
        * 读取cookie。可以指定读取某个属性的值，也可以读取当前路径下所有值。
        * @param {String} name 存储名称
        * @type String
        * @return 指定值或整个cookie内容
        * <p>
        * <b>注意：</b>path会影响cookie的读取，不同目录下只能读取到当前目录的cookie，比如
        * Example:<br/>
        * <pre>
        * //项目当前路径为"/cookie",下面还有文件夹"inner"
        * Cookie.setCookie('a',100,0.5)
        * Cookie.setCookie('a',200,0.5,"/cookie/inner")
        * //在当前路径只能读取到a的值为100，但在inner文件夹下可以读取到200
        * </pre></p>
        * </p>
        */
        this.getCookie = function (name) {
            if (name) {
                var rs = document.cookie.match(new RegExp(name + "=[^;]*"));
                if (!rs) return '';
                return decodeURIComponent(rs[0].split('=')[1]);
            }
            return decodeURIComponent($Document.cookie);
        }

    }
    Mc.Msg = function (msg, func) {
        alert(msg);
        if (typeof (func) != "undefined") {
            func();
        }
        //$.gritter.add({
        //    title: '提示',
        //    text: msg,
        //    class_name: 'gritter-success',
        //    before_close: function () {
        //        if (typeof (func) != "undefined") {
        //            func();
        //        }
        //    },
        //    time: '3000'
        //});
    }
    Mc.Warning = function (msg) {
        alert(msg);
        //$.gritter.add({
        //    title: '警告',
        //    text: msg,
        //    sticky: true,
        //    class_name: 'gritter-info'
        //});

    }

    Mc.Error = function (msg) {
        alert(msg);
        //$.gritter.add({
        //    title: '错误',
        //    text: msg,
        //    sticky: true,
        //    class_name: 'gritter-error'
        //});

        return false;
    }

})(window);


