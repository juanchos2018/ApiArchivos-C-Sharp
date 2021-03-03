using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class ExisteController : ApiController
    {
        public HttpResponseMessage Post()
        {

            string resputesta = "";
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
            if (anio == null)
            {
                 path = a + ruc + "\\" + subdiario + "\\" + ncomprobante + "\\" + tipo + "\\" + nombre;
            }
           
            else
            {
                 path = a + ruc + "\\" + anio + "\\" + subdiario + "\\" + ncomprobante + "\\" + tipo + "\\" + nombre;
            }

            if (File.Exists(path))
            {
                resputesta = "Archivo-Existe";
            }
            else
            {
                resputesta = "Archivo-No-Existe";
            }
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(resputesta);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
            return response;
        }
    }
}
