using Application.Requests;
using Application.Requests.Enums;
using Application.Requests.ValueObjects;
using Application.Services;
using Infrastructure.Context;
using Infrastructure.Repository;

namespace Application
{
    public class Program
    {
        private static IUserService _userService;
        private static ITaskService _taskService;
        private static GetUserResponse? usuarioLogado;
        static Program()
        {
            var dbContext = new TaskManagerContext();
            var userRepository = new UserRepository(dbContext);
            var taskRepository = new TasksRepository(dbContext);

            _userService = new UserService(userRepository);
            _taskService = new TaskService(taskRepository);
            usuarioLogado = null;
        }

        private static void FazerLogin(string email, string senha)
        {
            Console.Clear();
            Console.WriteLine("\t*** Login de usuário ***");

            while (usuarioLogado == null)
            {
                GetUserResponse? userResponse = _userService.GetUserByEmailAndPassword(email, senha);
                if (userResponse != null)
                {
                    usuarioLogado = userResponse;
                    Console.WriteLine("Usuário logado com sucesso!");
                    MenuUsuario(usuarioLogado);
                }
                else
                    Console.WriteLine("Email ou senha incorretos. Tente novamente.");
            }
        }

        private static void FazerLogout()
        {
            if (usuarioLogado == null)
                Console.WriteLine("Não há usuário logado para deslogar.");
            else
            {
                Console.Clear();
                usuarioLogado = null;
                Console.WriteLine("Usuário deslogado com sucesso!");
            }
        }

        private static void CriarUsuario()
        {
            Role role;
            bool conversaoValida;

            Console.Clear();
            Console.WriteLine("\t*** Cadastro de usuário ***");
            Console.Write("Nome: ");
            string name = Console.ReadLine();
            Console.Write("Email: ");
            string email = Console.ReadLine();
            Console.Write("Senha: ");
            string senha = Console.ReadLine();

            do
            {
                Console.WriteLine("Obs.: Cargos válidos para usuário: Developer ou TechLeader.");
                Console.Write("Cargo:");
                conversaoValida = Enum.TryParse<Role>(Console.ReadLine(), out role);
            } while (!conversaoValida);

            var usuario = new UserRequest()
            {
                Name = name,
                Email = email,
                Password = new Password(senha),
                Role = role
            };

            _userService.CreateUser(usuario);

            Console.WriteLine("Usuário criado com sucesso!");
        }

        private static void MenuUsuario(GetUserResponse usuarioLogado)
        {

        }

        static void Main(string[] args)
        {
            do
            {
                int opcao;
                bool conversaoValida;

                Console.Clear();
                Console.WriteLine("\t****** Gerenciador de Tarefas ******\n");

                do
                {
                    Console.WriteLine("Deseja fazer o login (1), logout (2) ou criar um usuário (3)?");
                    conversaoValida = int.TryParse(Console.ReadLine(), out opcao);
                } while (!conversaoValida || opcao < 1 || opcao > 3);

                switch (opcao)
                {
                    case 1:
                        Console.Write("Email: ");
                        string? email = Console.ReadLine();

                        Console.Write("Senha: ");
                        string? senha = Console.ReadLine();
                        FazerLogin(email, senha);
                        break;

                    case 2:
                        FazerLogout();
                        break;

                    case 3:
                        CriarUsuario();
                        break;

                    default:
                        Console.WriteLine("Entrada inválida!");
                        break;
                }

            } while (true);
        }
    }
}
