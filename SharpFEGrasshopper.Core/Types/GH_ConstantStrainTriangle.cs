namespace SharpFEGrasshopper.Core.TypeClass
{
    using System;
    using System.Collections.Generic;
    using Rhino.Geometry;
    using SharpFE;

    public class GH_ConstantStrainTriangle : GH_Element
    {
        private List<GH_Node> Points
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
            this.Points = new List<GH_Node>();
            this.Points.Add(new GH_Node(p0));
            this.Points.Add(new GH_Node(p1));
            this.Points.Add(new GH_Node(p2));
            
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
            throw new NotImplementedException("Triangle element. Geometry");
        }
        
        public override GeometryBase GetDeformedGeometry(GH_Model model)
        {
            throw new NotImplementedException("Triangle element. Deformed geometry");
        }
    }
}