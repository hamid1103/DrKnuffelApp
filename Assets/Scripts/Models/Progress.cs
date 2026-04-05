using System;

namespace DefaultNamespace.Models
{
    [Serializable]
    public class Progress
    {
        public string id;
        public string userData_id;
        public string step_id;
        public bool completed;
        public string completed_at;
    }
}