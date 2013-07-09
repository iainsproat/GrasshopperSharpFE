namespace SharpFEGrasshopper.Core.TypeClass
{
    using System;
    using System.Collections.Generic;
    using Rhino.Geometry;
    using SharpFE;
    
    public class GH_Model : SimpleGooImplementation
    {
        public FiniteElementModel Model
        {
            get;
            protected set;
        }
        
        public FiniteElementResults Results
        {
            get;
            protected set;
        }
        
        public IList<GH_Element> Elements
        {
            get;
            protected set;
        }
        
        public IList<GH_Load> Loads
        {
            get;
            protected set;
        }
        
        public IList<GH_Support> Supports
        {
            get;
            protected set;
        }
        
        public ModelType ModelType
        {
            get;
            protected set;
        }

        public GH_Model(ModelType modelType, IList<GH_Element> ele, IList<GH_Load> lds, IList<GH_Support> spts)
        {
            this.ModelType = modelType;
            this.Model = new FiniteElementModel(this.ModelType);
            this.Results = null;
            this.Elements = ele;
            this.Loads = lds;
            this.Supports = spts;
        }
        
        public IFiniteElementNode FindOrCreateNode(Point3d pos)
        {
            try
            {
                return this.Model.FindNodeNearTo(pos);
            }
            catch (InvalidOperationException)
            {
                switch (this.ModelType)
                {
                    case ModelType.Full3D:
                        return this.Model.NodeFactory.Create(pos.X, pos.Y, pos.Z);
                    case ModelType.Truss2D:
                        return this.Model.NodeFactory.CreateFor2DTruss(pos.X, pos.Z);
                    default:
                        throw new Exception("Model type not valid: " + this.ModelType);
                }
            }
        }

        public override string ToString()
        {
            return this.Model.ToString();
        }
        
        public void Solve()
        {
            IFiniteElementSolver solver = new LinearSolverSVD(this.Model);
            this.Results = solver.Solve();
        }
        
        public Vector3d GetNodeDisplacement(IFiniteElementNode node)
        {
            if (this.Results == null)
            {
                throw new Exception("Results not availiable, please solve model first");
            }

            Vector3d vector = new Vector3d();

            switch (this.ModelType)
            {
                case ModelType.Truss2D:
                    if (this.Model.IsConstrained(node, DegreeOfFreedom.X))
                    {
                        vector.X = 0;
                    }
                    else
                    {
                        vector.X = Results.GetDisplacement(node).X;
                    }
                    
                    if (this.Model.IsConstrained(node, DegreeOfFreedom.Z))
                    {
                        vector.Z = 0;
                    }
                    else
                    {
                        vector.Z = Results.GetDisplacement(node).Z;
                    }
                    
                    break;
                case ModelType.Full3D:
                    if (this.Model.IsConstrained(node, DegreeOfFreedom.X))
                    {
                        vector.X = 0;
                    }
                    else
                    {
                        vector.X = Results.GetDisplacement(node).X;
                    }
                    
                    if (this.Model.IsConstrained(node, DegreeOfFreedom.Y))
                    {
                        vector.Y = 0;
                    }
                    else
                    {
                        vector.Y = Results.GetDisplacement(node).Y;
                    }

                    if (this.Model.IsConstrained(node, DegreeOfFreedom.Z))
                    {
                        vector.Z = 0;
                    }
                    else
                    {
                        vector.Z = Results.GetDisplacement(node).Z;
                    }
                    
                    break;
                default:
                    throw new Exception("No such model type: " + this.ModelType);
            }
            
            return vector;
        }
        
        public Vector3d GetNodeReaction(Point3d location)
        {
            Vector3d vector = new Vector3d();
            
            if (this.Results == null)
            {
                throw new Exception("Results not availiable, please solve model first");
            }
            else
            {
                IFiniteElementNode node = this.Model.FindNodeNearTo(location);
                ReactionVector reaction = this.Results.GetReaction(node);
                vector.X = reaction.X;
                vector.Y = reaction.Y;
                vector.Z = reaction.Z;
            }
            
            return vector;
        }
        
        public Point3d GetDisplacedPoint(Point3d point)
        {
            IFiniteElementNode node = this.Model.FindNodeNearTo(point);
            return this.GetDisplacedPoint(node);
        }
        
        public Point3d GetDisplacedPoint(IFiniteElementNode node)
        {
            return node.ToPoint3d() + this.GetNodeDisplacement(node);
        }
        
        public void AssembleSharpModel()
        {
            //Loop through and create elements
            foreach (GH_Element element in Elements)
            {
                element.ToSharpElement(this);
            }
            
            //Loop through and create supports
            foreach (GH_Support support in Supports)
            {
                support.ToSharpSupport(this);
            }

            //Set loads
            foreach (GH_Load load in Loads)
            {
                load.ToSharpLoad(this);
            }
        }
    }
}