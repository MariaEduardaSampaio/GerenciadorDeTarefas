using ClassLibrary.Tarefas;
using ClassLibrary.Usuarios;

namespace GerenciadorDeTarefas
{
    internal class Program
    {
        BancoDeDados banco = new BancoDeDados();

        private void CadastrarDesenvolvedor()
        {
            Console.WriteLine("\tCadastro do desenvolvedor");
            Console.WriteLine("Nome:");
            string nome = Console.ReadLine();
            Console.WriteLine("Email:");
            string email = Console.ReadLine();
            Console.WriteLine("Senha:");
            Senha senha = new Senha(Console.ReadLine());
            banco.CadastrarDesenvolvedor(nome, email, senha);
        }

        private void CadastrarTechLeader()
        {
            Console.WriteLine("\tCadastro do Tech Leader");
            Console.WriteLine("Nome:");
            string nome = Console.ReadLine();
            Console.WriteLine("Email:");
            string email = Console.ReadLine();
            Console.WriteLine("Senha:");
            Senha senha = new Senha(Console.ReadLine());
            banco.CadastrarTechLeader(nome, email, senha);
        }

        private void Login()
        {
            Console.WriteLine("\tLOGIN");
            Console.WriteLine("Email usuário:");
            string email = Console.ReadLine();
            Console.WriteLine("Senha: ");
            string senha = Console.ReadLine();
            banco.AutenticarUsuario(email, senha);
        }

        private void Logout()
        {
            Console.WriteLine("\tLOGOUT");
            banco.Logout();
        }

        public static void Main(string[] args)
        {
            Program programa = new Program(); // Criando uma instância de Program
            Desenvolvedor dev = new Desenvolvedor("duda", "duda@gmail.com", new Senha("dudadevsenior2024"));
            TechLeader dev2 = new TechLeader("maria", "maria@gmail.com", new Senha("dudatechleader2024"));
            //programa.CadastrarDesenvolvedor();
            programa.CadastrarDesenvolvedor();
            programa.CadastrarTechLeader();

            programa.Login();
            programa.banco.ListarTarefas();

            // colocar try catches
            // implemetnar funcoes do banco
            // separar classe banco de dados em talvez uma outra solution
            // fazer um menu em program
        }
    }
}
