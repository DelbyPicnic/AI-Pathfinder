using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Pathfinder
{
    class Map
    {
        List<Node> mapNodes = new List<Node>();             //List to store all nodes
        public List<string> cList = new List<string>();            //List to store all of the MapDraw Actions for stepthrough system

        //Function to load data from file
        public void LoadFile(string path)
        {            
            try
            {
                if (File.Exists(path))
                {
                    //Open file and read all contents
                    StreamReader fReader = File.OpenText(path);
                    string fContents = fReader.ReadToEnd();

                    //Throw exception if the selected file is empty.
                    if (fContents == "")
                    {
                        throw new Exception("Selected file is empty");
                    }

                    //Separate each value in file into an array
                    string[] fValues = fContents.Split(',');

                    //Check that all required values exist
                    if (fValues.Length == (Convert.ToInt32(fValues[0]) * 2) + (Convert.ToInt32(fValues[0]) * Convert.ToInt32(fValues[0])) + 1)
                    {
                        //All values are present
                        MessageBox.Show("Number of Nodes: " + fValues[0] + " All required values are present. T: " + Convert.ToString(fValues.Length));  //FOR DEBUG ONLY; REMOVE LATER.

                        //Create node instances for each node
                        for (int index = 0; index < Convert.ToInt32(fValues[0]); index++)
                        {
                            Node newNode = new Node();                              //Define a new node to be added to the list
                            newNode.NodeID = index;                              //Set the new node's index value (Not actually required but whatever)
                            newNode.PosX = Convert.ToInt32(fValues[(index+1)+(index)]);     //Set the new node's X Coordinate
                            newNode.PosY = Convert.ToInt32(fValues[(index+1)+(index+1)]);     //Set the new node's Y Coordinate

                            //Create a list for the new node's path relationship data
                            List<int> lstPaths = new List<int>();
                            try
                            {
                                for (int iindex = 0; iindex < Convert.ToInt32(fValues[0]); iindex++)
                                {
                                    int curValue = Convert.ToInt32(fValues[1 + (2 * Convert.ToInt32(fValues[0])) + (index * Convert.ToInt32(fValues[0])) + iindex]); ;
                                    if (curValue > 1 || curValue < 0)
                                    {
                                        throw new Exception("Path relationship value must be either 0 or 1. Check file and reload");
                                    }
                                    else
                                    {
                                        lstPaths.Add(curValue); //If something goes wrong it's probably this...
                                    }
                                }
                                newNode.PathRelations = lstPaths;
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show("Something's not right: " + e.Message);
                            }
                            mapNodes.Add(newNode);
                        }                           
                    }                    
                }
                else
                {
                    throw new Exception("Target file doesn't exist");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Something's not right: " + e.Message);
            }
        }

        public List<int> CalculateRoute()
        {
            List<int> route = new List<int>();
            try
            {
                if(mapNodes.Count > 0)
                {
                    CalcDistances();
                    route = GetRoute();
                    int count = 0;
                    foreach (int edge in route)
                    {
                        if (count < route.Count - 1)
                        {
                            cList.Add("ROUTE" + edge + ",ADD,GREEN," + mapNodes[edge].PosX + "," + mapNodes[edge].PosY + "," + mapNodes[edge].Previous.PosX + "," + mapNodes[edge].Previous.PosY);
                            count++;
                        }
                    }
                }
                else
                {
                    throw new Exception("No nodes have been loaded for this map");
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("Something's not right here: " + e.Message);
            }
            return route;
        }

        private void CalcDistances()
        {
            mapNodes[0].Distance = 0.0;
            List<Node> unvNodes = new List<Node>();
            foreach(Node curNode in MapNodes)
            {
                unvNodes.Add(curNode);
            }
            
            while (unvNodes.Count > 0)
            {
                Node uNode = mapNodes[GetNearest(unvNodes).NodeID];
                
                unvNodes.Remove(uNode);
                foreach (Node nearNode in GetNearby(uNode))
                {
                    cList.Add("EVAL" + nearNode.NodeID + ",ADD,BLUE,"+ uNode.PosX + "," + uNode.PosY + "," + nearNode.PosX + "," + nearNode.PosY);               
                    double nDist = uNode.Distance + GetDistance(uNode, nearNode);
                    if (nDist < nearNode.Distance)
                    {                        
                        mapNodes[nearNode.NodeID].Distance = nDist;
                        mapNodes[nearNode.NodeID].Previous = uNode;
                        
                    }
                    cList.Add("EVAL" + nearNode.NodeID + ",RMV,BLUE," + uNode.PosX + "," + uNode.PosY + "," + nearNode.PosX + "," + nearNode.PosY);
                    cList.Add("EVAL" + nearNode.NodeID + ",RMV,BLACK," + uNode.PosX + "," + uNode.PosY + "," + nearNode.PosX + "," + nearNode.PosY);
                }                
            }
            
        }

        private List<int> GetRoute()
        {
            List<int> nodePath = new List<int>();
            Node aNode = mapNodes[mapNodes.Count - 1];
            while (aNode.NodeID != 0)
            { 
                nodePath.Add(aNode.NodeID);
                aNode = aNode.Previous;
                
            }
            nodePath.Add(aNode.NodeID);
            return nodePath;
        }


        private List<Node> GetNearby(Node thisNode)
        {
            List<Node> nearbyNodes = new List<Node>();
            int index = 0;
            foreach(int nodePath in thisNode.PathRelations)
            {
                if(nodePath == 1)
                {
                    nearbyNodes.Add(MapNodes[index]);
                }
                index++;
            }
            return nearbyNodes;
        }

        private Node GetNearest(List<Node> nodeList)
        {
            double nDist = double.MaxValue;
            Node nNode = mapNodes[0];
            foreach(Node curNode in nodeList)
            { 
                if(curNode.Distance < nDist)
                {
                    nDist = curNode.Distance;
                    nNode = curNode;
                }
            }
            return nNode;
        }
        
        private double GetDistance(Node nodeA, Node nodeB)
        {
            double x_coord = (nodeB.PosX - nodeA.PosX);
            double y_coord = (nodeB.PosY - nodeA.PosY);
            double dist = Math.Sqrt(x_coord * x_coord + y_coord * y_coord);
            return dist;
        }

        //Modifier to provide access to nodes from MainWindow class
        public List<Node> MapNodes
        {
            get { return mapNodes; }
        }
    }
}
