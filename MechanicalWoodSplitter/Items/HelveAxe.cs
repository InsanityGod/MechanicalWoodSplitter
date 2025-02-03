using MechanicalWoodSplitter.FakeStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;

namespace MechanicalWoodSplitter.Items
{
    public class HelveAxe : Item
    {
        public readonly FakeBlockEntityAnvil FakeBlockEntityAnvil;

        public float HelveAxeDamage { get; private set; }

        public HelveAxe() => FakeBlockEntityAnvil = new(this);

        public override void OnLoaded(ICoreAPI api)
        {
            base.OnLoaded(api);

            HelveAxeDamage = Attributes["HelveAxeDamage"].AsFloat(1);
        }
    }
}