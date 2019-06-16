function Post(url, data, callback) {

    let xhr = new XMLHttpRequest();

    xhr.open('POST', url, true);

    xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");

    xhr.onreadystatechange = function () {
        if (this.readyState === 4 && this.status === 200) {
            callback(this.responseText);
        }
    }

    let keyList = Object.keys(data);

    let tmp_data = "";

    for (let i = 0; i < keyList.length; i++) {
        let key = keyList[i];
        tmp_data += `${key}=${data[key]}&`;
    }

    tmp_data = tmp_data.substring(0, tmp_data.length - 1);
    // console.log(tmp_data);
    xhr.send(tmp_data);
}

function Get(url, callback) {

    let xhr = new XMLHttpRequest();

    xhr.open('GET', url, true);

    xhr.onreadystatechange = function () {
        if (this.readyState === 4 && this.status === 200) {
            callback(this.responseText);
        }
    }

    xhr.send();
}
