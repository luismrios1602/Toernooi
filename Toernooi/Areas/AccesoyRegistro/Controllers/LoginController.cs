using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Toernooi.Areas.AccesoyRegistro.Controllers
{
    public class LoginController : Controller
    {
        // GET: AccesoyRegistro/Login
        //Objeto de conexion con la base de datos. Este objeto se crea al crear la conexion de ADO.
        readonly Context_BD _coBDToernooi = new Context_BD();

        //Action de la página principal del controlador Login
        public ActionResult Index()
        {
            
            return View("Login");
        }

        //Método para validar los datos de Login enviados por el formulario
        [HttpGet]
        public ActionResult Loguearse(Usuarios usuarioLogin)
        {
            //Variable para definir si se logueó o no... Dudo que haga algo :v
            bool login = false;

            //Lista de usuarios extraídos de la base de datos
            List<Usuarios> L_usuarios = _coBDToernooi.Usuarios.ToList();

            //Recorremos esta lista
            foreach (var usuario in L_usuarios)
            {
                //Esta condicion es para cuando recorriendo a la lista de usuarios, si el usuario actual corresponde a un correo de la base de datos que entre a validar la contraseña.
                if (usuario.correo == usuarioLogin.correo)
                {
                    //Si encuentra ese correo en base de datos, revisamos si la contraseña de ese registro es la misma que la ingresada en sistema.
                    if (usuario.password == usuarioLogin.password)
                    {
                        login = true;
                        //TempData["idUsuario"] = usuario.idUsuario; --> Usé el tempdata pero es mejor Session porque el TempData se pierde con la recarga de la página.

                        Session["Usuario"] = usuario; //La Session va a almacenar el objeto completo para utilizar sus datos desde la vista.
                        Session["Nombre Usuario Logueado"] = usuario.nomUsuario + " " + usuario.apeUsuario; //Creo una session para mandar los datos a la vista principal que está en otro controlador.
                        break;
                    }

                } 
            }

            // Ah sizas sí uso la varible booleana :v Si es verdad la mando al Index de torneos que es la página de "Mis torneos" pero sino, que mande el parametro error para que el JS valLogin haga la magia
            if (login)
            return Redirect(@Url.Action("Index", "Torneos", new { Area = "Gestion" }));
            else return Redirect(Url.Action("Index", "Login")+"?Action=error");
        }
    }
}