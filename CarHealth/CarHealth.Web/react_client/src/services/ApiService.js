
import { AuthService } from "../services/AuthService.js";

const superagent = require('superagent');

export class ApiService {

    constructor() {
        this.authService = new AuthService();
    }

    httpChek(url, callBack, authToken = null) {

        var xhr = new XMLHttpRequest();
        //xhr.withCredentials = true; // force to show browser's default auth dialog
        xhr.open('GET', url);


        xhr.onload = function () {
            if (xhr.status === 200) {
                console.log("Ok")
            }
            else if (xhr.status === 500) {
                console.error('Request failed.  Returned status of ' + xhr.status);
                alert('Request failed.  Returned status of ' + xhr.status);
            } else if (xhr.status === 401 || xhr.status === 403) {
                console.error('Request failed.  Returned status of ' + xhr.status);
                // document.location.href = identityUrl + "/Account/Login";
            }
            else {
                console.error('Request failed.  Returned status of ' + xhr.status);
            }

        };

        if (authToken !== null) {
            xhr.setRequestHeader("Authorization", "Bearer " + authToken);
        }

        xhr.send();
    }

    httpRequest(url, data, typeOfrequest, callback, authToken = null) {

        var xhr = new XMLHttpRequest();

        //   xhr.withCredentials = true; // force to show browser's default auth dialog
        xhr.open(typeOfrequest, url, true);

        xhr.setRequestHeader('Content-Type', 'application/json');
        xhr.onload = function () {
            if (xhr.status === 200) {



            } else if (xhr.status === 500) {
                console.error('Request failed.  Returned status of ' + xhr.status);
                alert('Request failed.  Returned status of ' + xhr.status);
            } else if (xhr.status === 401 || xhr.status === 403) {
                console.error('Request failed.  Returned status of ' + xhr.status);
                alert('Request failed.  Returned status of ' + xhr.status + ', unauthorized user');
            }
            callback(xhr.status);
        };

        if (authToken !== null) {
            xhr.setRequestHeader("Authorization", "Bearer " + authToken);
        }

        xhr.send(JSON.stringify(data));
    }

    deleteRequest(url, authToken = null) {

        var xhr = new XMLHttpRequest();

        //  xhr.withCredentials = true; // force to show browser's default auth dialog
        xhr.open('DELETE', url, true);

        xhr.onload = function () {
            if (xhr.status === 200) {

                console.log("Item successfully deleted");

            } else if (xhr.status === 500) {
                console.error('Request failed.  Returned status of ' + xhr.status);
                alert('Request failed.  Returned status of ' + xhr.status);
            } else if (xhr.status === 401 || xhr.status === 403) {
                console.error('Request failed.  Returned status of ' + xhr.status);
                // document.location.href = identityUrl + "/Account/Login";
            }
        };

        if (authToken !== null) {
            xhr.setRequestHeader("Authorization", "Bearer " + authToken);
        }
        xhr.send();
    }

    async getRequest(url) {
        const user = await this.authService.getUser();
        if (user && user.access_token) {
            return new Promise(function (resolve, reject) {
                superagent
                    .get(url)
                    .set('Content-Type', 'application/json; charset=utf-8 ')
                    .set("Authorization", `Bearer ${user.access_token}`)
                    .then(res => {
                        if (res.text !== "") {

                            console.log("----HttpService.getRequest:", JSON.parse(res.text));

                            resolve(JSON.parse(res.text));

                        }
                    })
                    .catch(err => {
                        reject(err);
                    });
            });
        } else {
            this.authService.login();
        }//else if (user) {
        //   return this.authService.renewToken().then(renewedUser => {
        //     return this._callApi(renewedUser.access_token);
        //   });
        //}
        // else {
        //     throw new Error('user is not logged in');
        // }

    }

}
