namespace SharpFEGrasshopper.Core.TypeClass
{
    using System;
    using SharpFE;

    public abstract class GH_Material : SimpleGooImplementation
    {
        public abstract IMaterial ToSharpMaterial();
    }
}
