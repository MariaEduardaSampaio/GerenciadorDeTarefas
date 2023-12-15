using ClassLibrary.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Tarefas
{
    public enum StatusTarefa
    {
        ABANDONADA = 0,
        IMPEDIDA = 1,
        EM_ANALISE = 2,
        INICIADA = 3,
        CONCLUIDA = 4,
        ATRASADA = 5
    }
    public class Tarefa
    {
        private static int ultimoId = 0;
        public int Id { get; private set; }
        public StatusTarefa statusTarefa {  get; private set; }
        public string EmailDoResponsavel { get; private set; }
        public DateTime DiaCriacao { get; private set; }
        public DateTime DiaFinalizada { get; private set; }
        public string Objetivo { get; set; }
        public string Descricao { get; set; }

        public Tarefa(string EmailDoResponsavel, string Objetivo, string Descricao)
        {
            Id = ultimoId++;
            statusTarefa = StatusTarefa.EM_ANALISE;
            this.EmailDoResponsavel = EmailDoResponsavel;
            DiaCriacao = DateTime.Now;
            this.Objetivo = Objetivo;
            this.Descricao = Descricao;
        }

        public void ImprimirTarefa()
        {
            Console.WriteLine("\n***************************************************");
            Console.WriteLine($"TAREFA {Id} : {statusTarefa}");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"Responsável: {EmailDoResponsavel}");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"Data de criação: {DiaCriacao}");
            Console.WriteLine($"Data em que foi finalizada: {DiaFinalizada}");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"Objetivo: {Objetivo}");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"Descrição: {Descricao}");
            Console.WriteLine("***************************************************");
        }
    }
}
