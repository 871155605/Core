function redirect(error) {//跳转路径要对于引入该JS的页面
    console.log(error);
    var status = error.response.status;
    switch (status) {
        case 401: top.location.href = "../home/login.html";
            break;
        case 403: window.location.href = "../error/403.html";
            break;
        case 404: window.location.href = "../error/404.html";
            break;
        case 500: window.location.href = "../error/500.html";
            break;
    }
}