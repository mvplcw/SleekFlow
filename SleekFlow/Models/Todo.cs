using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SleekFlow.Models
{
    public class Todo
    {
        public int todo_id { get; set; }
        public string todo_name { get; set; }
        public string todo_description { get; set; }
        public string todo_due_date { get; set; }
        public string todo_status { get; set; }
        public List<Tag> todo_tags { get; set; }
    }
}
