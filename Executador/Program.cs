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

        static void Main()
        {
            do
            {
                Console.Clear();
                Console.WriteLine("\t****** Gerenciador de Tarefas ******\n");
                MenuPrincipal();
            } while (true);
        }

        private static void MenuPrincipal()
        {
            int opcao;
            bool conversaoValida;

            do
            {
                Console.WriteLine("Deseja fazer o login (1), logout (2) ou criar um usuário (3)?");
                conversaoValida = int.TryParse(Console.ReadLine(), out opcao);
            } while (!conversaoValida || opcao < 1 || opcao > 3);

            switch (opcao)
            {
                case 1:
                    FazerLogin();
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
        }

        private static void FazerLogin()
        {
            Console.Clear();
            Console.WriteLine("\t*** Login de usuário ***");

            int opcao;
            do
            {
                Console.Write("Email: ");
                string email = Console.ReadLine()!;

                Console.Write("Senha: ");
                string senha = Console.ReadLine()!;

                GetUserResponse? userResponse = _userService.GetUserByEmailAndPassword(email, senha);
                if (userResponse != null)
                {
                    usuarioLogado = userResponse;
                    Console.WriteLine("Usuário logado com sucesso!");
                    if (usuarioLogado.Role == Role.TechLeader)
                        MenuTechLeader();
                    else
                        MenuDeveloper();
                    break;
                }
                else
                {
                    Console.WriteLine("Email ou senha incorretos.");
                    Console.WriteLine("Deseja tentar novamente ou voltar ao menu inicial?\n" +
                        "Digite 1 para tentar novamente e qualquer outra para voltar ao menu.");
                    opcao = PegarInteiroValido();

                    if (opcao != 1)
                        break;
                }
            } while (opcao == 1);
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
            MenuPrincipal();
        }

        private static void CriarUsuario()
        {
            Role role;
            bool conversaoValida;

            Console.Clear();
            Console.WriteLine("\t*** Cadastro de usuário ***");
            Console.Write("Nome: ");
            string name = Console.ReadLine()!;
            Console.Write("Email: ");
            string email = Console.ReadLine()!;
            Console.Write("Senha: ");
            string senha = Console.ReadLine()!;

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
            MenuPrincipal();
        }

        private static void MenuTechLeader()
        {
            int opcao;

            do
            {
                Console.Clear();
                Console.WriteLine($"\t*** Menu Tech Leader ***");
                Console.WriteLine($"Bem vindo, {usuarioLogado.Name}!");
                ListarOpcoesTechLeader();
                opcao = PegarInteiroValido();

                switch (opcao)
                {
                    case 1:
                        VerTarefasDoSistema();
                        break;

                    case 2:
                        VerEstatisticasTarefas();
                        break;

                    case 3:
                        CriarTarefaPorTechLeader();
                        break;

                    case 4:
                        AssumirTarefa();
                        break;

                    case 5:
                        MudarStatusTarefa();
                        break;

                    case 6:
                        MudarResponsavelPorTarefa();
                        break;

                    case 7:
                        FazerLogout();
                        break;

                    default:
                        ListarOpcoesTechLeader();
                        break;
                }

            } while (opcao != 7);
        }

        private static void ListarOpcoesTechLeader()
        {
            Console.WriteLine("\n1. Ver todas as tarefas do sistema.");
            Console.WriteLine("2. Ver estatísticas de tarefas.");
            Console.WriteLine("3. Criar tarefa.");
            Console.WriteLine("4. Assumir uma tarefa.");
            Console.WriteLine("5. Mudar status de tarefas.");
            Console.WriteLine("6. Mudar responsável de tarefa.");
            Console.WriteLine("7. Fazer logout.\n");
        }

        private static void VerTarefasDoSistema()
        {
            Console.WriteLine("\t* Tarefas do Sistema *");
            _taskService.GetAllTasks().ForEach(task => ImprimirTarefa(task));
        }

        private static void VerEstatisticasTarefas()
        {
            Console.WriteLine("\t* Estatísticas gerais *");
            int tarefasEmAnalise = _taskService.GetAllTasks().Count(task => task.Status == TaskStatusEnum.UnderAnalysis);
            int tarefasInicializadas = _taskService.GetAllTasks().Count(task => task.Status == TaskStatusEnum.Initialized);
            int tarefasImpedidas = _taskService.GetAllTasks().Count(task => task.Status == TaskStatusEnum.Prevented);
            int tarefasConcluidas = _taskService.GetAllTasks().Count(task => task.Status == TaskStatusEnum.Concluded);
            int tarefasAtrasadas = _taskService.GetAllTasks().Count(task => task.Status == TaskStatusEnum.Late);
            int tarefasAbandonadas = _taskService.GetAllTasks().Count(task => task.Status == TaskStatusEnum.Abandoned);

            Console.WriteLine($"Tarefas em análise: {tarefasEmAnalise}");
            Console.WriteLine($"Tarefas inicializadas: {tarefasInicializadas}");
            Console.WriteLine($"Tarefas impedidas: {tarefasImpedidas}");
            Console.WriteLine($"Tarefas concluídas: {tarefasConcluidas}");
            Console.WriteLine($"Tarefas atrasadas: {tarefasAtrasadas}");
            Console.WriteLine($"Tarefas abandonadas: {tarefasAbandonadas}");
        }

        private static void CriarTarefaPorTechLeader()
        {
            DateTime prazoFinal;
            string email, objetivo, descricao;

            Console.WriteLine("\t*Criar Tarefa *");

            Console.Write("Email do responsável: ");
            email = PegarEmailValido();

            Console.Write("Prazo final: ");
            prazoFinal = PegarDataValida();

            Console.Write("Objetivo: ");
            objetivo = Console.ReadLine()!;

            Console.Write("Descrição: ");
            descricao = Console.ReadLine()!;

            _taskService.CreateTask(new TaskRequest());
        }

        private static void AssumirTarefa()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("\t* Assumir Tarefa *");
                VerTarefasDoSistema();
                Console.WriteLine("Qual o ID da tarefa que deseja assumir?");
                GetTaskResponse task = PegarTaskValida();

                var updateTaskRequest = new UpdateTaskRequest()
                {
                    EmailResponsable = usuarioLogado.Email,
                    Status = task.Status,
                    EndDate = task.EndDate,
                    Objective = task.Objective,
                    Description = task.Description,
                };

                _taskService.UpdateTask(updateTaskRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro ao tentar assumir a tarefa: {ex}");
            }
        }

        private static void MudarStatusTarefa()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("\t* Atualizar Status de Tarefa *");
                VerTarefasDoSistema();

                Console.WriteLine("Qual o ID da tarefa que deseja atualizar o status?");
                GetTaskResponse task = PegarTaskValida();

                Console.WriteLine("Qual o novo status da tarefa?");
                ImprimirPossiveisStatusTarefa();
                TaskStatusEnum status = PegarStatusTarefaValido();

                var updateTaskRequest = new UpdateTaskRequest()
                {
                    EmailResponsable = task.EmailResponsable,
                    Status = status,
                    EndDate = task.EndDate,
                    Objective = task.Objective,
                    Description = task.Description,
                };

                _taskService.UpdateTask(updateTaskRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro ao tentar mudar status da tarefa: {ex}");
            }
        }

        private static void MudarResponsavelPorTarefa()
        {
            try
            {
                Console.WriteLine("\t* Atualizar Responsável pela Tarefa *");
                VerTarefasDoSistema();

                Console.WriteLine("Qual o ID da tarefa que deseja atualizar o responsável?");
                GetTaskResponse task = PegarTaskValida();

                Console.WriteLine("Qual o email do novo responsável pela tarefa?");
                string email = PegarEmailValido();

                var updateTaskRequest = new UpdateTaskRequest()
                {
                    EmailResponsable = email,
                    Status = task.Status,
                    EndDate = task.EndDate,
                    Objective = task.Objective,
                    Description = task.Description,
                };

                _taskService.UpdateTask(updateTaskRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro ao mudar responsável pela tarefa: {ex}");
            }
        }

        private static void MenuDeveloper()
        {

        }


        private static void ImprimirPossiveisStatusTarefa()
        {
            Console.WriteLine("Possíveis Status de uma Tarefa:");
            Console.WriteLine("Abandoned, Prevented, UnderAnalysis, Initialized, Concluded, Late.");
        }

        private static void ImprimirTarefa(GetTaskResponse task)
        {
            Console.WriteLine($"ID: {task.Id}");
            Console.WriteLine($"***************************************");
            Console.WriteLine($"Objetivo: {task.Objective}");
            Console.WriteLine($"***************************************");
            Console.WriteLine($"Status:{task.Status}");
            Console.WriteLine($"***************************************");
            Console.WriteLine($"Data de criação: {task.CreatedDate}");
            Console.WriteLine($"Prazo final:{task.EndDate}");
            Console.WriteLine($"Responsável: {task.EmailResponsable}");
            Console.WriteLine($"***************************************");
            Console.WriteLine($"Descrição: {task.Description}\n");
        }

        private static bool VerificarSeEmailExiste(string email)
        {
            GetUserResponse? user = _userService.GetUserByEmail(email);
            return user != null;
        }

        private static TaskStatusEnum PegarStatusTarefaValido()
        {
            TaskStatusEnum status;
            bool taskStatusValida;

            do
            {
                taskStatusValida = Enum.TryParse<TaskStatusEnum>(Console.ReadLine(), out status);

            } while (!taskStatusValida);
            return status;
        }

        private static int PegarInteiroValido()
        {
            int opcao;
            bool conversaoValida;
            do
            {
                conversaoValida = int.TryParse(Console.ReadLine(), out opcao);
            } while (!conversaoValida);
            return opcao;
        }

        private static DateTime PegarDataValida()
        {
            DateTime data;
            bool conversaoValida;
            do
            {
                conversaoValida = DateTime.TryParse(Console.ReadLine(), out data);
            } while (!conversaoValida);
            return data;
        }

        private static string PegarEmailValido()
        {
            string email;
            do
            {
                email = Console.ReadLine()!;
            } while (!VerificarSeEmailExiste(email));
            return email;
        }

        private static GetTaskResponse PegarTaskValida()
        {
            GetTaskResponse? task;
            int id;

            do
            {
                id = PegarInteiroValido();
                task = _taskService.GetTaskByID(id);
            } while (task == null);
            return task;
        }
    }
}
