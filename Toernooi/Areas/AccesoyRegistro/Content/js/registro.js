
//Este codigo ya no va porque tuve que separarlo en validación para login y para el login

/*const { Swal } = require("../../../../Content/lib/sweetalert2/sweetalert2")*/

    // Example starter JavaScript for disabling form submissions if there are invalid fields
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


