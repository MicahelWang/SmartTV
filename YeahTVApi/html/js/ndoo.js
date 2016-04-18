/*
" --------------------------------------------------
"   FileName: ndoo.coffee
"       Desc: ndoo.js主结构文件 for mini
"     Author: chenglifu
"    Version: ndoo.js(v0.1b2)
" LastChange: 03/19/2014 09:54
" --------------------------------------------------
 */

/* Notice: 不要修改本文件，本文件由ndoo.coffee自动生成 */
(function($) {
  var _func, _n, _stor, _vars;
  _n = this;
  _n._delayRunHandle = function() {
    var fn, _i, _len, _ref;
    if (this._delayArr[0].length) {
      _ref = this._delayArr[0];
      for (_i = 0, _len = _ref.length; _i < _len; _i++) {
        fn = _ref[_i];
        fn[1]();
      }
      if (this._isDebug) {
        this._delayArr[0].length = 0;
      }
    }
    if (this._delayArr[1].length || this._delayArr[2].length) {
      $(function() {
        var fns, _j, _k, _len1, _len2;
        fns = _n._delayArr[1];
        for (_j = 0, _len1 = fns.length; _j < _len1; _j++) {
          fn = fns[_j];
          fn[1]();
        }
        fns = _n._delayArr[2];
        for (_k = 0, _len2 = fns.length; _k < _len2; _k++) {
          fn = fns[_k];
          fn[1]();
        }
        if (_n._isDebug) {
          _n._delayArr[1].length = 0;
          fns.length = 0;
        }
      });
    }
    if (this._delayArr[3].length) {
      $(window).bind('load', function() {
        var fns, _j, _len1;
        fns = _n._delayArr[3];
        for (_j = 0, _len1 = fns.length; _j < _len1; _j++) {
          fn = fns[_j];
          fn[1]();
        }
        if (_n._isDebug) {
          fns.length = 0;
        }
      });
    }
  };

  /* storage module {{{ */
  _n.storage = function(key, value, force, destroy) {
    var data;
    data = _n['storage'].data;
    if (value === void 0) {
      return data[key];
    }
    if (destroy) {
      delete data[key];
      return true;
    }
    if (!force && data.hasOwnProperty(key)) {
      return false;
    }
    return data[key] = value;
  };
  _n.storage.data = {};

  /* }}} */

  /* define app package {{{ */
  _n.app = function(name, app) {
    var _base;
    (_base = _n.app)[name] || (_base[name] = {});
    $.extend(_n.app[name], app);
  };

  /* }}} */
  _vars = _n.vars;
  _func = _n.func;
  _stor = _n.storage;
  $.extend(_n, {

    /*自增量 */
    _pk: 0,
    getPK: function() {
      return ++this._pk;
    },

    /*初始化 */
    init: function() {

      /*页面标识 */
      _n.pageId = $('#scriptArea').data('pageId');
      this.delayRun(this.DELAY_DOM, function() {
        var actionId, actionName, controller, controllerId, pageIdMatched, rawParams;
        _n.common();
        if (_n.pageId) {
          if (pageIdMatched = _n.pageId.match(/([^/]+)(?:\/?)([^?#]*)(.*)/)) {
            controllerId = pageIdMatched[1];
            actionId = pageIdMatched[2];
            rawParams = pageIdMatched[3];
          }
          if (controller = _n.app[controllerId]) {
            if (actionId) {
              actionName = actionId.replace(/(\/.)/, function(char) {
                return char.substring(1, 2).toUpperCase();
              });
            } else {
              actionName = '_empty';
            }
            if (!controller.inited && controller.init) {
              controller.inited = true;
              controller.init();
            }
          }
          if (actionName) {
            if (controller[actionName + 'Before']) {
              controller[actionName + 'Before'](rawParams);
            }
            if (controller[actionName + 'Action']) {
              controller[actionName + 'Action'](rawParams);
            }
            if (controller[actionName + 'After']) {
              controller[actionName + 'After'](rawParams);
            }
          }
        }
      });

      /*延迟执行DOMLOAD */
      this._delayRunHandle();
    },

    /*公共调用 */
    common: function() {

      /*init tpl */
      _n.hook('addTrack');
      _n.hook('App:Init');
    },

    /*初始化Dialog模板 initTpl */
    initTpl: function() {
      var $code, e, text;
      $code = $('#tplCode');
      if ($code.length) {
        text = $code.get(0).text.replace(/^\s*|\s*$/g, '');
        if (text !== '') {
          try {
            $(text).appendTo('#tplArea');
          } catch (_error) {
            e = _error;
            return false;
          }
        }
        return true;
      }
      return false;
    }
  });

  /*初始化入口 */
  return _n;
}).call(this.N = this.ndoo = this.ndoo || {}, this.Zepto || this.jQuery);
