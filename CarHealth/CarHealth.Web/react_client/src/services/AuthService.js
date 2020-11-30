
import Oidc from 'oidc-client';

class AuthService {

    constructor(config) {
        this.config = config;

        this.authConfig = {

            authority: this.config.auth.authority, 
            client_id: this.config.auth.clientId, 
            redirect_uri: this.config.auth.redirectUri,
            response_type: this.config.auth.responseType,
            scope: this.config.auth.scope,
            post_logout_redirect_uri: this.config.auth.postLogoutRedirectUri,
            monitorSession: true,
            checkSessionInterval: 30000,
            revokeAccessTokenOnSignout: true,
            clockSkew: 300
        };

        this.userManager = new Oidc.UserManager( this.authConfig);

        // setup logging

        Oidc.Log.logger = console;
        Oidc.Log.level = 4;

        this.userManager.events.addUserLoaded(function () {
            console.log("userLoaded");
        });
        this.userManager.events.addUserUnloaded(function () {
            console.log("userUnloaded");
        });
        this.userManager.events.addAccessTokenExpiring(function () {
            console.log("access_token expiring...");
        });
        this.userManager.events.addAccessTokenExpired(function () {
            console.log("access_token expired");
        });
        this.userManager.events.addSilentRenewError(function (err) {
            console.error("silentRenewError: ", err);
        });

    }

    getUser(callback) {
        this.userManager.getUser().then(function (user) {
            callback(user);

        });
    }

    login() {
        // Инициировать логин
        this.userManager.signinRedirect();
    }
    logout() {
        // Инициировать логаут
        this.userManager.signoutRedirect();
    }


    requestUrl(mgr, url) {
        mgr.getUser().then(function (user) {
            var xhr = new XMLHttpRequest();
            xhr.open("GET", url);
            xhr.onload = function () {
                //log(xhr.status, 200 == xhr.status ? JSON.parse(xhr.responseText) : "An error has occured.");
            }
            // добавляем заголовок Authorization с access_token в качестве Bearer - токена. 
            xhr.setRequestHeader("Authorization", "Bearer " + user.access_token);
            xhr.send();
        });
    }

    log() {
        document.getElementById('results').innerText = '';

        Array.prototype.forEach.call(arguments, function (msg) {
            if (msg instanceof Error) {
                msg = "Error: " + msg.message;
            }
            else if (typeof msg !== 'string') {
                msg = JSON.stringify(msg, null, 2);
            }
            document.getElementById('results').innerHTML += msg + '\r\n';
        });
    }

}

export default AuthService;