using System;
using System.Collections.Generic;

namespace DefaultNamespace
{
    [Serializable]
    public class LocalSave
    {
        public List<int> CompletedSteps { get; set; }

        public LocalSave()
        {
            CompletedSteps = new List<int>();
        }

        //Prevent duplicates in the List. Doesn't really matter, but it's nice to have
        //minimal and clean save files
        public void CompleteStep(int step)
        {
            if(!CompletedSteps.Contains(step))
                CompletedSteps.Add(step);
        }
    }
}