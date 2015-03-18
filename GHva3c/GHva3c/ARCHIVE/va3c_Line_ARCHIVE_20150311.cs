﻿using System;
using System.Dynamic;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Timers;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using GHva3c.Properties;

using Newtonsoft.Json;

namespace GHva3c
{
    public class va3c_Line_ARCHIVE_20140311 : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the va3c_Line class.
        /// </summary>
        public va3c_Line_ARCHIVE_20140311()
            : base("vA3C_Line", "vA3C_Line",
                "Creates a vA3C line",
                "vA3C", "geometry")
        {
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.hidden;
            }
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddLineParameter("Line", "L", "A line to convert into a va3c JSON representation of the line", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Line JSON", "Lj", "Line JSON output to feed into the scene compiler component", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //local variables
            GH_Line line = null;

            //catch inputs and populate local variables
            if (!DA.GetData(0, ref line))
            {
                return;
            }

            //create JSON from line
            string outJSON = lineJSON(line.Value);
            
            //output results
            DA.SetData(0, outJSON);

        }

        private string lineJSON(Line line)
        {
            //create a dynamic object to populate
            dynamic jason = new ExpandoObject();

            //top level properties
            jason.uuid = Guid.NewGuid();
            jason.type = "Geometry";
            jason.data = new ExpandoObject();

            //populate data object properties
            jason.data.vertices = new object[6];
            jason.data.vertices[0] = Math.Round( line.FromX * -1.0 , 5);
            jason.data.vertices[1] = Math.Round(line.FromZ, 5);
            jason.data.vertices[2] = Math.Round(line.FromY, 5);
            jason.data.vertices[3] = Math.Round(line.ToX * -1.0, 5);
            jason.data.vertices[4] = Math.Round(line.ToZ, 5);
            jason.data.vertices[5] = Math.Round(line.ToY, 5);
            jason.data.normals = new object[0];
            jason.data.uvs = new object[0];
            jason.data.faces = new object[0];
            jason.data.scale = 1;
            jason.data.visible = true;
            jason.data.castShadow = true;
            jason.data.receiveShadow = false;


            //return
            return JsonConvert.SerializeObject(jason);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                return Resources.LINE_01_OLD;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{d575e0de-e3a3-4c1b-a91a-06cf271f1f35}"); }
        }
    }
}