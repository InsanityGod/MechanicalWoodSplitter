using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.GameContent;

namespace MechanicalWoodSplitter.HarmonyPatches
{
    public static class FixItemStackSizeOfChoppingBlock
    {
        public static void PostFix(BlockEntityDisplay __instance)
        {
            __instance.Inventory[0].MaxSlotStackSize = 1;
        }
    }
}