
(function ($) {
    $.fn.extend({
        binduploadifyForAndriod: function (options) {
            var defaults = {
                callback: function (data) {
                    console.log(data);
                }
            };
            var uploadUrl = "/Attachment/UploadApk";
            var opts = $.extend(defaults, options);
            return this.each(function () {
                var $obj = $(this);
                var type = $obj.attr("data_type");
                var formData = { type: type };
                var oBtn = $obj;
                new AjaxUpload(oBtn, {
                    action: uploadUrl,
                    name: "upload",
                    data: formData,
                    onSubmit: function (file, ext) {
                        var filter = "^(apk)$";
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
                    onChange: function (file, extension,sender) {
                        var fileSize = 0;
                        var isIE = /msie/i.test(navigator.userAgent) && !window.opera;
                        if (isIE) {
                            var fileSystem = new ActiveXObject("Scripting.FileSystemObject");
                            var fileObject = fileSystem.GetFile(file);
                            fileSize = fileObject.Size;
                            alert("IsIe");
                        } else {
                            fileSize = sender.files[0].size;
                        }
                        var size = fileSize / (1024 * 1024);
                        if (size > 20) {
                            Mc.Error("附件不能大于20M");
                            return false;
                        }
                    },
                    onComplete: function (file, response) {
                        if (response != null) {
                            var data = eval("(" + response + ")");
                            if (!data) {
                                alert("上传失败！");
                            } else {
                                oBtn.removeAttr("disabled");
                                oBtn.text("修改上传文件");
                                opts.callback(data, oBtn);
                            }
                        }
                    }
                });
            });
        }
    });
})(jQuery);