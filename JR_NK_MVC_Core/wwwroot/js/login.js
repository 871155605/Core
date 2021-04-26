var loginVue = new Vue({
    el: '#loginDiv',
    data: {
        loginType: 'user_password_realm',
        username: 'string',
        password: 'string',
        phoneNumber: '',
        checkCode: '',
        isThirdPartLogin: false,
        remembered: false,
        responseMsg: ''
    },
    methods: {
        //切换登录方式
        changeLoginType: function (type) {
            if (type == "user_phone_realm") {
                this.responseMsg = "短信登录暂未接入";
                return;
            }
            this.loginType = type;
            if (this.loginType === 'user_password_realm') {
                this.checkCode = '';
            }
            if (this.loginType === 'user_phone_realm') {
                this.password = '';
            }
        },
        //登录请求
        login: function () {
            if (this.loginType === 'user_password_realm') {
                if (this.username == '' || this.password == '') {
                    this.responseMsg = "请输入用户名和密码！";
                    return;
                }
            }
            if (this.loginType === 'user_phone_realm') {
                if (this.phoneNumber == '' || this.checkCode == '') {
                    this.responseMsg = "请输入手机号和短信验证码！";
                    return;
                }
            }
            //var salt = "bsmg";//使用固定的salt
            //var password = JSON.parse(JSON.stringify(this.password));//深克隆获得用户输入的密码，用于加密
            //this.password = md5(salt.charAt(3) + salt.charAt(2) + password + salt.charAt(0) + salt.charAt(1));
            axios.post('/admin/login', this.$data).then(response => {
                //password = "";
                if (response.data.code === 1) {
                    //JSON.stringify将JSON对象转换为字符串 因为sessionStorage.setItem只支持存储字符串格式
                    sessionStorage.setItem("Authorization", JSON.stringify(response.data.data.tokenJson));
                    sessionStorage.setItem("PermissionMenus", JSON.stringify(response.data.data.permissionMenuList));
                    window.location.href = "main.html";
                } else {
                    this.responseMsg = response.data.message;
                }
            }).catch(function (error) {
                console.log(error);
            });
        },
        //是否第三方登录
        thirdPartLogin: function () {
            this.isThirdPartLogin = !this.isThirdPartLogin;
        },
        //获取验证码
        getCheckCode: function () {
            if (this.phoneNumber == '') {
                this.responseMsg = "请输入手机号";
                return;
            }
            axios.post('/checkCode/getCheckCode', this.phoneNumber).then(function (response) {
                if (response.data.code === -1) {
                    this.responseMsg = response.data.message;
                }
                if (response.data.code === 0) {
                    this.responseMsg = response.data.message;
                }
            }).catch(function (error) {
                console.log(error);
            });
        },
        //方便输错密码一键删除
        cleanUpThePassword: function () {
            this.password = '';
            this.checkCode = '';
        }
    }
});

