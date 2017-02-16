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
        List<Node> mapNodes = new List<Node>();

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
                                newNode.NodeValue = index;                              //Set the new node's index value (Not actually required but whatever)
                                newNode.PosX = Convert.ToInt32(fValues[(index*2)-1]);     //Set the new node's X Coordinate
                                newNode.PosY = Convert.ToInt32(fValues[index*2]);     //Set the new node's Y Coordinate

                                //Create a list for the new node's path relationship data
                                List<int> lstPaths = new List<int>();
                                try
                                {
                                    for (int i = 0; i < Convert.ToInt32(fValues[0]); i++)
                                    {
                                        int curValue = Convert.ToInt32(fValues[(Convert.ToInt32(fValues[0]) * 2) + (Convert.ToInt32(fValues[0]) * index) + 1 + i]);
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

                            for (int i = 0; i < mapNodes.Count; i++)
                            {
                                MessageBox.Show("COORDINATES: " + mapNodes[i].PosX + "," + mapNodes[i].PosY);
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

        //Modifier to provide access to nodes from MainWindow class
        public List<Node> MapNodes
        {
            get { return mapNodes; }
        }
    }
}
