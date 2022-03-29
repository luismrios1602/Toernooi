//Este archivo es para definir el comportamiento del formulario de registro, con las claves y que todos los datos estén llenos.

//Método para validar que todos los campos están llenos. Método propio de Bootstrap
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
                } else {

                    valContra()
                }

                form.classList.add('was-validated')
            }, false)
        })

    registo()
})()


//Con este método reviso que la contraseña tenga minimo 8 caracteres y que la validación de contraseña sea igual. Si no cumple, que en el control me diga el error.
function valContra() {
    const pass = document.getElementById('password')
    const valpass = document.getElementById('validarPass')

    var coincidencia = pass.value == valpass.value

    const div_valContra = document.getElementById("div-valContra")
    const CorreccionContra = document.getElementById("CorreccionContra")
    const div_Contra = document.getElementById("div-Contra")
    const p_Contra = document.getElementById("p-Contra")

    if (pass.value.length < 8) {
        event.preventDefault()
        event.stopPropagation()
        div_Contra.removeAttribute("style")
        p_Contra.textContent = "La contraseña debe tener mínimo 8 caracteres"
        p_Contra.style.color = "red"

    } else if (!coincidencia) {
        div_Contra.style.display = "none"
        event.preventDefault()
        event.stopPropagation()

        div_valContra.removeAttribute("style")

        CorreccionContra.textContent = "Las contraseñas no coinciden"
        CorreccionContra.style.color = "red"
    } else { div_valContra.style.display = "none" }

}


/*Con este método, llamo al metodo getParameterByName que está ubicado en Content/Common para que defina el argumento de la página.
Este parametro (sucess o error) se lo paso en el controlador si la clave o correo están correctos o no.*/

function registo() {
    var state = getParameterByName('Action')
    if (state == 'success') {
        Swal.fire(
            'Registro éxitoso',
            'Usuario registrado éxitosamente',
            'success'
        )
    } else if (state == 'error') {
        Swal.fire(
            'Error al registrar el usuario',
            'Por favor verifique los datos suministrados.',
            'error'
        )
    } else if (state == "") {
        //Si no tiene argumento mando una bienvenida.
        Swal.fire(
            'REGISTRARSE',
            '¡Bienvenido! suministra los datos solicitados para realizar el registro en sistema.',
            'warning'
        )
    }

}