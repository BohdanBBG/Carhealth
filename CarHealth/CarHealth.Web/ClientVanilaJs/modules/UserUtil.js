
import Oidc from 'oidc-client';

class UserUtil {

    constructor(config) {
        this.config = config;

        this.authConfig = {

            // authority: this.config.auth.authority,
            // client_id: this.config.auth.clientId,
            // redirect_uri: this.config.auth.redirectUri,
            // response_type: this.config.auth.responseType,
            // scope: this.config.auth.scope,
            // post_logout_redirect_uri: this.config.auth.postLogoutRedirectUri,

            // // access_token renew
            // automaticSilentRenew: true,
            // silentRequestTimeout: 10000,
            // accessTokenExpiringNotificationTime: 3 * 60, // 3 mins in secs

            authority: "http://localhost:5005", // Адрес нашего IdentityServer
            client_id: "CarHealth.Web", // должен совпадать с указанным на IdentityServer
            // Адрес страницы, на которую будет перенаправлен браузер после прохождения пользователем аутентификации
            // и получения от пользователя подтверждений - в соответствии с требованиями OpenId Connect
            redirect_uri: "http://localhost:5003/callback.html",
            // Response Type определяет набор токенов, получаемых от Authorization Endpoint
            // Данное сочетание означает, что мы используем Implicit Flow
            // http://openid.net/specs/openid-connect-core-1_0.html#Authentication
            response_type: "id_token token",
            // Получить subject id пользователя, а также поля профиля в id_token, а также получить access_token для доступа к api1 (см. наcтройки IdentityServer)
            scope: "openid profile email CarHealth.Api",
            // Страница, на которую нужно перенаправить пользователя в случае инициированного им логаута
            post_logout_redirect_uri: "http://localhost:5003/index.html",
            // следить за состоянием сессии на IdentityServer, по умолчанию true
            monitorSession: true,
            // интервал в миллисекундах, раз в который нужно проверять сессию пользователя, по умолчанию 2000
            checkSessionInterval: 30000,
            // отзывает access_token в соответствии со стандартом https://tools.ietf.org/html/rfc7009
            revokeAccessTokenOnSignout: true,
            // допустимая погрешность часов на клиенте и серверах, нужна для валидации токенов, по умолчанию 300
            // https://github.com/IdentityModel/oidc-client-js/blob/1.3.0/src/JoseUtil.js#L95
            clockSkew: 300,
            // делать ли запрос к UserInfo endpoint для того, чтоб добавить данные в профиль пользователя
            loadUserInfo: true,
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
            //user.access_token = 'eyJhbGciOiJSUzI1NiIsImtpZCI6IkY0MzgyNUFFMjQ4NDNFNjMzMjYwQjlBOTU1RDExNDQ4NkZGRURCRUMiLCJ0eXAiOiJKV1QiLCJ4NXQiOiI5RGdscmlTRVBtTXlZTG1wVmRFVVNHXy0yLXcifQ.eyJuYmYiOjE1NjQwNTE5MjQsImV4cCI6MTU2NDA1NTUyNCwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTAwNCIsImF1ZCI6WyJodHRwczovL2xvY2FsaG9zdDo1MDA0L3Jlc291cmNlcyIsIndlYmFwaSJdLCJjbGllbnRfaWQiOiJ3ZWJzcGEiLCJzdWIiOiI1ZDM5ODk5YjA5ZDAyNjI1NzRkNWYwOWYiLCJhdXRoX3RpbWUiOjE1NjQwNTE5MjIsImlkcCI6ImxvY2FsIiwic2NvcGUiOlsib3BlbmlkIiwicHJvZmlsZSIsIndlYmFwaSIsIm9mZmxpbmVfYWNjZXNzIl0sImFtciI6WyJwd2QiXX0.n9vNo2wkZEEtNS2hHHEroDYiUG0OPoXFkF86poJEJXNhQrndAjQdVMc4FeFehUDP7GOj7pSCAmcllvRiRsUjAQ-IeV5DEjtgFsLxZon8svbb5UPJ-efjULcHT-U2u5a-eWqRQXck1gZ2W9fIzCcaBYzptV_K9gjlhuFLlUVs-L2PQe0gULHu0fKYmZjtdO-bI8hBYo8ZSvvwrRVMVgKp798bmlIX5z12mnh_knLCWCUe-tCn4qe0X0oHgHb3KGRneTR2JpocCQWSvdYYkPm-rR-XK3m8EETq_kxXCdTd1nRcV2pKa0sSynpcLW2MOQiOZ61wuFcsHolGV6K9zWnBFQ'
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
                log(xhr.status, 200 == xhr.status ? JSON.parse(xhr.responseText) : "An error has occured.");
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

export default UserUtil;