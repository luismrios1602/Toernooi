using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Toernooi.Areas.Gestion.Controllers
{
    public class ActualizadorController : Controller
    {
        //Controlador que va a hacer todo lo que tenga que ver con la actualización de los datos de un torneo.

        readonly Context_BD _coBDToernooi = new Context_BD();

        // GET: Gestion/Actualizador
        public ActionResult Index()
        {
            //Declaro un objeto de tipo usuario para almacenar los datos de usuario que estén almacenados en la Session Usuario que creo cuando me logueo y así traerme los datos de Login pal controlador de Torneos. Trucazo, no?
            Usuarios usuario;
            //Declaro una variable entera que voy usar para almacenar el id del usuario que acabo de declarar para mandarselo como modelo a la vista de mis torneos
            int idUsuario;

            //Si el session de Usuario no tiene nada es porque no hay ningún usuario logueado, entonces vete pal login so' carón
            if (Session["Usuario"] == null)
            {
                return Redirect(Url.Action("Index", "Login", new { Area = "AccesoYRegistro" }));
            }
            else
            {
                //Pero sí si hay algo, entonces ese usuario que está ahí lo almaceno en el objeto recién creado para extraer más fácil los datos
                usuario = (Usuarios)Session["Usuario"];
                idUsuario = usuario.idUsuario;
                //idUsuario = int.Parse(TempData["idUsuario"].ToString());  //Lo había hecho con un TempData solamente pero el TD vale mondá :v 
            }

            return View(Session["idTorneo"]);
        }

        //Método para actualizar los datos del torneo
        [HttpPost]
        public ActionResult updateTorneo(Torneos torneo)
        {
            if (ModelState.IsValid)
            {
                //Hago una consulta a la base de datos del torneo específico que estoy buscando
                var torneoBD = _coBDToernooi.Torneos.Where(x => x.idTorneo == torneo.idTorneo).ToList();
                //Cuando ya lo tenga, hago una condicion pa saber que ese id está correcto. 
                if (torneoBD.Count > 0)
                {
                    //Si hay al menos 1 dato (que debería haber solo 1 :v) actualizdo todos los elementos de ese dato con los que me llegan.
                    torneoBD[0].idTorneo = torneo.idTorneo;
                    torneoBD[0].tipoEnteOrganizador_idTipoEnte = torneo.tipoEnteOrganizador_idTipoEnte;
                    torneoBD[0].nomEnteOrgan = torneo.nomEnteOrgan;
                    torneoBD[0].docResponsable = torneo.docResponsable;
                    torneoBD[0].nomResponsable = torneo.nomResponsable;
                    torneoBD[0].nomTorneo = torneo.nomTorneo;
                    torneoBD[0].tiposTorneos_idTipoTorneo = torneo.tiposTorneos_idTipoTorneo;
                    torneoBD[0].fechaInicio = torneo.fechaInicio;
                    torneoBD[0].fechaFin = torneo.fechaFin;
                    torneoBD[0].cantEquipos = torneo.cantEquipos;
                    torneoBD[0].idaYvuelta = torneo.idaYvuelta;
                    _coBDToernooi.SaveChanges();

                    return Redirect(Url.Action("Index", "Actualizador", torneo.idTorneo) + "?Action=updated");
                } 
                
            }
            else
            {
                return Redirect(Url.Action("Index", "Actualizador", torneo.idTorneo) + "?Action=error");
            }

            return View();
        }

        //Método para cargar la vista parcial de los equipos
        public ActionResult cargarVP_Equipos()
        {
            int idTorneo = int.Parse(Session["IdTorneo"].ToString());
            //Hago una lista con todos los equipos inscritos mientras que el idTorneo sea el torneo actual y la mando como modelo de la vista parcial, para que la vista parcial haga la magia.
            List<InscripcionEquipos> equiposInscritos = _coBDToernooi.InscripcionEquipos.Where(x=>x.Torneos_idTorneo == idTorneo).ToList();

            //Retorna la vista parcial con la tabla de equipos, que como es parcial el JS asignado hace la magia
            return View("_tablaEquiposAct", equiposInscritos);
        }

        //Método para cargar la vista parcial de jugadores
        public ActionResult cargarVP_Jugadores()
        {
            int idTorneo = int.Parse(Session["idTorneo"].ToString());
            //Con el Where, voy a buscar solo las inscripciones al torneo en el que estoy actualmente
            List<InscripcionEquipos> listaInsEqui = 
                _coBDToernooi.InscripcionEquipos.Where(x => x.Torneos_idTorneo == idTorneo).ToList();

            //Le mando la lista de jugadores porque de esta lista me voy a basar para ver la lista de jugadores registrados por equipo
            return View("_tablaJugadoresAct", listaInsEqui);
        }

        public ActionResult updateEquipo(Equipos equipo)
        {
            int idTorneo = int.Parse(Session["idTorneo"].ToString());

            if (ModelState.IsValid)
            {
                //Hago una consulta a la base de dato del equipo específico que estoy buscando
                var equipoBD = _coBDToernooi.Equipos.Where(x => x.idEquipo == equipo.idEquipo).ToList();
                //Cuando ya lo tenga, hago una condicion pa saber que ese id está correcto. 
                if (equipoBD.Count > 0)
                {
                    //Si hay al menos 1 dato (que debería haber solo 1 :v) actualizdo todos los elementos de ese dato con los que me llegan.
                    equipoBD[0].idEquipo = equipo.idEquipo;
                    equipoBD[0].gradoOcarrera = equipo.gradoOcarrera;
                    equipoBD[0].nomResponsable = equipo.nomResponsable;
                   
                    _coBDToernooi.SaveChanges();

                    return Redirect(Url.Action("Index", "Actualizador", idTorneo) + "?Action=updated");
                } 
                
            }
            else
            {
                return Redirect(Url.Action("Index", "Actualizador", idTorneo) + "?Action=error");
            }

            return View("Index");
        }
    }
}