using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.Tarefas;

namespace ClassLibrary.Usuarios
{
    public class Desenvolvedor(string Nome, string Email, Senha senha) : Usuario(Nome, AcessoAoSistema.PARCIAL, Email, senha)
    {
        public void criarTarefa()
        {
            Console.WriteLine("Criar tarefa (pelo desenvolvedor):");
            Console.WriteLine("Entre com o objetivo da tarefa: ");
            string objetivo = Console.ReadLine();
            Console.WriteLine("Entre com a descrição da tarefa: ");
            string descricao = Console.ReadLine();
            var tarefa = new Tarefa(Email, objetivo, descricao);
            tarefa.ImprimirTarefa();
            Console.WriteLine("\nTarefa criada com sucesso!");
        }
    }
}
