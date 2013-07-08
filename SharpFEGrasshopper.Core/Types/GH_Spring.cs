
using System;
using System.Collections.Generic;
using Rhino.Geometry;
using SharpFE;

namespace SharpFEGrasshopper.Core.TypeClass
{

    public class GH_Spring : GH_Element<LinearConstantSpring>
    {
        private GH_Node Start
        {
            get;
            set;
        }
        
        private GH_Node End
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
        	this.Start = new GH_Node(start);
        	this.End = new GH_Node(end);
        	this.SpringConstant = springConstant;
        }

        public override string ToString()
        {
            return string.Format("Spring from {0} to {1}", this.Start, this.End);
        }

        public override LinearConstantSpring ToSharpElement(GH_Model model)
        {
        	IFiniteElementNode startNode = Start.ToSharpElement(model);
        	IFiniteElementNode endNode = End.ToSharpElement(model);
        	
        	return model.Model.ElementFactory.CreateLinearConstantSpring(startNode,endNode, this.SpringConstant);
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