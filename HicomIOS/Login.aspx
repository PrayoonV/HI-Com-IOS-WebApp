<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="HicomIOS.Master.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" rel="stylesheet" />

    <style type="text/css">
        .login {
            border-radius: 0px;
        }

        .page-login {
            margin: 0 auto;
            padding-top: 50px;
            text-align: center;
        }

        .head-login {
            color: #990000;
            font-weight: bold;
            font-size: 55px;
        }

        .width-login {
            width: 350px;
        }

        .btn-login {
            color: #FFFFFF;
            background-color: #990000;
            border-color: #990000;
        }

            .btn-login:hover {
                color: #FFFFFF;
                background-color: #b12525;
                border-color: #b12525;
            }

        .has-error .form-control {
            border-color: #e40703 !important;
        }
    </style>

</head>
<body>
    <div class="width-login page-login">
        <div>
            <h1 class="head-login">Hi-Com IOS</h1>
        </div>
        <h4 style="color: #7f8c8d;">ยินดีต้อนรับสู่ระบบบริหารจัดการข้อมูล</h4>
        <form id="formlogin" enctype="multipart/form-data" action="" method="post" onsubmit="return false;">
            <div class="form-group">
                <input id="username" type="text" class="form-control login" placeholder="ชื่อผู้ใช้งาน" data-rule-required="true" data-msg-required="กรุณากรอกชื่อผู้ใช้งาน" />
            </div>

            <div class="form-group">
                <input id="password" type="password" class="form-control login" placeholder="รหัสผ่าน" maxlength="15" data-rule-required="true" data-msg-required="กรุณากรอกรหัสผ่าน" />
            </div>

            <div class="form-group">
                <button type="submit" onclick="submit_login()" class="btn btn-login width-login login">Login</button>
            </div>


        </form>
        <p>
            <small>Copyrights &copy; 2018 Hi-Com. All Rights Reserved. <br />Designed by S-Planet</small>
        </p>

         <p>
            <small>System version Hi-Com IOS ( 1.0.0 )</small>
        </p>
    </div>
</body>
<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
<script type="text/javascript" src=" https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
<script>

    $('#username').keyup(function () {
        if ($('#username').val() != "") {
            $('#username').parent().removeClass("has-error");
            $('#username').next().remove('.help-block');
        }
    });
    $('#password').keyup(function () {
        if ($('#password').val() != "") {
            $('#password').parent().removeClass("has-error");
            $('#password').next().remove('.help-block');
        }
    });
    function submit_login() {
        if ($('#username').val() === "") {
            var username = $('#username');
            var error_username = '<span id="' + (username.attr('id')) + '-error" style="color: #e40703;" class="help-block">' + username.attr("data-msg-required") + '</span>';
            username.parent().addClass('has-error');

            if ($('#' + username.attr('id') + '-error').hasClass('help-block') === false) {
                username.after(error_username);
            }
            return false;
        }
        if ($('#password').val() === "") {
            console.log("password");
            var password = $('#password');
            var error_password = '<span id="' + (password.attr('id')) + '-error" style="color: #e40703;" class="help-block">' + password.attr("data-msg-required") + '</span>';
            password.parent().addClass('has-error');
            console.log("error_password");

            if ($('#' + password.attr('id') + '-error').hasClass('help-block') === false) {
                password.after(error_password);
            }
            return false;
        }

        var data = ({
            username: $("#username").val(),
            password: $("#password").val()

        });
        $.ajax({
            type: "POST",
            url: "Login.aspx/CheckLogin",
            data: '{username:"' + $("#username").val() + '" ,password:"' + $("#password").val() + '" }',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data.d == 'success') {
                    window.location.href = "/Default.aspx"
                } else {
                    var username = $('#username');
                    var error_username = '<span id="' + (username.attr('id')) + '-error" style="color: #e40703;" class="help-block">' + "กรุณากรอกชื่อผู้ใช้งานให้ถูกต้อง" + '</span>';
                    username.parent().addClass('has-error');

                    if ($('#' + username.attr('id') + '-error').hasClass('help-block') === false) {
                        username.after(error_username);
                    }


                }
            }

        });
    }
    $(document).ready(function () {
        $.ajax({
            type: "POST",
            url: "/Login.aspx/CheckLogout",
            data: {},
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                window.location.href = "/Default.aspx"
                //if (data.d == 'success') {
                //    location.reload();
                //} else {
                //    console.info("data", data.d);
                //}
            }

        });
    });


</script>
</html>


