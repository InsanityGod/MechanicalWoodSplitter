using HarmonyLib;
using MechanicalWoodSplitter.FakeStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.GameContent;

namespace MechanicalWoodSplitter.HarmonyPatches
{
    [HarmonyPatch(typeof(BlockEntityAnvil), nameof(BlockEntityAnvil.OnHelveHammerHit))]
    public class ReimplementDefaultSound
    {
        public static void Postfix(BlockEntityAnvil __instance)
        {
            if (__instance is not FakeBlockEntityAnvil && __instance.Api.Side == EnumAppSide.Client)
            {
                __instance.Api.World.PlaySoundAt(new AssetLocation("game:sounds/effect/anvilhit"), __instance.Pos.X, __instance.Pos.Y, __instance.Pos.Z + 0.5f, null, 0.3f + (float)__instance.Api.World.Rand.NextDouble() * 0.2f, 12f);
            }
        }
    }
}