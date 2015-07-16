$(function () {
    $("a.add-cart").click(function () {
        var hinh = $(this).parents(".box>div").find(".image");
        var css = ".cart-fly {background: url(" + hinh.attr("src") + "); background-size:100% 100%;z-index: 9999;}";
        $("#cart-fly").html(css);
        var option = { to: "#cart", className: "cart-fly" }
        hinh.effect("transfer", option, 1000);

        href = $(this).attr("href");

        $.ajax({
            url: href,
            success: function (response) { }
        });
        return false;
    });
});