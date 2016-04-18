/*
" --------------------------------------------------
"   FileName: main.leaf.coffee
"       Desc: main.js webapp逻辑脚本
"     Author: chenglifu
"    Version: v0.1
" LastChange: 03/19/2014 11:19
" --------------------------------------------------
 */

/* Notice: 不要修改本文件，本文件由app.coffee自动生成 */
(function($) {
  var _func, _n, _stor, _vars;
  _n = this;
  _vars = _n.vars;
  _func = _n.func;
  _stor = _n.storage;
  _func.bindData = function(data, id) {
    var val;
    val = data[id];
    if (val.length) {
      $('#' + id).html(val);
    }
  };

  /* 模块定义 {{{ */
  _n.app('home', {
    init: function() {},
    indexAction: function() {
      _func.renderPage = function(data) {
        var BrandTitle, images, index, item, itemGroup, label, labelGroup, offset, pos, serviceName, services, _i, _imglist, _j, _k, _l, _len, _len1, _len2, _len3, _obj, _ref, _service, _style;
        _obj = data.obj;
        _style = _vars.themeConf[_obj.hotelStyle];
        if (_style.length) {
          $('.TV_main').addClass(_style);
        }
        $('#loadwait').hide();
        $('.TV_main').show();
        BrandTitle = _obj.BrandTitle;
        _func.bindData(_obj, 'BrandTitle');
        _func.bindData(_obj, 'BrandDescription');
        images = _obj.Images;
        _imglist = '';
        for (_i = 0, _len = images.length; _i < _len; _i++) {
          item = images[_i];
          _imglist += "<li class='Lfll'><img class='img' src='" + item.ImageURL + "' /></li>";
        }
        $('#imglist').html(_imglist);
        setTimeout(function() {
          $('#imglistclip').kxbdMarquee({
            direction: 'left',
            isEqual: false,
            hover: false,
            scrollDelay: 30
          });
        }, 300);
        services = _obj.services;
        labelGroup = [];
        itemGroup = [];
        for (_j = 0, _len1 = services.length; _j < _len1; _j++) {
          item = services[_j];
          serviceName = item.TypeName;
          offset = labelGroup.indexOf(serviceName);
          if (offset < 0) {
            labelGroup.push(serviceName);
            itemGroup.push([item.FacilityName]);
          } else {
            itemGroup[offset].push(item.FacilityName);
          }
        }
        _service = '';
        for (index = _k = 0, _len2 = labelGroup.length; _k < _len2; index = ++_k) {
          label = labelGroup[index];
          _service += "<div class='item'> <div class='title Lovh'><span class='txt'>" + label + "</span><span class='line'></span></div> <div class='text Lmt5'>";
          _ref = itemGroup[index];
          for (pos = _l = 0, _len3 = _ref.length; _l < _len3; pos = ++_l) {
            item = _ref[pos];
            _service += item;
            if (pos > 0) {
              _service += '、';
            }
          }
          _service += '</div></div>';
        }
        $('#serviceList').html(_service);
      };
      if (_vars.pageData) {
        _func.renderPage(_vars.pageData);
      } else {
          $.get(_vars.pageDataUrl, function (data) {
              if (typeof data === 'string') {
                  data = $.parseJSON(data);
              }
          _func.renderPage(data);
        });
      }
    }
  });
  return _n;
}).call(this.N = this.ndoo = this.ndoo || {}, this.Zepto || this.jQuery);
