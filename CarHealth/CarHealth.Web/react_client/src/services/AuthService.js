
import { Log, UserManager } from 'oidc-client';

import { store } from '../index.js';

export class AuthService {

    constructor() {
        this.config = store.getState().config.appConfig.auth;

        this.userManager = new UserManager({
            authority: this.config.authority,
            client_id: this.config.clientId,
            redirect_uri: this.config.redirectUri,
            response_type: this.config.responseType,
            scope: this.config.scope,
            post_logout_redirect_uri: this.config.postLogoutRedirectUri,
            monitorSession: true,
            checkSessionInterval: 30000,
            revokeAccessTokenOnSignout: true,
            clockSkew: 300,
        });

        Log.logger = console;
        Log.level = 4;

        this.userManager.events.addUserLoaded(function () {
            console.log("userLoaded");
        });
        this.userManager.events.addUserUnloaded(function () {
            console.log("userUnloaded");
        });
        this.userManager.events.addAccessTokenExpiring(function () {
            console.log("access_token expiring...");
        });
        this.userManager.events.addAccessTokenExpired( () => {
            console.log("access_token expired");
            this.login();
        });
        this.userManager.events.addSilentRenewError(function (err) {
            console.error("silentRenewError: ", err);
        });

    }

    getUser() {
        return this.userManager.getUser();
    }
   
    login() {
        return this.userManager.signinRedirect();
    }
    logout() {
        return this.userManager.signoutRedirect();
    }

}
