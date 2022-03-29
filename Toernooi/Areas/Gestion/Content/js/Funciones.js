//Aqui tenemos las funciones de que voy a usar en todo el controlador.

//Funcion para validar que el control de partidos de ida y vuelta esté checkeado y mande en el formulario el valor true.
function chequear() {
    var check_idaYvuelta = document.getElementById('idaYvuelta')
    var isChecked = check_idaYvuelta.checked;
    if (isChecked) {
        check_idaYvuelta.value = "true"
        //alert(check_idaYvuelta.value)
    } else {
        //check_idaYvuelta.value = "false"
    }
}

//No sé por qué lo puse aquí también si ya está en common pero bueno mejor no lo quito :v
function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    window.history.pushState(null, "", window.location.pathname);//Elimino los parametros
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

//Configuracion de toastr
toastr.options = {
    "closeButton": false,
    "debug": false,
    "newestOnTop": false,
    "progressBar": true,
    "positionClass": "toast-top-right",
    "preventDuplicates": false,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
}

//Funcion para mostar un "alert" cuando le dé clic en el botón de finalizar.
function finalizarRegistroTorneo() {
    Swal.fire(
        'Torneo registrado.',
        'A continuación, ingrese los datos de los equipos inscritos.',
        'sucess').then();
}

function valorInputEquipo(){

    return document.getElementById("idEquipoBus").value
}