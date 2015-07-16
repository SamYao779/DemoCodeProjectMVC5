$(document).ready(function () {
    // Start Image Slider
    $(".slider").bxSlider({
        slideWidth: 300,
        minSlides: 4,
        maxSlides: 4,
        moveSlides: 3,
        speed: 1000,
        auto: true,
    });

    $("#Ads").bxSlider({
        auto: true,
        mode: "vertical",
        speed: 7000,
        pause: false,
        controls: false,
    });

    $("#slider").layerSlider({
        responsive: true,
        autoStart: true,
        firstLayer: 2,
        navPrevNext: true,
        navStartStop: false,
        navButtons: false,
        randomSlideshow: true,
        skin: 'v5',
        skinPatch: '~/LayerSlider/skins/',
    });
    // End Image Slider

    // Start cart-fly
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
    // End Cart-fly

    // Start Remove-cart
    $("a.remove-cart").click(function () {
        $(this).parents("tr").hide(1000, function () {
            $(this).html("");
        });

        href = $(this).attr("href");
        $.ajax({
            url: href,
            success: function (result) {
                // Cập nhật lại phần Total
                $("#amount").html('<b>' + result.amout + '</b>');
            }
        });
        return false;
    });
    // End Remove-cart

    // Start Dialog
    $("#button1").click(function () {
        $("<div><b>Vui lòng đăng nhập để mua hàng</b></div>").addClass('dialog1').dialog({
            width: 400, autoOpen: false, title: 'Thông báo',
            modal: true, show: 'blind', hide: 'explode',
            buttons: {
                "Ok": {
                    text: 'Ok',
                    // Khi click Ok sẽ chuyển sang modal đăng nhập
                    click: function (e) {
                        e.preventDefault();
                        var href = "/Account/Login/";

                        $(".modal-dialog").load(href, function () {
                            $('#myModal').modal('show');
                        });
                        $(".dialog1").remove();
                    }
                },
                "Cancel": function () {
                    $(this).dialog('close');
                    $(".dialog1").remove();
                },

            },
            open: function (e) {
                // Tìm thẻ span, addclass vào thẻ span, thêm chức năng click
                $(".ui-dialog-titlebar").find("span").add("dialogclose").click(function () {
                    $(".dialog1").remove();
                });
            }
        });
        $(".dialog1").dialog("open");
    });
    $("#button2").click(function () {
        $("<div><b>Vui Lòng chọn sản phẩm</b></div>").addClass('dialog2').dialog({
            width: 400, autoOpen: false, title: 'Thông báo',
            modal: true, show: 'blind', hide: 'explode',
            buttons: {
                "OK": function () {
                    $(this).dialog('close');
                    $(".dialog2").remove();
                }
            },
            open: function (e) {
                // Tìm thẻ span, addclass vào thẻ span, thêm chức năng click
                $(".ui-dialog-titlebar").find("span").addClass("dialogclose").click(function () {
                    $(".dialog2").remove();
                });
            }
        });
        $(".dialog2").dialog("open");
    });
    // End Dialog

    // Start Language
    $("a[data-lang]").click(function () {
        expiry = new Date();
        expiry.setDate(expiry.getDate() + 30);
        lang = $(this).attr("data-lang");
        document.cookie = "lang=" + lang + ";expires" + expiry.toGMTString() + ";path=/";

        location.reload();
    });
    // End language

    // Start Back-to-top
    $(".backtop").addClass("backtophide");
    $(window).scroll(function () {
        if ($(this).scrollTop() != 0) {
            $(".backtop").fadeIn();
        }
        else {
            $(".backtop").fadeOut(500);
        }
    });

    $(".backtop").click(function () {
        $("body").animate({ scrollTop: 0 }, 500);
    });
    // End back-to-top

    // Start Modal
    $(".btn-edit, .btn-create, .btn-delete, .btn-detail, .btn-check, .login, .forgotpass, .register, .editprofile, .ui-button-text").click(function (e) {
        e.preventDefault();
        var href = $(this).attr('href');

        $(".modal-dialog").load(href, function () {
            $('#myModal').modal('show');
        });
    });
    // End Modal
});

(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) return;
    js = d.createElement(s); js.id = id;
    js.src = "//connect.facebook.net/en_US/sdk.js#xfbml=1&appId=834879946536745&version=v2.0";
    fjs.parentNode.insertBefore(js, fjs);
}
    (document, 'script', 'facebook-jssdk'));