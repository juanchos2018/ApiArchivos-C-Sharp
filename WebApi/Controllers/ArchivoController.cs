using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class ArchivoController : ApiController
    {
       
        string temp = "/temp/";
        [System.Web.Http.HttpPost]      
        public async Task<HttpResponseMessage> Index()
        {
            //subdiario dianmico
            var tipos = new List<string> { "pdf", "xml", "docx", "doc", "png", "PNG","jpg","JPG", "jpeg","JPEG", "xls", "xlsx" };

            List<DocumentosSubidos> lista = new List<DocumentosSubidos>();
            List<DocumentosSubidos> lista2 = new List<DocumentosSubidos>();
            if (Request.Content.IsMimeMultipartContent())
            {
                var serverPath = "";
                var path = "";            
               
               //   var streamProvider = new MultipartFormDataStreamProvider(Path.Combine(StoragePath, "Upload")); //esto si funciona
                var streamProvider = new MultipartFormDataStreamProvider(temp); //esto si funciona
                                                                                
                string existearchivo = "";    
                await Request.Content.ReadAsMultipartAsync(streamProvider);
                string ruc               = streamProvider.FormData["ruc"];
                string anio              = streamProvider.FormData["anio"];
                string subdiario         = streamProvider.FormData["subdiario"];
                string ncomprobante      = streamProvider.FormData["ncomprobante"];    

                path = System.Web.HttpContext.Current.Server.MapPath("~/Archivos/");

                if (!Directory.Exists(Path.GetDirectoryName(path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }
                if (anio==null)
                {
                    path = System.Web.HttpContext.Current.Server.MapPath("~/Archivos/" + ruc + "/" + subdiario + "/");
                }
                else
                {
                    path = System.Web.HttpContext.Current.Server.MapPath("~/Archivos/" + ruc + "/" + anio + "/" + subdiario + "/");
                }                         

                if (!Directory.Exists(Path.GetDirectoryName(path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }
                foreach (MultipartFileData fileData in streamProvider.FileData)
                {
                    DocumentosSubidos obj = new DocumentosSubidos();
                   
                    if (string.IsNullOrEmpty(fileData.Headers.ContentDisposition.FileName))
                    {
                        return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Esta solicitud no tiene el formato adecuado");
                    }
                    string fileName = fileData.Headers.ContentDisposition.FileName;
                    if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                    {
                        fileName = fileName.Trim('"');
                    }
                    if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                    {
                        fileName = Path.GetFileName(fileName);
                    }
                    var file = streamProvider.Contents.FirstOrDefault();
                    var ext = Path.GetExtension(fileName);
              
                    if (ext.Equals(".pdf"))
                    {
                        if ( anio == null)
                        {
                            serverPath = System.Web.HttpContext.Current.Server.MapPath("~/Archivos/" + ruc + "/" + subdiario + "/" + ncomprobante + "/pdf/");
                        }
                        else
                        {
                            serverPath = System.Web.HttpContext.Current.Server.MapPath("~/Archivos/" + ruc + "/" + anio + "/" + subdiario + "/" + ncomprobante + "/pdf/");
                        }

                        if (!Directory.Exists(Path.GetDirectoryName(serverPath)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(serverPath));
                        }                       
                        if (File.Exists(Path.Combine(serverPath, fileName)))
                        {                            
                            existearchivo = "Archivo Remplazado";
                            File.Delete(Path.Combine(serverPath, fileName));
                        }
                        else
                        {
                            existearchivo = "Archivo Subido";
                        }                       
                        obj.nombre = fileName;
                        obj.rutafile = "pdf\\" + fileName;
                        obj.estado = existearchivo;
                        obj.tipo = "pdf";
                      
                        File.Move(fileData.LocalFileName, Path.Combine(serverPath, fileName));
                        foreach (DocumentosSubidos item in lista2)
                        {
                            if (item.nombre.Equals(fileName))
                            {
                                string result;
                                result = Path.ChangeExtension(fileName, ".jpeg");
                                string thumbail = "thumpdf-";
                                obj.thumbail = "thumbail\\" + thumbail+ result; 
                            }
                        }
                        lista.Add(obj);
                    }
                    if (ext.Equals(".jpg")|| ext.Equals(".JPG") || ext.Equals(".png") || ext.Equals(".PNG") || ext.Equals(".jpeg") || ext.Equals(".JPEG"))
                    {                       
                        string thumbail = "thumpdf-";
                        bool existe = fileName.Contains(thumbail);
                        if (existe)
                        {
                            DocumentosSubidos obj1 = new DocumentosSubidos();                         
                          
                            if ( anio == null)
                            {
                                serverPath = System.Web.HttpContext.Current.Server.MapPath("~/Archivos/" + ruc + "/" + subdiario + "/" + ncomprobante + "/pdf/thumbail/");
                            }
                            else
                            {
                                serverPath = System.Web.HttpContext.Current.Server.MapPath("~/Archivos/" + ruc + "/" + anio + "/" + subdiario + "/" + ncomprobante + "/pdf/thumbail/");
                            }

                            if (!Directory.Exists(Path.GetDirectoryName(serverPath)))
                            {
                                Directory.CreateDirectory(Path.GetDirectoryName(serverPath));
                            }
                            if (File.Exists(Path.Combine(serverPath, fileName)))
                            {
                                File.SetAttributes(Path.Combine(serverPath, fileName), FileAttributes.Normal);
                                File.Delete(Path.Combine(serverPath, fileName));
                                existearchivo = "Archivo Remplazado";
                            }
                            else
                            {
                                existearchivo = "Archivo Subido";
                            }
                            string[] letras = fileName.Split('-');
                            string name = letras[1];
                            string result;
                            result = Path.ChangeExtension(name,".pdf");                          
                            File.Move(fileData.LocalFileName, Path.Combine(serverPath, fileName));
                            File.SetAttributes(Path.Combine(serverPath, fileName), FileAttributes.Normal);
                            obj.tipo = "imagen thumpdf";
                            obj.nombre = result;
                            obj.estado = existearchivo;                         
                            obj.thumbail = "thumbail\\" + fileName;
                            lista2.Add(obj);
                        }                      
                       
                        else
                        {
                            if (anio == null)
                            {
                                serverPath = System.Web.HttpContext.Current.Server.MapPath("~/Archivos/" + ruc + "/" + subdiario + "/" + ncomprobante + "/imagen/");
                            }
                            else
                            {
                                serverPath = System.Web.HttpContext.Current.Server.MapPath("~/Archivos/" + ruc + "/" + anio + "/" + subdiario + "/" + ncomprobante + "/imagen/");

                            }                          
                            if (!Directory.Exists(Path.GetDirectoryName(serverPath)))
                            {
                                Directory.CreateDirectory(Path.GetDirectoryName(serverPath));
                            }
                            if (File.Exists(Path.Combine(serverPath, fileName)))
                            {
                                File.SetAttributes(Path.Combine(serverPath, fileName), FileAttributes.Normal);
                                File.Delete(Path.Combine(serverPath, fileName));
                                existearchivo = "Archivo Remplazado";
                            }
                            else
                            {
                                existearchivo = "Archivo Subido";
                            }
                         
                            File.Move(fileData.LocalFileName, Path.Combine(serverPath, fileName));
                            File.SetAttributes(Path.Combine(serverPath, fileName), FileAttributes.Normal);
                            obj.tipo = "imagen";
                            obj.nombre = fileName;
                            obj.rutafile = "imagen\\" + fileName;
                            obj.estado = existearchivo;
                            obj.thumbail = "thumbail\\"+ fileName;
                            lista.Add(obj);
                            //Esto esta generando Errores we
                            Redimensionar(fileName, ruc, serverPath);
                        }      

                    }                 
                  
                    if (ext.Equals(".docx") || ext.Equals(".doc"))
                    {
                        if (anio == null)
                        {
                            serverPath = System.Web.HttpContext.Current.Server.MapPath("~/Archivos/" + ruc + "/" + subdiario + "/" + ncomprobante + "/doc/");
                        }
                        else
                        {
                            serverPath = System.Web.HttpContext.Current.Server.MapPath("~/Archivos/" + ruc + "/" + anio + "/" + subdiario + "/" + ncomprobante + "/doc/");
                        }

                        if (!Directory.Exists(Path.GetDirectoryName(serverPath)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(serverPath));
                        }
                        if (File.Exists(Path.Combine(serverPath, fileName)))
                        {
                            File.Delete(Path.Combine(serverPath, fileName));
                            existearchivo = "Archivo Remplazado";
                        }
                        else
                        {
                            existearchivo = "Archivo Subido";
                        }
                        File.Move(fileData.LocalFileName, Path.Combine(serverPath, fileName));
                        File.SetAttributes(serverPath + "\\" + fileName, FileAttributes.Normal);
                        obj.nombre = fileName;
                        obj.rutafile = "doc\\" + fileName;
                        obj.estado = existearchivo;
                        obj.tipo = "doc";
                        obj.thumbail ="";
                        lista.Add(obj);
                    }
                    if (ext.Equals(".xls") || ext.Equals(".xlsx"))
                    {
                        if ( anio == null)
                        {
                            serverPath = System.Web.HttpContext.Current.Server.MapPath("~/Archivos/" + ruc + "/" + subdiario + "/" + ncomprobante + "/xls/");
                        }
                        else
                        {
                            serverPath = System.Web.HttpContext.Current.Server.MapPath("~/Archivos/" + ruc + "/" + anio + "/" + subdiario + "/" + ncomprobante + "/xls/");
                        }

                        if (!Directory.Exists(Path.GetDirectoryName(serverPath)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(serverPath));
                        }
                        if (File.Exists(Path.Combine(serverPath, fileName)))
                        {
                            File.Delete(Path.Combine(serverPath, fileName));
                            existearchivo = "Archivo Remplazado";
                        }
                        else
                        {
                            existearchivo = "Archivo Subido";
                        }
                        File.Move(fileData.LocalFileName, Path.Combine(serverPath, fileName));
                      
                        obj.nombre = fileName;
                        obj.rutafile = "xls\\" + fileName;
                        obj.estado = existearchivo;
                        obj.tipo = "xls";
                        obj.thumbail = "";
                        lista.Add(obj);
                    }
                
                    if (!tipos.Contains(ext.Trim().TrimStart('.')))
                        throw new ArgumentException("El tipo de archivo proporcionado no es compatible!");
                }     
                return Request.CreateResponse(HttpStatusCode.OK, lista, Configuration.Formatters.JsonFormatter);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Esta solicitud no tiene el formato adecuado");
            }

        }

        public void Redimensionar(string filename,string ruc,string servepath)
        {
            int thumbWi = 100;
            int thumbHi = 100;
            bool maintainAspect = true;
            var originalFile = servepath + filename;             
            Image source = null;
            using (FileStream stream = new FileStream(originalFile, FileMode.Open))
            {
                source = Image.FromStream(stream);
            }

            if (source.Width <= thumbWi && source.Height <= thumbHi) return;
            Bitmap thumbnail;
            try
            {
                int wi = thumbWi;
                int hi = thumbHi;
                if (maintainAspect)
                {
                    // Maintain the aspect ratio despite the thumbnail size parameters
                    if (source.Width > source.Height)
                    {
                        wi = thumbWi;
                        hi = (int)(source.Height * ((decimal)thumbWi / source.Width));
                    }
                    else
                    {
                        hi = thumbHi;
                        wi = (int)(source.Width * ((decimal)thumbHi / source.Height));
                    }
                }
                thumbnail = new Bitmap(wi, hi);
                using (Graphics g = Graphics.FromImage(thumbnail))
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.FillRectangle(Brushes.Transparent, 0, 0, wi, hi);
                    g.DrawImage(source, 0, 0, wi, hi);
                }            
                var thumbnailName = servepath + "thumbail/";            
                if (!Directory.Exists(Path.GetDirectoryName(thumbnailName)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(thumbnailName));
                }

                if (File.Exists(Path.Combine(thumbnailName, filename)))
                {
                    File.SetAttributes(Path.Combine(thumbnailName, filename), FileAttributes.Normal);
                    File.Delete(Path.Combine(thumbnailName, filename));
                  
                }
                thumbnail.Save(Path.Combine(thumbnailName, filename));
                thumbnail.Dispose();
                
                File.SetAttributes(Path.Combine(thumbnailName, filename), FileAttributes.Normal);              

            }
            catch(Exception )
            {
                throw;
            }   
        }
    }
    public class DocumentosSubidos
    {        
        public string nombre { get; set; }
        public string rutafile { get; set; }
        public string estado { get; set; }
        public string thumbail { get; set; }
        public string tipo { get; set; }
    }
}
