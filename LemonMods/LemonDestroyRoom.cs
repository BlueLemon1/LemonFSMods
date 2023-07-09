using FSLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LemonMods
{
    [ModInfo("Lemon_DestroyRoom","Destroy rooms without limitations","Lemon",1,0)]
    internal class LemonDestroyRoom : Mod
    {
        [Hook("ConstructionMgr::CanDestroyRoom(Room)")]
        public void Hook_CanDestroyRoom(CallContext context, Room room)
        {
            context.IsHandled = true;
            context.ReturnValue = true;
        }
    }
}
