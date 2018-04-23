$(".remove").on("click", function () {
    $(this).parent().remove();
    if ($("#removeSounds").val()) {
        $("#removeSounds").val($("#removeSounds").val() + ";" + $(this).prev().find("span").html());
    } else {
        $("#removeSounds").val($("#removeSounds").val() + $(this).prev().find("span").html());
    }

});