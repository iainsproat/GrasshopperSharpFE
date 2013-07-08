namespace SharpFEGrasshopper.Core.TypeClass
{
    using System;
    using SharpFE;

    public class GH_RectangularCrossSection : GH_CrossSection
    {
        public double Height
        {
            get;
            set;
        }
        
        public double Width
        {
            get;
            set;
        }
        
        public GH_RectangularCrossSection(double hgt, double wdth)
        {
            this.Height = hgt;
            this.Width = wdth;
        }
        
        public override ICrossSection ToSharpCrossSection()
        {
            ICrossSection crossSection = new SolidRectangle(this.Height, this.Width);
            return crossSection;
        }
    }
}
