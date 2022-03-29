//Archivo JS con las funciones que van a revisar si se pudieron cargas las vistas parciales

'use strict'
$(document).ready(function () {
    cargarVP_equipos();
    cargarVP_jugadores();
});

//La vista parcial de la tabla de equipos en la inscripcion
function cargarVP_equipos() {
    //En la página de inscribir equipos tengo un DIV con el id div_TablaEquipos y ahí voy a mostrar la vista que retorna el método cargarVP_Equipos en el controlador Torneos
    $("#div_TablaEquipos").load("/Gestion/Torneos/cargarVP_Equipos", function (responseTxt, statusTxt, xhr) {
        if (statusTxt==="success") {
            toastr["success"]("Listado de equipos inscritos actualizada exitosamente.", "Equipos Inscritos")
        } else if (statusTxt === "error") {
            toastr["error"]("No se han encontrado Equipos", "Equipos Inscritos")
        }
    })
}

//La vista parcial con la tabla de jugadores
function cargarVP_jugadores() {
    //En la página de inscribir jugadores tengo un DIV con el id divTablaJugadores y ahí voy a mostrar la vista que retorna el método cargarVP_Jugadores en el controlador Torneos
    $("#divTablaJugadores").load("/Gestion/Torneos/cargarVP_Jugadores", function (responseTxt, statusTxt, xhr) {
        if (statusTxt === "success") {
            toastr["success"]("Listado de jugadores inscritos actualizada exitosamente.", "Jugadores Inscritos")
        } else if (statusTxt === "error") {
            toastr["error"]("No se han encontrado jugadores.", "Jugadores")
        }
    })
}