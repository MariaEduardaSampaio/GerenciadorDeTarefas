﻿using Application.Requests;
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
            _taskService = new TaskService(taskRepository, userRepository);
            usuarioLogado = null;
        }

        static void Main()
        {

            Console.Clear();
            Console.WriteLine("\t****** Gerenciador de Tarefas ******\n");
            MenuPrincipal();
        }

        private static void MenuPrincipal()
        {
            int opcao;
            bool conversaoValida;

            do
            {
                do
                {
                    Console.WriteLine("1. Fazer o login\n2. Fazer logout\n3. Criar um usuário\n4. Sair do programa");
                    Console.Write("Opção: ");
                    conversaoValida = int.TryParse(Console.ReadLine(), out opcao);
                } while (!conversaoValida);

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

                    case 4:
                        Console.WriteLine("Saindo do gerenciador de tarefas...");
                        break;

                    default:
                        Console.WriteLine("Entrada inválida! Tente novamente.\n");
                        break;
                }
            } while (opcao != 4);
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
                string senha = LerSenha();

                GetUserResponse? userResponse = _userService.GetUserByEmailAndPassword(email, senha);
                if (userResponse != null)
                {
                    usuarioLogado = userResponse;
                    Console.WriteLine("Usuário logado com sucesso!");

                    Console.WriteLine($"\nPressione qualquer tecla para prosseguir para o Menu do {usuarioLogado.Role}...");
                    Console.ReadKey();

                    if (usuarioLogado.Role == Role.TechLeader)
                        MenuTechLeader();
                    else
                        MenuDeveloper();
                    break;
                }
                else
                {
                    Console.WriteLine("Email ou senha incorretos.");
                    Console.WriteLine("Deseja tentar novamente (1) ou voltar ao menu inicial (2)?");
                    do
                    {
                        Console.Write("Opção: ");
                        opcao = PegarInteiroValido();
                    } while (opcao < 1 || opcao > 2);

                    if (opcao == 2)
                        break;
                }
            } while (opcao == 1);
        }

        private static string LerSenha()
        {
            string senha = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                if (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Backspace)
                {
                    senha += key.KeyChar;
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && senha.Length > 0)
                {
                    senha = senha.Substring(0, senha.Length - 1);
                    Console.Write("\b \b");
                }
            } while (key.Key != ConsoleKey.Enter);

            Console.WriteLine();
            return senha;
        }

        private static void FazerLogout()
        {
            if (usuarioLogado == null)
                Console.WriteLine("Não há usuário logado para deslogar.\n");
            else
            {
                Console.Clear();
                usuarioLogado = null;
                Console.WriteLine("Usuário deslogado com sucesso!\n");
            }
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
            Console.ReadLine();
            MenuPrincipal();
        }

        private static void MenuTechLeader()
        {
            int opcao;

            do
            {
                Console.Clear();
                Console.WriteLine($"\t*** Menu Tech Leader ***");
                Console.WriteLine($"Bem vindo, {usuarioLogado!.Name}!");
                ListarOpcoesTechLeader();
                Console.Write("Opção: ");
                opcao = PegarInteiroValido();

                switch (opcao)
                {
                    case 1:
                        VerTarefasDoSistema();
                        break;

                    case 2:
                        VerTarefasPorResponsavel();
                        break;

                    case 3:
                        VerTarefasPorResponsavelOuPorObjetivo();
                        break;

                    case 4:
                        VerTarefasPorObjetivo();
                        break;

                    case 5:
                        VerEstatisticasTarefas();
                        break;

                    case 6:
                        CriarTarefaPorTechLeader();
                        break;

                    case 7:
                        AssumirTarefa();
                        break;

                    case 8:
                        MudarStatusDeTarefa();
                        break;

                    case 9:
                        MudarResponsavelPorTarefa();
                        break;

                    case 10:
                        MudarPrazoFinalDeTarefa();
                        break;

                    case 11:
                        FazerLogout();
                        break;

                    default:
                        ListarOpcoesTechLeader();
                        break;
                }
                Console.WriteLine($"\nPressione qualquer tecla para voltar para o menu...");
                Console.ReadKey();
            } while (opcao != 11);
        }

        private static void ListarOpcoesTechLeader()
        {
            Console.WriteLine("\n1. Ver todas as tarefas do sistema.");
            Console.WriteLine("2. Ver suas tarefas.");
            Console.WriteLine("3. Ver suas tarefas e tarefas relacionadas.");
            Console.WriteLine("4. Ver tarefas por objetivo.");
            Console.WriteLine("5. Ver estatísticas de tarefas.");
            Console.WriteLine("6. Criar tarefa.");
            Console.WriteLine("7. Assumir uma tarefa.");
            Console.WriteLine("8. Mudar status de tarefa.");
            Console.WriteLine("9. Mudar responsável de tarefa.");
            Console.WriteLine("10. Mudar prazo final de tarefa.");
            Console.WriteLine("11. Fazer logout.\n");
        }

        private static void VerTarefasDoSistema()
        {
            Console.Clear();
            Console.WriteLine("\t* Tarefas do Sistema *");
            _taskService.GetAllTasks().ForEach(ImprimirTarefa).;
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
            try
            {
                Console.Clear();
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

                var task = new TaskRequest()
                {
                    EmailResponsable = email,
                    EndDate = prazoFinal,
                    Objective = objetivo,
                    Description = descricao
                };

                _taskService.CreateTask(task);
                Console.WriteLine("Tarefa criada com sucesso!\n");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro ao criar uma tarefa: {ex}");
            }
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
                    Id = task.Id,
                    EmailResponsable = usuarioLogado!.Email,
                    Status = task.Status,
                    EndDate = task.EndDate,
                    Objective = task.Objective,
                    Description = task.Description,
                };

                _taskService.UpdateTask(updateTaskRequest);

                Console.WriteLine("Tarefa atualizada com sucesso!\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro ao tentar assumir a tarefa: {ex}");
            }
        }

        private static void MudarStatusDeTarefa()
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

                if (status == TaskStatusEnum.Concluded)
                    task.EndDate = DateTime.Now;

                var updateTaskRequest = new UpdateTaskRequest()
                {
                    Id = task.Id,
                    EmailResponsable = task.EmailResponsable,
                    Status = status,
                    EndDate = task.EndDate,
                    Objective = task.Objective,
                    Description = task.Description,
                };

                _taskService.UpdateTask(updateTaskRequest);
                Console.WriteLine($"Status da tarefa {task.Id} atualizado para {status} com sucesso!\n");
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
                Console.Clear();
                Console.WriteLine("\t* Atualizar Responsável pela Tarefa *");
                VerTarefasDoSistema();

                Console.Write("ID da tarefa: ");
                GetTaskResponse task = PegarTaskValida();

                Console.Write("Email do novo responsável: ");
                string email = PegarEmailValido();

                var updateTaskRequest = new UpdateTaskRequest()
                {
                    Id = task.Id,
                    EmailResponsable = email,
                    Status = task.Status,
                    EndDate = task.EndDate,
                    Objective = task.Objective,
                    Description = task.Description,
                };

                _taskService.UpdateTask(updateTaskRequest);
                Console.WriteLine($"Responsável da tarefa {task.Id} atualizado para {email} com sucesso!\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro ao mudar responsável pela tarefa: {ex}");
            }
        }

        private static void MudarPrazoFinalDeTarefa()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("\t* Atualizar prazo final de Tarefa *");
                VerTarefasDoSistema();

                Console.Write("ID da tarefa: ");
                GetTaskResponse task = PegarTaskValida();

                Console.Write("Novo prazo: ");
                DateTime prazoFinal = PegarDataValida();

                var updateTaskRequest = new UpdateTaskRequest()
                {
                    Id = task.Id,
                    EmailResponsable = task.EmailResponsable,
                    Status = task.Status,
                    EndDate = prazoFinal,
                    Objective = task.Objective,
                    Description = task.Description,
                };

                _taskService.UpdateTask(updateTaskRequest);
                Console.WriteLine($"Prazo final da tarefa {task.Id} atualizado para {prazoFinal} com sucesso!\n");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro ao mudar o prazo final de tarefa: {ex}");
            }
        }

        private static void MenuDeveloper()
        {
            int opcao;

            do
            {
                Console.Clear();
                Console.WriteLine($"\t*** Menu Desenvolvedor ***");
                Console.WriteLine($"Bem vindo, {usuarioLogado!.Name}!");
                ListarOpcoesDeveloper();
                Console.Write("Opção: ");
                opcao = PegarInteiroValido();

                switch (opcao)
                {
                    case 1:
                        CriarTarefaPorDeveloper();
                        break;

                    case 2:
                        VerTarefasPorResponsavel();
                        break;

                    case 3:
                        VerTarefasPorResponsavelOuPorObjetivo();
                        break;

                    case 4:
                        FazerLogout();
                        break;

                    default:
                        ListarOpcoesDeveloper();
                        break;
                }

                Console.WriteLine($"\nPressione qualquer tecla para voltar para o menu...\n");
                Console.ReadKey();

            } while (opcao != 4);
        }

        private static void ListarOpcoesDeveloper()
        {
            Console.WriteLine("\n1. Criar tarefa.");
            Console.WriteLine("2. Ver suas tarefas.");
            Console.WriteLine("3. Ver suas tarefas e tarefas relacionadas.");
            Console.WriteLine("4. Fazer Logout.");
        }

        private static void CriarTarefaPorDeveloper()
        {
            try
            {
                string objetivo, descricao;

                Console.WriteLine("\t* Criar Tarefa *");

                Console.Write("Objetivo: ");
                objetivo = Console.ReadLine()!;

                Console.Write("Descrição: ");
                descricao = Console.ReadLine()!;

                var task = new TaskRequest()
                {
                    EmailResponsable = usuarioLogado!.Email,
                    Objective = objetivo,
                    Description = descricao
                };

                _taskService.CreateTask(task);
                Console.WriteLine("Tarefa criada com sucesso!\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro ao criar uma tarefa: {ex}");
            }
        }

        private static void VerTarefasPorResponsavel()
        {
            Console.WriteLine("\t*Tarefas do usuário *");
            _taskService.GetAllTasksByEmail(usuarioLogado!.Email!).ForEach(ImprimirTarefa);
        }

        private static void VerTarefasPorObjetivo()
        {
            Console.WriteLine("\t*Tarefas por objetivo *");
            Console.WriteLine("Qual o objetivo que deseja filtrar as tarefas?");
            Console.Write("Objetivo: ");
            string objetivo = Console.ReadLine()!;
            _taskService.GetAllTasksByObjective(objetivo).ForEach(ImprimirTarefa);
        }

        private static void VerTarefasPorResponsavelOuPorObjetivo()
        {
            Console.WriteLine("\t*Tarefas do usuário *");
            List<GetTaskResponse> tarefasDoUsuario = _taskService.GetAllTasksByEmail(usuarioLogado!.Email!);
            List<GetTaskResponse> tarefasRelacionadas = new();

            foreach (var tarefa in tarefasDoUsuario)
            {
                tarefasRelacionadas.AddRange(_taskService.GetAllTasksByObjective(tarefa.Objective));
            }

            tarefasDoUsuario.AddRange(tarefasRelacionadas);
            List<GetTaskResponse> tarefasPorResponsavelOuObjetivo = tarefasDoUsuario.DistinctBy(tarefa => tarefa.Id).OrderBy(tarefa => tarefa.Id).ToList();
            tarefasPorResponsavelOuObjetivo.ForEach(ImprimirTarefa);
        }



        private static void ImprimirPossiveisStatusTarefa()
        {
            Console.WriteLine("Possíveis Status de uma Tarefa:");
            Console.WriteLine("Abandoned, Prevented, UnderAnalysis, Initialized, Concluded, Late.");
        }

        private static void ImprimirTarefa(GetTaskResponse task)
        {
            Console.WriteLine($"\n--------------------------------------------------------------------------------------");
            Console.WriteLine($"ID: {task.Id}");
            Console.WriteLine($"--------------------------------------------------------------------------------------");
            Console.WriteLine($"Objetivo: {task.Objective}");
            Console.WriteLine($"--------------------------------------------------------------------------------------");
            Console.WriteLine($"Status: {task.Status}");
            Console.WriteLine($"--------------------------------------------------------------------------------------");
            Console.WriteLine($"Data de criação: {task.CreatedDate}");
            Console.WriteLine($"Prazo final: {task.EndDate}");
            Console.WriteLine($"Responsável: {task.EmailResponsable}");
            Console.WriteLine($"--------------------------------------------------------------------------------------");
            Console.WriteLine($"Descrição: {task.Description}");
            Console.WriteLine($"--------------------------------------------------------------------------------------\n\n");
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
            conversaoValida = DateTime.TryParse(Console.ReadLine(), out data);
            if (!conversaoValida)
            {
                do
                {
                    Console.WriteLine("Entre com uma data válida: ");
                    conversaoValida = DateTime.TryParse(Console.ReadLine(), out data);
                } while (!conversaoValida);
            }
            return data;
        }

        private static string PegarEmailValido()
        {
            string email;
            email = Console.ReadLine()!;

            if (!VerificarSeEmailExiste(email))
            {
                do
                {
                    Console.Write("Entre com um email válido: ");
                    email = Console.ReadLine()!;
                } while (!VerificarSeEmailExiste(email));
            }
            return email;
        }

        private static GetTaskResponse PegarTaskValida()
        {
            GetTaskResponse? task;
            int id;
            id = PegarInteiroValido();
            task = _taskService.GetTaskByID(id);

            if (task == null)
            {
                do
                {
                    Console.Write($"Não existe tarefa com ID {id}. Tente novamente: ");
                    id = PegarInteiroValido();
                    task = _taskService.GetTaskByID(id);
                } while (task == null);
            }
            return task;
        }
    }
}
