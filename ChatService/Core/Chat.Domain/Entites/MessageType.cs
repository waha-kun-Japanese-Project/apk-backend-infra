using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Domain.Entites
{
    public enum MessageType
    {
        Text = 0,

        Image = 1,

        Voice = 2,

        File = 3,

        QuickReply = 4,

        System = 5
    }
}
