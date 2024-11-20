using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent.Mechanics;

namespace MechanicalWoodSplitter.HarmonyPatches
{
    [HarmonyPatch(typeof(BEHelveHammer), "GenHammerMesh")]
    public static class UseItemShapeForHelveAnimator
    {
        private static bool Prefix(BEHelveHammer __instance, ref MeshData __result)
        {
            Block block = __instance.Api.World.BlockAccessor.GetBlock(__instance.Pos);

            if (block.BlockId == 0)
            {
                __result = null;
                return false;
            }

            ITesselatorAPI tesselator = ((ICoreClientAPI)__instance.Api).Tesselator;

            var item = Traverse.Create(__instance).Field("hammerStack").GetValue<ItemStack>();
            var itemShape = item.Item?.Shape?.Base;
            Shape shapeBase = null;

            if (itemShape != null)
            {
                shapeBase = Shape.TryGet(__instance.Api, $"{itemShape.Domain}:shapes/{itemShape.Path}.json");
            }
            shapeBase ??= Shape.TryGet(__instance.Api, "shapes/block/wood/mechanics/helvehammer.json");

            tesselator.TesselateShape("HelveItemHead", shapeBase, out var modeldata, __instance, new Vec3f(0f, block.Shape.rotateY, 0f), 0, 0, 0);
            __result = modeldata;

            return false;
        }
    }
}