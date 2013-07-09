namespace SharpFEGrasshopper.Core.TypeClass
{
    using System;
    using Rhino.Geometry;
    using SharpFE;

    public class GH_Node : GH_Element
    {
        public Point3d Position
        {
            get;
            set;
        }

        public GH_Node(Point3d position)
        {
            this.Position = position;
        }

        public override string ToString()
        {
            return string.Format("Node at {0}", this.Position);
        }

        public override void ToSharpElement(GH_Model model)
        {
            FiniteElementNode node = null;
            
            //Check if node already exist at position
            int index = model.Points.IndexOf(this.Position);
            if (index != -1)  //node already exists, so retrieve it and return
            {
                node = model.Nodes[index];
                this.Index = index;
                return;
            }
            
            switch (model.ModelType)
            {
                case ModelType.Full3D:
                    node = model.Model.NodeFactory.Create(this.Position.X, this.Position.Y, this.Position.Z);
                    break;
                case ModelType.Truss2D:
                    node = model.Model.NodeFactory.CreateFor2DTruss(this.Position.X, this.Position.Z);
                    break;
                default:
                    throw new Exception("Model type not valid: " + model.ModelType);                 
            }
            
            model.Nodes.Add(node);
            model.Points.Add(this.Position);
            this.Index = model.Points.Count - 1;
        }
        
        public override GeometryBase GetGeometry(GH_Model model)
        {
            throw new NotImplementedException("Node element. Geometry");
        }
        
        public override GeometryBase GetDeformedGeometry(GH_Model model)
        {
            throw new NotImplementedException("Node element. Deformed geometry");
        }
    }
}