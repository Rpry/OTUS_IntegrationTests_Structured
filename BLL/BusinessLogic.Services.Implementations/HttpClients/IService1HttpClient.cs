using System.Threading.Tasks;

namespace BusinessLogic.Services.HttpClients;

public interface IService1HttpClient
{
    /// <summary>
    /// Сохранить параметры ответа от Конфигуратора.
    /// </summary>
    public Task SendRequestAsync();
}