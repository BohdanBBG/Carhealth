
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
        this.userManager.signinRedirect();
    }
    logout() {
        this.userManager.signoutRedirect();
    }
}

export default AuthService;