using HarmonyLib;
using InDappledGroves.BlockEntities;
using MechanicalWoodSplitter.Code.EntityBehaviors;
using MechanicalWoodSplitter.Code.Items;
using Vintagestory.GameContent;
using Vintagestory.GameContent.Mechanics;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using InDappledGroves.Util.Config;

namespace MechanicalWoodSplitter.Code.FakeStuff
{
    public class FakeBlockEntityAnvil : BlockEntityAnvil
    {
        public IDGBEWorkstation workstation { get; internal set; }

        public ItemStack HelveAxeStack => (HelveAxeBehavior.Blockentity as BEHelveHammer)?.HammerStack;

        public readonly HelveAxeBaseBlockEntitybehavior HelveAxeBehavior;

        public FakeBlockEntityAnvil(HelveAxeBaseBlockEntitybehavior helveAxeBehavior) => HelveAxeBehavior = helveAxeBehavior;

        public override void OnHelveHammerHit()
        {
            if (HelveAxeBehavior.Api.Side != EnumAppSide.Server || workstation == null || HelveAxeStack?.Collectible is not HelveAxe) return;

            workstation.recipecomplete = false;
            if (workstation.InputSlot.Empty) return;

            HelveAxeBehavior.Api.World.PlaySoundAt(new AssetLocation("sounds/block/chop2"), workstation.Pos.X, workstation.Pos.Y, workstation.Pos.Z, null);

            workstation.recipeHandler.GetMatchingRecipes(
                HelveAxeBehavior.Api.World,
                workstation.InputSlot,
                "chopping",
                workstation.Block.Attributes["inventoryclass"].ToString(),
                workstation.Block.Attributes["workstationproperties"]["workstationtype"].ToString(),
                out var recipe
            );

            workstation.recipeHandler.recipe = recipe;
            if (recipe == null) return;


            ProgressRecipe();
            workstation.updateMeshes();
            workstation.MarkDirty(true);
        }

        public bool ProgressRecipe()
        {
            if (HelveAxeBehavior.Api.Side != EnumAppSide.Server) return false;
            
            ItemStack inputStack = workstation.InputSlot.Itemstack;
            workstation.recipeHandler.resistance = (inputStack.Block != null ? inputStack.Block.Resistance : inputStack.Item.Attributes["resistance"].AsFloat(0f)) * IDGToolConfig.Current.baseWorkstationResistanceMult;

            workstation.recipeHandler.curDmgFromMiningSpeed = (HelveAxeStack.Collectible as HelveAxe).HelveAxeDamage * IDGToolConfig.Current.baseWorkstationMiningSpdMult;
            workstation.recipeHandler.currentMiningDamage += 0.5f * workstation.recipeHandler.curDmgFromMiningSpeed;
            workstation.recipeHandler.recipeProgress = workstation.recipeHandler.currentMiningDamage / workstation.recipeHandler.resistance;

            if (workstation.recipeHandler.currentMiningDamage >= workstation.recipeHandler.resistance)
            {
                FinishRecipe();
                return true;
            }

            return false;
        }

        public void FinishRecipe()
        {
            workstation.recipecomplete = true;
            ItemStack resolvedItemstack = workstation.recipeHandler.recipe.ReturnStack.ResolvedItemstack;
            var craftedStack = workstation.recipeHandler.recipe.TryCraftNow(HelveAxeBehavior.Api, workstation.InputSlot);
            workstation.InputSlot.Itemstack = null;

            if (resolvedItemstack.Collectible.FirstCodePart(0) != "air")
            {
                if (resolvedItemstack.Collectible.FirstCodePart(0) == craftedStack.Collectible.FirstCodePart(0))
                {
                    craftedStack.StackSize += 1;
                }
                else workstation.InputSlot.Itemstack = resolvedItemstack.Clone();
            }

            SpawnOutput(craftedStack);

            workstation.recipeHandler.clearRecipe(true);
        }

        public void SpawnOutput(ItemStack output)
        {
            for (int i = output.StackSize; i > 0; i--)
            {
                HelveAxeBehavior.Api.World.SpawnItemEntity(new ItemStack(output.Collectible, 1), workstation.Pos.ToVec3d(), new Vec3d(0.05000000074505806, 0.10000000149011612, 0.05000000074505806));
            }
        }
    }
}