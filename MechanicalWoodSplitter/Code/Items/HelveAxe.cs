using MechanicalWoodSplitter.Code.FakeStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;

namespace MechanicalWoodSplitter.Code.Items
{
    public class HelveAxe : Item
    {

        public float HelveAxeDamage { get; private set; }

        public override void OnLoaded(ICoreAPI api)
        {
            base.OnLoaded(api);

            HelveAxeDamage = Attributes["HelveAxeDamage"].AsFloat(1);
        }
    }
}