function loadFile() {
    var file = new FormData($('#frmSubirArchivo')[0]);
    //$("#frmSubirVideo").attr("action", "/Musico/UploadMultimedia?TipoMultimediaId=1&Id="
    //    + $('#idGrupo').val() + "&Nombre=''&NombreGpo=" + $("#NombreGrupo").val());

    $.ajax({
        url: '../../SafeAuto/LoadFile',
        type: 'POST',
        contentType: false,
        processData: false,
        async: true,
        //dataType: "json",
        //data: "file" + file,
        data: file,
        success: function (d) {
            console.log(d);
            if (d === 'OK') {
                GetEstimation();
            }
            else {
                alert(d);
            }
        },
        error: function () {
            alert('Error al cargar el archivo');
        }
    });
}

function GetEstimation(){
    $.ajax({
        url: '../../SafeAuto/GetEstimation',
        type: 'POST',
        async: true,
        dataType: "json",
        success: function (d) {
            console.log('Hola');
            console.log(d);
            var result = $('#result');
            var cadena;

            result.empty();
            result.append("<div class='col-md-4'>Name</div>");
            result.append("<div class='col-md-4'>Distance</div>");
            result.append("<div class='col-md-4'>Velocity</div>");

            for (i = 0; i < d.length; i++){
                cadena = "<div class='col-md-4'>"+ d[i].driver.name +"</div>";
                cadena = cadena + "<div class='col-md-4'>" + d[i].milles + "</div>";
                cadena = cadena + "<div class='col-md-4'>" + d[i].velocity +"</div>";
                result.append(cadena);
            }
            alert('El archivo fue cargado con éxito');
        },
        error: function () {
        }
    });
}