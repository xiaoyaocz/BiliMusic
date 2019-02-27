using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace BiliMusic.Controls
{
    public class MyUICommand
    {
        public MyUICommand(string lable)
        {
            Label = lable;
        }
        public MyUICommand(string lable, EventHandler<MyUICommand> invoked)
        {
            Label = lable;
            Invoked = invoked;
        }
        public object Id { get; set ; }
        public EventHandler<MyUICommand> Invoked { get; set; }
        public string Label { get; set; }


    }
}
