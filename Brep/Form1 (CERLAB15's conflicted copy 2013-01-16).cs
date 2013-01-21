using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Inventor;
using IronPython.Hosting;
using IronPython.Runtime;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;




namespace Brep
{
    public partial class Form1 : Form
    {

        List<VO> listVO = new List<VO>();

        List<VO> listVO1 = new List<VO>();

        List<VO> listVO2 = new List<VO>(); //for anisotropic space

        List<EigenDecomp> EigenList = new List<EigenDecomp>(); //Use to read the nt2 files

        List<EigenSize> EigenSize = new List<EigenSize>(); //Use to read the nt2 files

        bool anysotropy = false;
        bool tensorclie = false;

        private Inventor.Application mApp = null;

        private ScriptEngine pyEngine = null;
        private ScriptRuntime pyRuntime = null;
        private ScriptScope pyScope = null;
       // private SimpleLogger _logger = new SimpleLogger();

        public Form1(Inventor.Application oApp)
        {
            InitializeComponent();
            anysotropy = false;
            SetupPython();
           // PurgeCommand();

            lbl_status.Text = "Status ...";

            //EigenDecompReadFile();
            mApp = oApp;
        }

        private void SetupPython()
        {
            if (pyEngine == null)
            {
                pyEngine = Python.CreateEngine();
                pyScope = pyEngine.CreateScope();
                //pyScope.SetVariable("log", _logger);
                //_logger.AddInfo("Python Initialized");
            }
        }


        private void EigenDecompReadFile()
        {
            string exePath = System.Windows.Forms.Application.StartupPath + @"\meshanid.nt2";
            StreamReader sr = new StreamReader(exePath); //It works with Sytem IO

            int counter = 0;

            while (!sr.EndOfStream)
            {

                string[] split = sr.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                Console.WriteLine("startWithColumn(split.ToString()): " + startWithColumn(split.ToString()).ToString());

                counter++;
                Console.WriteLine(counter.ToString());


                //float val = (float)System.Convert.ToSingle(text);


                EigenList.Add(new EigenDecomp(
                             Convert.ToString(split[0]),
                             (float)Convert.ToSingle(split[1]),
                             (float)Convert.ToSingle(split[2]),
                             (float)Convert.ToSingle(split[3])));


            }

            EigenDecomp(EigenList);

        }

        private void EigenDecomp(List<EigenDecomp> EigenList)
        {

            float T;
            float D;
            float L1;
            float L2;

               string exePath = System.Windows.Forms.Application.StartupPath + @"\posanisiz.txt";

               using (System.IO.StreamWriter file = new System.IO.StreamWriter(exePath, true))
               {
                   //file.WriteLine("POINTS");

                   for (int i = 0; i < EigenList.Count; i++)
                   {
                      // Console.WriteLine("Tip total/rate:" + ControlChars.Tab + "{0,8:c} ({1:p1})", tip, tipRate)

                       T = (float)(EigenList[i].m11 + EigenList[i].m22);
                       D = EigenList[i].m11 * EigenList[i].m22 - EigenList[i].m12 * EigenList[i].m12;

                       L1 = T / 2 + (float)Math.Pow((T * T) / 4 + D, 1 / 2);
                       L2 = T / 2 + (float)Math.Pow((T * T) / 4 - D, 1 / 2);

                       file.WriteLine(L1.ToString() + "\t" + L2.ToString());

                       EigenSize.Add(new EigenSize(
                           Convert.ToSingle(L1),
                           Convert.ToSingle(L2)));
                    
                   }

                   file.Close();

               }


        }


        private String CreatePythonScript1()
        {
            String result = "";
            string[] lines =
                {
                 //   @"def DoIt1(logObj):",
                  //  @"   logObj.AddInfo('Executed in a function call using log object input.')",
                   // @"",
                    @"# Function definition is here",
                    @"sum = lambda arg1, arg2: arg1 + arg2;",

                    @"# Now you can call sum as a function",
                    @"print sum( 10, 20 )",
                    @"print sum( 20, 20 )",
                  
                };
            result = String.Join("\r", lines);
            return result;
        }



        private void Button1_Click(object sender, EventArgs e)
        {
            if (((mApp.ActiveDocument != null)))
            {
                if ((mApp.ActiveDocument.DocumentType == DocumentTypeEnum.kPartDocumentObject))
                {
                    PartDocument oDoc = mApp.ActiveDocument as PartDocument;

                    if ((oDoc.SelectSet.Count == 1))
                    {
                        if (((oDoc.SelectSet[1]) is Edge))
                        {
                            Edge oEdge = oDoc.SelectSet[1] as Edge;

                            CurveEvaluator oCurveEval = oEdge.Evaluator;

                            double MinParam = 0;
                            double MaxParam = 0;

                            oCurveEval.GetParamExtents(out MinParam, out MaxParam);

                            double length = 0;
                            oCurveEval.GetLengthAtParam(MinParam, MaxParam, out length);

                            double MidParam = 0;
                            oCurveEval.GetParamAtLength(MinParam, length * 0.5, out MidParam);

                            double[] Params = { MidParam };

                            double[] Points = new double[3 * Params.Length];
                            oCurveEval.GetPointAtParam(ref Params, ref Points);

                            double[] Tangents = new double[3 * Params.Length];
                            oCurveEval.GetTangent(ref Params, ref Tangents);


                            string strResult = "Curve Properties: \n\n";

                            strResult += " - Length: " + length.ToString("F2") + "\n\n";

                            strResult += " - Middle point: [" + Points[0].ToString("F2") + ", " + Points[1].ToString("F2") + ", " + Points[2].ToString("F2") + "]" + "\n\n";

                            strResult += " - Tangent: [" + Tangents[0].ToString("F2") + ", " + Tangents[1].ToString("F2") + ", " + Tangents[2].ToString("F2") + "]" + "\n\n";


                            Inventor.Point pos = mApp.TransientGeometry.CreatePoint(Points[0], Points[1], Points[2]);

                            oDoc.ComponentDefinition.WorkPoints.AddFixed(pos, false);

                            UnitVector dir = mApp.TransientGeometry.CreateUnitVector(Tangents[0], Tangents[1], Tangents[2]);

                            oDoc.ComponentDefinition.WorkAxes.AddFixed(pos, dir, false);

                            System.Windows.Forms.MessageBox.Show(strResult, "Curve Evaluator");

                            return;
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("Selected entity must be an Edge", "Curve Evaluator");
                            return;
                        }
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("A single Edge must be selected first", "Curve Evaluator");
                        return;
                    }
                }
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {

            pBar.Value = 0;
            anysotropy = false;

            if (((mApp.ActiveDocument != null)))
            {
                if ((mApp.ActiveDocument.DocumentType == DocumentTypeEnum.kPartDocumentObject))
                {
                    PartComponentDefinition oPartCom = mApp.ActiveDocument as PartComponentDefinition;
                    PartDocument oDoc = mApp.ActiveDocument as PartDocument;

                    //oPartCom.SurfaceBodies[1].Faces[1].get_AlternateBody(1);

                    if ((oDoc.SelectSet[1]) is Face)
                    {
                        System.Windows.Forms.MessageBox.Show("You just selected a surface", "Surface Evaluator");

                        Face oFace = oDoc.SelectSet[1] as Face;

                         double[] vertexCoordinates=new double[0];
                         double[] normalVector=new double[0];
                         int[] vertexIndices=new int[0];

                        double tolerance = 0.01;
                        int vertexCount = 0;
                        int facetCount = 0;


                        oFace.CalculateFacets(tolerance,
                            out vertexCount,
                            out facetCount,
                            out vertexCoordinates,
                            out normalVector,
                            out vertexIndices);

                        //System.Windows.Forms.MessageBox.Show(vertexCoordinates[0].ToString(), "You just selected a surface coordinate");

                                 pBar.Value = 10;
                                 pBar.Refresh();

                        Thread.Sleep(new TimeSpan(0, 0, 1));
                        lbl_status.Text = "Creating STL from Face Selected";
                        lbl_status.Refresh();
                        STL(vertexCoordinates, normalVector, vertexIndices, vertexCount,facetCount);
                                pBar.Value = 33;
                                pBar.Refresh();

                        Thread.Sleep(new TimeSpan(0, 0, 3));
                        lbl_status.Text = "Reading SRF File";
                        lbl_status.Refresh();
                        ReadSRF();
                                pBar.Value = 67;
                                pBar.Refresh();

                        lbl_status.Text = "Transforming SRF File to Mesh";
                        lbl_status.Refresh();
                        Thread.Sleep(new TimeSpan(0, 0, 3));
                        SRFtoMesh();

                                 pBar.Value = 100;
                                 pBar.Refresh();
                 
                       
                        return;



                    }

                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Create a string array that consists of three lines.
            string[] lines = { "First line", "Second line", "Third line" };

            string exePath = System.Windows.Forms.Application.StartupPath + @"\WriteLines.txt";

            System.IO.File.WriteAllLines(exePath, lines);


        }

        private void STL(   double[] vertexCoordinates,
                            double[] normalVector,
                            int[] vertexIndices,
                            int vertexCount,
                            int facetCount )
        {

            string exePath = System.Windows.Forms.Application.StartupPath + @"\input.stl";

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(exePath, true))
            {
                
                file.WriteLine("solid examplar");


                int Kcount = 0;
                int Jcount = 0;

                for (int i = 0; i < vertexIndices.Length; i++)
                {
                    if(Kcount<=facetCount)
                    {
                        if (i % 3 == 0)
                        {

                            file.WriteLine("\t facet normal\t" + normalVector[(vertexIndices[i] - 1) * 3] +
                        "\t" + normalVector[(vertexIndices[i] - 1) * 3 + 1] +
                        "\t" + normalVector[(vertexIndices[i] - 1) * 3 + 2]);

                            file.WriteLine("\t\t outer loop");
                            Jcount = i + 2;

                            Kcount++;
                        }

                        file.WriteLine("\t\t\t vertex\t" + vertexCoordinates[(vertexIndices[i] - 1) * 3] +
                        "\t" + vertexCoordinates[(vertexIndices[i] - 1) * 3 + 1] +
                        "\t" + vertexCoordinates[(vertexIndices[i] - 1) * 3 + 2]);

                        if (i==Jcount)
                        {
                            file.WriteLine("\t\t endloop");


                           // Kcount++;
                        }


                    }



                }

                  file.WriteLine("endsolid examplar");

                  file.Close();
            }

            
            string strCmdText;
           // strCmdText = "/C volmesh --GEP --PGEP -infile input.stl -ceaddnonmanifold -cereconsider -osrm output.srf";
            
            strCmdText = "/C volmesh --GEP --PGEP -infile input.stl -invert -reorient -ceaddnonmanifold -ceangthr 60 -scale 1 1 0 -osrm output.srf";

            Console.WriteLine("/C volmesh --GEP --PGEP -infile input.stl -invert -reorient -ceaddnonmanifold -ceangthr 60 -scale 1 1 0 -osrm output.srf");

            System.Diagnostics.Process.Start("CMD.exe", strCmdText);

            Console.WriteLine("MESH COMPLETED");

        }

        private void SRFtoMesh()
        {

            double bsize = Convert.ToDouble(_tbx_bubblesize.Text);// * 0.8; // *0.6;
                string strCmdText;
               // strCmdText = "/C volmesh --BUB --PBUB -infile output.srf -out meshd.srf -autoboundary -tfd U"+bsize+" -e 666 -f 3500";


        /*
                strCmdText = "/C volmesh --BUB --PBUB -infile output.srf -out meshd.srf -autoboundary -tfd  U" + bsize + " -e 1200 -f 4800";
                Console.WriteLine(strCmdText);
                Thread.Sleep(new TimeSpan(0, 0, 5));
                //desired size  * 0.6 = bubble size
               
             
                System.Diagnostics.Process.Start("CMD.exe", strCmdText);
*/
                
   
            // -f can be cheap for background mesh in Ved's COde
           //    strCmdText = "/C volmesh --BUB --PBUB -infile output.srf -out meshd.srf -autoboundary -tfd  U" + bsize + " -e 100 -f 400";

                strCmdText = "/C volmesh --BUB --PBUB -infile output.srf -out meshd.srf -autoboundary -tfd U" + bsize + "  -e 1200 -f 4800";
                Console.WriteLine(strCmdText);
                Thread.Sleep(new TimeSpan(0, 0, 10));
                System.Diagnostics.Process.Start("CMD.exe", strCmdText);
          
          //2013
              //  strCmdText = "/C del output.srf input.stl";
              //  System.Diagnostics.Process.Start("CMD.exe", strCmdText);

              //  Thread.Sleep(new TimeSpan(0, 0, 1));

                Console.WriteLine("MESH COMPLETED");


        }




       private void ReadSRF()
        {
            string exePath = System.Windows.Forms.Application.StartupPath + @"\output.srf";
            StreamReader sr = new StreamReader(exePath); //It works with Sytem IO

            bool foundF = false;
            int counter = 0;

            while (!sr.EndOfStream && foundF==false)
            {

                string[] split = sr.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                Console.WriteLine("startWithColumn(split.ToString()): " + startWithColumn(split.ToString()).ToString());


                if (split[0]=="SURF")
                {
                    Console.WriteLine("FOUND SURF" );

                }
                else if (split[0] == "F")
                {

                    Console.WriteLine("FOUND F");
                    foundF = true;
                }
                else
                {
                    counter++;
                    Console.WriteLine(counter.ToString());

                    listVO.Add(new VO(
                             Convert.ToString(split[0]),
                             Convert.ToDouble(split[1]),
                             Convert.ToDouble(split[2]),
                             Convert.ToDouble(split[3])));

                   
                }

            }

        }


        char[] columnChars = new char[] { 'S', 'U', 'R', 'F' };
        private bool startWithColumn(string toCheck)
        {
            //char c = toCheck[0];

            return toCheck != null
                       && toCheck.Length > 0
                       && columnChars.Any(x => x == toCheck[0]);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // string strCmdText;
            //  strCmdText = "/C del output.srf input.stl";
            //   System.Diagnostics.Process.Start("CMD.exe", strCmdText);


            PartDocument oDoc = default(PartDocument);
            oDoc = (PartDocument)mApp.ActiveDocument;

            // Get the XZ Plane12
            WorkPlane oWorkPlane = oDoc.ComponentDefinition.WorkPlanes[Convert.ToInt16(_tbx_select_plane.Text)];

            //oWorkPlane.Plane.Evaluator.GetNormal

            PlanarSketch oSketch = oDoc.ComponentDefinition.Sketches.Add(oWorkPlane, false);


            TransientGeometry oTG = mApp.TransientGeometry;

            ObjectCollection oFitPoints = mApp.TransientObjects.CreateObjectCollection();

            PartComponentDefinition oCompDef = default(PartComponentDefinition);
            oCompDef = oDoc.ComponentDefinition;
            Camera oCamera = mApp.ActiveView.Camera;

            oCamera.ViewOrientationType = ViewOrientationTypeEnum.kIsoTopLeftViewOrientation;
            oCamera.Apply();

            mApp.ActiveView.Fit(true);

            Point2d[,] oPointMat = new Point2d[10, 10];

            Point2d[] oPointVect = new Point2d[100];

            SketchCircle[,] oCircle = new SketchCircle[10, 10];

            SketchCircle[] oCircleVect = new SketchCircle[100];

             string exePath = "";
             if (anysotropy == true)
             {
                 exePath = System.Windows.Forms.Application.StartupPath + @"\meshanid.srf";
             }
             else
             {
                 exePath = System.Windows.Forms.Application.StartupPath + @"\meshd.srf";
             }

            StreamReader sr = new StreamReader(exePath);
            bool foundF = false;
            int lineCount = 0;

            while (!sr.EndOfStream && foundF == false)
            {

                string[] split = sr.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (split[0] == "Surf")
                {
                    Console.WriteLine("FOUND SURF");

                }
                else if (split[0] == "F")
                {

                    Console.WriteLine("FOUND F");
                    foundF = true;
                }
                else
                {
                    lineCount++;
                    listVO1.Add(new VO(
                             Convert.ToString(split[0]),
                             Convert.ToDouble(split[1]),
                             Convert.ToDouble(split[2]),
                             Convert.ToDouble(split[3])));
                }

            }

            Console.WriteLine("COUNTER LINES: " + lineCount.ToString());

            Profile oProfile = default(Profile);

            ExtrudeDefinition oExtrudeDef = default(ExtrudeDefinition);

            ExtrudeFeature oExtrude = default(ExtrudeFeature);

            Point2d oPoint2d = mApp.TransientGeometry.CreatePoint2d(0, 0);
            //613

            for (int i = 0; i < lineCount; i++) //chnage here the number of bubbles used
            {
                if (Convert.ToInt16(_tbx_select_plane.Text) == 1)
                {
                    oPoint2d.X = listVO1[i].value2;
                    oPoint2d.Y = listVO1[i].value3;

                 ///   oSketch.SketchCircles.AddByCenterRadius(mApp.TransientGeometry.CreatePoint2d(listVO1[i].value2,
                 ///      listVO1[i].value3), Convert.ToDouble(_tbx_bubblesize.Text) / 2); //change here the value for the araius to be use

                //    oSketch.SketchCircles.AddByCenterRadius(oPoint2d, Convert.ToDouble(_tbx_bubblesize.Text) / 2); //change here the value for the araius to be use


                    if (anysotropy == true)
                    {
                        oSketch.SketchCircles.AddByCenterRadius(oPoint2d, EigenSize[i].d2/4.0); //change here the value for the araius to be use

                    }
                    else
                    {
                        oSketch.SketchCircles.AddByCenterRadius(oPoint2d, Convert.ToDouble(_tbx_bubblesize.Text) / 2); //change here the value for the araius to be use

                    }
                }
                else if (Convert.ToInt16(_tbx_select_plane.Text) == 2)
                {

                    oPoint2d.X = -listVO1[i].value1;
                    oPoint2d.Y = listVO1[i].value3;

                    if (anysotropy == true)
                    {
                        oSketch.SketchCircles.AddByCenterRadius(oPoint2d, EigenSize[i].d2/4.0); //change here the value for the araius to be use

                    }
                    else
                    {
                        oSketch.SketchCircles.AddByCenterRadius(oPoint2d, Convert.ToDouble(_tbx_bubblesize.Text) / 4); //change here the value for the araius to be use

                    }
                  
                    //oSketch.SketchCircles.AddByCenterRadius(mApp.TransientGeometry.CreatePoint2d(-listVO1[i].value1,
                     //   listVO1[i].value3), Convert.ToDouble(_tbx_bubblesize.Text) / 2); //change here the value for the araius to be use

                }

                else if (Convert.ToInt16(_tbx_select_plane.Text) == 3)
                {
                   /// oSketch.SketchCircles.AddByCenterRadius(mApp.TransientGeometry.CreatePoint2d(listVO1[i].value1,
                   ///   listVO1[i].value2), Convert.ToDouble(_tbx_bubblesize.Text) / 2); //change here the value for the araius to be use

                    oPoint2d.X = listVO1[i].value1;
                    oPoint2d.Y = listVO1[i].value2;

                    if (anysotropy == true)
                    {
                        oSketch.SketchCircles.AddByCenterRadius(oPoint2d, EigenSize[i].d2/4.0); //change here the value for the araius to be use

                    }
                    else
                    {
                        oSketch.SketchCircles.AddByCenterRadius(oPoint2d, Convert.ToDouble(_tbx_bubblesize.Text) / 2); //change here the value for the araius to be use
            
                    }
                }

                else if (Convert.ToInt16(_tbx_select_plane.Text) == 4)
                {

                    oSketch.SketchCircles.AddByCenterRadius(mApp.TransientGeometry.CreatePoint2d(listVO1[i].value1,
                   listVO1[i].value2), Convert.ToDouble(_tbx_bubblesize.Text) / 2); //change here the value for the araius to be use

                    /*
                    oSketch.SketchCircles.AddByCenterRadius(mApp.TransientGeometry.CreatePoint(listVO1[i].value1,
                      listVO1[i].value2, listVO1[i].value3), Convert.ToDouble(_tbx_bubblesize.Text) / 2); //change here the value for the araius to be use
                    */
                }


                // oSketch.SketchCircles.AddByCenterRadius(mApp.TransientGeometry.CreatePoint2d(listVO[i].value2,
                //      listVO[i].value3 ), 0.15); //change here the value for the araius to be use


            }


            oProfile = oSketch.Profiles.AddForSolid();

            oExtrudeDef = oCompDef.Features.ExtrudeFeatures.CreateExtrudeDefinition(oProfile,
               PartFeatureOperationEnum.kCutOperation);

            oExtrudeDef.SetDistanceExtent(Convert.ToDouble(_tb_extru_dim.Text) / 10, PartFeatureExtentDirectionEnum.kSymmetricExtentDirection);

            oExtrude = oCompDef.Features.ExtrudeFeatures.Add(oExtrudeDef);


        }

        private void pBarCounter(int postionBar)
        {
            if (postionBar >=100) postionBar = 100;
            for (int i = 0; i < postionBar; i++) //chnage here the number of bubbles used
            {
                pBar.Value= i; //progress bar back to zero
            }
        
        }



        private void PurgeLists(List<VO> listV)
        {

            for (int i = listV.Count - 1; i >= 0; i--)
            {
                if (listV.Count > 0)
                    listV.RemoveAt(i);
            }


        }

        private void PurgeCommand()
        {
            string strCmdText;

            lbl_status.Text = "Deleting STL and SRF Files ...";
            lbl_status.Refresh();


            strCmdText = "/C del input.stl *.srf *.tfd *.nt2 *.srm";

            pBarCounter(100);

            Thread.Sleep(new TimeSpan(0, 0, 1));

            System.Diagnostics.Process.Start("CMD.exe", strCmdText);

            lbl_status.Text = "No more files here...";
            lbl_status.Refresh();

            PurgeLists(listVO);
            PurgeLists(listVO1);
            PurgeLists(listVO2);
           



            pBar.Value = 0;
            Thread.Sleep(new TimeSpan(0, 0, 1));
                     
        }


        private void button5_Click(object sender, EventArgs e)
        {
            PurgeCommand();

          //   listVO = null;
         //    listVO1 = null;
          //   listVO2 = null;


        }

        private void button6_Click(object sender, EventArgs e)
        {
             if (((mApp.ActiveDocument != null)))
            {
                if ((mApp.ActiveDocument.DocumentType == DocumentTypeEnum.kPartDocumentObject))
                {
                    PartComponentDefinition oPartCom = mApp.ActiveDocument as PartComponentDefinition;
                    PartDocument oDoc = mApp.ActiveDocument as PartDocument;

                    //oPartCom.SurfaceBodies[1].Faces[1].get_AlternateBody(1);

                        System.Windows.Forms.MessageBox.Show("You just selected a surface", "Surface Evaluator");

                        Inventor.PartDocument partDoc = (Inventor.PartDocument)mApp.ActiveDocument;

                        Inventor.Face oFace = (Inventor.Face)mApp.CommandManager.Pick(Inventor.SelectionFilterEnum.kPartFacePlanarFilter, "Pick a face.");

                        Inventor.PlanarSketch oSketch = partDoc.ComponentDefinition.Sketches.Add(oFace, false);


                        PartComponentDefinition oCompDef = default(PartComponentDefinition);
                        oCompDef = oDoc.ComponentDefinition;


                        string exePath = System.Windows.Forms.Application.StartupPath + @"\meshd.srf";

                        StreamReader sr = new StreamReader(exePath);
                        bool foundF = false;
                        int lineCount = 0;

                        while (!sr.EndOfStream && foundF == false)
                        {

                            string[] split = sr.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                            //     Console.WriteLine("startWithColumn(split.ToString()): " + startWithColumn(split.ToString()).ToString());


                            if (split[0] == "Surf")
                            {
                                Console.WriteLine("FOUND SURF");

                            }
                            else if (split[0] == "F")
                            {

                                Console.WriteLine("FOUND F");
                                foundF = true;
                            }
                            else
                            {
                                lineCount++;
                                listVO1.Add(new VO(
                                         Convert.ToString(split[0]),
                                         Convert.ToDouble(split[1]),
                                         Convert.ToDouble(split[2]),
                                         Convert.ToDouble(split[3])));
                            }

                        }

                        Console.WriteLine("COUNTER LINES: " + lineCount.ToString());

                        Profile oProfile = default(Profile);

                        ExtrudeDefinition oExtrudeDef = default(ExtrudeDefinition);

                        ExtrudeFeature oExtrude = default(ExtrudeFeature);
                    
                        for (int i = 0; i < lineCount; i++) //chnage here the number of bubbles used
                        {
                            if (Convert.ToInt16(_tbx_select_plane.Text) == 1)
                            {

                                oSketch.SketchCircles.AddByCenterRadius(mApp.TransientGeometry.CreatePoint2d(listVO1[i].value2,
                                    listVO1[i].value3), Convert.ToDouble(_tbx_bubblesize.Text) / 2); //change here the value for the araius to be use

                            }
                            else if (Convert.ToInt16(_tbx_select_plane.Text) == 2)
                            {
                                oSketch.SketchCircles.AddByCenterRadius(mApp.TransientGeometry.CreatePoint2d(-listVO1[i].value1,
                                    listVO1[i].value3), Convert.ToDouble(_tbx_bubblesize.Text) / 2); //change here the value for the araius to be use

                            }

                            else if (Convert.ToInt16(_tbx_select_plane.Text) == 3)
                            {
                                oSketch.SketchCircles.AddByCenterRadius(mApp.TransientGeometry.CreatePoint2d(listVO1[i].value1,
                                  listVO1[i].value2), Convert.ToDouble(_tbx_bubblesize.Text) / 2); //change here the value for the araius to be use

                            }

                            else if (Convert.ToInt16(_tbx_select_plane.Text) == 4)
                            {

                                oSketch.SketchCircles.AddByCenterRadius(mApp.TransientGeometry.CreatePoint(listVO1[i].value1,
                                  listVO1[i].value2, listVO1[i].value3), Convert.ToDouble(_tbx_bubblesize.Text) / 2); //change here the value for the araius to be use
                                //This is not working because the points have to be on top of the plane.... chnage the coordinate s of the plane to make sure it works


                            }


                            // oSketch.SketchCircles.AddByCenterRadius(mApp.TransientGeometry.CreatePoint2d(listVO[i].value2,
                            //      listVO[i].value3 ), 0.15); //change here the value for the araius to be use


                        }


                        oProfile = oSketch.Profiles.AddForSolid();

                        oExtrudeDef = oCompDef.Features.ExtrudeFeatures.CreateExtrudeDefinition(oProfile,
                           PartFeatureOperationEnum.kCutOperation);

                        oExtrudeDef.SetDistanceExtent(Convert.ToDouble(_tb_extru_dim.Text) / 10, PartFeatureExtentDirectionEnum.kSymmetricExtentDirection);

                        oExtrude = oCompDef.Features.ExtrudeFeatures.Add(oExtrudeDef);


                }
             }

   }

        private void button7_Click(object sender, EventArgs e)
        {
            button4.Enabled = false;

            /*
            # Input mesh: 20x20C_packed.srm (an intermediate mesh that is a reasonably
            # sized, ~uniform sampling of the original domain)

            # First step: create BCs
            # Input is series of commands of the form:
            #   --insertbc pos_x pos_y scope_radius m_11 m_12 m_22 --insertbc ...
            tensorquad.exe --geom 20x20C_packed.srm --bgmesh __  ^
              --insertbc  0.0  0.0   2.5    2.0  0.0  0.1    ^
              --insertbc -9.5 -9.5   2.5    2.0  0.0  2.05   ^
              --insertbc  9.5 -9.5   2.5    2.0  0.0  2.05   ^
              --insertbc -9.5  9.5   2.5    0.1  0.0  0.105  ^
              --insertbc  9.5  9.5   2.5    0.1  0.0  0.105  ^
              --savebgmesh insert.srf --savefield insert.nt2
            */

            pBar.Value = 0;
            //CHECK that the boundaries are correct
            string strCmdText;
            strCmdText = "/C tensorcli.exe --geom meshd.srf --disablepadbgmesh --bgmesh __ " // __ means mesh.srf
             + "--insertbc " + " 44 -8  0 1.15 0.0  0.1  "
             + "--insertbc " + " 35 -8  0 0.7 0.0  0.2  "
             + "--insertbc " + " 38 -15  0 0.9 0.0  0.5 "
             + "--insertbc " + " 42 -20  0 0.9 0.0  0.5 "
             + "--insertbc " + " 44 -25  0 1.15 0.0  0.02 "
           //  + "--insertbc " + " 50 -8  0 1.0 0.0  1.15  "


             + " --savebgmesh insert.srf --savefield insert.nt2 --persevere"; 
            System.Diagnostics.Process.Start("CMD.exe", strCmdText);
            pBar.Value = 10;
            pBar.Refresh();

            lbl_status.Text = "Creating insert.srf and insert.nt2 files";
            lbl_status.Refresh();

            Thread.Sleep(new TimeSpan(0, 0, 15));
            pBar.Value = 50;
            pBar.Refresh();

            /*
            :: Second step: generate field and export
            tensorquad.exe --geom insert.srf --disablepadbgmesh --bgmesh __ ^
              --field insert.nt2                                        ^
              --enablelogeuclideaninterp --enablepseudotensors          ^
              --genfield 1e-6 --genexteriorfield                        ^
              --savefield tensor.nt2 --savetfdlattice tensor.tfd 100 100
            */

          
            string strCmdText1;
            strCmdText1 = "/C  tensorcli.exe --geom insert.srf --bgmesh __ "
                + "--field insert.nt2 --enablelogeuclideaninterp --enablepseudotensors "
                + "--genfield 1e-6 --genexteriorfield "
                + "--savefield tensor.nt2 --savetfdlattice tensor.tfd 100 100";

           
            lbl_status.Text = "Creating tensor.nt2 and tensor.tfd files";
            lbl_status.Refresh();

            System.Diagnostics.Process.Start("CMD.exe", strCmdText1);

            pBar.Value = 60;
            pBar.Refresh();
            Thread.Sleep(new TimeSpan(0, 0, 15));

             /*:: Outputs:
               :: 1) tensor.tfd: to provide to VolMesh
               :: 2) insert.srf and tensor.nt2: mesh containing vertices and file containing
               :: tensor data on vertices
               :: Test: meshes using the produced TFD
               volmesh.exe --BUB --PBUB -infile meshd.srm -tfd tensor.tfd ^
               -remeshqualitymode -out test.srm -e 1200 -f 400

               volmesh.exe --BUB --PBUB -infile meshd.srm -tfd tensor.tfd ^
               -remeshqualitymode -qdom -out test_qdom.srm -e 1200 -f 400
             */

           
             string strCmdText2;
             strCmdText2 = "/C volmesh --BUB --PBUB -infile meshd.srf -tfd tensor.tfd"
                + " -remeshqualitymode -out meshanid.srf -e 1200 -f 400";

             lbl_status.Text = "Creating  meshanid.srf file ";
             lbl_status.Refresh();

            System.Diagnostics.Process.Start("CMD.exe", strCmdText2);
            pBar.Value = 70;
            pBar.Refresh();
            Thread.Sleep(new TimeSpan(0, 0, 15));
         
            string strCmdText3; //what is this for!???
            strCmdText3 = "/C tensorcli.exe --geom insert.srf --bgmesh __ "
               + " --field tensor.nt2 --enablelogeuclideaninterp"
               + " --resamplefieldontomesh meshanid.srf meshanid.nt2";

            lbl_status.Text = "Creating meshanid.srf and meshanid.nt2 files";
            lbl_status.Refresh();

            System.Diagnostics.Process.Start("CMD.exe", strCmdText3);
            pBar.Value = 90;
            pBar.Refresh();
            Thread.Sleep(new TimeSpan(0, 0, 10));
      
            pBar.Value = 100;
            pBar.Refresh();

            anysotropy = true;
            tensorclie = true;

            EigenDecompReadFile();

            button4.Enabled = true;

        }

       

        private void pythonInterface_Click(object sender, EventArgs e)
        {
           CompileSourceAndExecute(@"MatrixDecomp");
        
        }

        private void CompileSourceAndExecute(String code)
        {
           // ScriptSource source = pyEngine.CreateScriptSourceFromString(code, SourceCodeKind.Statements); working

            ScriptSource source = pyEngine.CreateScriptSourceFromFile("fileReader.py");
            CompiledCode compiled = source.Compile();
            // Executes in the scope of Python
            compiled.Execute(pyScope);
        }

        private void TensorClient_Click(object sender, EventArgs e)
        {
            if (tensorclie == true)
            {
                lbl_status.Text = @"Opening file using Tensorquad";
                lbl_status.Refresh();
                string strCmdText; //what is this for!???
                strCmdText = "/C tensorquad.exe --geom meshanid.srf --disablepadbgmesh --bgmesh __ --field meshanid.nt2 --gui";
                System.Diagnostics.Process.Start("CMD.exe", strCmdText);

            }
            else
            {
                lbl_status.ForeColor = System.Drawing.Color.Red;
                lbl_status.Text = @"RUN ANISOTROPY FIRST";
                lbl_status.Refresh();
            }

        }



    }

    class VO
    {
        public int id { get; set; }
        public char idC { get; set; }
        public string idS { get; set; }
        public double value1 { get; set; }
        public double value2 { get; set; }
        public double value3 { get; set; }
        public double value4 { get; set; }
        public double value5 { get; set; }
        public double value6 { get; set; }

        public VO(int id, double v1, double v2, double v3, double v4, double v5, double v6)
        {
            this.id = id;
            this.value1 = v1;
            this.value2 = v2;
            this.value3 = v3;
            this.value4 = v4;
            this.value5 = v5;
            this.value6 = v6;
        }

        public VO(double v1, double v2, double v3)
        {
            this.value1 = v1;
            this.value2 = v2;
            this.value3 = v3;
        }

        public VO(char V, double v1, double v2, double v3)
        {
            this.idC = V;
            this.value1 = v1;
            this.value2 = v2;
            this.value3 = v3;
        }

        public VO(string V, double v1, double v2, double v3)
        {
            this.idS = V;
            this.value1 = v1;
            this.value2 = v2;
            this.value3 = v3;
        }



    }

    class EigenDecomp
    {
        public string idS { get; set; }
        public float m11 { get; set; }
        public float m12 { get; set; }
        public float m22 { get; set; }

        List<EigenDecomp> Lamda;

        public EigenDecomp(string V, float v1, float v2, float v3)
        {
            this.idS = V;
            this.m11 = v1;
            this.m12 = v2;
            this.m22 = v3;
        }

      


    }

    class EigenSize
    {
        public float d1 { get; set; }
        public float d2 { get; set; }

        public EigenSize(float v1, float v2)
        {
            this.d1 = v1;
            this.d2 = v2;
        }




    }
}
