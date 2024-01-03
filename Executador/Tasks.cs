namespace Application
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
    public class Tasks
    {
        public int Id { get; private set; }
        public StatusTarefa Status { get; private set; }
        public string EmailResponsable { get; private set; }
        public DateTime CreatedDate { get; private set; } = DateTime.MinValue;
        public DateTime EndDate { get; private set; } = DateTime.MinValue;
        public string Objective { get; set; }
        public string Description { get; set; }
        public Tasks(string Objetivo, string Descricao)
        {
            AnalisarTarefa();
            CreatedDate = DateTime.Now;
            Objective = Objetivo;
            Description = Descricao;
        }
        public void AbandonarTarefa()
        {
            if (CreatedDate == DateTime.MinValue)
                throw new ArgumentException("Não é possível abandonar uma tarefa que não foi iniciada.");
            Status = StatusTarefa.ABANDONADA;
            EndDate = DateTime.Now;
            Console.WriteLine($"Tasks {Id} abandonada... :(");
        }

        public void ImpedirTarefa()
        {
            if (CreatedDate == DateTime.MinValue)
                throw new ArgumentException("Não é possível impedir uma tarefa que não foi iniciada.");
            Status = StatusTarefa.IMPEDIDA;
            Console.WriteLine($"Tasks {Id} impedida... :(");
        }

        public void AnalisarTarefa()
        {
            Status = StatusTarefa.EM_ANALISE;
            Console.WriteLine($"Tasks {Id} em análise pelo tech leader!");
        }

        public void IniciarTarefa()
        {
            Status = StatusTarefa.INICIADA;
            CreatedDate = DateTime.Now;
            Console.WriteLine($"Tasks {Id} iniciada pelo tech leader!");
        }

        public void ConcluirTarefa()
        {
            if (CreatedDate == DateTime.MinValue)
                throw new ArgumentException("Não é possível concluir uma tarefa que não foi iniciada.");
            Status = StatusTarefa.CONCLUIDA;
            EndDate = DateTime.Now;
            Console.WriteLine($"Tasks {Id} concluída! :)");
        }

        public void AtrasarTarefa()
        {
            if (CreatedDate == DateTime.MinValue)
                throw new ArgumentException("Não é possível atrasar uma tarefa que não foi iniciada.");
            Status = StatusTarefa.ATRASADA;
            Console.WriteLine($"Tasks {Id} está atrasada :(");
        }

        public void ImprimirTarefa()
        {
            Console.WriteLine("\n***************************************************");
            Console.WriteLine($"TAREFA {Id} : {Status}");
            Console.WriteLine("--------------------------------------------------");
            //Console.WriteLine($"Responsável: {EmailDoResponsavel}");
            //Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"Data de criação: {CreatedDate}");
            Console.WriteLine($"Data em que foi finalizada: {EndDate}");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"Objective: {Objective}");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"Descrição: {Description}");
            Console.WriteLine("***************************************************");
        }

        public static void ListarStatusTarefa()
        {
            Console.WriteLine("Lista de Status de Tasks:");
            foreach (var status in Enum.GetValues(typeof(StatusTarefa)))
            {
                Console.WriteLine($"{(int)status}: {status}");
            }
        }
    }
}
