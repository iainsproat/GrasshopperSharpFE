namespace SharpFEGrasshopper.Core.TypeClass
{
    using System;
    using System.Collections.Generic;
    using Rhino.Geometry;
    using SharpFE;

    public class GH_ConstantStrainTriangle : GH_Element
    {
       private IList<Point3d> Points
       {
           get;
           set;
       }
        
        private GH_Material Material
        {
            get;
            set;
        }
        
        private double Thickness
        {
            get;
            set;
        }

        public GH_ConstantStrainTriangle(Point3d p0, Point3d p1, Point3d p2, GH_Material material, double thickness)
        {
            this.Points = new List<Point3d>(){p0, p1, p2};
            this.Material = material;
            this.Thickness = thickness;
        }

        public override string ToString()
        {
            return string.Format("Triangle element: p0 = {0}; p1 = {1}; p2 = {2};", this.Points[0], this.Points[1], this.Points[2]);
        }

        public override void ToSharpElement(GH_Model model)
        {
            IFiniteElementNode n0 = model.FindOrCreateNode(this.Points[0]);
            IFiniteElementNode n1 = model.FindOrCreateNode(this.Points[1]);
            IFiniteElementNode n2 = model.FindOrCreateNode(this.Points[2]);

            model.Model.ElementFactory.CreateLinearConstantStrainTriangle(n0, n1, n2, this.Material.ToSharpMaterial(), this.Thickness);
        }
        
        public override GeometryBase GetGeometry(GH_Model model)
        {
            return this.CreateMesh(this.Points);
        }
        
        public override GeometryBase GetDeformedGeometry(GH_Model model)
        {
            IList<Point3d> displacedPoints = new List<Point3d>(this.Points.Count);
            foreach (Point3d pnt in this.Points)
            {
                displacedPoints.Add(model.GetDisplacedPoint(pnt));
            }
            return this.CreateMesh(displacedPoints);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mshPnts"></param>
        /// <returns></returns>
        protected Mesh CreateMesh(IList<Point3d> mshPnts)  //FIXME every face is a new mesh!
        {
            Mesh mesh = new Mesh();
            mesh.Vertices.Add(mshPnts[0]);
            mesh.Vertices.Add(mshPnts[1]);
            mesh.Vertices.Add(mshPnts[2]);
            mesh.Faces.AddFace(0, 1, 2); //FIXME this appears quite brittle - what if it is not 0, 1 & 2?
            return mesh;
        }
    }
}