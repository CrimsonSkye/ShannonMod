using ShannonMod.BlockEntities;
using ShannonMod.Blocks;
using ShannonMod.CollectibleBehaviors;
using System;
using Vintagestory.API.Common;

namespace ShannonMod
{
    public class ShannonMod : ModSystem
    {
        public override void Start(ICoreAPI api)
        { // Registers all classes to the Vintage Story interface during launch
            base.Start(api);
            //register blocks
            api.RegisterBlockClass("cuttingboard", typeof(CuttingBoard));

            //register block entities
            api.RegisterBlockEntityClass("becuttingboard", typeof(BECuttingBoard));

            //register collectible behaviors
            api.RegisterCollectibleBehaviorClass("FoodCutting", typeof(ExampleBehavior));
        }
    }
}
