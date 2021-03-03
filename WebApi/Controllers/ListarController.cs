using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class ListarController : ApiController
    {
        public IHttpActionResult Post()
        {          
            List<Documentos> lista = new List<Documentos>();
            var a = System.Web.HttpContext.Current.Server.MapPath("~/Archivos/");            
            var streamProvider = new MultipartFormDataStreamProvider(a);
            Request.Content.ReadAsMultipartAsync(streamProvider);
            string ruc          = streamProvider.FormData["ruc"];
            string anio         = streamProvider.FormData["anio"];
            string subdiario    = streamProvider.FormData["subdiario"];
            string ncomprobante = streamProvider.FormData["ncomprobante"];
            string tipo         = streamProvider.FormData["tipo"];
            string server       = "";
            if ( anio == null)
            {
                 server = a + ruc + "\\" + subdiario + "\\" + ncomprobante + "\\" + tipo;
            }
            else
            {
                 server = a + ruc + "\\" + anio + "\\" + subdiario + "\\" + ncomprobante + "\\" + tipo;

            }
          
            if (Directory.Exists(Path.GetDirectoryName(server)))
            {
                foreach (var path in Directory.GetFiles(server))
                {
                    Documentos o = new Documentos();

                    File.SetAttributes(path, FileAttributes.ReadOnly);
                    FileInfo info = new FileInfo(path);
                    o.tipo = tipo;
                   
                    o.ruta = tipo + "\\" + System.IO.Path.GetFileName(path);
                    o.nombre = System.IO.Path.GetFileName(path);

                    o.fechacreacion = info.CreationTime.ToString();                  
                    if (tipo.Equals("pdf"))
                    {
                        string rutathumbail = server + "\\" + "thumbail\\";
                        foreach (var item  in Directory.GetFiles(rutathumbail))
                        {
                            string ex = Path.ChangeExtension(System.IO.Path.GetFileName(item), ".pdf");                        
                            string[] letras = ex.Split('-');
                            string name = letras[1];
                            if (name.Equals(System.IO.Path.GetFileName(path)))
                            {
                                o.rutathumbail = "thumbail" + "\\" + Path.ChangeExtension(System.IO.Path.GetFileName(item),".jpg"); 
                            }
                        }
                       
                    }
                    else if (tipo.Equals("imagen"))
                    {                        
                        o.rutathumbail = "thumbail" + "\\" + o.nombre;
                    }
                    else if (tipo.Equals("doc") || tipo.Equals("docx"))
                    {
                        o.rutathumbail = "null";
                    }
                    else if (tipo.Equals("xls") || tipo.Equals("xlsx"))
                    {
                        o.rutathumbail = "null";
                    }

                    lista.Add(o);
                }
                return Ok(new { data = lista });

            }
            else
            {
                string respueta = "No Existe la Ruta";
                return Ok(new { data = respueta });

            }
         
          
        }
    }
    public class Documentos
    {
        public string tipo { get; set; }
        public string ruta { get; set; }
        public string nombre { get; set; }             
        public string rutathumbail { get; set; }
        public string fechacreacion { get; set; }
    }
}
