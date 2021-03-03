
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;

namespace WebApi.Controllers
{
    [RoutePrefix("api")]
    public class XmlController : ApiController
    {
        [Route("xml/datos")]
        public IHttpActionResult xml([FromBody] DatosXml datos)             
        {
          //  XDocument xmldata = XDocument.Load(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/prueba.xml"));
            string path = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/prueba.xml");

            List<Comprobante> lista1 = new List<Comprobante>();
            List<ComprobateDetalle> lista2 = new List<ComprobateDetalle>();

            Comprobante o = new Comprobante();
            o.id = "1";
            o.cliente = "juan carlos";
            o.fecha = "12/12/2020";
            o.numero = "0001";
            lista1.Add(o);

            Comprobante o1 = new Comprobante();
            o1.id = "2";
            o1.cliente = " pepep lococo ";
            o1.fecha = "12/12/2020";
            o1.numero = "0002";
            lista1.Add(o1);

            ComprobateDetalle ob = new ComprobateDetalle();
            ob.id_comprobante = "1";
            ob.idproducto = "111";
            ob.nombre_articulo = "ceramicas";
            ob.precio = "1000";
            ob.cantidad = "2";
            lista2.Add(ob);

            ComprobateDetalle ob3 = new ComprobateDetalle();
            ob3.id_comprobante = "1";
            ob3.idproducto = "111";
            ob3.nombre_articulo = "camote ";
            ob3.precio = "1000";
            ob3.cantidad = "2";
            lista2.Add(ob3);

            ComprobateDetalle ob4 = new ComprobateDetalle();
            ob4.id_comprobante = "1";
            ob4.idproducto = "111";
            ob4.nombre_articulo = "manzanas";
            ob4.precio = "1000";
            ob4.cantidad = "2";
            lista2.Add(ob4);


            ComprobateDetalle ob5 = new ComprobateDetalle();
            ob5.id_comprobante = "2";
            ob5.idproducto = "111";
            ob5.nombre_articulo = "ceramicas";
            ob5.precio = "1000";
            ob5.cantidad = "2";
            lista2.Add(ob5);

            string url = "https://192.168.0.19/backendSigma/Comprobante/GetLista/";  
            datos._crearXml(path,"Factura");
            datos._Añadir(datos.id, datos.nombre,datos.apellido, datos.direccion);

            foreach (ComprobateDetalle item in lista2)
            {
                datos.Detalle(item.idproducto,item.nombre_articulo,item.precio,item.cantidad);
            }

            //return Ok(new { repuesta = resultado });
            return Ok();

        }
    }

    public class DatosXml
    {
        public string id { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string direccion { get; set; }

        XmlDocument doc;
        string rutaXml;

        public void _crearXml(string ruta, string nodoRaiz)
        {

            this.rutaXml = ruta;
            doc = new XmlDocument();

            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);

            XmlNode root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);


            XmlNode element1 = doc.CreateElement(nodoRaiz);
            doc.AppendChild(element1);
            doc.Save(ruta);



        }

        public void _Añadir(string id, string nom, string ape, string dir)
        {
            doc.Load(rutaXml);

            XmlNode empleado = _Crear_Empleado(id, nom, ape, dir);

            XmlNode nodoRaiz = doc.DocumentElement;

            nodoRaiz.InsertAfter(empleado, nodoRaiz.LastChild);

            doc.Save(rutaXml);


        }

        public void Detalle(string idprodto,string nombre,string precio,string cantidad)
        {
            doc.Load(rutaXml);

            XmlNode empleado = crearDetalle(idprodto, nombre, precio, cantidad);

            XmlNode nodoRaiz = doc.DocumentElement;

            nodoRaiz.InsertAfter(empleado, nodoRaiz.LastChild);

            doc.Save(rutaXml);
        }

        private XmlNode crearDetalle(string id_articulo, string articulo, string precio, string cantidad)
        {//para el detalle  
            XmlNode detalle = doc.CreateElement("detalle");

            XmlElement xid = doc.CreateElement("id_articulo");
            xid.InnerText = id_articulo;
            detalle.AppendChild(xid);

            XmlElement xarticulo = doc.CreateElement("articulo");
            xarticulo.InnerText = articulo;
            detalle.AppendChild(xarticulo);

            XmlElement xprecio = doc.CreateElement("precio");
            xprecio.InnerText = precio;
            detalle.AppendChild(xprecio);

            XmlElement xcantidad = doc.CreateElement("cantidad");
            xcantidad.InnerText = cantidad;
            detalle.AppendChild(xcantidad);
            return detalle;
        }
        private XmlNode _Crear_Empleado(string id, string nom, string ape, string dir)
        {

            XmlNode empleado = doc.CreateElement("persona");

            XmlElement xid = doc.CreateElement("id");
            xid.InnerText = id;
            empleado.AppendChild(xid);


            XmlElement xnombre = doc.CreateElement("nombre");
            xnombre.InnerText = nom;
            empleado.AppendChild(xnombre);


            XmlElement xapellidos = doc.CreateElement("apellidos");
            xapellidos.InnerText = ape;
            empleado.AppendChild(xapellidos);


            XmlElement xdireccion = doc.CreateElement("direccion");
            xdireccion.InnerText = dir;
            empleado.AppendChild(xdireccion);


            return empleado;
        }

    }
    public class Comprobante
    {
        public string id { get; set; }
        public string numero { get; set; }
        public string cliente { get; set; }
        public string fecha { get; set; }
    }
    public class ComprobateDetalle
    {

        public string id_comprobante { get; set; }
        public string idproducto { get; set; }
        public string nombre_articulo { get; set; }
        public string precio { get; set; }
        public string cantidad { get; set; }
    }

}
