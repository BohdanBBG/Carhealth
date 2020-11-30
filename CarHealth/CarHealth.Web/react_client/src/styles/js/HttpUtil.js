
class HttpUtil {

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

    httpGet(url, authToken = null) { //+

        return new Promise(function (resolve, reject) {
            var xhr = new XMLHttpRequest();
            //   xhr.withCredentials = true; // force to show browser's default auth dialog
            xhr.open('GET', url);

            xhr.onload = function () {
                if (xhr.status === 200) {
                    if (xhr.responseText !== "") {
                        var data = JSON.parse(xhr.responseText);

                        console.log("----HttpUtil.httpGet:", data);

                        resolve(data);
                    }

                    resolve(data);

                } else if (xhr.status === 500) {
                    console.error('Request failed.  Returned status of ' + xhr.status);
                    alert('Request failed.  Returned status of ' + xhr.status);

                    var error = new Error(this.statusText);
                    error.code = this.status;
                    
                    reject(error);

                } else if (xhr.status === 401 || xhr.status === 403) {
                    console.error('Request failed.  Returned status of ' + xhr.status);

                    var error = new Error(this.statusText);
                    error.code = this.status;

                    reject(error);

                }
                else {
                    console.error('Request failed.  Returned status of ' + xhr.status);

                    var error = new Error(this.statusText);
                    error.code = this.status;

                    reject(error);
                }
            };

            xhr.onerror = function() {
                reject(new Error("Network Error"));
              };

            if (authToken !== null) {
                xhr.setRequestHeader("Authorization", "Bearer " + authToken);
            }

            xhr.send();
        });
    }
}

export default HttpUtil;