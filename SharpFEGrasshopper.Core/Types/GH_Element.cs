
using Rhino.Geometry;
using SharpFE;


namespace SharpFEGrasshopper.Core.TypeClass
{

    public abstract class GH_Element<T> : SimpleGooImplementation where T:IFiniteElement
    {   
        public abstract T ToSharpElement(GH_Model model);
        
        public abstract GeometryBase GetGeometry(GH_Model model);
        
        public abstract GeometryBase GetDeformedGeometry(GH_Model model);
    }

}