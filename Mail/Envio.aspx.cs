using System;
using System.IO;
using System.Web.UI;

namespace Mail
{
    public partial class Envio : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Objeto responsável pelo envio de e-mail.
                Email _email = new Email();

                #region Recupera os Request da página.

                var nameToUsuario = Request["name"].ToString();
                var emailToUsuario = Request["email"].ToString();
                var ofertaUsuario = Request["form-ofertas"];
                if (ofertaUsuario == "on")
                {
                    ofertaUsuario = "Sim";
                }
                else
                {
                    ofertaUsuario = "No";
                }

                #endregion

                #region Configurações internas do sistema.

                Stream attach = new FileStream(Server.MapPath("/pdf/01-whitepaper-onix-x-hb20.pdf"), FileMode.Open);

                var nameFrom = "Teste";
                var emailFrom = "teste@teste.com.br";

                #endregion

                #region Envia o e-mail para o usuário.

                var emailSubjectUsuario = "Onix x HB20 - A Batalha pela liderança";
                var emailMessageUsuario = "Obrigado por baixar o estudo Teste. Fique ligado no nosso site e receba ofertas exclusivas!";

                var envioUsuario = _email.Send(emailFrom, nameFrom, emailToUsuario, emailSubjectUsuario, emailMessageUsuario, attach, "ApresentacaoFlylap.pdf", "application/pdf");

                #endregion

                #region Envia o e-mail para o administrador.

                var emailToAdm = "teste@teste.com.br";
                var emailSubjectAdm = "WhitePaper";
                var emailMessageAdm = "Informações<br /><br />Nome: " + nameToUsuario + "<br />Email: " + emailToUsuario + "<br />Receber ofertas: " + ofertaUsuario;

                var envioAdm = _email.Send(emailFrom, nameFrom, emailToAdm, emailSubjectAdm, emailMessageAdm);

                #endregion

                #region Salva os dados do usuário em texto csv.

                // Escreve os dados no arquivo csv.
                using (StreamWriter writer = new StreamWriter(Server.MapPath("/csv/wm-crm-whitepaper.csv"), true))
                {
                    string[] dados = new string[3] { nameToUsuario, emailToUsuario, ofertaUsuario.ToString() };
                    var conteudo = string.Join(",", dados);
                    writer.WriteLine(conteudo);
                }

                #endregion

                // Resposta de sucesso para a aplicação web.
                Response.Write(envioUsuario);
            }
            catch (Exception ex)
            {
                // Resposta de falha para a aplicação web.
                Response.Write("Message: " + ex.Message + "<br/>InnerException: " + ex.InnerException);
            }
        }
    }
}