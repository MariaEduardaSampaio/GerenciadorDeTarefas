namespace Application.Usuarios
{
    public class TechLeader(string Nome, string Email, Senha senha) : Usuario(Nome, AcessoAoSistema.TOTAL, Email, senha)
    {
        public override void CriarTarefa(string email, string objetivo, string descricao)
        {
            Console.WriteLine("Criando tarefa (pelo tech leader):");
            var tarefa = new Tasks(email, objetivo, descricao);
            tarefa.ImprimirTarefa();
            Console.WriteLine("\nTarefa criada com sucesso!");
        }

    }
}
