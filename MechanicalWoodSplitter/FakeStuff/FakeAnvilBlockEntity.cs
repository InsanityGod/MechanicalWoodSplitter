using HarmonyLib;
using InDappledGroves.Util.RecipeTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace MechanicalWoodSplitter.FakeStuff
{
    public class FakeBlockEntityAnvil : BlockEntityAnvil
    {
        internal BlockEntityDisplay IDGChoppingBlockContainer;

        public FakeBlockEntityAnvil()
        {
        }

        public IDGRecipeNames.ChoppingBlockRecipe GetMatchingChoppingBlockRecipe(IWorldAccessor world, ItemSlot slots, string toolmode)
        {
            List<IDGRecipeNames.ChoppingBlockRecipe> choppingBlockRecipes = IDGRecipeNames.IDGRecipeRegistry.Loaded.ChoppingBlockRecipes;
            if (choppingBlockRecipes == null)
            {
                return null;
            }

            for (int i = 0; i < choppingBlockRecipes.Count; i++)
            {
                if (choppingBlockRecipes[i].Matches(Api.World, slots) && choppingBlockRecipes[i].ToolMode == toolmode)
                {
                    return choppingBlockRecipes[i];
                }
            }

            return null;
        }

        public override void OnHelveHammerHit()
        {
            if (Api.Side != EnumAppSide.Server || IDGChoppingBlockContainer == null) return;

            var itemSlot = IDGChoppingBlockContainer.Inventory[0];

            if (itemSlot.Empty) return;
            Api.World.PlaySoundAt(new AssetLocation("sounds/block/chop2"), IDGChoppingBlockContainer.Pos.X, IDGChoppingBlockContainer.Pos.Y, IDGChoppingBlockContainer.Pos.Z, null);

            var recipe = GetMatchingChoppingBlockRecipe(Api.World, itemSlot, "chopping");
            if (recipe == null) return;
            var traverse = Traverse.Create(IDGChoppingBlockContainer)
            .Method("SpawnOutput", recipe, new EntityAgent { }, IDGChoppingBlockContainer.Pos);

            traverse.GetValue(recipe, null, IDGChoppingBlockContainer.Pos);

            if (recipe.ReturnStack.ResolvedItemstack.Collectible.FirstCodePart() != "air")
            {
                traverse.GetValue();
            }

            IDGChoppingBlockContainer.Inventory.Clear();
            IDGChoppingBlockContainer.Inventory.MarkSlotDirty(0);
        }
    }
}