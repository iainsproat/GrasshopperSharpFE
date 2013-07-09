


namespace SharpFEGrasshopper.Core.TypeClass
{
    using System;
    using System.Collections.Generic;
    using Rhino.Geometry;
    using SharpFE;
    
    public class GH_NodalLoad : GH_Load
    {
        private Vector3d Force
        {
            get;
            set;
        }
        
        private Vector3d Moment
        {
            get;
            set;
        }
        
        private IList<Point3d> Nodes
        {
            get;
            set;
        }
        
        public GH_NodalLoad(Point3d position, Vector3d force, Vector3d moment)
        {
            this.Nodes = new List<Point3d>();
            this.Force = force;
            this.Moment = moment;
            this.Nodes.Add(position);
        }

        public GH_NodalLoad(List<Point3d> positions, Vector3d force, Vector3d moment)
        {
            this.Nodes = positions;
            this.Force = force;
            this.Moment = moment;
            
        }
        
        public override string ToString()
        {
            return string.Format("Nodal Load: Force={0}, Moment={1}, at Nodes={2}]", Force, Moment, Nodes);
        }

        public override void ToSharpLoad(GH_Model model)
        {
            ForceVector forceVector = null;
            
            switch (model.ModelType)
            {
                case ModelType.Truss2D:
                    forceVector = model.Model.ForceFactory.CreateForTruss(Force.X, Force.Z);
                    break;
                case ModelType.Full3D:
                    forceVector = model.Model.ForceFactory.Create(Force.X, Force.Y, Force.Z, Moment.X, Moment.Y, Moment.Z);
                    break;
                case ModelType.Membrane3D:
                    forceVector = model.Model.ForceFactory.Create(Force.X, Force.Y, Force.Z, 0, 0, 0);
                    break;
                default:
                    throw new Exception("No such model type implemented: "  + model.ModelType);
            }
            
            foreach (Point3d node in Nodes)
            {
                IFiniteElementNode FEnode = model.FindOrCreateNode(node);
                model.Model.ApplyForceToNode(forceVector, FEnode);
            }
        }
    }
}