using Domain.AggregateObjects;
using Infrastructure.Context;
namespace Application
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var context = new TaskManagerContext();
            context.Tasks.Add(new TaskModel()
            {
                Status = 1,
                CreatedDate = DateTime.Now,
                Description = "descricao muito longa",
                Objective = "objetivo",
                Responsable = 1
            });
            context.SaveChanges();
        }
    }
}
