/*
" --------------------------------------------------
"   FileName: ndoo_prep.coffee
"       Desc: ndoo.js前置文件 for mini
"     Author: chenglifu
"    Version: ndoo.js(v0.1b2)
" LastChange: 03/19/2014 09:50
" --------------------------------------------------
 */

/* Notice: 不要修改本文件，本文件由ndoo_prep.coffee自动生成 */
(function() {
  var _n;
  _n = this;

  /*变量名称空间 */
  _n.vars || (_n.vars = {});

  /*函数名称空间 */
  _n.func || (_n.func = {});

  /*页面脚本空间 */
  _n.app || (_n.app = {});

  /*调试开关 */
  _n.isDebug = 0;
  _n.DELAY_FAST = 0;
  _n.DELAY_DOM = 1;
  _n.DELAY_DOMORLOAD = 2;
  _n.DELAY_LOAD = 3;
  _n._delayArr = [[], [], [], []];
  _n.delayRun = function(level, req, fn) {
    fn || (fn = [req, req = []][0]);
    if (typeof req === 'string') {
      req = req.split(',');
    }
    this._delayArr[level].push([req, fn]);
    return void 0;
  };
  _n._hookData = {};
  _n.hook = function(name, call, isOverwrite) {
    var args;
    if (call && call.apply) {
      if (this._hookData[name] && !isOverwrite) {
        return false;
      }
      this._hookData[name] = call;
      return true;
    } else {
      if (call = [this._hookData[name], args = [].concat(call) || []][0]) {
        return call.apply(null, args);
      }
    }
  };
  return _n;
}).call(this.N = this.ndoo = this.ndoo || {});
