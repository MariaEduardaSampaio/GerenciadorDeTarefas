namespace ClassLibrary.Usuarios
{
    public enum AcessoAoSistema
    {
        TOTAL = 1,
        PARCIAL = 2
    }

    public class Usuario(string Nome, AcessoAoSistema TipoDeAcesso, string Email, Senha senha)
    {
        public string? Nome { get; private set; } = Nome;
        public AcessoAoSistema TipoDeAcesso { get; private set; } = TipoDeAcesso;

        public string Email { get; private set; } = Email;
        public Senha senha { get; set; } = senha;

        public void ListarInformacoes()
        {
            Console.WriteLine($"Nome: {Nome}\tEmail: {Email}");
        }
    }
}
