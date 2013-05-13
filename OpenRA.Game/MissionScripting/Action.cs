using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenRA.MissionScripting
{
    /// <summary>
    /// This interface represents actions that can be executed by scripted map triggers.
    /// </summary>
    public interface Action
    {
        void Execute();
    }
}
