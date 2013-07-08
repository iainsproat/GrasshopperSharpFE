namespace SharpFEGrasshopper.Core.TypeClass
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Rhino.Geometry;
    using SharpFE;

    public class GH_Model : SimpleGooImplementation
    {
        public FiniteElementModel Model
        {
            get;
            set;
        }
        
        public FiniteElementResults Results
        {
            get;
            set;
        }
        
        public IDictionary<IFiniteElementNode, Point3d> Points
        {
            get;
            private set;
        }
        
        public List<GH_Element<IFiniteElement>> Elements
        {
            get;
            set;
        }
        
        public List<GH_Load> Loads
        {
            get;
            set;
        }
        
        public List<GH_Support> Supports
        {
            get;
            set;
        }
        
        public SharpFE.ModelType ModelType
        {
            get;
            set;
        }

        public GH_Model(ModelType modelType)
        {
            this.ModelType = modelType;
            this.Model = new FiniteElementModel(ModelType);
            this.Points = new Dictionary<IFiniteElementNode, Point3d>();
            this.Results = null;
            this.Elements = new List<GH_Element<IFiniteElement>>();
            this.Loads = new List<GH_Load>();
            this.Supports = new List<GH_Support>();
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
        
        public Vector3d GetNodeDisplacement(Point3d pos)
        {
            if (this.Results == null)
            {
                throw new InvalidOperationException("Results not availiable, please solve model first");
            }

            Vector3d vector = new Vector3d();
            IFiniteElementNode node = this.Points.First(kvp =>  kvp.Value == pos).Key;

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
                    throw new NotImplementedException(string.Format("No such model type: {0}", this.ModelType));
            }
            
            return vector;
        }
        
        public Vector3d GetNodeReaction(Point3d pos)
        {
            if (this.Results == null)
            {
                throw new InvalidOperationException("Results not availiable, please solve model first");
            }
            
            Vector3d vector = new Vector3d();
            IFiniteElementNode node = this.Points.First(kvp=>kvp.Value == pos).Key;
            ReactionVector reaction = this.Results.GetReaction(node);
            vector.X = reaction.X;
            vector.Y = reaction.Y;
            vector.Z = reaction.Z;
            return vector;
        }
        
        public Point3d GetDisplacedPoint(Point3d pos)
        {
            return pos + this.GetNodeDisplacement(pos);
        }

        public void AssembleSharpModel()
        {
            foreach (GH_Element<IFiniteElement> element in this.Elements)
            {
                element.ToSharpElement(this);
            }
            
            foreach (GH_Support support in this.Supports)
            {
                support.ToSharpSupport(this);
            }

            foreach (GH_Load load in this.Loads)
            {
                load.ToSharpLoad(this);
            }
        }
    }
}