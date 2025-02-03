using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.GameContent.Mechanics;

namespace MechanicalWoodSplitter.Code.HarmonyPatches
{
    [HarmonyPatch(typeof(BEHelveHammer), "Angle", MethodType.Getter)]
    public static class RemoveDefaultSoundCode
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var code = new List<CodeInstruction>(instructions);

            int? startIndex = null;

            for (var index = 0; index < code.Count - 54; index++)
            {
                var instruction = code[index];
                if (instruction.opcode == OpCodes.Ldarg_0 && code[index + 53].operand is System.Reflection.MethodInfo { Name: "PlaySoundAt" })
                {
                    startIndex = index;
                    break;
                }
            }

            if (startIndex.HasValue)
            {
                code.RemoveRange(startIndex.Value, 54);
            }

            return code;
        }
    }
}