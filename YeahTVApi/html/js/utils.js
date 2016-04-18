function showMask(json) {
    var h = document.body.clientHeight;
    $("<div class=\"ui_mask\"></div>").css({ display: "block", width: "100%", height: h }).appendTo("body");
    $("<div class=\"ui_mask_msg\"></div>").html(json).appendTo("body").css({
        display: "block",
        left: ($(document.body).outerWidth(true) - 190) / 2,
        top: (h - 45) / 2
    });
}

function showMask() {
    var h = document.body.clientHeight;
    $("<div class=\"ui_mask\" id=\"ui_mask\"></div>").css({ display: "block", width: "100%", height: h }).appendTo("body");
    $("<div class=\"ui_mask_msg\" id=\"ui_mask_msg\"></div>").appendTo("body").css({
        display: "block",
        left: ($(document.body).outerWidth(true) - 190) / 2,
        top: (h - 45) / 2
    });
}

function hideMask() {
    $('.ui_mask').remove();
    $('.ui_mask_msg').remove();
    
   
}

