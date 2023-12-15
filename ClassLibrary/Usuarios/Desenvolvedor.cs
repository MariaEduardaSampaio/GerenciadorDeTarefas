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
        public override void CriarTarefa(string email, string objetivo, string descricao)
        {
            Console.WriteLine("Criando tarefa (pelo desenvolvedor):");
            var tarefa = new Tarefa(Email, objetivo, descricao);
            tarefa.ImprimirTarefa();
            Console.WriteLine("\nTarefa criada com sucesso!");
        }
    }
}
