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
        public override void CriarTarefa(string email, string objetivo, string descricao) 
        {
            Console.WriteLine("Criando tarefa (pelo tech leader):");
            var tarefa = new Tarefa(email, objetivo, descricao);
            tarefa.ImprimirTarefa();
            Console.WriteLine("\nTarefa criada com sucesso!");
        }

    }
}
