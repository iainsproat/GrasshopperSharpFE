/*
 * Iain Sproat, 2013
 */

namespace SharpFEGrasshopper.Core.TypeClass
{
    using System;
    using SharpFE;

    public class GH_GenericCrossSection : GH_CrossSection
    {
        public double Area
        {
            get;
            private set;
        }
        
        public double Iyy
        {
            get;
            private set;
        }
        
        public double Izz
        {
            get;
            private set;
        }
        
        public double Ixx
        {
            get;
            private set;
        }
        
        public GH_GenericCrossSection(double area, double iyy, double izz, double ixx)
        {
            this.Area = area;
            this.Iyy = iyy;
            this.Izz = izz;
            this.Ixx = ixx;
        }
        
        public override ICrossSection ToSharpCrossSection()
        {
            ICrossSection crossSection = new GenericCrossSection(this.Area, this.Iyy, this.Izz, this.Ixx);
            return crossSection;
        }
    }
}
