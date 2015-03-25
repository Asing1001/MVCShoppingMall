$(document).ready(function () {

    $('#FileUpload1').change(function () {
        $('#pUrl').text('預估url為:/Content/Images/Product/' + this.files[0].name);

        var reader = new FileReader();

        reader.readAsDataURL(this.files[0]);
        reader.onload = function (e) {
            $('#img1').attr('src', e.target.result);

        }
    });

    $('#myTheme>li>a').click(function () {
        $('#theme').attr('href', '../../Content/bootstrap_' + $(this).text() + '.css');
        //$.ajax({
        //    url: "/Home/LoadTheme/?Theme=" + $(this).text(),
        //    success: function (data) {
        //        $("link[rel=stylesheet]").attr({ href: data });
        //        return false;
        //    }
        //})
    });


    $('.carousel').carousel({
        interval: 3000
    });
});
