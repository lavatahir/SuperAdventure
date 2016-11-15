using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class MessageEventArgs : EventArgs
    {
        public string message { get; private set; }
        public bool addExtraNewLine { get; private set; }

        public MessageEventArgs(string msg, bool addExtraNewLine)
        {
            message = msg;
            this.addExtraNewLine = addExtraNewLine;
        }
    }
}
