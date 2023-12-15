using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.Tarefas;
using ClassLibrary.Usuarios;

namespace GerenciadorDeTarefas
{
    public class BancoDeDados
    {
        public List<Usuario> usuarios { get; private set; }
        public List<Tarefa> tarefas { get; private set; }
        public Usuario usuarioLogado { get; private set; } = null;

        public BancoDeDados()
        {
            usuarios = new List<Usuario>();
            tarefas = new List<Tarefa>();
        }

        public void ListarTarefas()
        {
            if (usuarioLogado == null)
                throw new ArgumentException("É preciso estar logado para poder ver as tarefas do sistema.");
            else if (usuarioLogado.TipoDeAcesso == AcessoAoSistema.TOTAL)
            {
                foreach (var tarefa in tarefas)
                    tarefa.ImprimirTarefa();
            }
            else if (usuarioLogado.TipoDeAcesso == AcessoAoSistema.PARCIAL)
            {
                List<Tarefa> tarefasPorResponsavel = tarefas.Where(tarefa => tarefa.EmailDoResponsavel == usuarioLogado.Email).ToList();
                List<Tarefa> tarefasPorObjetivo = new List<Tarefa>();
                List<Tarefa> tarefasDisponiveis = new List<Tarefa>();

                foreach (var tarefaPorResponsavel in tarefasPorResponsavel)
                    tarefasPorObjetivo.AddRange(tarefas.Where(tarefa => tarefa.Objetivo == tarefaPorResponsavel.Objetivo));

                tarefasDisponiveis.AddRange(tarefasPorResponsavel);
                tarefasDisponiveis.AddRange(tarefasPorObjetivo);
                tarefasDisponiveis = tarefasDisponiveis.Distinct().ToList();

                foreach (var tarefa in tarefas)
                    tarefa.ImprimirTarefa();
            }
        }

        public void ListarUsuarios()
        {
            foreach (var usuario in usuarios)
            {
                usuario.ListarInformacoes();
            }
        }

        public bool EmailEhValido(string email)
        {
            return usuarios.All(usuario => usuario.Email != email);
        }

        public void ValidarEmail(string email)
        {
            if (!EmailEhValido(email))
                throw new ArgumentException("Já existe um usuário cadastrado com este email.");
        }

        public void CadastrarDesenvolvedor(string nome, string email, Senha senha)
        {
            ValidarEmail(email);
            Desenvolvedor usuario = new Desenvolvedor(nome, email, senha);
            usuarios.Add(usuario);
        }

        public void CadastrarTechLeader(string nome, string email, Senha senha)
        {
            ValidarEmail(email);
            TechLeader usuario = new TechLeader(nome, email, senha);
            usuarios.Add(usuario);
        }

        public Tarefa EscolherTarefa() { }
        public void AlterarEstadoTarefa() { }

        public void CriarTarefa()
        {
            // implementar
        }

        public void AutenticarUsuario(string email, string senha)
        {
            Usuario usuario = usuarios.FirstOrDefault(usuario => usuario.Email == email);
            if (usuario == null || usuario.senha.senha != senha)
                throw new ArgumentException("Email ou senha incorretos.");

            usuarioLogado = usuario;
            Console.WriteLine("Usuário logado com sucesso!");
        }

        public void Logout()
        {
            if (usuarioLogado == null)
                throw new ArgumentException("Não há nenhum login ativo no momento.");
        
            usuarioLogado = null;
            Console.WriteLine("Usuário deslogado com sucesso!");
        }
    }
}
