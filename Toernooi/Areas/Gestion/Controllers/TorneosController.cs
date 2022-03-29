using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace Toernooi.Areas.Gestion.Controllers
{
    public class TorneosController : Controller
    {
        //Objeto de conexion a la base de datos creado al conectar con ADO.
        readonly Context_BD _coBDToernooi = new Context_BD();


        //Lista de Inscripcion de equipos que declaro global para poderla usar desde varios métodos. Esta lista la hago para llenarla con cada equipo que inscriba en el torneo actual y así no loquear pa buscarlos
        List<InscripcionEquipos> listaEquipos = new List<InscripcionEquipos>();
        
        // GET: Gestion/Torneos

        //Método principal del Controlador que va a abrir la página de torneos si hay alguien logueado, sino pailas, lo mando al login
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

            //Si hay datos de usuario entonces retorno la vista "Mis torneos" y le mando como modelo de esa vista el ID de usuario, el cual usaré para ver los torneos asociados
            return View("MisTorneos", idUsuario);
        }

        /*Método para abrir la vista de registrar un torneo. 
         * Esto solo valida que haya un usuario logueado al querer entrar a esta vista y si sí lo hay, mando el ID de ese usuario porque lo voy a necesitar al registrar el torneo*/
        public ActionResult RegistrarTorneo()
        {
            int idUsuario;
            Usuarios usuario;

            //Hago la misma validación de ahorita
            if (Session["Usuario"] == null)
            {
                return Redirect(Url.Action("Index", "Login", new { Area = "AccesoYRegistro" }));
            }
            else {
                usuario = (Usuarios) Session["Usuario"];
                idUsuario = usuario.idUsuario;
                //idUsuario = int.Parse(TempData["idUsuario"].ToString()); 
            }
           
            return View(idUsuario);
        }

        /*Metodo que realiza el registro de un torneo, cuando hace la magia del registro me manda a la vista de Inscripcion de equipos
        * Este método se llama desde el fomulario de registro */
        [HttpPost]
        public ActionResult insertTorneo(Torneos torneo)
        {
            if (ModelState.IsValid)
            {
                //Registramos el torneo
                torneo.nomResponsable = torneo.nomResponsable.ToUpper();
                torneo.nomTorneo = torneo.nomTorneo.ToUpper();
                torneo.fechaRegistro = DateTime.Now; //Tengo que mandar la fecha de registro justo antes de mandar el archivo porque el modelo la está mandando null
                _coBDToernooi.Torneos.Add(torneo);
                _coBDToernooi.SaveChanges();

                //Este ciclo lo creamos para que busque todos los torneos y que cuando encuentre un torneo con el ID del Usuario que está logueado, guarde el ID del torneo hasta llegar al último
                
                foreach (var item in _coBDToernooi.Torneos.ToList())
                {
                    if (item.Usuarios_idUsuario==torneo.Usuarios_idUsuario)
                    {
                        //Creamos un Session para guardar el ID del torneo creado y poder usarlo durante toda la sesion
                        Session["idTorneo"] = item.idTorneo;
                        //Creamos un Session para guardar la cantidad de equipos registrados porque lo voy a necesitar cuando esté registrando los equipos
                        Session["cantEquipos"] = item.cantEquipos;
                        //Creamos una Session con los equipos inscritos que va a ser de 0 para poder usarla desde aquí. Si la declaro en la vista de registro de equipos se me reinicia en 0 siempre que meta 1
                        Session["cantEquiposInscritos"] = 0;
                        //Creo una session para almacenar la lista de equipos que voy a ir agregando y poderla usar cuando termine.
                        Session["lisEquiposIns"] = null;
                    }
                }
                //Si todos los datos fueron correctos, entonces que me diga "compa usted cargó bien la wea, vaya a la página de ingresar equipos"
                return Redirect(Url.Action("InscribirEquipos","Torneos")+"?Action=success");
            }
            else
            {
                //Pero si hubo una cagada, que me recargue esta misma página con error pa que el JS haga la magía
                var modelErrors = ModelState.Select(x => x.Value.Errors)
                             .Where(y => y.Count > 0)
                             .ToList();
                return Redirect(Url.Action("RegistrarTorneo","Torneos")+"?Action=error");
            }
            
        }

        //Método para abrir la vista de inscripcion de equipos
        public ActionResult InscribirEquipos()
        {
            //Creo una variable para almacenar el ID del torneo creado y mandarlo como modelo a la vista para mandar ese dato en el formulario de registro de equipos
            int idTorneo;
            
            //La misma validación de que si no hay un usuario logueado lo mande al login, esto es más viejo que las mujeres mintiendo
            if (Session["Usuario"] == null)
            {
                return Redirect(Url.Action("Index", "Login", new { Area = "AccesoYRegistro" }));
            }
            else
            {
                //Si hay un usuario logueado, entonces Uso el Session creado desde el insertTorneo
                idTorneo = int.Parse(Session["idTorneo"].ToString());

                //Creo el Id de Equipo y Numero de equipo aquí para darle valores cuando vaya a añadir jugadores
                Session["numEquipo"] = 0;
                Session["idEquipo"] = Session["numEquipo"].ToString();
            }
            
            //Mando el ID del torneo como modelo de la vista
            return View(idTorneo);
        }

        /*Método para insertar un equipo. Este lo que hace es devolver la misma vista de inscripcion de equipos con el id de torneo
        * Este método se llama desde el formulario*/
        [HttpPost]
        public ActionResult insertEquipos(Equipos equipo)
        {
            //Creo dos variables para almacenar el ID del torneo que está en la session por ahí flotando, y el ID de equipo que voy a hacer una marañita con ese id más adelante
            int idEquipo=0;
            int idTorneo = int.Parse(Session["idTorneo"].ToString());

            //Si los datos enviados por el formulario están correctos que me haga la magia
            if (ModelState.IsValid)
            {
                //Pongo todos los datos en mayúcula
                equipo.nomEquipo = equipo.nomEquipo.ToUpper();
                equipo.gradoOcarrera = equipo.gradoOcarrera.ToUpper();
                equipo.nomResponsable = equipo.nomResponsable.ToUpper();

                /*Creo una lista para almacenar todos los equipos.
                *Y tú te preguntarás ¿Pa qué la otra lista? Pues con esta lista cojo todos los equipos registrados para saber el último ID, con la otra es para almacenar los equipos que voy creando aquí*/
               List < Equipos> listEquipos = _coBDToernooi.Equipos.ToList();
                foreach (var item in listEquipos)
                {
                    /*Le mando el ID del equipo basándome en el último equipo registrado y sumando 1 al ID.*/
                    idEquipo = int.Parse(item.idEquipo.ToString());
                    idEquipo++;
                }
                //Cuando tenga ese ID lo agrego al objeto que voy a registrar y BOOORAAAAAAA! :v
                equipo.idEquipo = idEquipo;
                _coBDToernooi.Equipos.Add(equipo);
                _coBDToernooi.SaveChanges();

                //Cada vez que añada un equipo, creo una variable local que va a guardar la cantidad de equipos que lleve registrados hasta ese momento
                int cantEquInsc = int.Parse(Session["cantEquiposInscritos"].ToString());
                //Sumo 1 a esa cantidad de equipos
                cantEquInsc++;
                //Le mando este nuevo valor a la Session de equipos Inscritos, el cual voy a usar desde la vista. Está declarado cuando creo un torneo junto con la cantidad de equipos.
                Session["cantEquiposInscritos"] = cantEquInsc;

                //Llamamos a la función de inscribir equipos y le mandamos el ID del torneo que tenemos activo 
                insertInscripcionEquipoTorneo(idEquipo, idTorneo);

                //LLamo a la funcion que va guardando los equipos registrados en una lista y lo mando a la Session de equipos inscritos para mandar la lista llena cuando vaya a inscribir jugadores.
                Session ["lisEquiposIns"] = listarEquiposInscritos(idEquipo);

                //LLamo a la funcion que va a cargar la vista parcial
                cargarVP_Equipos();
            }

            //Retorno la misma vista de Inscripcion de equipo sin redirección para que no se borre nada
            return View("InscribirEquipos",idTorneo);
        }

        //Método para registrar una inscripcion automáticamente se registre un equipo
        public void insertInscripcionEquipoTorneo(int idEquipo, int idTorneo)
        {
            InscripcionEquipos inscripcion = new InscripcionEquipos
            {
                fechaInscripcion = DateTime.Now,
                Equipos_idEquipo = idEquipo,
                Torneos_idTorneo = idTorneo
            };
            _coBDToernooi.InscripcionEquipos.Add(inscripcion);
            _coBDToernooi.SaveChanges();
        
        }

        //Método para cargar la vista parcial con la tabla de Equipos registrados hasta el momento
        public ActionResult cargarVP_Equipos()
        {

            //Hago una lista con todos los equipos inscritos hasta el momento y la mando como modelo de la vista parcial, para que la vista parcial haga la magia.
            List<InscripcionEquipos> equiposInscritos = _coBDToernooi.InscripcionEquipos.ToList();

            //Retorna la vista parcial con la tabla de equipos, que como es parcial el JS asignado hace la magia
            return View("_tablaEquipos",equiposInscritos);
        }

        //Método para registrar un técnico a uno de los equipos registrados
        [HttpPost]
        public ActionResult insertCuerpoTecnico(Tecnicos tecnico)
        {
            //Creo una variable que almacene el ID del torneo para mandarselo como modelo a la vista de inscripcion de equipos, la cual se recarga cada vez que haga un cambio.
            int idTorneo = int.Parse(Session["idTorneo"].ToString());
            if (ModelState.IsValid)
            {
                tecnico.nomTecnico = tecnico.nomTecnico.ToUpper();
                _coBDToernooi.Tecnicos.Add(tecnico);
                _coBDToernooi.SaveChanges();
            }

            return View("InscribirEquipos", idTorneo);
        }

        /*Método para guardar los equipos que voy creando en una lista que después le voy a mandar a la session que usaré en la inscripcion de jugadores.
        * Este método es llamada desde la inscripcion de los equipos*/
        public List<InscripcionEquipos> listarEquiposInscritos(int idEquipo)
        {

            foreach(var equipo in _coBDToernooi.InscripcionEquipos)
            {
                if (equipo.Torneos_idTorneo.ToString().Equals(Session["idTorneo"].ToString()))
                {
                    listaEquipos.Add(equipo);
                }       

            }
            return listaEquipos;
        }

        //Método para la mostrar la vista de inscripción de jugadores
        public ActionResult InscribirJugadores()
        {

            //Como la lista equipos ahora está vacía, pero no el Session, voy a mandarle ese Session a la lista para que se llene de nuevo, trucazo. 
            listaEquipos = (List<InscripcionEquipos>) Session["lisEquiposIns"];
            //Le mando a la cantidad de equipos y la cantidad de equipos inscritos
            Session["cantEquipos"] = listaEquipos.Count.ToString();
            Session["cantEquiposIns"] = 1;
            Session["NombreEquipo"] = "";

            
            ////Mando como retorno la lista de equipos registrados en esta sesion
            return View(listaEquipos);
        }

        //Método para cargar la vista parcial de jugadores
        public ActionResult cargarVP_Jugadores()
        {
            List<Jugadores> listaJugadores = _coBDToernooi.Jugadores.ToList();
            //Le mando la lista de jugadores porque de esta lista me voy a basar para ver la lista de jugadores registrados por equipo
            return View("_tablaJugadores", listaJugadores);
        }

        //Método para ingresar jugadores.
        [HttpPost]
        public ActionResult insertJugadores(Jugadores jugador) {

            if (ModelState.IsValid)
            {
                jugador.nomJugador = jugador.nomJugador.ToUpper();
                jugador.apeJugador = jugador.apeJugador.ToUpper();
                _coBDToernooi.Jugadores.Add(jugador);
                _coBDToernooi.SaveChanges();
                Session["idEquipo"] = jugador.Equipos_idEquipo;
                
            }

            return View("InscribirJugadores", listaEquipos);
        }

        //Método para cambiar la posición del equipo en la lista de equipos
        [HttpGet]
        public ActionResult siguienteEquipo()
        {
            /*Cuando uso este método, yo llamo es a la vista de Inscripcion de equipos, no vuelvo a la Action Result InscribirEquipos, así que tengo que llenar la lista que voy a mandar como módelo desde aquí*/
            listaEquipos = (List<InscripcionEquipos>)Session["lisEquiposIns"];
            int numEquipo = int.Parse(Session["numEquipo"].ToString());
            int cantEquiosIns = int.Parse(Session["cantEquiposIns"].ToString());
            numEquipo++;
            cantEquiosIns++;
            Session["cantEquiposIns"] = cantEquiosIns;
            Session["numEquipo"] = numEquipo;

            return View("InscribirJugadores", listaEquipos);
        
        }

        //Creo un método que me lleve desde la vista de torneos hasta la vista de editar y que le mande el ID del torneo.
        [HttpGet]
        public ActionResult Actualizar(int IdTorneo)
        {
            if (ModelState.IsValid)
            {
                Session["idTorneo"] = IdTorneo;
                return Redirect(Url.Action("Index", "Actualizador"));
            }

            return Redirect(Url.Action("Index", "Torneos") + "?Action=error");
        }

        
    }
}