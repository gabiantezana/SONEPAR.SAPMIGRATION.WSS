using RazorEngine;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YAMBOLY.GESTIONRUTAS.HELPER;

namespace SONEPAR.SAPMIGRATION.WSS.HELPER
{
    public static class MailHelper
    {
        public static List<string> FindImagesIdsFromHtmlString(string html)
        {
            var ids = new List<string>();

            for (int i = html.IndexOf("src=\"cid:"); i > -1; i = html.IndexOf("src=\"cid:", i + 1))
            {
                var realIndex = i + 9;
                string newText = html?.Substring(realIndex, (html.Length - 1) - realIndex);
                var id = newText?.Substring(0, newText.IndexOf('"'));
                ids.Add(id);
            }
            return ids;
        }

        public static void AddImagesToHtml(ref MailMessage mailMessage, string parentImagesRoute, string htmlBody)
        {
            //SET IMAGES IN MAIL
            var alternativeView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);
            alternativeView.ContentId = Guid.NewGuid().ToSafeString();
            alternativeView.TransferEncoding = TransferEncoding.SevenBit;

            var imagesIdInHtml = MailHelper.FindImagesIdsFromHtmlString(htmlBody);
            foreach (var imageId in imagesIdInHtml.ToList())
            {
                LinkedResource linkedResource = new LinkedResource(ConstantHelper.MailConstant.PathImagesResources + imageId);
                linkedResource.ContentType = new ContentType("image/jpeg");
                linkedResource.ContentId = imageId;
                linkedResource.TransferEncoding = TransferEncoding.Base64;

                alternativeView.LinkedResources.Add(linkedResource);
            }
            mailMessage.AlternateViews.Add(alternativeView);
        }

        public static SmtpClient SetSmtpClientFromConfig(ref MailMessage mailMessage)
        {
            SmtpClient oSMTP = new SmtpClient(ConfigurationManager.AppSettings[ConstantHelper.APPCONFIGKEY.SmtpHost].ToSafeString(), ConfigurationManager.AppSettings[ConstantHelper.APPCONFIGKEY.SmtpPort].ToInt32());
            oSMTP.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings[ConstantHelper.APPCONFIGKEY.mailFromAddress].ToSafeString(), ConfigurationManager.AppSettings[ConstantHelper.APPCONFIGKEY.mailFromPassword].ToSafeString());
            oSMTP.EnableSsl = true;
            mailMessage.From = new MailAddress(ConfigurationManager.AppSettings[ConstantHelper.APPCONFIGKEY.mailFromAddress].ToSafeString());
            return oSMTP;
        }

        public static string RenderHtmlFromTemplate(EmbebbedFileName embebbedFileNameHtml,  MailModel mailBody, Assembly assembly)
        {
            var template = XMLHelper.GetXMLString(embebbedFileNameHtml, assembly);
            var result = Razor.Parse(template, mailBody);

            return result;
        }
    }
}
