using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace Mail
{
    public class Email
	{
        /// <summary>
        /// Método de envio de email
        /// </summary>
        /// <param name="pEmailFrom">Email do Remetente [deixar vazio para usar o email do serviço smtp]</param>
        /// <param name="pNameFrom">Nome do Remetente</param>
        /// <param name="pEmailTo">Email de destino</param>
        /// <param name="pEmailSubject">Assunto do Email</param>
        /// <param name="pEmailMessage">Corpo do Email</param>
        /// <param name="pAttach">Anexo em formato Stream [Opcional]</param>
        /// <param name="pAttachName">Nome do Anexo [Opcional]</param>
        /// <param name="pAttachType">Tipo do Anexo [Opcional]</param>
        /// <returns>Mensagem de sucesso ou falha</returns>
        public bool Send(string pEmailFrom, string pNameFrom, string pEmailTo, string pEmailSubject, string pEmailMessage, Stream pAttach = null, string pAttachName = "", string pAttachType = "")
        {
            try
            {
                // Configurações de smtp do cliente.
                var SmtpUser = "teste@lteste.com.br";
                var SmtpPassword = "123456789";
                var SmtpPort = 587;
                var SmtpServer = "smtp.zoho.com";
                var SmtpSsl = true;
                
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Credentials = new NetworkCredential(SmtpUser, SmtpPassword);
                smtpClient.Port = SmtpPort;
                smtpClient.Host = SmtpServer;
                smtpClient.EnableSsl = SmtpSsl;

                MailMessage email = new MailMessage();

                email.From = new MailAddress(pEmailFrom.Length > 0 ? pEmailFrom : SmtpUser, pNameFrom, Encoding.UTF8);

                string[] addresses = pEmailTo.Split(Char.Parse(";"));
                foreach (var addr in addresses)
                {
                    email.To.Add(addr);
                }

                email.Subject = pEmailSubject;
                email.IsBodyHtml = true;
                email.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(pEmailMessage, Encoding.UTF8, MediaTypeNames.Text.Html));

                //Anexo
                if (pAttach != null)
                {
                    email.Attachments.Add(new Attachment(pAttach, pAttachName, pAttachType));
                }
                
                email.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                smtpClient.Send(email);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}