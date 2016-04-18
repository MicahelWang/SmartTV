function UpdaloadImage(obj) {
    var Coversetting = {
        single: true,
        limitSize: 250,
        container: $(obj).closest("tr").find("img"),
        targetOjb: $(obj).closest("tr").find("input[name=attributeValue]"),
        showFullPath: true
    };
    $(obj).binduploadify(Coversetting);
}
function UpdaloadMusic(obj) {
    var Coversetting = {
        single: true,
        container: $(obj).closest("tr").find("audio"),
        targetOjb: $(obj).closest("tr").find("input[name=attributeValue]"),
        showFullPath: true,
        filter: "mp3",
        limitSize: 1024 * 2
    };
    $(obj).binduploadify(Coversetting);
}

function UpdaloadMovie(obj) {
    var Coversetting = {
        single: true,
        container: $(obj).closest("tr").find("img"),
        targetOjb: $(obj).closest("tr").find("input[name=attributeValue]"),
        showFullPath: true,
        filter: "mp4",
        limitSize: 1024 * 10
    };
    $(obj).binduploadify(Coversetting);
}