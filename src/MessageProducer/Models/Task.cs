using System.Collections.Generic;

namespace MessageProducer.Models
{
    public class Task
    {
        public Task(string message, int parameter)
        {
            Message = message;
            Parameters = new List<object> {parameter};
        }

        public string Message { get; }
        public List<object> Parameters { get; }
    }
}