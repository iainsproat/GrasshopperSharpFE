namespace SharpFEGrasshopper
{
    using System;
    using Rhino.Geometry;
    using SharpFE;
    
    public static class FiniteElementModelExtensions
    {
        public static IFiniteElementNode FindNodeNearTo(this FiniteElementModel model, Point3d position)
        {
            return model.FindNodeNearTo(position.X, position.Y, position.Z);
        }
        
        public static IFiniteElementNode FindNodeNearTo(this FiniteElementModel model, Point3d position, double tolerance)
        {
            return model.FindNodeNearTo(position.X, position.Y, position.Z, tolerance);
        }
    }
}
