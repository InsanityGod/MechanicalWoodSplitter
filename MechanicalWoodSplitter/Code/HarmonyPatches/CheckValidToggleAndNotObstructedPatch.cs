using HarmonyLib;
using InDappledGroves.BlockEntities;
using MechanicalWoodSplitter.Code.EntityBehaviors;
using MechanicalWoodSplitter.Code.Items;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;
using Vintagestory.GameContent.Mechanics;

namespace MechanicalWoodSplitter.Code.HarmonyPatches
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

                var helveAxeBehavior = __instance.GetBehavior<HelveAxeBaseBlockEntitybehavior>();
                if (helveAxeBehavior == null) return;

                var targetAnvil = targetAnvilField.GetValue<BlockEntityAnvil>();

                if (targetAnvil != helveAxeBehavior.FakeAnvil)
                {
                    targetAnvilField.SetValue(helveAxeBehavior.FakeAnvil);
                }

                var pos = traverse.Field("anvilPos").GetValue<BlockPos>();
                helveAxeBehavior.FakeAnvil.ChoppingBlock = __instance.Api.World.BlockAccessor.GetBlockEntity<IDGBEWorkstation>(pos);
            }
        }
    }
}