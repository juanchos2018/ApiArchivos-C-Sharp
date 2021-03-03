using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class SubirController : ApiController
    {

        [HttpPost]
        public async Task<HttpResponseMessage> Index(string ruc)
        {
            var supportedTypes = new List<string> { "pdf", "zip", "xml", "docx", "png", "jpg", "jpeg" };
            if (!Request.Content.IsMimeMultipartContent())
            {
                return Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, "The request doesn't contain valid content!");
            }
            try
            {
                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);
                string fullpath = "";
                var file = provider.Contents.FirstOrDefault();
                if (file != null)
                {
                    if (string.IsNullOrEmpty(file.Headers.ContentDisposition.FileName))
                        throw new ArgumentNullException("file", "Se proporcionó un archivo no válido!");

                    var serverPath = "";
                    var path = "";
                    var fileNameParam = provider.Contents[0].Headers.ContentDisposition.Parameters.FirstOrDefault(x => x.Name.ToLower() == "filename");
                    string fileName = (fileNameParam == null) ? "" : fileNameParam.Value.Trim('"');
                    char primera = fileName[0];
                    string ruc_empresa = ruc;
                    string a = primera.ToString();
                    var ext = Path.GetExtension(fileName);                
                    path = System.Web.HttpContext.Current.Server.MapPath("~/sigma/");

                    if (!Directory.Exists(Path.GetDirectoryName(path)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(path));
                    }
                    switch (ext)
                    {
                        case ".pdf":
                          
                            string[] words1 = fileName.Split('-');
                            string nameRuc1 = words1[0];                           
                            serverPath = System.Web.HttpContext.Current.Server.MapPath("~/sigma/" + ruc_empresa + "/pdf/");
                            if (!Directory.Exists(Path.GetDirectoryName(serverPath)))
                            {
                                Directory.CreateDirectory(Path.GetDirectoryName(serverPath));
                            }

                            break;
                        case ".jpg":
                            
                            serverPath = System.Web.HttpContext.Current.Server.MapPath("~/sigma/" + ruc_empresa + "/Image/jpg/");
                            if (!Directory.Exists(Path.GetDirectoryName(serverPath)))
                            {
                                Directory.CreateDirectory(Path.GetDirectoryName(serverPath));
                            }                         
                            break;

                        case ".png":
                          
                            serverPath = System.Web.HttpContext.Current.Server.MapPath("~/sigma/" + ruc_empresa + "/Image/png/");
                            if (!Directory.Exists(Path.GetDirectoryName(serverPath)))
                            {
                                Directory.CreateDirectory(Path.GetDirectoryName(serverPath));
                            }                            
                            break;

                        case ".jpeg":
                           
                            serverPath = System.Web.HttpContext.Current.Server.MapPath("~/sigma/" + ruc_empresa + "/Image/jpeg/");
                            if (!Directory.Exists(Path.GetDirectoryName(serverPath)))
                            {
                                Directory.CreateDirectory(Path.GetDirectoryName(serverPath));
                            }                
                            break;

                        case ".xml":

                            string path_ruc2 = "";
                            string[] words2 = fileName.Split('-');
                            string nameRuc2 = words2[0];
                            path_ruc2 = System.Web.HttpContext.Current.Server.MapPath("~/sigma/" + ruc_empresa + "/");
                            if (!Directory.Exists(Path.GetDirectoryName(path_ruc2)))
                            {
                                Directory.CreateDirectory(Path.GetDirectoryName(path_ruc2));
                            }
                            serverPath = System.Web.HttpContext.Current.Server.MapPath("~/sigma/" + ruc_empresa + "/xml/");
                            if (!Directory.Exists(Path.GetDirectoryName(serverPath)))
                            {
                                Directory.CreateDirectory(Path.GetDirectoryName(serverPath));
                            }
                            break;

                        case ".zip":
                            if (a.Equals("R") || a.Equals("r"))
                            {
                                // para cdr
                                string path_ruc = "";
                                string[] words = fileName.Split('-');
                                string nameRuc = words[1];
                                path_ruc = System.Web.HttpContext.Current.Server.MapPath("~/sigma/" + ruc_empresa + "/");
                                if (!Directory.Exists(Path.GetDirectoryName(path_ruc)))
                                {
                                    Directory.CreateDirectory(Path.GetDirectoryName(path_ruc));
                                }

                                serverPath = System.Web.HttpContext.Current.Server.MapPath("~/sigma/" + ruc_empresa + "/cdr/");
                                if (!Directory.Exists(Path.GetDirectoryName(serverPath)))
                                {
                                    Directory.CreateDirectory(Path.GetDirectoryName(serverPath));
                                }
                            }
                            else
                            {
                                string path_ruc = "";
                                string[] words = fileName.Split('-');
                                string nameRuc = words[0];
                                path_ruc = System.Web.HttpContext.Current.Server.MapPath("~/sigma/" + ruc_empresa + "/");
                                if (!Directory.Exists(Path.GetDirectoryName(path_ruc)))
                                {
                                    Directory.CreateDirectory(Path.GetDirectoryName(path_ruc));
                                }

                                serverPath = System.Web.HttpContext.Current.Server.MapPath("~/sigma/" + ruc_empresa + "/zip/");
                                if (!Directory.Exists(Path.GetDirectoryName(serverPath)))
                                {
                                    Directory.CreateDirectory(Path.GetDirectoryName(serverPath));
                                }
                            }
                            break;

                        default:
                            break;
                    }

                    if (!supportedTypes.Contains(ext.Trim().TrimStart('.')))
                        throw new ArgumentException("El tipo de archivo proporcionado no es compatible!");

                    fullpath = Path.Combine(serverPath, Path.GetFileName(fileName));
                    var dataStream = await file.ReadAsStreamAsync();
                    using (var fileStream = File.Create(fullpath))
                    {
                        dataStream.Seek(0, System.IO.SeekOrigin.Begin);
                        dataStream.CopyTo(fileStream);
                    }
                }

                var response = Request.CreateResponse(HttpStatusCode.OK);

                response.StatusCode.ToString();
                return response;
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message);
            }

        }

    }
}
