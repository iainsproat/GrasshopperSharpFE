namespace SharpFEGrasshopper
{
    using System;
    using Rhino.Geometry;
    using SharpFE;

    public static class XYZExtensions
    {
        public static Point3d ToPoint3d(this XYZ node)
        {
            return new Point3d(node.X, node.Y, node.Z);
        }
        
        public static Point3d ToPoint3d(this IFiniteElementNode node)
        {
            return new Point3d(node.X, node.Y, node.Z);
        }
    }
}
