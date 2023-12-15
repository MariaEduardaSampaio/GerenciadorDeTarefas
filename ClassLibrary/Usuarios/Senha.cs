using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Usuarios
{
    public class Senha
    {
        public string senha { get; set; }

        public Senha(string senha)
        {
            ValidarSenha(senha);
        }

        private void ValidarSenha(string senha)
        {
            if (String.IsNullOrEmpty(senha))
                throw new ArgumentNullException("Senha não pode ser nula.");
            else if (senha.Length < 8)
                throw new ArgumentException("Senha deve ter ao menos 8 caracteres.");
            else if (!senha.Any(letra => char.IsNumber(letra)))
                throw new ArgumentException("Senha deve conter ao menos um número.");
            else if (!senha.Any(letra => char.IsLetter(letra)))
                throw new ArgumentException("Senha deve conter ao menos uma letra.");
            this.senha = senha; 
        }
    }
}
