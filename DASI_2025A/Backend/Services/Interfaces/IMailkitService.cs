using Shared;

namespace Backend;

public interface IMailkitService
{
  Task<bool> SendActivationMail(string para, string nombre, string codigo);
  Task<bool> SendPasswordRecoveryMail(string para, string nombre, string codigo);
  Task SendMail(string para, string asunto, string htmlCuerpo);
}