function checkPermission(pTitle, sTitle, buttonVisible, permissions) {
    permissions = JSON.parse(sessionStorage.getItem((pTitle + sTitle).toLowerCase()));
    if (permissions == null) top.location.href = "../home/login.html";
    var buttons = Object.keys(buttonVisible);
    for (var p = 0; p < permissions.length; p++) {
        for (var b = 0; b < buttons.length; b++) {
            if ((permissions[p].toLowerCase() === (pTitle + buttons[b] + sTitle).toLowerCase())) {
                buttonVisible[buttons[b]] = true;
                continue;
            }
        }
    }
}