using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class EliminarController : ApiController
    {

        public HttpResponseMessage Post()
        {

            var a = System.Web.HttpContext.Current.Server.MapPath("~/Archivos/");
            var streamProvider = new MultipartFormDataStreamProvider(a);
            Request.Content.ReadAsMultipartAsync(streamProvider);

            string ruc = streamProvider.FormData["ruc"];
            string anio = streamProvider.FormData["anio"];
            string subdiario = streamProvider.FormData["subdiario"];
            string ncomprobante = streamProvider.FormData["ncomprobante"];
            string tipo = streamProvider.FormData["tipo"];
            string nombre = streamProvider.FormData["nombre"];
            string path = "";
            if ( anio == null)
            {
                path = a + ruc + "\\"  + subdiario + "\\" + ncomprobante + "\\" + tipo + "\\" + nombre;
            }
            else
            {
                path = a + ruc + "\\" + anio + "\\" + subdiario + "\\" + ncomprobante + "\\" + tipo + "\\" + nombre;
            }
           
            string esta = "";
            if (File.Exists(path))
            {
                File.SetAttributes(path, FileAttributes.Normal);
                File.Delete(path);
                esta = "Eliminado Archivo";
                if (tipo.Equals("pdf"))
                {
                    string result;
                    result = Path.ChangeExtension(nombre, ".jpeg");

                    string paththumbail = "";
                    if ( anio == null)
                    {
                         paththumbail = a + ruc + "\\" + subdiario + "\\" + ncomprobante + "\\" + tipo + "\\" + "thumbail" + "\\" + "thumpdf-" + result;
                    }
                    else
                    {
                         paththumbail = a + ruc + "\\" + anio + "\\" + subdiario + "\\" + ncomprobante + "\\" + tipo + "\\" + "thumbail" + "\\" + "thumpdf-" + result;
                    }

                    if (File.Exists(paththumbail))
                    {
                        File.SetAttributes(paththumbail, FileAttributes.Normal);
                        File.Delete(paththumbail);
                    }

                }
                if (tipo.Equals("imagen"))
                {
                    string paththumbail = "";
                    if ( anio == null)
                    {
                         paththumbail = a + ruc + "\\" + subdiario + "\\" + ncomprobante + "\\" + tipo + "\\" + "thumbail" + "\\" + nombre;
                    }
                    else
                    {
                         paththumbail = a + ruc + "\\" + anio + "\\" + subdiario + "\\" + ncomprobante + "\\" + tipo + "\\" + "thumbail" + "\\" + nombre;
                    }
                    
                    if (File.Exists(paththumbail))
                    {
                        File.SetAttributes(paththumbail, FileAttributes.Normal);
                        File.Delete(paththumbail);
                    }
                }
            }
            else
            {
                esta = "No Existe Ruta";                
            }
            //var response = new HttpResponseMessage(HttpStatusCode.OK);
            //response.Content = new StringContent("Eliminado Archivo :" + nombre);
           // response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
            //return response;            
            var data = new
                {
                estado = esta,
                archivo = nombre,               
               };

            return Request.CreateResponse(HttpStatusCode.OK, data, Configuration.Formatters.JsonFormatter);
            //  return Request.CreateResponse(HttpStatusCode.OK);
        }

    }
}
