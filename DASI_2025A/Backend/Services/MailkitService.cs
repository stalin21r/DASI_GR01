using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Shared;

namespace Backend;

public class MailkitService : IMailkitService
{
  private readonly IConfiguration _config;
  private readonly IUserRepository _userRepository;

  public MailkitService(IConfiguration config, IUserRepository userRepository)
  {
    _userRepository = userRepository;
    _config = config;
  }

  public async Task<bool> SendActivationMail(string para, string nombre, string codigo)
  {
    if (codigo == null) return false;
    // Construir la URL de activación
    var urlBase = _config["App:UrlBase"] ?? throw new InvalidOperationException("App:UrlBase no está configurada");
    var tokenEncoded = Uri.EscapeDataString(codigo);
    var emailEncoded = Uri.EscapeDataString(para);
    var urlActivacion = $"{urlBase.TrimEnd('/')}/activar-cuenta?token={tokenEncoded}&email={emailEncoded}";

    // Obtener la plantilla HTML
    var plantillaHtml = GetActivationTemplate();

    // Reemplazar los placeholders en la plantilla
    var htmlProcesado = plantillaHtml
        .Replace("{{Nombre}}", nombre)
        .Replace("{{URL}}", urlActivacion);

    // Crear y enviar el mensaje
    var mensaje = new MimeMessage();

    mensaje.From.Add(new MailboxAddress("SS.CC. Rumipamba", _config["Smtp:From"]));
    mensaje.To.Add(MailboxAddress.Parse(para));
    mensaje.Subject = "Activación de cuenta - SS.CC. Rumipamba";

    var builder = new BodyBuilder
    {
      HtmlBody = htmlProcesado
    };

    mensaje.Body = builder.ToMessageBody();

    using var smtp = new SmtpClient();

    await smtp.ConnectAsync(_config["Smtp:Host"], int.Parse(_config["Smtp:Port"]!), SecureSocketOptions.StartTls);
    await smtp.AuthenticateAsync(_config["Smtp:User"], _config["Smtp:Pass"]);
    await smtp.SendAsync(mensaje);
    await smtp.DisconnectAsync(true);
    return true;
  }

  public async Task<bool> SendPasswordRecoveryMail(string para, string nombre, string codigo)
  {
    if (codigo == null) return false;
    // Construir la URL de activación
    var urlBase = _config["App:UrlBase"] ?? throw new InvalidOperationException("App:UrlBase no está configurada");
    var tokenEncoded = Uri.EscapeDataString(codigo);
    var emailEncoded = Uri.EscapeDataString(para);
    var urlActivacion = $"{urlBase.TrimEnd('/')}/recuperar-contrasena?token={tokenEncoded}&email={emailEncoded}";

    // Obtener la plantilla HTML
    var plantillaHtml = GetPasswordRecoveryTemplate();

    // Reemplazar los placeholders en la plantilla
    var htmlProcesado = plantillaHtml
        .Replace("{{Nombre}}", nombre)
        .Replace("{{URL}}", urlActivacion);

    // Crear y enviar el mensaje
    var mensaje = new MimeMessage();

    mensaje.From.Add(new MailboxAddress("SS.CC. Rumipamba", _config["Smtp:From"]));
    mensaje.To.Add(MailboxAddress.Parse(para));
    mensaje.Subject = "Recuperación de contraseña - SS.CC. Rumipamba";

    var builder = new BodyBuilder
    {
      HtmlBody = htmlProcesado
    };

    mensaje.Body = builder.ToMessageBody();

    using var smtp = new SmtpClient();

    await smtp.ConnectAsync(_config["Smtp:Host"], int.Parse(_config["Smtp:Port"]!), SecureSocketOptions.StartTls);
    await smtp.AuthenticateAsync(_config["Smtp:User"], _config["Smtp:Pass"]);
    await smtp.SendAsync(mensaje);
    await smtp.DisconnectAsync(true);
    return true;
  }

  private string GetActivationTemplate()
  {
    return @"<!DOCTYPE html>
    <html lang=""es"">

    <head>
      <meta charset=""UTF-8"">
      <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
      <title>SS.CC. Rumipamba - Activación de Cuenta</title>
      <style>
        body {
          margin: 0;
          padding: 0;
          font-family: Arial, sans-serif;
          background-color: #f4f4f4;
        }

        .email-container {
          width: 100%;
          max-width: 600px;
          margin: 0 auto;
          background-color: #ffffff;
        }

        .header {
          background-color: #2E8B57;
          color: white;
          text-align: center;
          padding: 20px 0;
        }

        .logo-section {
          text-align: center;
          padding: 20px 0;
        }

        .content {
          padding: 20px 30px;
        }

        .button {
          background-color: #2E8B57;
          color: white;
          padding: 12px 25px;
          text-decoration: none;
          border-radius: 5px;
          display: inline-block;
          font-weight: bold;
          margin: 20px 0;
        }

        .button:hover {
          background-color: #228B22;
        }

        .link-text {
          font-size: 12px;
          color: #666;
          word-break: break-all;
        }

        .footer {
          background-color: #f9f9f9;
          padding: 15px 30px;
          font-size: 12px;
          color: #666;
          border-top: 1px solid #ddd;
        }

        h1 {
          margin: 0;
          font-size: 28px;
        }

        h2 {
          color: #2E8B57;
          font-size: 20px;
          margin-bottom: 15px;
        }

        p {
          line-height: 1.6;
          margin-bottom: 15px;
        }
      </style>
    </head>

    <body>
      <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" width=""100%"" style=""background-color: #f4f4f4;"">
        <tr>
          <td align=""center"" style=""padding: 20px 0;"">
            <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" width=""600"" class=""email-container"">
              <!-- Header -->
              <tr>
                <td class=""header"">
                  <h1>SS.CC. Rumipamba</h1>
                </td>
              </tr>

              <!-- Logo Section -->
              <tr>
                <td class=""logo-section"">
                  <img src=""https://i.imgur.com/XrdnRZP.png"" alt=""Logo SS.CC. Rumipamba"" width=""150"" height=""150""
                    style=""border-radius: 10px;"">
                </td>
              </tr>

              <!-- Content -->
              <tr>
                <td class=""content"">
                  <h2>¡Te has registrado exitosamente!</h2>

                  <p>Estimado/a {{Nombre}},</p>

                  <p>Te damos la bienvenida a <strong>SS.CC. Rumipamba</strong>. Tu cuenta ha sido creada exitosamente en
                    nuestro sistema, pero necesitas activarla para poder acceder a todos nuestros servicios y recursos.</p>

                  <p>Para completar el proceso de registro y activar tu cuenta, simplemente haz clic en el botón de abajo:
                  </p>

                  <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" width=""100%"">
                    <tr>
                      <td align=""center"" style=""padding: 20px 0;"">
                        <a href=""{{URL}}"" class=""button"" style=""color: white; text-decoration: none;"">
                          ACTIVAR MI CUENTA
                        </a>
                      </td>
                    </tr>
                  </table>

                  <p><strong>Si el botón no funciona</strong>, copia y pega el siguiente enlace en tu navegador:</p>
                  <p class=""link-text"">{{URL}}</p>

                </td>
              </tr>

              <!-- Footer -->
              <tr>
                <td class=""footer"">
                  <p><strong>¿No solicitaste este registro?</strong></p>
                  <p>Si no te registraste en SS.CC. Rumipamba o no sabes por qué recibiste este correo, puedes ignorarlo sin
                    problemas. También puedes contactar con nuestra administración escribiendo a: <a
                      href=""mailto:admin@sscc-rumipamba.com"">admin@sscc-rumipamba.com</a></p>

                  <hr style=""border: none; border-top: 1px solid #ddd; margin: 15px 0;"">

                  <p style=""text-align: center; margin: 0;"">
                    <strong>SS.CC. Rumipamba</strong><br>
                    Siempre listos para servir<br>
                    © 2025 Todos los derechos reservados
                  </p>
                </td>
              </tr>
            </table>
          </td>
        </tr>
      </table>
    </body>

    </html>";
  }

  private string GetPasswordRecoveryTemplate()
  {
    return @"<!DOCTYPE html>
    <html lang=""es"">

    <head>
      <meta charset=""UTF-8"">
      <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
      <title>SS.CC. Rumipamba - Recuperar Contraseña</title>
      <style>
        body {
          margin: 0;
          padding: 0;
          font-family: Arial, sans-serif;
          background-color: #f4f4f4;
        }

        .email-container {
          width: 100%;
          max-width: 600px;
          margin: 0 auto;
          background-color: #ffffff;
        }

        .header {
          background-color: #2E8B57;
          color: white;
          text-align: center;
          padding: 20px 0;
        }

        .logo-section {
          text-align: center;
          padding: 20px 0;
        }

        .content {
          padding: 20px 30px;
        }

        .button {
          background-color: #2E8B57;
          color: white;
          padding: 12px 25px;
          text-decoration: none;
          border-radius: 5px;
          display: inline-block;
          font-weight: bold;
          margin: 20px 0;
        }

        .button:hover {
          background-color: #228B22;
        }

        .link-text {
          font-size: 12px;
          color: #666;
          word-break: break-all;
        }

        .footer {
          background-color: #f9f9f9;
          padding: 15px 30px;
          font-size: 12px;
          color: #666;
          border-top: 1px solid #ddd;
        }

        h1 {
          margin: 0;
          font-size: 28px;
        }

        h2 {
          color: #2E8B57;
          font-size: 20px;
          margin-bottom: 15px;
        }

        p {
          line-height: 1.6;
          margin-bottom: 15px;
        }
      </style>
    </head>

    <body>
      <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" width=""100%"" style=""background-color: #f4f4f4;"">
        <tr>
          <td align=""center"" style=""padding: 20px 0;"">
            <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" width=""600"" class=""email-container"">
              <!-- Header -->
              <tr>
                <td class=""header"">
                  <h1>SS.CC. Rumipamba</h1>
                </td>
              </tr>

              <!-- Logo Section -->
              <tr>
                <td class=""logo-section"">
                  <img src=""https://i.imgur.com/XrdnRZP.png"" alt=""Logo SS.CC. Rumipamba"" width=""150"" height=""150""
                    style=""border-radius: 10px;"">
                </td>
              </tr>

              <!-- Content -->
              <tr>
                <td class=""content"">
                  <h2>¡Solicitud de Recuperación de Contraseña!</h2>

                  <p>Estimado/a {{Nombre}},</p>

                  <p>Te damos la bienvenida a <strong>SS.CC. Rumipamba</strong>. Hemos recibido una solicitud de recuperación de contraseña para tu cuenta, por favor sigue los siguientes pasos:</p>

                  <p>Para completar el proceso de recuperación de contraseña, simplemente haz clic en el botón de abajo:
                  </p>

                  <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" width=""100%"">
                    <tr>
                      <td align=""center"" style=""padding: 20px 0;"">
                        <a href=""{{URL}}"" class=""button"" style=""color: white; text-decoration: none;"">
                          RECUPERAR CONTRASEÑA
                        </a>
                      </td>
                    </tr>
                  </table>

                  <p><strong>Si el botón no funciona</strong>, copia y pega el siguiente enlace en tu navegador:</p>
                  <p class=""link-text"">{{URL}}</p>

                </td>
              </tr>

              <!-- Footer -->
              <tr>
                <td class=""footer"">
                  <p><strong>¿No solicitaste esto?</strong></p>
                  <p>Si no solicitaste una recuperación de contraseña en SS.CC. Rumipamba o no sabes por qué recibiste este correo, puedes ignorarlo sin
                    problemas. También puedes contactar con nuestra administración escribiendo a: <a
                      href=""mailto:admin@sscc-rumipamba.com"">admin@sscc-rumipamba.com</a></p>

                  <hr style=""border: none; border-top: 1px solid #ddd; margin: 15px 0;"">

                  <p style=""text-align: center; margin: 0;"">
                    <strong>SS.CC. Rumipamba</strong><br>
                    Siempre listos para servir<br>
                    © 2025 Todos los derechos reservados
                  </p>
                </td>
              </tr>
            </table>
          </td>
        </tr>
      </table>
    </body>

    </html>";
  }

  // Mantener el método original para compatibilidad si es necesario
  public async Task SendMail(string para, string asunto, string htmlCuerpo)
  {
    var mensaje = new MimeMessage();

    mensaje.From.Add(new MailboxAddress("SS.CC. Rumipamba", _config["Smtp:From"]));
    mensaje.To.Add(MailboxAddress.Parse(para));
    mensaje.Subject = asunto;

    var builder = new BodyBuilder
    {
      HtmlBody = htmlCuerpo
    };

    mensaje.Body = builder.ToMessageBody();

    using var smtp = new SmtpClient();

    await smtp.ConnectAsync(_config["Smtp:Host"], int.Parse(_config["Smtp:Port"]!), SecureSocketOptions.StartTls);
    await smtp.AuthenticateAsync(_config["Smtp:User"], _config["Smtp:Pass"]);
    await smtp.SendAsync(mensaje);
    await smtp.DisconnectAsync(true);
  }
}