using MechanicalWoodSplitter.Code.FakeStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;

namespace MechanicalWoodSplitter.Code.EntityBehaviors
{
    public class HelveAxeBaseBlockEntitybehavior : BlockEntityBehavior
    {
        public readonly FakeBlockEntityAnvil FakeAnvil;

        public HelveAxeBaseBlockEntitybehavior(BlockEntity blockentity) : base(blockentity)
        {
            FakeAnvil = new(this);
        }
    }
}
