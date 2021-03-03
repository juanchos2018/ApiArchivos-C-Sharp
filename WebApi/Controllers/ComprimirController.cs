using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApi.Controllers
{
    [RoutePrefix("api")]
    public class ComprimirController : ApiController
    {
        [Route("comprimir/todo")]
        public IHttpActionResult Todo(){

            string status = "";
            List<Documentos> lista = new List<Documentos>();
            var a = System.Web.HttpContext.Current.Server.MapPath("~/Archivos/");
            var streamProvider = new MultipartFormDataStreamProvider(a);
            Request.Content.ReadAsMultipartAsync(streamProvider);
            string dia = DateTime.Now.Day.ToString();
            string mes = DateTime.Now.Month.ToString();
            string anio = DateTime.Now.Year.ToString();
            string fecha = dia + mes + anio;

            string anioo = streamProvider.FormData["anio"];
            string ruc = streamProvider.FormData["ruc"];
            string subdiario = streamProvider.FormData["subdiario"];
            string ncomprobante = streamProvider.FormData["ncomprobante"];
            string tipo = streamProvider.FormData["tipo"];
            string server = "";
            if (anioo == null)
            {
                server = a + ruc +"\\" + subdiario + "\\" + ncomprobante + "\\" + tipo + "\\";
            }
            else
            {
                server = a + ruc + "\\" + anioo + "\\" + subdiario + "\\" + ncomprobante + "\\" + tipo + "\\";

            }           

            string filename =tipo+"-"+ subdiario + "-" + ncomprobante + "-" + fecha + ".zip";
            string outputpath = a + ruc + "\\"+anioo+ "\\" + subdiario + "\\" + ncomprobante + "\\" +filename;
            string ruta1 = "";
            if (Directory.Exists(Path.GetDirectoryName(server)))
            {
                if (File.Exists(outputpath))
                {
                    File.SetAttributes(outputpath, FileAttributes.Normal);
                    File.Delete(outputpath);
                }
                ZipFile.CreateFromDirectory(server, outputpath);

                if ( anioo == null)
                {
                    ruta1 = "Archivo" + "\\" + ruc +"\\" + subdiario + "\\" + ncomprobante + "\\" + filename;
                }
                else
                {
                    ruta1 = "Archivo" + "\\" + ruc + "\\" + anio + "\\" + subdiario + "\\" + ncomprobante + "\\" + filename;
                }
            
               
                status = "Zip Creado";
            }

            else
            {
                status = "No existe ruta";
            }
            return Ok(new { repuesta = status, ruta = ruta1 });
        }


        [Route("comprimir/rutas")]        
        public IHttpActionResult Rutas([FromBody] Datos datos)
        {
            var serverpath = "";
            string ruc = datos.ruc;
            string subidiario = datos.subdiario;
            string ncomprobante = datos.ncomprobante;
            string anio = datos.anio;
            List<Comprimidos> listacomprimidos = new List<Comprimidos>();
            if (anio == null)
            {
                serverpath = System.Web.HttpContext.Current.Server.MapPath("~/Archivos/" + ruc + "/" + subidiario + "/" + ncomprobante + "/");
                
            }
            else
            {
                serverpath = System.Web.HttpContext.Current.Server.MapPath("~/Archivos/" + ruc + "/" + anio + "/" + subidiario + "/" + ncomprobante + "/");

            }

            Dictionary<string, byte[]> fileList = new Dictionary<string, byte[]>();           

            if (datos.rutas.Count>0)
            {
                foreach (archivosComprimir item in datos.rutas)
                {
                    if (File.Exists(Path.Combine(serverpath, item.ruta)))
                    {
                        fileList.Add(Path.GetFileName(Path.Combine(serverpath, item.ruta)), System.IO.File.ReadAllBytes(Path.Combine(serverpath, item.ruta)));
                        Comprimidos o = new Comprimidos();
                        o.nombre = item.ruta;
                        listacomprimidos.Add(o);
                    }
                }
            }
            else
            {               
                string estado = "Archivo Zip No Creado";
                return Ok(new { estatus = estado });              
            }           
            try
            {
                if (listacomprimidos.Count>0)
                {
                    string dia = DateTime.Now.Day.ToString();
                    string mes = DateTime.Now.Month.ToString();



                    string anioi = DateTime.Now.Year.ToString();
                    string fecha = dia + mes + anioi;
                    string rutafile = CompressToZip(subidiario + "-" + ncomprobante + "-" + fecha + ".zip", fileList, ruc,anio, subidiario, ncomprobante);

                    if (File.Exists(rutafile))
                    {
                        string filename = subidiario + "-" + ncomprobante + "-" + fecha + ".zip";
                        string ruta = "";
                        if ( anio == null)
                        {
                             ruta = "Archivos" + "\\" + ruc + "\\" + subidiario + "\\" + ncomprobante + "\\" + filename;
                        }
                        else
                        {
                             ruta = "Archivos" + "\\" + ruc + "\\" + anio + "\\" + subidiario + "\\" + ncomprobante + "\\" + filename;
                        }
                       
                        return Ok(new { ruta = ruta, Comprimidos = listacomprimidos });
                    }
                    else
                    {
                        string estado = "Ruta no Existe";
                        return Ok(new { estatus = estado });
                    }
                }
                else
                {
                    string estado = "Ninguna Ruta Existe !";
                    return Ok(new { estatus = estado });

                }       
            }
            catch (Exception)
            {

                throw;
            }

        }
        private string CompressToZip(string fileName, Dictionary<string, byte[]> fileList,string ruc,string anio,string subdiario,string ncomprobante)
        {

            string ruta = "";
            var path = "";

            if (anio == null)
            {
                path = System.Web.HttpContext.Current.Server.MapPath("~/Archivos/" + ruc +  "/" + subdiario + "/" + ncomprobante + "/");
            }
            else
            {
                path = System.Web.HttpContext.Current.Server.MapPath("~/Archivos/" + ruc + "/" + anio + "/" + subdiario + "/" + ncomprobante + "/");

            }
               
                using (var memoryStream = new MemoryStream())
                {
                    using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        foreach (var file in fileList)
                        {
                            var demoFile = archive.CreateEntry(file.Key);

                            using (var entryStream = demoFile.Open())
                            using (var b = new BinaryWriter(entryStream))
                            {
                                b.Write(file.Value);
                            }
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                    {
                        memoryStream.Seek(0, SeekOrigin.Begin);
                        memoryStream.CopyTo(fileStream);
                        ruta = path + fileName;
                    }
            }            
            return ruta;

        }
        public class archivosComprimir
        {
            public string ruta { get; set; }
        }
        public class Datos
        {
            public string ruc { get; set; }
            public string anio { get; set; }
            public string subdiario { get; set; }
            public string ncomprobante { get; set; }
            public List<archivosComprimir> rutas { get; set; }
        }
        public class Comprimidos
        {
            public string nombre { get; set; }
        }
    }
}
