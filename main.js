$(document).ready(function() {
     
    var $uploadCrop = $('#my-img').croppie({
    enableOrientation: true,
    viewport: {
        width: 200,
        height: 200,
        type: 'circle'
    },
    boundary: {
        height: 300
    }
});

$('#file-uplad').on('change', function() { uploadImg(this); });

function uploadImg(input) {
  if (input.files && input.files[0]) {
    
    var reader = new FileReader();
    
    reader.onload = function(e) {
      $('#my-img').addClass('ready');
      $uploadCrop.croppie('bind', {
        url: e.target.result
      });
    };
    
    reader.readAsDataURL(input.files[0]);
    
  } else {
    console.log("Sorry - you're browser doesn't support the FileReader API");
  };
};

$('#cropImageBtn').on('click', function (ev) {
    $uploadCrop.croppie('get');
  $uploadCrop.croppie('result', {
    type: 'canvas',
    size: 'viewport',
    format: 'jpeg',
    circle: false
  }).then(function (download) {
    $('#item-img-output').attr('src', download);
  });
});

$('#rotate-left').on('click', function() {
  $uploadCrop.croppie('rotate', 90);
});
$('#rotate-right').on('click', function() {
  $uploadCrop.croppie('rotate', -90);
});
   
 });  








	