$(document).ready(function () {

var file;

document.getElementById('upload-photo').addEventListener('change', function(){
    file = this.files[0];
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
            $('#pick-image-container').hide();
            if(xhr.responseText === "false"){
              $('#sad-image-container').show();
              $('#continue').show();
            }else{
              $('#happy-image-container').show();
              $('#continue').show();
            }
        }
    };
    fd.append("", file);
    xhr.send(fd);

}

$('#continue').click(function(){
  $('#pick-image-container').show();
  $('.emotions').hide();
});


});
