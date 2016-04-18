var mouseX;
var mouseY;
var displayContent;
(function ($) {
    $('body').after("<div style='font-size:14px;;width:20%;position:absolute;top:0px;left:0px;z-index:2000;display:none;background:#FFFFFF' id='fullContent'></div>")
    $.fn.extend({
        reduceCotent: function (displayNum) {
            $('.displayContent').each(function (i) {
                var content = $(this).parent().find('.cotentHid').attr('value');
                if (content.length > displayNum) {
                    $(this).text(content.substr(0, displayNum) + "...");
                } else {
                    $(this).text(content);
                }
            })
            this.mouseover(function () {
               displayContent = $(this).find('.cotentHid').attr('value');
                
                t = setTimeout('showDiv();' ,1000);
            }).mouseout(function (){
                clearTimeout(t);
                document.getElementById('fullContent').style.display = 'none';
            })
           
        }
    });
})(jQuery);
function showDiv() {
    document.getElementById('fullContent').style.display = 'block';
    document.getElementById('fullContent').style.left = (mouseX+15) + "px";
    document.getElementById('fullContent').style.top = mouseY + "px";
    document.getElementById('fullContent').innerHTML = displayContent;
}
document.onmousemove = mouseMove;
function mouseMove(ev) {
    ev = ev || window.event;
    var mousePos = mouseCoords(ev);
    mouseX = mousePos.x;
    mouseY = mousePos.y;
}
function mouseCoords(ev) {
    if (ev.pageX || ev.pageY) {
        return { x: ev.pageX, y: ev.pageY };
    }
    return {
        x: ev.clientX + document.body.scrollLeft - document.body.clientLeft,
        y: ev.clientY + document.body.scrollTop - document.body.clientTop
    };
}
