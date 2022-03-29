//Este archivo es para definir el comportamiento cuando la clave esté mal.

//Método para validar que todos los campos están llenos. Método propio de Bootstrap
(function validarLogin() {
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
                } else {

                    valContra()
                }

                form.classList.add('was-validated')
            }, false)
        })

    mostrarAlert()
})()

/*Con este método, llamo al metodo getParameterByName que está ubicado en Content/Common para que defina el argumento de la página. 
Este parametro (sucess o error) se lo paso en el controlador si la clava o correo están correctos o no.*/
function mostrarAlert() {
    //El método me devuelve el parámetro de la URL.
    var state = getParameterByName('Action')
    if (state == 'success') {
        Swal.fire(
            '¡Bienvenido!',
            'Usuario logueado éxitosamente',
            'success'
        )
    } else if (state == 'error') {
        Swal.fire(
            'Error al Iniciar sesión',
            'El correo y/o la contraseña son incorrectos.',
            'error'
        )
    }

}