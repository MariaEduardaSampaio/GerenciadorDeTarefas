using ClassLibrary.Tarefas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Usuarios
{
    public class TechLeader(string Nome, string Email, Senha senha) : Usuario(Nome, AcessoAoSistema.TOTAL, Email, senha)
    {
        public void criarTarefa(string responsavelPelaTarefa) 
        {
            Console.WriteLine("Criar tarefa (pelo tech leader):");
            Console.WriteLine("Entre com o objetivo da tarefa: ");
            string objetivo = Console.ReadLine();
            Console.WriteLine("Entre com a descrição da tarefa: ");
            string descricao = Console.ReadLine();
            var tarefa = new Tarefa(responsavelPelaTarefa, objetivo, descricao);
            tarefa.ImprimirTarefa();
            Console.WriteLine("\nTarefa criada com sucesso!");
        }
    }
}
