using System;
using System.Threading.Tasks;

namespace _Sources.LoadingStepByStep
{
    public class LoadingStep
    {
        public string Description {get; private set;}
    
        public Func<Task> ActionAsync {get; private set;}

        public LoadingStep(string description, Func<Task> actionAsync)
        {
            Description = description;
            ActionAsync = actionAsync;
        }
    }
}
