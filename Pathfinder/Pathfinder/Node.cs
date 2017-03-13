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
        private int nodeid;                                 //Store the indexed value for this node
        private double distance = double.MaxValue;          //Store the distance to this node - Init as max double value
        private Node prevNode;                              //Store the previous node to this one in the route
        private int posX, posY;                             //Store the X & Y coordinates for this node
        private List<int> pathRelations = new List<int>();  //Store the navigation relationship between this node and every other node


        public int NodeID                                   //Modifier method for setting and retrieving the NodeValue
        {
            get { return nodeid; }
            set { nodeid = value; }
        }

        public double Distance                              //Modifier method for setting and retrieving the distance to this node
        {
            get { return distance; }
            set { distance = value; }
        }

        public Node Previous
        {
            get { return prevNode; }
            set { prevNode = value; }
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
    }
}
