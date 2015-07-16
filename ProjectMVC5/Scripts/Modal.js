$(document).ready(function () {
    // Start Modal
    $(".btn-edit, .btn-create, .btn-delete, .btn-detail, .btn-check, .login, .forgotpass, .register, .editprofile").click(function (e) {
        e.preventDefault();
        var href = $(this).attr('href');

        $(".modal-dialog").load(href, function () {
            $('#myModal').modal('show');
        });
    });
    // End Modal
});