//Aqui coloco la validacion del formulario de registro de torneos

//Método para validar que el formulario esté lleno en sus datos requeridos
(function validarRegistro() {
    'use strict'

    // Fetch all the forms we want to apply custom Bootstrap validation styles to
    var forms = document.querySelectorAll('.needs-validation')

    // Loop over them and prevent submission
    Array.prototype.slice.call(forms)
        .forEach(function (form) {
            form.addEventListener('submit', function (event) {
                if (!form.checkValidity()) {
                    event.preventDefault()
                    event.stopPropagation()
                }

                form.classList.add('was-validated')
            }, false)
        })

    //Llamo a la funcion de registro pa saber si al registrar el torneo está todo correcto. Esto se va a activar cada vez que recargue la página
    registoTorneo()
})()


//Método para revisar si al recargar la página cuando mande a registrar, el parametro es success o error. Este getParameter está en el archivo Funciones.
function registoTorneo() {
    var state = getParameterByName('Action')
    if (state == 'error') {
        Swal.fire(
            'Error al registrar el torneo',
            'Por favor verifique los datos suministrados.',
            'error'
        )
    } else if (state == 'success') {
        Swal.fire(
            'Torneo registrado.',
            'A continuación, ingrese los datos de los equipos inscritos.',
            'success'
        ) 
    } else if (state == 'updated') {
        Swal.fire(
            'Torneo actualizado.',
            'Los datos fueron actualizados éxitosamente.',
            'success'
        )
        
    }

}