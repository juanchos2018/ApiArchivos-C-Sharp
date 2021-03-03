using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApi.Clases
{
    public class CrearDirectorio
    {
        public bool Creardirectorio(string ruc,string subdiario,string ncomprobante,string tipo)
        {
            Boolean creado = false;

            string path;
            path = System.Web.HttpContext.Current.Server.MapPath("~/Archivos/");
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                creado = true;
            }

            path = System.Web.HttpContext.Current.Server.MapPath("~/Archivos/" + ruc + "/" + subdiario + "/");
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                creado = true;
            }

            return creado;
        }
    }
}