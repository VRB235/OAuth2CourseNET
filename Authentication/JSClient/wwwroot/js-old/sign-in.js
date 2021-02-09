var createState = function () {
    return "SessionValueMakeIrAbITlODASDASDDDDDDDDDDDDDDDDDDSD";
}
var createNonce = function () {
    return "NonceValueSADDDDDDDDDDDDDDACASCASCASSSSSSSSSSSSSSSSSSSSSSSSSS";
}

var signIn = function () {
    var redirectUri = encodeURIComponent("https://localhost:4437/Home/SignIn");
    var responseType = encodeURIComponent("id_token token");
    var scope = encodeURIComponent("openid ApiOne");
    var authUrl = "/connect/authorize/callback"+
                "?client_id=client_id_js"+
                "&redirect_uri="+redirectUri+
                "&response_type="+responseType+
                "&scope="+scope+
                "&nonce="+ createNonce()+
                "&state="+createState();
    var returnUrl = encodeURIComponent(authUrl);
    console.log(authUrl)
    console.log(redirectUri)

    window.location.href = "https://localhost:44339/Auth/Login?ReturnUrl=" + returnUrl;
}