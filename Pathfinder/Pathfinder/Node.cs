using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Pathfinder
{
    class Node
    {
        private int nodeValue;                              //Store the indexed value for this node
        private int posX, posY;                             //Store the X & Y coordinates for this node
        private List<int> pathRelations = new List<int>();  //Store the navigation relationship between this node and every other node


        public int NodeValue                                //Modifier method for setting and retrieving the NodeValue
        {
            get { return nodeValue; }
            set { nodeValue = value; }
        }

        public int PosX                                     //Modifier method for setting and retrieving the X Coordinate
        {
            get { return posX; }
            set { posX = value; }
        }
        public int PosY                                     //Modifier method for setting and retrieving the Y Coordinate
        {
            get { return posY; }
            set { posY = value; }
        }

        public List<int> PathRelations                      //Modifier method for setting and retrieving the navigation relationship list
        {
            get { return pathRelations; }
            set { pathRelations = value; }
        }

        public bool PathToNode(int node)                    //Method to determine if an AI can pass from this node to another node 
        {
            try
            {
                if (node <= pathRelations.Count || node < 0)
                {
                    if (pathRelations[node] == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                throw new Exception("Node value is out of range for this map");
            }
            catch (Exception e)
            {
                MessageBox.Show("Something's not right: ", e.Message);
            }
            return false;
        }
    }
}
