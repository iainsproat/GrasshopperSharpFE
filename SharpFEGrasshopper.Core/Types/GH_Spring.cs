
using System;
using System.Collections.Generic;
using Rhino.Geometry;
using SharpFE;

namespace SharpFEGrasshopper.Core.TypeClass
{
    public class GH_Spring : GH_Element
    {
        private Point3d Start 
        { 
            get; 
            set; 
        }
        
        private Point3d End 
        { 
            get; 
            set; 
        }
        
        private double SpringConstant 
        { 
            get;
            set; 
        }

        public GH_Spring(Point3d start, Point3d end, double springConstant)
        {
        	this.Start = start;
        	this.End = end;
        	this.SpringConstant = springConstant;
        }

        public override string ToString()
        {
            string s = "Spring from " + this.Start + " to " + this.End;
            return s;
        }

        public override void ToSharpElement(GH_Model model)
        {
        	IFiniteElementNode startNode = model.FindOrCreateNode(this.Start);
            IFiniteElementNode endNode = model.FindOrCreateNode(this.End);
        	model.Model.ElementFactory.CreateLinearConstantSpring(startNode, endNode, this.SpringConstant);        	
        }
    	
		public override GeometryBase GetGeometry(GH_Model model)
		{
			throw new NotImplementedException("Spring element. Geometry");
		}
    	
		public override GeometryBase GetDeformedGeometry(GH_Model model)
		{
			throw new NotImplementedException("Spring element. Deformed geometry");
		}
    }
}