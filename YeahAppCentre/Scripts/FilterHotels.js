
var isFirst = true;
var filterHotelExistHotels;

var openFilterHotelsDialog = function (filterOpt) {

    var getFilterHotelsOpt = function() {

        var opt = {};
        opt.title = "酒店筛选";
        opt.url = "/PartView/FilterHotels";

        opt.saveclick = function () {

            var selectHotels="";

            $("#select-right").find("option").each(function () {

                selectHotels=selectHotels+"{'Id':'" + $(this).val() + "','Name':'" + $(this).text() + "'},";

            });

            filterOpt.saveEvent(eval("[" + selectHotels + "]"));
            event.preventDefault();
            return true;
        };

        return opt;
    };

    isFirst = true;
    filterHotelExistHotels=filterOpt.existHotels;
    openDialog(getFilterHotelsOpt(), 0);

};



$(document).delegate("#GroupId", "change",
    function () {
        var groupId = $(this).val();
        var obj = $("#BrandId");
        debugger;
        BrandFromGroup(groupId, obj, "", "/Hotel/GetBrandsByGroup/" + groupId);

    });

$(document).delegate("#selectItem", "click",
    function () {

        var rightSelect = $("#select-right");

        $("#select-left").find("option:selected").each(function () {

            if ($("#select-right option[value=" + $(this).val() + "]").length == 0) {

                var option = $("<option value=" + $(this).val() + ">" + $(this).text() + "</option>");
                option.appendTo(rightSelect);
            }
        });

    });

$(document).delegate("#selectAll", "click",
    function () {

        var rightSelect = $("#select-right");

        $("#select-left").find("option").each(function () {

            if ($("#select-right option[value=" + $(this).val() + "]").length == 0) {

                var option = $("<option value=" + $(this).val() + ">" + $(this).text() + "</option>");
                option.appendTo(rightSelect);
            }
        });

    });

$(document).delegate("#select-left option", "dblclick",
    function () {

        $("#selectItem").click();

    });

$(document).delegate("#deleteItem", "click",
    function () {

        var selectIndex = $("#select-right").get(0).selectedIndex;

        $("#select-right").find("option:selected").each(function () {

            this.remove();
        });

        if (selectIndex !== 0) {
            $("#select-right").get(0).selectedIndex = selectIndex - 1;
        } else {
            $("#select-right").get(0).selectedIndex = selectIndex;
        }
    });

$(document).delegate("#clearItem", "click",
    function () {

        $("#select-right").empty();

    });


$(document).delegate(".tab-search button.btn-success", "click",
    function () {

        $.post("/PartView/SearchHotels", { HotelName: $("#HotelName").val(), GroupId: $("#GroupId").val(), BrandId: $("#BrandId").val() },
            function (result) {

                var obj = $("#select-left");

                obj.empty();

                $(result).each(function () {
                    var option = $("<option value=" + this.Id + ">" + this.Name + "</option>");
                    option.appendTo(obj);
                });

                if (isFirst) {
                    var rightobj = $("#select-right");
                    $(filterHotelExistHotels).each(function () {
                        var option = $("<option value=" + this.Id + ">" + this.Name + "</option>");
                        option.appendTo(rightobj);
                    });
                    isFirst = false;
                }

            });
    });

function BrandFromGroup(groupId, obj, selectVal, requestUrl) {
    obj.empty();
    var defaultOption = $("<option value=\"\">-请选择-</option>");
    defaultOption.appendTo(obj);
    if (groupId === "")
        return;
    var url = requestUrl;
    $.getJSON(url, function (data) {

        $(data).each(function () {
            var option = $("<option value=" + this.Id + ">" + this.BrandName + "</option>");
            option.appendTo(obj);
        });
        if (selectVal) {
            obj.val(selectVal);
        }
    });
}
