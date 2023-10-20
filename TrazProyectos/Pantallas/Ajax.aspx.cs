using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;

namespace TrazProyectosDiverscan.Pantallas
{
    public partial class Ajax : System.Web.UI.Page
    {
        #region VARIABLES GLOBALES

        #endregion


        #region EVENTOS

        protected void Page_Load(object sender, EventArgs e)
        {
            var opcion = Request.Form["opcion"];

            switch (opcion)
            {
                case "ObtenerComponentes":

                    ObtenerComponentes();
                    break;

                case "ObtenerRespaldos":

                    ObtenerRespaldo();
                    break;

                case "AgregarRespaldos":

                    string resp;

                    try
                    {
                        AgregarRespaldo(Convert.ToInt32(Request.Form["Sist"]), Convert.ToInt32(Request.Form["CompSistema"]), Convert.ToInt32(Request.Form["NombreDev"]), Convert.ToInt32(Request.Form["NombreCert"]), Convert.ToDateTime(Request.Form["FechaMod"]), Convert.ToBoolean(Request.Form["Git"]), Convert.ToBoolean(Request.Form["Nube"]), Convert.ToBoolean(Request.Form["Fisico"]), Convert.ToBoolean(Request.Form["DB"]), Request.Form["Version"].ToString(), Request.Form["Comentario"].ToString());
                        resp = "Respaldo registrado exitosamente!";
                    }
                    catch (Exception)
                    {
                        resp = "Ha fallado el registro del respaldo, ";
                    }

                    Response.Clear();
                    Response.Write(resp);
                    Response.End();

                    break;

                case "cargaSist":

                    cargaSist();
                    break;

                case "cargaCompSist":

                    int idSist = Convert.ToInt32(Request.Form["Sist"]);

                    cargaCompSist(idSist);
                    break;

                case "cargaDevs":

                    cargaDevs();
                    break;
            }
        }

        #endregion


        #region METODOS


        public void ObtenerComponentes()
        {
            //Instancio el objeto Entity
            using (var db = new Diverscan())
            {
                //Realizo un INNER JOIN
                var queryComponentes = (from CS in db.ComponenteSistema
                                        join S in db.Sistema on CS.IdSistema equals S.IdSistema
                                        join E in db.EmpresaCliente on S.IdEmpresa equals E.IdEmpresa
                                        join D in db.Desarrolladores on CS.IdDesarrollador equals D.IdDesarrollador
                                        orderby E.Nombre ascending
                                        select new
                                        {
                                            Empresa = E.Nombre,
                                            Sistema = S.Nombre,
                                            Componente = CS.Nombre,
                                            Descripcion = CS.Descripcion,
                                            Desarrollador = D.Nombre + " " + D.Apellidos,
                                            FechaLiberacion = CS.FechaLiberacion,
                                            Version = CS.Version
                                        })
                            .ToList();

                string jsonString = JsonConvert.SerializeObject(queryComponentes);

                Response.Clear();
                Response.Write(jsonString);
                Response.End();

            }
        }

        public void ObtenerRespaldo()
        {
            using (var db = new Diverscan())
            {
                try
                {
                    var querySistema = (from R in db.Respaldo
                                        join S in db.Sistema on R.IdSistema equals S.IdSistema
                                        join E in db.EmpresaCliente on S.IdEmpresa equals E.IdEmpresa
                                        join CS in db.ComponenteSistema on R.IdComponenteSistema equals CS.IdComponenteSistema
                                        join D in db.Desarrolladores on CS.IdDesarrollador equals D.IdDesarrollador
                                        join DR in db.Desarrolladores on R.IdDesarrollador equals DR.IdDesarrollador
                                        join DC in db.Desarrolladores on R.IdDesarrolladorCertifica equals DC.IdDesarrollador
                                        orderby R.FechaUltimoRespaldo descending
                                        select new
                                        {
                                            Sistema = E.Nombre + " - " + S.Nombre,
                                            Componente = CS.Nombre,
                                            Desarrollador = D.Nombre + " " + D.Apellidos,
                                            R.FechaUltimoRespaldo,
                                            R.FechaUltimaModificacion,
                                            R.FechaPruebaRespaldo,
                                            R.RespaldoGIT,
                                            R.RespaldoNube,
                                            R.RespaldoFisico,
                                            RespaldoDB = R.RespaldoDB,
                                            RespaldoPor = DR.Nombre + " " + DR.Apellidos,
                                            Certificador = DC.Nombre + " " + DC.Apellidos ?? "Sin Registro",
                                            R.Comentarios
                                        })
                                                .AsEnumerable() // Cambio a LINQ to Objects
                                        .Select(r => new
                                        {
                                            r.Sistema,
                                            r.Componente,
                                            r.Desarrollador,
                                            FechaUltimoRespaldo = r.FechaUltimoRespaldo.Year + "/" + r.FechaUltimoRespaldo.Month.ToString().PadLeft(2, '0') + "/" + r.FechaUltimoRespaldo.Day.ToString().PadLeft(2, '0'),
                                            FechaUltimaModificacion = r.FechaUltimaModificacion.Value.Year + "/" + r.FechaUltimaModificacion.Value.Month.ToString().PadLeft(2, '0') + "/" + r.FechaUltimaModificacion.Value.Day.ToString().PadLeft(2, '0'),
                                            FechaPruebaRespaldo = r.FechaPruebaRespaldo.Year + "/" + r.FechaPruebaRespaldo.Month.ToString().PadLeft(2, '0') + "/" + r.FechaPruebaRespaldo.Day.ToString().PadLeft(2, '0'),
                                            r.RespaldoGIT,
                                            r.RespaldoNube,
                                            r.RespaldoFisico,
                                            r.RespaldoDB,
                                            r.RespaldoPor,
                                            r.Certificador,
                                            r.Comentarios
                                        })
                                        .ToList();

                    string jsonString = JsonConvert.SerializeObject(querySistema);

                    Response.Clear();
                    Response.Write(jsonString);
                    Response.End();
                }
                catch (Exception e)
                {

                    throw e;
                }
            }
        }

        public void AgregarRespaldo(int idSist, int idCompSist, int idRespPor, int idCertPor, DateTime fechaMod, bool GIT, bool nube, bool fisico, bool dba, string version, string comentario)
        {
            DateTime fechaHoy = DateTime.Today;

            using (var db = new Diverscan())
            {
                try
                {
                    //Hacemos el nuevo registro
                    var nuevoRegistro = new Respaldo
                    {
                        IdSistema = idSist,
                        IdComponenteSistema = idCompSist,
                        IdDesarrollador = idRespPor,
                        //FechaLiberacion = fechaLib,
                        FechaUltimaModificacion = fechaMod,
                        FechaUltimoRespaldo = fechaHoy,
                        FechaPruebaRespaldo = fechaHoy,
                        RespaldoGIT = GIT,
                        RespaldoNube = nube,
                        RespaldoDB = dba,
                        RespaldoFisico = fisico,
                        Comentarios = comentario,
                        Version = version,
                        IdDesarrolladorCertifica = idCertPor,
                    };

                    //Agregamos el nuevo registro a la instancia
                    db.Respaldo.Add(nuevoRegistro);

                    //Insertamos el nuevo registro
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    throw e;
                }

            }
        }

        public void cargaSist()
        {
            using (var db = new Diverscan())
            {
                var querySist = (from S in db.Sistema
                                 join E in db.EmpresaCliente on S.IdEmpresa equals E.IdEmpresa
                                 orderby E.Nombre ascending
                                 select new
                                 {
                                     S.IdSistema,
                                     Nombre = E.Nombre + " - " + S.Nombre
                                 }).ToList();


                string jsonStringSist = JsonConvert.SerializeObject(querySist);

                Response.Clear();
                Response.Write(jsonStringSist);
                Response.End();
            }
        }

        public void cargaCompSist(int Sist)
        {
            using (var db = new Diverscan())
            {
                var queryCompSist = from CS in db.ComponenteSistema
                                    join S in db.Sistema on CS.IdSistema equals S.IdSistema
                                    where S.IdSistema == Sist
                                    orderby S.Nombre ascending
                                    select new
                                    {
                                        CS.IdComponenteSistema,
                                        CS.Nombre
                                    };

                string jsonStringCompSist = JsonConvert.SerializeObject(queryCompSist);

                Response.Clear();
                Response.Write(jsonStringCompSist);
                Response.End();
            }
        }

        public void cargaDevs()
        {
            using (var db = new Diverscan())
            {
                var queryDev = db.Desarrolladores
                    .Select(d => new { d.IdDesarrollador, d.Nombre })
                    .ToList();

                string jsonStringDevs = JsonConvert.SerializeObject(queryDev);

                Response.Clear();
                Response.Write(jsonStringDevs);
                Response.End();
            }
        }

        #endregion
    }
}