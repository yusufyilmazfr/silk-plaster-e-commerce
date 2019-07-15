using SilkPlaster.BusinessLayer.Abstract;
using SilkPlaster.UI.Models.Helpers.Url;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace SilkPlaster.UI.Controllers
{
    public class SeoController : Controller
    {
        private IProductManager _productManager { get; set; }

        public SeoController(IProductManager productManager)
        {
            _productManager = productManager;
        }

        public ActionResult Index()
        {
            return View();
        }

        [Route("siteMap.xml")]
        public ActionResult SiteMapXml()
        {
            string siteUrl = Request.Url.GetLeftPart(UriPartial.Authority);

            Response.Clear();
            Response.ContentType = "text/xml";

            XmlTextWriter xmlTextWriter = new XmlTextWriter(Response.OutputStream, Encoding.UTF8);
            xmlTextWriter.WriteStartDocument();
            xmlTextWriter.WriteStartElement("urlset");
            xmlTextWriter.WriteAttributeString("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");
            xmlTextWriter.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            xmlTextWriter.WriteAttributeString("xsi:schemaLocation", "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd");

            xmlTextWriter.WriteStartElement("url");
            xmlTextWriter.WriteElementString("loc", siteUrl + Url.Action("Index", "Home"));
            xmlTextWriter.WriteElementString("lastmod", DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd"));
            xmlTextWriter.WriteElementString("priority", "0.5");
            xmlTextWriter.WriteElementString("changefreq", "weekly");
            xmlTextWriter.WriteEndElement();


            xmlTextWriter.WriteStartElement("url");
            xmlTextWriter.WriteElementString("loc", siteUrl + Url.Action("AboutUs", "Home"));
            xmlTextWriter.WriteElementString("lastmod", DateTime.Now.AddDays(-20).ToString("yyyy-MM-dd"));
            xmlTextWriter.WriteElementString("priority", "0.5");
            xmlTextWriter.WriteElementString("changefreq", "monthly");
            xmlTextWriter.WriteEndElement();


            xmlTextWriter.WriteStartElement("url");
            xmlTextWriter.WriteElementString("loc", siteUrl + Url.Action("FAQ", "Home"));
            xmlTextWriter.WriteElementString("lastmod", DateTime.Now.AddDays(-20).ToString("yyyy-MM-dd"));
            xmlTextWriter.WriteElementString("priority", "0.5");
            xmlTextWriter.WriteElementString("changefreq", "monthly");
            xmlTextWriter.WriteEndElement();

            xmlTextWriter.WriteStartElement("url");
            xmlTextWriter.WriteElementString("loc", siteUrl + Url.Action("Contact", "Home"));
            xmlTextWriter.WriteElementString("lastmod", DateTime.Now.AddDays(-20).ToString("yyyy-MM-dd"));
            xmlTextWriter.WriteElementString("priority", "0.5");
            xmlTextWriter.WriteElementString("changefreq", "monthly");
            xmlTextWriter.WriteEndElement();

            xmlTextWriter.WriteStartElement("url");
            xmlTextWriter.WriteElementString("loc", siteUrl + Url.Action("Login", "Account"));
            xmlTextWriter.WriteElementString("lastmod", DateTime.Now.AddDays(-20).ToString("yyyy-MM-dd"));
            xmlTextWriter.WriteElementString("priority", "0.5");
            xmlTextWriter.WriteElementString("changefreq", "monthly");
            xmlTextWriter.WriteEndElement();

            xmlTextWriter.WriteStartElement("url");
            xmlTextWriter.WriteElementString("loc", siteUrl + Url.Action("Register", "Account"));
            xmlTextWriter.WriteElementString("lastmod", DateTime.Now.AddDays(-20).ToString("yyyy-MM-dd"));
            xmlTextWriter.WriteElementString("priority", "0.5");
            xmlTextWriter.WriteElementString("changefreq", "monthly");
            xmlTextWriter.WriteEndElement();

            foreach (var product in _productManager.GetProductsWithDetails())
            {
                xmlTextWriter.WriteStartElement("url");
                xmlTextWriter.WriteElementString("loc", siteUrl + "/" + CreateFriendlyUrl.GenerateSlug(product.Name) + "-" + product.Id);
                xmlTextWriter.WriteElementString("lastmod", product.AddedDate.ToString("yyyy-MM-dd"));
                xmlTextWriter.WriteElementString("priority", "0.5");
                xmlTextWriter.WriteElementString("changefreq", "monthly");
                xmlTextWriter.WriteEndElement();
            }

            xmlTextWriter.WriteEndDocument();

            xmlTextWriter.Flush();
            xmlTextWriter.Close();
            Response.End();

            return View();
        }

        [Route("RSS")]
        public ActionResult Rss()
        {
            string siteUrl = Request.Url.GetLeftPart(UriPartial.Authority);

            Response.Clear();
            Response.ContentType = "text/xml";

            XmlTextWriter xmlTextWriter = new XmlTextWriter(Response.OutputStream, Encoding.UTF8);
            xmlTextWriter.WriteStartDocument();
            xmlTextWriter.WriteStartElement("rss");
            xmlTextWriter.WriteAttributeString("version", "1.0");
            xmlTextWriter.WriteStartElement("channel");

            foreach (var product in _productManager.GetProductsWithDetails())
            {
                xmlTextWriter.WriteElementString("title", product.Name);
                xmlTextWriter.WriteElementString("link", siteUrl + "/" + CreateFriendlyUrl.GenerateSlug(product.Name) + "-" + product.Id);
                xmlTextWriter.WriteElementString("description", HttpUtility.HtmlDecode(product.LongDescription));
                xmlTextWriter.WriteElementString("pubDate", product.AddedDate.ToString("dd.MM.yyyy"));
                xmlTextWriter.WriteElementString("language", "tr-TR");
            }

            xmlTextWriter.WriteEndDocument();

            xmlTextWriter.Flush();
            xmlTextWriter.Close();
            Response.End();

            return View();
        }
    }
}