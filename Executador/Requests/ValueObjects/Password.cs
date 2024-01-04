namespace Application.Requests.ValueObjects
{
    public class Password
    {
        public string password { get; set; }

        public Password(string password)
        {
            ValidarSenha(password);
        }

        private void ValidarSenha(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("Senha não pode ser nula.");
            else if (password.Length < 8)
                throw new ArgumentException("Senha deve ter ao menos 8 caracteres.");
            else if (!password.Any(letra => char.IsNumber(letra)))
                throw new ArgumentException("Senha deve conter ao menos um número.");
            else if (!password.Any(letra => char.IsLetter(letra)))
                throw new ArgumentException("Senha deve conter ao menos uma letra.");
            this.password = password;
        }
    }
}
