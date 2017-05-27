$(document).ready(function () {

var file;

document.getElementById('upload-photo').addEventListener('change', function(){
    file = this.files[0];
    console.log("name : " + file.name);
    console.log("size : " + file.size);
    console.log("type : " + file.type);
    console.log(file);
    uploadFile(file);
}, false);

function uploadFile(file){
    var url = 'http://riemu-api.azurewebsites.net/api/happy';
    var xhr = new XMLHttpRequest();
    var fd = new FormData();
    xhr.open("POST", url, true);
    xhr.onreadystatechange = function() {
        if (xhr.readyState == 4 && xhr.status == 200) {
            // Every thing ok, file uploaded
            console.log(xhr.responseText); // handle response.
        }
    };
    fd.append("upload_file", file);
    xhr.send(fd);
}

});
