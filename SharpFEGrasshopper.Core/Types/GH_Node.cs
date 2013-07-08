namespace SharpFEGrasshopper.Core.TypeClass
{
    using System;
    using System.Linq;
    using Rhino.Geometry;
    using SharpFE;
    
    public class GH_Node
    {
        public Point3d Position { get; set; }

        public GH_Node(Point3d pos)
        {
            if (pos == null)
            {
                throw new ArgumentNullException("pos");
            }
            
            this.Position = pos;
        }

        public override string ToString()
        {
            return string.Format("Node at {0}", this.Position);
        }

        public IFiniteElementNode ToSharpElement(GH_Model model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }
            
            try
            {
                return model.Points.First(kvp => kvp.Value == this.Position).Key;
            }
            catch (InvalidOperationException)
            {
                return this.CreateNewNode(model);
            }          
        }
        
        protected IFiniteElementNode CreateNewNode(GH_Model model)
        {
            FiniteElementNode node = null;
            switch (model.ModelType)
            {
                case ModelType.Full3D:
                    node = model.Model.NodeFactory.Create(this.Position.X, this.Position.Y, this.Position.Z);
                    break;
                case ModelType.Truss2D:
                    node = model.Model.NodeFactory.CreateFor2DTruss(this.Position.X, this.Position.Z);
                    break;
                default:
                    throw new NotImplementedException(string.Format("Model type not valid: {0}", model.ModelType));
            }
            
            model.Points.Add(node, this.Position);
            return node;
        }
    }
}