import {userUtil} from '../index.js';

class HttpUtil {

    constructor() {
    }

    httpChek(url, callBack, authToken = null) {

        var xhr = new XMLHttpRequest();
        //xhr.withCredentials = true; // force to show browser's default auth dialog
        xhr.open('GET', url);


        xhr.onload = function () {
            if(xhr.status === 200)
            {
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

        var identityUrl = this.identityUrl;

        var xhr = new XMLHttpRequest();

     //   xhr.withCredentials = true; // force to show browser's default auth dialog
        xhr.open(typeOfrequest, url, true);

        xhr.setRequestHeader('Content-Type', 'application/json');
        xhr.onload = function () {
            if (xhr.status === 200) {

                callback();

            } else if (xhr.status === 500) {
                console.error('Request failed.  Returned status of ' + xhr.status);
                alert('Request failed.  Returned status of ' + xhr.status);
            } else if (xhr.status === 401 || xhr.status === 403) {
                console.error('Request failed.  Returned status of ' + xhr.status);
             //   document.location.href = identityUrl + "/Account/Login";
            }
        };

        if (authToken !== null) {
            xhr.setRequestHeader("Authorization", "Bearer " + authToken);
        }

        xhr.send(JSON.stringify(data));
    }

    deleteRequest(url, authToken = null) {

        var identityUrl = this.identityUrl;

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

    httpGet(url, callBack, authToken = null) {

        var identityUrl = this.identityUrl;

        var xhr = new XMLHttpRequest();
     //   xhr.withCredentials = true; // force to show browser's default auth dialog
        xhr.open('GET', url);


        xhr.onload = function () {
            if (xhr.status === 200) {
                if ( xhr.responseText !== "")
                {
                    var data = JSON.parse(xhr.responseText);
                    console.log(1, data);
                }
              
                callBack(data);
               
            } else if (xhr.status === 500) {
                console.error('Request failed.  Returned status of ' + xhr.status);
                alert('Request failed.  Returned status of ' + xhr.status);
                callBack(null);
            } else if (xhr.status === 401 || xhr.status === 403) {
                console.error('Request failed.  Returned status of ' + xhr.status);
                callBack(null);
               // document.location.href = identityUrl + "/Account/Login";
            }
            else {
                console.error('Request failed.  Returned status of ' + xhr.status);
                callBack(null);
            }

        };

        if (authToken !== null) {
            xhr.setRequestHeader("Authorization", "Bearer " + authToken);
        }

        xhr.send();
    }

}

export default HttpUtil;