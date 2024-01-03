using Application.Usuarios;
using Domain.AggregateObjects;
using Domain.Interfaces;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ManagerService
    {
        private readonly ITaskManagerRepository _taskManagerRepository;
        public List<Usuario> Usuarios { get; private set; }
        public List<Tasks> Tarefas { get; private set; }
        public Usuario UsuarioLogado { get; private set; } = null;

        public ManagerService(ITaskManagerRepository taskManagerRepository)
        {
            Usuarios = new List<Usuario>();
            Tarefas = new List<Tasks>();
            _taskManagerRepository = taskManagerRepository;
        }

        public bool EmailNaoExiste(string email)
        {
            return Usuarios.All(usuario => usuario.Email != email);
        }

        public void ValidarCadastroEmail(string email)
        {
            if (!EmailNaoExiste(email))
                throw new ArgumentException("Já existe um usuário cadastrado com este email.");
        }

        public void CadastrarDesenvolvedor(string nome, string email, string senha)
        {
            ValidarCadastroEmail(email);
            Senha senhaUsuario = new Senha(senha);
            Desenvolvedor usuario = new Desenvolvedor(nome, email, senhaUsuario);
            Usuarios.Add(usuario);
        }

        public void CadastrarTechLeader(string nome, string email, string senha)
        {
            ValidarCadastroEmail(email);
            Senha senhaUsuario = new Senha(senha);
            TechLeader usuario = new TechLeader(nome, email, senhaUsuario);
            Usuarios.Add(usuario);
        }

        public void ListarUsuarios()
        {
            foreach (var usuario in Usuarios)
            {
                usuario.ListarInformacoes();
            }
        }

        public void AutenticarUsuario(string email, string senha)
        {
            Usuario usuario = Usuarios.FirstOrDefault(usuario => usuario.Email == email);
            if (UsuarioEstaLogado())
                throw new ArgumentException("Usuário já está logado.");
            if (usuario == null || usuario.senha.senha != senha)
                throw new ArgumentException("Email ou senha incorretos.");

            UsuarioLogado = usuario;
            Console.WriteLine("Usuário logado com sucesso!");
        }

        public void Logout()
        {
            if (!UsuarioEstaLogado())
                throw new AccessViolationException("Não há nenhum login ativo no momento.");

            UsuarioLogado = null;
            Console.WriteLine("Usuário deslogado com sucesso!");
        }
        public void ListarTarefas()
        {
            if (!UsuarioEstaLogado())
                throw new AccessViolationException("É preciso estar logado para poder ver as Tarefas do sistema.");
            else if (UsuarioLogado.TipoDeAcesso == AcessoAoSistema.TOTAL)
            {
                foreach (var tarefa in Tarefas)
                    tarefa.ImprimirTarefa();
            }
            else if (UsuarioLogado.TipoDeAcesso == AcessoAoSistema.PARCIAL)
            {
                /*
                List<Tasks> tarefasPorResponsavel = Tarefas.Where(tarefa => tarefa.EmailDoResponsavel == UsuarioLogado.Email).ToList();
                foreach (var tarefaPorResponsavel in tarefasPorResponsavel)
                    tarefasPorObjetivo.AddRange(Tarefas.Where(tarefa => tarefa.Objective == tarefaPorResponsavel.Objective));
                tarefasDisponiveis.AddRange(tarefasPorResponsavel);
                 
                 */
                List<Tasks> tarefasPorObjetivo = new();
                List<Tasks> tarefasDisponiveis = new();


                tarefasDisponiveis.AddRange(tarefasPorObjetivo);
                tarefasDisponiveis = tarefasDisponiveis.Distinct().ToList();

                foreach (var tarefa in Tarefas)
                    tarefa.ImprimirTarefa();
            }
        }

        public bool UsuarioEstaLogado()
        {
            if (UsuarioLogado == null)
                return false;
            return true;
        }

        public static bool StatusTarefaExiste(int statusEscolhido)
        {
            return Enum.IsDefined(typeof(StatusTarefa), statusEscolhido);
        }

        public Tasks EscolherTarefa()
        {
            Tasks tarefaEscolhida;
            int id;
            ListarTarefas();
            do
            {
                Console.WriteLine("Entre com o ID da tarefa: ");
                id = int.Parse(Console.ReadLine());
                tarefaEscolhida = Tarefas.FirstOrDefault(tarefa => tarefa.Id == id);
            } while (tarefaEscolhida == null);

            return tarefaEscolhida;
        }

        public void AlterarEstadoTarefa(StatusTarefa statusTarefa, Tasks tarefa)
        {
            switch (statusTarefa)
            {
                case StatusTarefa.ABANDONADA:
                    tarefa.AbandonarTarefa();
                    break;

                case StatusTarefa.IMPEDIDA:
                    tarefa.ImpedirTarefa();
                    break;

                case StatusTarefa.EM_ANALISE:
                    tarefa.AnalisarTarefa();
                    break;

                case StatusTarefa.INICIADA:
                    tarefa.IniciarTarefa();
                    break;

                case StatusTarefa.CONCLUIDA:
                    tarefa.ConcluirTarefa();
                    break;

                case StatusTarefa.ATRASADA:
                    tarefa.AtrasarTarefa();
                    break;

                default:
                    throw new ArgumentException("Não existe este status de tarefa.");
            }
        }

        public void ReceberEstadoTarefa()
        {
            if (UsuarioLogado == null)
                throw new AccessViolationException("Usuário deve estar logado para poder alterar estado de uma tarefa.");

            if (UsuarioLogado.TipoDeAcesso == AcessoAoSistema.PARCIAL)
                throw new AccessViolationException("Apenas o Tech Leader pode alterar estado da tarefa.");

            Tasks tarefaEscolhida = EscolherTarefa();
            int statusEscolhido;
            Tasks.ListarStatusTarefa();
            do
            {
                Console.WriteLine($"Entre com o novo status da tarefa {tarefaEscolhida.Id}");
                statusEscolhido = int.Parse(Console.ReadLine());
            } while (!StatusTarefaExiste(statusEscolhido));

            AlterarEstadoTarefa((StatusTarefa)statusEscolhido, tarefaEscolhida);
        }

        public int CriarTarefa()
        {
            string objetivo, descricao, email = "";

            if (UsuarioLogado == null)
                throw new AccessViolationException("É preciso estar logado para criar uma tarefa.");

            Console.WriteLine("Entre com o objetivo da tarefa: ");
            objetivo = Console.ReadLine();
            Console.WriteLine("Entre com a descrição da tarefa: ");
            descricao = Console.ReadLine();

            if (UsuarioLogado.TipoDeAcesso == AcessoAoSistema.PARCIAL)
                email = UsuarioLogado.Email;
            else if (UsuarioLogado.TipoDeAcesso == AcessoAoSistema.TOTAL)
            {
                ListarUsuarios();
                do
                {
                    Console.WriteLine("Entre com o email do responsável pela tarefa: ");
                    email = Console.ReadLine();
                } while (EmailNaoExiste(email));
            }
            var task = new Tasks(objetivo, descricao);

            Tarefas.Add(task);
            UsuarioLogado.CriarTarefa(email, objetivo, descricao);

            var taskModel = new TaskModel()
            {
                Description = task.Description,
                EmailResponsable = task.EmailResponsable,
                CreatedDate = task.CreatedDate,
                EndDate = task.EndDate,
                Objective = task.Objective,
                Status = (int)task.Status
            };
            int id = _taskManagerRepository.CreateTask(taskModel);
            return id;
        }

        public void EstatisticasTarefas() { }
    }
}
