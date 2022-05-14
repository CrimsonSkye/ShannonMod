using ShannonMod.BlockEntities;
using ShannonMod.Blocks;
using System;
using Vintagestory.API.Common;

namespace ShannonMod
{
    public class ShannonMod : ModSystem
    {
        public override void Start(ICoreAPI api)
        {
            base.Start(api);
            //register blocks
            api.RegisterBlockClass("cuttingboard", typeof(CuttingBoard));

            //register block entities
            api.RegisterBlockEntityClass("becuttingboard", typeof(BECuttingBoard));
        }
    }
}
