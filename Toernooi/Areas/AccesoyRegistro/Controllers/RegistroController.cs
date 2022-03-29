using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Toernooi.Areas.AccesoyRegistro.Controllers
{
    public class RegistroController : Controller
    {
        //Objeto que se conecta con la base de datos creado al usar el ADO.
        readonly Context_BD _coBDToernooi = new Context_BD();

        // GET: AccesoyRegistro/Registro

        //ActionResult que me va a mandar a la página Registro cuando inicie el controlador Registro
        public ActionResult Index()
        {
            return View("Registro");
        }

        //Método para registrar un usuario
        [HttpPost]
        public ActionResult RegistrarUsuario(Usuarios usuario)
        {
            //Colocamos la condición para validar que los datos enviados por el formulario corresponden con un objeto de tipo Usuario
            if (ModelState.IsValid)
            {
                //Creo un id de usuario para almacenar el ID del usuario creado y lo mando para asignar roles
                int idUsuario = 0;

                //Si los datos están correctos, colocamos el nombre el mayúscula, mandamos el objeto y luego guardamos los datos que significa ir a la BD a guardar
                usuario.nomUsuario = usuario.nomUsuario.ToUpper();
                usuario.apeUsuario = usuario.apeUsuario.ToUpper();
                _coBDToernooi.Usuarios.Add(usuario);
                _coBDToernooi.SaveChanges();

                //Para asignar el rol de usuario reviso todos los usuarios y a último creado le asigno el rol
                foreach (var obj_usuario in _coBDToernooi.Usuarios)
                {
                    idUsuario = obj_usuario.idUsuario;
                }

                //Mando a llamar el método para asignarle el rol de usuario. 
                asignarRoles(idUsuario);

                //Si todo está correcto vamos a la misma página de registro pero con el parametro Action sucess para que el JS de validacion haga su magia
                return Redirect(Url.Action("Index", "Registro") + "?Action=success");
            }
            else {
                //Sino, entonces avisamos que hay un error yendo a esta misma pagina pero con el parametro error para avisar que algo la cagó y avise el JS
                return Redirect(Url.Action("Index", "Registro") + "?Action=error");
            }
        }

        public void asignarRoles(int idUsuario) {
            //Creo un objeto de asignacion de roles para los datos que voy a enviar. 
            AsignacionRoles ar = new AsignacionRoles();

            //Envío el id del usuario creado y el id de rol 3 que es un usuario normalito
            ar.fechaAsignacion = DateTime.Now;
            ar.Roles_idRol = 3;
            ar.Usuarios_idUsuario = idUsuario;

            //Mando esa asignacion a la tabla de la BD
            _coBDToernooi.AsignacionRoles.Add(ar);
            _coBDToernooi.SaveChanges();
        }
    }
}