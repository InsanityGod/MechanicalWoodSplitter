using HarmonyLib;
using InDappledGroves.BlockEntities;
using MechanicalWoodSplitter.FakeStuff;
using MechanicalWoodSplitter.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;
using Vintagestory.GameContent.Mechanics;

namespace MechanicalWoodSplitter.HarmonyPatches
{
    [HarmonyPatch(typeof(BEHelveHammer), "CheckValidToggleAndNotObstructed")]
    public static class CheckValidToggleAndNotObstructedPatch
    {
        public static void Postfix(BEHelveHammer __instance)
        {
            var traverse = Traverse.Create(__instance);
            var targetAnvilField = traverse.Field("targetAnvil");
            if (__instance.HammerStack?.Item is HelveAxe helveAxe)
            {
                helveAxe.FakeBlockEntityAnvil.Api = __instance.Api;

                var targetAnvil = targetAnvilField.GetValue<BlockEntityAnvil>();
                if(targetAnvil != helveAxe.FakeBlockEntityAnvil)
                {
                    targetAnvilField.SetValue(helveAxe.FakeBlockEntityAnvil);
                }

                var pos = traverse.Field("anvilPos").GetValue<BlockPos>();
                var workStation = __instance.Api.World.BlockAccessor.GetBlockEntity<IDGBEWorkstation>(pos);
                if (workStation != null)
                {
                    helveAxe.FakeBlockEntityAnvil.ChoppingBlock = workStation;
                }
                else
                {
                    helveAxe.FakeBlockEntityAnvil.ChoppingBlock = null;
                }
                
            }
        }
    }
}