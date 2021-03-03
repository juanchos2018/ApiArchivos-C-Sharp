using Calculartion;
using SRTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;
using Tesseract;

namespace WebApi.Controllers
{
    public class ConsultaController : ApiController
    {

       
        [System.Web.Http.HttpPost]
        public IHttpActionResult Post([FromBody] Empresa datos)

        {
          // var ocr = new TesseractEngine(Path.Combine(Environment.CurrentDirectory, "tessdata"), "eng", EngineMode.Default);
          //string   tessPath = HostingEnvironment.MapPath(@"~ /tessdata");
         //   ocr.SetVariable("tessedit_char_whitelist", "0123456789");

        //  Sunat consultarRuc = new Sunat();
          //var a= consultarRuc.ConsultarRUC(datos.Ruc);

            Reniec re = new Reniec();      
            var a = re.ConsultarDNI_JNE(datos.dni,true);
            Calcular o = new Calcular();
            int resultado =o.Sumar(1,10);
       
            return Ok(new { repuesta = a });
        }
        // var data= consultarRuc.ConsultarRUC("111");
    }


    public class Empresa
    {
        public string dni { get; set; }
    }
}
