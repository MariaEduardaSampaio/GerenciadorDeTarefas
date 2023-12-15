using Services;

namespace GerenciadorDeTarefas
{
    internal class Program
    {
        GerenciadorService gerenciador = new GerenciadorService();

        private void CadastrarDesenvolvedor()
        {
            try
            {
                Console.WriteLine("\tCadastro do desenvolvedor");
                Console.WriteLine("Nome:");
                string nome = Console.ReadLine();
                Console.WriteLine("Email:");
                string email = Console.ReadLine();
                Console.WriteLine("Senha:");
                string senha = Console.ReadLine();
                gerenciador.CadastrarDesenvolvedor(nome, email, senha);
            } catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro ao tentar cadastrar desenvolvedor: {ex}");
            }
        }

        private void CadastrarTechLeader()
        {
            try
            {
                Console.WriteLine("\tCadastro do Tech Leader");
                Console.WriteLine("Nome:");
                string nome = Console.ReadLine();
                Console.WriteLine("Email:");
                string email = Console.ReadLine();
                Console.WriteLine("Senha:");
                string senha = Console.ReadLine();
                gerenciador.CadastrarTechLeader(nome, email, senha);
            } catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro ao tentar cadastrar Tech Leader: {ex}");
            }
        }

        private void Login()
        {
            try
            {
                Console.WriteLine("\tLOGIN");
                Console.WriteLine("Email usuário:");
                string email = Console.ReadLine();
                Console.WriteLine("Senha: ");
                string senha = Console.ReadLine();
                gerenciador.AutenticarUsuario(email, senha);
            } catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro ao tentar fazer login de usuário: {ex}");
            }
        }

        private void Logout()
        {
            try
            {
                Console.WriteLine("\tLOGOUT");
                gerenciador.Logout();
            } catch(Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro ao tentar fazer logout de usuário: {ex}");
            }
        }

        private void CriarTarefa()
        {
            try
            {
                Console.WriteLine("\tCriar tarefa");
                gerenciador.CriarTarefa();
            } catch(Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro ao tentar criar uma tarefa: {ex}");
            }
        }

        private void RealizarOpcao(int opcao)
        {
            Program program = new Program();
            switch (opcao)
            {
                case 1:
                    program.CadastrarDesenvolvedor();
                    break;
                case 2:
                    program.CadastrarTechLeader();
                    break;
                case 3:
                    program.Login();
                    break;
                case 4:
                    break;
                    // criar tarefa
                    // alterar estado da tarefa
                    // listar tarefas

                case 8: 
                    program.Logout();
                    break;
            }
        }

        public static void Main(string[] args)
        {
            try
            {
                GerenciadorService gerenciador = new GerenciadorService();
                Program program = new Program();
                int opcao;

                Console.WriteLine("\tGERENCIADOR DE TAREFAS");
                do
                {
                    Console.WriteLine("Deseja fazer o cadastro de um desenvolvedor (1), de um Tech Leader (2), ou quer " +
                        "fazer o login (3)?");
                    Console.WriteLine("Obs.: Faça o login para ter acesso a outras partes do sistema.");
                    opcao = int.Parse(Console.ReadLine());
                } while ((opcao < 0 || opcao > 3) && !gerenciador.UsuarioEstaLogado());

                do
                {
                    gerenciador.usuarioLogado;
                } while (opcao < 4 || opcao > 10);
                program.RealizarOpcao(opcao);

            } catch(Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro no escopo geral: {ex}");
            }
        }
    }
}
