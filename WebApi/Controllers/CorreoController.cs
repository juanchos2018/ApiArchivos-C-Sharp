using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class CorreoController : ApiController
    {
        public IHttpActionResult Index([FromBody] Datos o)
        {
            var fromAddress = new MailAddress(o.remitente, o.nombre_empresa);
            var toAddress = new MailAddress(o.correo, "To Name");
            string fromPassword = o.passwword;
            //Universidad2020
            string subject = o.asunto;
            string body  = o.mensaje;
            string path  = System.Web.HttpContext.Current.Server.MapPath("~/Archivos/" + o.ruta);
            string path1 = System.Web.HttpContext.Current.Server.MapPath("~/Archivos/" + o.ruta1);
            // string path =o.ruta;
            // string path1 = o.ruta1;
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
            };
            if (File.Exists(path))
            {
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                })
                {   //Archivos\20604124167\00\020001\pdf
                    //Server.MapPath("~/App_Data/hello.pdf")
                    // message.Attachments.Add(new Attachment(o.ruta));
                    message.Attachments.Add(new Attachment(path));
                    message.Attachments.Add(new Attachment(path1));

                    smtp.Send(message);
                   
                }
            }
            else
            {
                return BadRequest("No exite la ruta");
            }
            return Ok("Enviado con Exito");
        }
    }

    public class Datos
    {
        public string correo { get; set; }
        public string ruta { get; set; }
        public string ruta1 { get; set; }
        public string asunto { get; set; }
        public string mensaje { get; set; }
        public string remitente { get; set; }
        public string nombre_empresa { get; set; }
        public string passwword { get; set; }
    }

}
