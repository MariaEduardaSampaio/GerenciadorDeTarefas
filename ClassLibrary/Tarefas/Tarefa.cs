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
        public DateTime DiaCriacao { get; private set; } = DateTime.MinValue;
        public DateTime DiaFinalizada { get; private set; } = DateTime.MinValue;
        public string Objetivo { get; set; }
        public string Descricao { get; set; }
        public Tarefa(string EmailDoResponsavel, string Objetivo, string Descricao)
        {
            Id = ultimoId++;
            AnalisarTarefa();
            this.EmailDoResponsavel = EmailDoResponsavel;
            this.Objetivo = Objetivo;
            this.Descricao = Descricao;
        }
        public void AbandonarTarefa()
        {
            if (DiaCriacao == DateTime.MinValue)
                throw new ArgumentException("Não é possível abandonar uma tarefa que não foi iniciada.");
            statusTarefa = StatusTarefa.ABANDONADA;
            DiaFinalizada = DateTime.Now;
            Console.WriteLine($"Tarefa {Id} abandonada... :(");
        }

        public void ImpedirTarefa()
        {
            if (DiaCriacao == DateTime.MinValue)
                throw new ArgumentException("Não é possível impedir uma tarefa que não foi iniciada.");
            statusTarefa = StatusTarefa.IMPEDIDA;
            Console.WriteLine($"Tarefa {Id} impedida... :(");
        }

        public void AnalisarTarefa()
        {
            statusTarefa = StatusTarefa.EM_ANALISE;
            Console.WriteLine($"Tarefa {Id} em análise pelo tech leader!");
        }

        public void IniciarTarefa()
        {
            statusTarefa = StatusTarefa.INICIADA;
            DiaCriacao = DateTime.Now;
            Console.WriteLine($"Tarefa {Id} iniciada pelo tech leader!");
        }

        public void ConcluirTarefa()
        {
            if (DiaCriacao == DateTime.MinValue)
                throw new ArgumentException("Não é possível concluir uma tarefa que não foi iniciada.");
            statusTarefa = StatusTarefa.CONCLUIDA;
            DiaFinalizada = DateTime.Now;
            Console.WriteLine($"Tarefa {Id} concluída! :)");
        }

        public void AtrasarTarefa()
        {
            if (DiaCriacao == DateTime.MinValue)
                throw new ArgumentException("Não é possível atrasar uma tarefa que não foi iniciada.");
            statusTarefa = StatusTarefa.ATRASADA;
            Console.WriteLine($"Tarefa {Id} está atrasada :(");
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

        public static void ListarStatusTarefa()
        {
            Console.WriteLine("Lista de Status de Tarefa:");
            foreach (var status in Enum.GetValues(typeof(StatusTarefa)))
            {
                Console.WriteLine($"{(int)status}: {status}");
            }
        }
    }
}
