
using System;
using System.Collections.Generic;
using Rhino.Geometry;
using SharpFE;
using System.Linq;

namespace SharpFEGrasshopper.Core.TypeClass
{

    public class GH_ConstantStrainTriangle : GH_Element<LinearConstantStrainTriangle>
    {


        private List<Point3d> Points { get; set; }
        
        
        private GH_Material Material {get; set;}
        
        private double Thickness {get; set;}


        public GH_ConstantStrainTriangle(Point3d p0, Point3d p1, Point3d p2, GH_Material material, double thickness)
        {
            this.Points = new List<Point3d>();
            this.Points.Add(p0);
            this.Points.Add(p1);
            this.Points.Add(p2);
            
            this.Material = material;
            this.Thickness = thickness;
        }

        public override string ToString()
        {
            string s = "Triangle element: p0 = " + this.Points[0] + " p1 = " + this.Points[1] + " p2 = " + this.Points[2];
            return s;
        }

        public override LinearConstantStrainTriangle ToSharpElement(GH_Model model)
        {
            
            IFiniteElementNode n0 = null;
            IFiniteElementNode  n1 = null;
            IFiniteElementNode  n2 = null;
            
            try
            {
                n0 = model.Points.First(kvp => kvp.Value == this.Points[0]).Key;
            }
            catch(InvalidOperationException)
            {
                n0 = createnewFENode(model, this.Points[0]);
            }
            
            try
            {
                n1 = model.Points.First(kvp => kvp.Value == this.Points[1]).Key;
            }
            catch(InvalidOperationException)
            {
                n1 = createnewFENode(model, this.Points[1]);
            }
            
            try
            {
                n2 = model.Points.First(kvp => kvp.Value == this.Points[2]).Key;
            }
            catch(InvalidOperationException)
            {
                n2 = createnewFENode(model, this.Points[2]);
            }
            
            if (n0 != null && n1 != null && n2 != null)
            {
                return model.Model.ElementFactory.CreateLinearConstantStrainTriangle(n0, n1, n2, this.Material.ToSharpMaterial(), this.Thickness);
                
            }
            
            throw new Exception("No LinearConstantStrainTriangle could be created!");
        }
        
        protected IFiniteElementNode createnewFENode(GH_Model model, Point3d pos)
        {
            IFiniteElementNode temp;
            Point3d pnt = pos;
            switch(model.ModelType)
            {
                case ModelType.Full3D:
                    temp = model.Model.NodeFactory.Create(pnt.X, pnt.Y, pnt.Z);
                    break;
                default:
                    throw new NotImplementedException(string.Format("GH_ConstantStrainTriangle.ToSharpElement is not implemented for a model type of {0}", model.ModelType));
            }
            
            model.Points.Add(temp, pnt);
            return temp;
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