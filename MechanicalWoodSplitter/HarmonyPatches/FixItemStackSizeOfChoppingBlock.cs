using HarmonyLib;
using InDappledGroves.BlockEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.GameContent;

namespace MechanicalWoodSplitter.HarmonyPatches
{
    [HarmonyPatch(typeof(IDGBEWorkstation), nameof(IDGBEWorkstation.Initialize))]
    public static class FixItemStackSizeOfChoppingBlock
    {
        public static void Postfix(BlockEntityDisplay __instance)
        {
            if(__instance.InventoryClassName != "choppingblock") return;
            __instance.Inventory[0].MaxSlotStackSize = 1;
            __instance.Inventory[1].MaxSlotStackSize = 0;
        }
    }
}