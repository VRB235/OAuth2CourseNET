var config = {
    userStore: new Oidc.WebStorageStateStore({store: window.localStorage}),
    authority: "https://localhost:44339",
    client_id: "client_id_js",
    redirect_uri: "https://localhost:44370/Home/SignIn",
    response_type: "id_token token",
    scope: "openid ApiOne ApiTwo rc.scope",
    post_logout_redirect_uri: "https://localhost:44370/Home/Index"
};

var userManager = new Oidc.UserManager(config);

var signIn = function () {
    userManager.signinRedirect();
}

var signOut = function () {
    userManager.signoutRedirect();
}

userManager.getUser().then(user => {
    console.log("user: ", user);
    if (user) {
        axios.defaults.headers.common["Authorization"] = "Bearer " + user.access_token;
    }
});

var callApi = function () {
    axios.get("https://localhost:44321/secret").then(res => {
        console.log(res);
    });
};

var refreshing = false;

axios.interceptors.response.use(
    function (response) { return response },
    function (error) {
        console.log(error.response);

        var axiosConfig = error.response.config;
        if (error.response.status === 401) {
            if (!refreshing) {
                console.log("Refrescando token")
                refreshing = true;

                userManager.signinSilent().then(res => {
                    console.log(res)
                    // Actualiza la solicitud http
                    axios.defaults.headers.common["Authorization"] = "Bearer " + res.access_token;
                    axiosConfig.headers["Authorization"] = "Bearer " + res.access_token;
                    return axios(axiosConfig);
                });


            }
            // Reintenta la solicitud http
        }
        return Promise.reject(error);
    });