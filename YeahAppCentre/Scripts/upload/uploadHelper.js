
(function ($) {
    $.fn.extend({
        binduploadify: function (options) {
            var defaults = {
                single:true,
                container: null,
                targetOjb: null,
                ext: "",
                limitSize: 30,
                showFullPath: false,
                filter: "",
                callback:null,
            };
            var uploadUrl = "/Attachment/Upload";
            var deleteUrl = "/Attachment/Delete/";
            var getUrl = "/Attachment/Get/";
            var getDocumentImgUrl = "/Template/GetDocumentImgById/";
            var opts = $.extend(defaults, options);
            return this.each(function () {
                var scope = this;
                var initializing=function() {
                    var value = opts.targetOjb.val();
                    if (!value) return;
                    getData(value, function (data) {
                        if (opts.single) {
                            var entity = data[0];
                            console.info(entity);
                            singleLoad(entity);
                        } else {
                            $.each(data, function() {
                                var item = this;
                                multiplyLoad(item);
                            });
                        }
                        

                    });

                }
                var getData = function (id, getCallBack) {
                    var url = getUrl + id;
                    if (!opts.showFullPath) {
                        $.getJSON(url, function (data) {
                        if (data) {
                            getCallBack(data);
                        } else {
                            Mc.Error("加载数据异常");
                        }
                    });
                }
                }
                var deleteAction = function (id, deleteCallBack)
                {
                    var url = deleteUrl + id;
                    $.post(url, function (data) {
                        if (data === "Success") {
                            deleteCallBack();
                        } else {
                            Mc.Error("删除异常");
                        }
                    });
                }

                var singleLoad= function(entity)
                {
                  
                    if (opts.showFullPath)
                    {
                        opts.targetOjb.val(entity.FilePath);
                        opts.container.attr("src", entity.FilePath);
                        opts.container.show();
                    }
                    else
                    {
                        if (opts.container) {
                    opts.targetOjb.val(entity.Id);
                        opts.container.attr("src", entity.FilePath);
                        opts.container.show();
                    }
                    }
                    
                   
                }
                var multiplyLoad= function(entity) {
                    var idArray = opts.targetOjb.val();
                    opts.targetOjb.val(idArray + "," + entity.Id);
                    var log = $("<div></div>");
                    $("<i class=\"icon-type " + entity.FileType + "\"></i>").appendTo(log);
                    $("<a href=\"" + entity.FilePath + "\">" + entity.FileName + "</a>").appendTo(log);
                    $("<b>" + entity.FileSize + "</b><i>" + entity.Unit + "<i>").appendTo(log);
                    log.appendTo(opts.container);
                    var delteCtrl = $("<span class=\"icon-delete\"></span>");
                    delteCtrl.click(function() {
                        $.post(deleteUrl, { id: entity.Id }, function (data) {
                            if (data !== "Success") {
                                alert("删除失败");
                            }
                        });
                    });
                    delteCtrl.appendTo(log);
                }
                var $obj = $(this);
                var type = $obj.attr("data_type");
                var formData = { type: type };
                var oBtn = $obj;
                initializing();
                new AjaxUpload(oBtn, {
                    action: uploadUrl,
                    name: "upload",
                    data: formData,
                    onSubmit: function (file, ext) {
                        var filter = "^(jpg|jpeg|png";
                        if (opts.ext.length>0) {
                            filter +="|"+opts.ext;
                        }
                        if (opts.filter.length > 0)
                        {
                            filter = "^(" + opts.filter;
                        }
                        filter += ")$";
                        var regExp=new RegExp(filter);
                        if (ext && regExp.test(ext)) {
                            //ext是后缀名
                            oBtn.text("正在上传…");
                            oBtn.attr("disabled", "disabled");
                        } else {
                            alert("不支持文件格式！");
                            return false;
                        }
                    },
                    responseType: false,
                    onChange: function (file, extension, sender) {
                        var fileSize = 0;
                        var isIE = /msie/i.test(navigator.userAgent) && !window.opera;
                        if (isIE) {
                            var fileSystem = new ActiveXObject("Scripting.FileSystemObject");
                            var fileObject = fileSystem.GetFile(file);
                            fileSize = fileObject.Size;
                        } else {
                            if (sender.files.length <= 0)
                            {
                                return false;
                            }
                            fileSize = sender.files[0].size;
                        }
                        var size = fileSize / 1024;

                        if (size > opts.limitSize) {
                            var tipSize = opts.limitSize + "KB";
                            if (opts.limitSize >= 1024)
                            {
                                tipSize = opts.limitSize / 1024 + "MB";
                            }
                            Mc.Error("上传的文件不能大于" + tipSize);
                            return false;
                        }
                    },
                    onComplete: function (file, response) {
                        if (response != null) {
                            var dataObj = eval("(" + response + ")");
                            if (!dataObj) {
                                alert("上传失败！");
                            }
                            else if (response == "ERROR:FORMAT")
                            {
                                oBtn.removeAttr("disabled");
                                oBtn.text("修改上传文件");
                                alert("不支持文件格式!");
                            }
                            else {
                                oBtn.removeAttr("disabled");
                                if (opts.single) {
                                    oBtn.text("修改上传文件");
                                    singleLoad(dataObj);
                                } else {
                                    oBtn.text("继续上传");
                                    multiplyLoad();
                                }
                            }
                            if (opts.callback)
                            {
                                opts.callback(dataObj);
                            }
                        }
                    }
                });
            });
        }
    });
})(jQuery);