using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.Tarefas;
using ClassLibrary.Usuarios;

namespace Services
{
    public class GerenciadorService
    {
        public List<Usuario> usuarios { get; private set; }
        public List<Tarefa> tarefas { get; private set; }
        public Usuario usuarioLogado { get; private set; } = null;

        public GerenciadorService()
        {
            usuarios = new List<Usuario>();
            tarefas = new List<Tarefa>();
        }

        public bool EmailNaoExiste(string email)
        {
            return usuarios.All(usuario => usuario.Email != email);
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
            usuarios.Add(usuario);
        }

        public void CadastrarTechLeader(string nome, string email, string senha)
        {
            ValidarCadastroEmail(email);
            Senha senhaUsuario = new Senha(senha);
            TechLeader usuario = new TechLeader(nome, email, senhaUsuario);
            usuarios.Add(usuario);
        }

        public void ListarUsuarios()
        {
            foreach (var usuario in usuarios)
            {
                usuario.ListarInformacoes();
            }
        }

        public void AutenticarUsuario(string email, string senha)
        {
            Usuario usuario = usuarios.FirstOrDefault(usuario => usuario.Email == email);
            if (UsuarioEstaLogado())
                throw new ArgumentException("Usuário já está logado.");
            if (usuario == null || usuario.senha.senha != senha)
                throw new ArgumentException("Email ou senha incorretos.");

            usuarioLogado = usuario;
            Console.WriteLine("Usuário logado com sucesso!");
        }

        public void Logout()
        {
            if (!UsuarioEstaLogado())
                throw new AccessViolationException("Não há nenhum login ativo no momento.");

            usuarioLogado = null;
            Console.WriteLine("Usuário deslogado com sucesso!");
        }
        public void ListarTarefas()
        {
            if (!UsuarioEstaLogado())
                throw new AccessViolationException("É preciso estar logado para poder ver as tarefas do sistema.");
            else if (usuarioLogado.TipoDeAcesso == AcessoAoSistema.TOTAL)
            {
                foreach (var tarefa in tarefas)
                    tarefa.ImprimirTarefa();
            }
            else if (usuarioLogado.TipoDeAcesso == AcessoAoSistema.PARCIAL)
            {
                List<Tarefa> tarefasPorResponsavel = tarefas.Where(tarefa => tarefa.EmailDoResponsavel == usuarioLogado.Email).ToList();
                List<Tarefa> tarefasPorObjetivo = new();
                List<Tarefa> tarefasDisponiveis = new();

                foreach (var tarefaPorResponsavel in tarefasPorResponsavel)
                    tarefasPorObjetivo.AddRange(tarefas.Where(tarefa => tarefa.Objetivo == tarefaPorResponsavel.Objetivo));

                tarefasDisponiveis.AddRange(tarefasPorResponsavel);
                tarefasDisponiveis.AddRange(tarefasPorObjetivo);
                tarefasDisponiveis = tarefasDisponiveis.Distinct().ToList();

                foreach (var tarefa in tarefas)
                    tarefa.ImprimirTarefa();
            }
        }

        public bool UsuarioEstaLogado()
        {
            if (usuarioLogado == null) 
                return false;
            return true;
        }

        public static bool StatusTarefaExiste(int statusEscolhido)
        {
            return Enum.IsDefined(typeof(StatusTarefa), statusEscolhido);
        }

        public Tarefa EscolherTarefa()
        {
            Tarefa tarefaEscolhida;
            int id;
            ListarTarefas();
            do
            {
                Console.WriteLine("Entre com o ID da tarefa: ");
                id = int.Parse(Console.ReadLine());
                tarefaEscolhida = tarefas.FirstOrDefault(tarefa => tarefa.Id == id);
            } while (tarefaEscolhida == null);

            return tarefaEscolhida;
        }

        public void AlterarEstadoTarefa(StatusTarefa statusTarefa, Tarefa tarefa)
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
            if (usuarioLogado == null)
                throw new AccessViolationException("Usuário deve estar logado para poder alterar estado de uma tarefa.");

            if (usuarioLogado.TipoDeAcesso == AcessoAoSistema.PARCIAL)
                throw new AccessViolationException("Apenas o Tech Leader pode alterar estado da tarefa.");

            Tarefa tarefaEscolhida = EscolherTarefa();
            int statusEscolhido;
            Tarefa.ListarStatusTarefa();
            do
            {
                Console.WriteLine($"Entre com o novo status da tarefa {tarefaEscolhida.Id}");
                statusEscolhido = int.Parse(Console.ReadLine());
            } while (!StatusTarefaExiste(statusEscolhido));

            AlterarEstadoTarefa((StatusTarefa)statusEscolhido, tarefaEscolhida);
        }

        public void CriarTarefa()
        {
            string objetivo, descricao, email = "";

            if (usuarioLogado == null)
                throw new AccessViolationException("É preciso estar logado para criar uma tarefa.");

            Console.WriteLine("Entre com o objetivo da tarefa: ");
            objetivo = Console.ReadLine();
            Console.WriteLine("Entre com a descrição da tarefa: ");
            descricao = Console.ReadLine();

            if (usuarioLogado.TipoDeAcesso == AcessoAoSistema.PARCIAL)
                email = usuarioLogado.Email;
            else if (usuarioLogado.TipoDeAcesso == AcessoAoSistema.TOTAL)
            {
                ListarUsuarios();
                do
                {
                    Console.WriteLine("Entre com o email do responsável pela tarefa: ");
                    email = Console.ReadLine();
                } while (EmailNaoExiste(email));
            }

            tarefas.Add(new Tarefa(email, objetivo, descricao));
            usuarioLogado.CriarTarefa(email, objetivo, descricao);
        }

        public void EstatisticasTarefas() { }
    }
}
