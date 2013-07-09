using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

using Grasshopper.Kernel;
using Rhino;
using Rhino.Geometry;
using SharpFEGrasshopper.Core.TypeClass;


namespace SharpFEGrasshopper.Core.ClassComponent {

    public class ConstantStrainTriangleComponent : GH_Component
    {

        public ConstantStrainTriangleComponent()
            : base("Constant Strain Triangle", "T", "A new constant strain triangle", "SharpFE", "Elements")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMeshParameter("Mesh", "Mesh", "Triangular mesh", GH_ParamAccess.item);
            
            pManager.AddGenericParameter("Material", "M", "Defines material of triangle", GH_ParamAccess.item);
            pManager.AddNumberParameter("Thickness", "T", "Thickness of element", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.Register_GenericParam("Constant Strain Triangle Elements", "E", "Triangle element output");
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Mesh mesh = new Mesh();
            GH_Material material = null;
            double thickness = 0;

            // Use the DA object to retrieve the data inside the first input parameter.
            // If the retieval fails (for example if there is no data) we need to abort.
            if (!DA.GetData(0, ref mesh )) { return; }
            if (!DA.GetData(1, ref material)) { return; }
            if (!DA.GetData(2, ref thickness)) { return; }
            
            List<GH_ConstantStrainTriangle> triangles = new List<GH_ConstantStrainTriangle>();
            foreach(MeshFace face in mesh.Faces)
            {
                if (!face.IsTriangle)
                {
                    continue; //silently skip
                }
                
                Point3d p0 = mesh.TopologyVertices[face.A];
                Point3d p1 = mesh.TopologyVertices[face.B];
                Point3d p2 = mesh.TopologyVertices[face.C];
                triangles.Add(new GH_ConstantStrainTriangle(p0, p1, p2, material, thickness));
            }
            
            DA.SetDataList(0, triangles);
        }

        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("4b81c7a4-ae6f-433b-b89c-c069a0d74c8c");
            }
        }
        
        //    protected override Bitmap Icon { get { return Resources.BarComponentIcon; } }
    }
}
