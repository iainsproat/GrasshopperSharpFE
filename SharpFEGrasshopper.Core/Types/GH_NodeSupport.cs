
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using SharpFE;

namespace SharpFEGrasshopper.Core.TypeClass
{

    public class GH_NodeSupport : GH_Support
    {
        private bool UX, UY, UZ, RX, RY, RZ;
        
        private IList<Point3d> Nodes
        {
            get;
            set;
        }
        
        public GH_NodeSupport(Point3d position, bool UX, bool UY, bool UZ, bool RX, bool RY, bool RZ)
        {
            this.Nodes = new List<Point3d>();
            this.Nodes.Add(position);
            this.UX = UX;
            this.UY = UY;
            this.UZ = UZ;
            this.RX = RX;
            this.RY = RY;
            this.RZ = RZ;
        }
        
        public GH_NodeSupport(List<Point3d> positions, bool UX, bool UY, bool UZ, bool RX, bool RY, bool RZ)
        {
            this.Nodes = positions;   
            this.UX = UX;
            this.UY = UY;
            this.UZ = UZ;
            this.RX = RX;
            this.RY = RY;
            this.RZ = RZ;
        }
        
        public override string ToString()
        {
            return string.Format("[Node Support: UX={0}, UY={1}, UZ={2}, RX={3}, RY={4}, RZ={5}]", UX, UY, UZ, RX, RY, RZ);
        }

        public override void ToSharpSupport(GH_Model model)
        {
            
            foreach (Point3d node in Nodes)
            {
                IFiniteElementNode FEnode = model.FindOrCreateNode(node);
                
                switch (model.ModelType)
                {
                    case ModelType.Truss2D:
                        if (UX) {model.Model.ConstrainNode(FEnode, DegreeOfFreedom.X);}
                        //	if (UY) {model.Model.ConstrainNode(node, DegreeOfFreedom.Y);}
                        if (UZ) {model.Model.ConstrainNode(FEnode, DegreeOfFreedom.Z);}
                        //	if (RX) {model.Model.ConstrainNode(node, DegreeOfFreedom.XX);}
                        //	if (RY) {model.Model.ConstrainNode(node, DegreeOfFreedom.YY);}
                        //	if (RZ) {model.Model.ConstrainNode(node, DegreeOfFreedom.ZZ);}
                        break;
                    case ModelType.Full3D:
                        if (UX) {model.Model.ConstrainNode(FEnode, DegreeOfFreedom.X);}
                        if (UY) {model.Model.ConstrainNode(FEnode, DegreeOfFreedom.Y);}
                        if (UZ) {model.Model.ConstrainNode(FEnode, DegreeOfFreedom.Z);}
                        if (RX) {model.Model.ConstrainNode(FEnode, DegreeOfFreedom.XX);}
                        if (RY) {model.Model.ConstrainNode(FEnode, DegreeOfFreedom.YY);}
                        if (RZ) {model.Model.ConstrainNode(FEnode, DegreeOfFreedom.ZZ);}
                        break;
                    default:
                        throw new Exception("No such model type: " + model.ModelType);
                }
            }
        }
    }
}

