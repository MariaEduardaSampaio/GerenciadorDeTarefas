using ClassLibrary.Tarefas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Usuarios
{
    public class TechLeader(string Nome, string Email, Senha senha) : Usuario(Nome, AcessoAoSistema.TOTAL, Email, senha)
    {
        private void ListarUsuarios(List<Usuario> usuarios)
        {
            foreach(Usuario usuario in usuarios)
            {
                usuario.ListarInformacoes();
            }
        }

        private bool EmailDeUsuarioExiste(List<Usuario> usuarios, string email)
        {
            foreach(Usuario usuario in usuarios)
            {
                if (usuario.Email == email)
                    return true;
            }

            return false;
        }
        public void CriarTarefa(List<Usuario> usuarios) 
        {
            string objetivo, descricao, email;
            Console.WriteLine("Criar tarefa (pelo tech leader):");
            Console.WriteLine("Entre com o objetivo da tarefa: ");
            objetivo = Console.ReadLine();
            Console.WriteLine("Entre com a descrição da tarefa: ");
            descricao = Console.ReadLine();

            ListarUsuarios(usuarios);
            do
            {
                Console.WriteLine("Escolha um desenvolvedor (pelo email) para ser o responsável da tarefa:");
                email = Console.ReadLine();
            } while (!EmailDeUsuarioExiste(usuarios, email));

            var tarefa = new Tarefa(email, objetivo, descricao);
            tarefa.ImprimirTarefa();
            Console.WriteLine("\nTarefa criada com sucesso!");
        }

    }
}
