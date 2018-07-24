using System.Threading.Tasks;

namespace DiscordBot {

    public interface ICommand
    {
        Task<string> Execute();
    }

}