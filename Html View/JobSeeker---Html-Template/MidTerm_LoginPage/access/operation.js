$(document).ready(function () {
    $(".signup").hover(
        function () {
            $(".login").addClass("box-hover");
        },
        function () {
            $(".login").removeClass("box-hover");
        }
    );
    $(".login").hover(
        function () {
            $(".s1").addClass("lg");
        },
        function () {
            $(".s1").removeClass("lg");
        }
    );
    $(".signup").hover(
        function () {
            $(".s2-1").addClass("su");
            $(".s2-2").addClass("su2");
        },
        function () {
            $(".s2-1").removeClass("su");
            $(".s2-2").removeClass("su2");
        }
    );
    $(".login").hover(
        function () {
            $(".sub1").addClass("appear");
            $(".btn1").addClass("appear");
        },
        function () {
            $(".sub1").removeClass("appear");
            $(".btn1").removeClass("appear");
        }
    );
    $(".signup").hover(
        function () {
            $(".sub2").addClass("appear");
            $(".btn2").addClass("appear");
        },
        function () {
            $(".sub2").removeClass("appear");
            $(".btn2").removeClass("appear");
        }
    );
    $(".btn1").click(
        function () {
            $(".login").addClass("pushing");
            $(".signup").addClass("bePushed");
            $(".backbtn").addClass("reAppear");
            $(".LGFORM").addClass("springUp");
        },
    );
    $(".backbtn").click(
        function () {
            $(".login").removeClass("pushing");
            $(".signup").removeClass("bePushed");
            $(".backbtn").removeClass("reAppear");
            $(".LGFORM").removeClass("springUp");
        },
    );
    $(".btn2").click(
        function () {
            $(".signup").addClass("pushing2");
            $(".login").addClass("bePushed2");
            $(".backbtn2").addClass("reAppear");
            $(".SGFORM").addClass("springUp2");
        },
    );
    $(".backbtn2").click(
        function () {
            $(".signup").removeClass("pushing2");
            $(".login").removeClass("bePushed2");
            $(".backbtn2").removeClass("reAppear");
            $(".SGFORM").removeClass("springUp2");
        },
    );
    $('#reveal1').click(
        function () {
            if ($( "#reveal1" ).hasClass('bi-eye-slash')) {
                $('#reveal1').addClass('bi-eye');
                $( "#reveal1" ).removeClass( 'bi-eye-slash');
                $(".passw1").attr('type','text');
            } 
            else{
                $('#reveal1').addClass('bi-eye-slash');
                $( "#reveal1" ).removeClass('bi-eye');
                $(".passw1").attr('type','password');
            }
        },
    );
    $('#reveal2').click(
        function () {
            if ($( "#reveal2" ).hasClass('bi-eye-slash')) {
                $('#reveal2').addClass('bi-eye');
                $( "#reveal2" ).removeClass( 'bi-eye-slash');
                $(".passw2").attr('type','text');
            } 
            else{
                $('#reveal2').addClass('bi-eye-slash');
                $( "#reveal2" ).removeClass('bi-eye');
                $(".passw2").attr('type','password');
            }
        },
    );
    $('#reveal3').click(
        function () {
            if ($( "#reveal3" ).hasClass('bi-eye-slash')) {
                $('#reveal3').addClass('bi-eye');
                $( "#reveal3" ).removeClass( 'bi-eye-slash');
                $(".passw3").attr('type','text');
            } 
            else{
                $('#reveal3').addClass('bi-eye-slash');
                $( "#reveal3" ).removeClass('bi-eye');
                $(".passw3").attr('type','password');
            }
        },
    );
});
