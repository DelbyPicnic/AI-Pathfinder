using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pathfinder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Define global instance of Map class
        Map sysMap;

        //Define index for stepthrough scene change
        int scene = 0;
        int drawFrame = 0;
        

        public MainWindow()
        {
            InitializeComponent();
            sysMap = new Map();
        }

        private void btnLoadFile_Click(object sender, RoutedEventArgs e)
        {
            //Open the file selector window
            string targetFilePath;
            OpenFileDialog file = new OpenFileDialog();
            try
            {
                //Show the file selector window to the user
                Nullable<bool> lfDialogResult = file.ShowDialog();
                
                if (lfDialogResult == true)
                {
                    //Load the file if the user selects OK
                    targetFilePath = file.FileName;
                    lblInfo.Content = "Current Map: \n" + System.IO.Path.GetFileName(targetFilePath);
                    sysMap.LoadFile(targetFilePath);
                    drawMap();
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Something's not right: " + err.Message);
            }
        }

        private void drawMap()
        {
            try
            {
                if (sysMap.MapNodes.Count > 0)
                {
                    //Nodes have been created.
                    for (int index = 0; index < sysMap.MapNodes.Count; index++)
                    {
                        Ellipse mapPoint = new Ellipse();
                        mapPoint.Stroke = System.Windows.Media.Brushes.Red;
                        mapPoint.Width = 5;
                        mapPoint.Height = 5;
                        mapPoint.Fill = System.Windows.Media.Brushes.Red;
                        mapPoint.Margin = new Thickness(sysMap.MapNodes[index].PosX * 15, sysMap.MapNodes[index].PosY * 10, 0, 0);
                        MapViewer.Children.Add(mapPoint);
                    }
                }
                else
                {
                    //Nodes haven't been created.
                    throw new Exception("There are no defined nodes to plot on the map.");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Something's not right: " + e.Message);
            }
        }

        private void btnFindPath_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sysMap.MapNodes.Count > 0)
                {
                    List<int> route = sysMap.CalculateRoute();

                    if(route.Count() > 0)
                    {
                        btnShowProcess.IsEnabled = true;
                        int count = 0;
                        foreach(int edge in route)
                        {
                            if (count < route.Count-1)
                            {
                                Line newEdge = new Line();
                                newEdge.Stroke = Brushes.Green;
                                newEdge.X1 = sysMap.MapNodes[edge].PosX * 15;
                                newEdge.Y1 = sysMap.MapNodes[edge].PosY * 10;
                                newEdge.X2 = sysMap.MapNodes[edge].Previous.PosX * 15;
                                newEdge.Y2 = sysMap.MapNodes[edge].Previous.PosY * 10;
                                newEdge.StrokeThickness = 2;
                                MapViewer.Children.Add(newEdge);
                            }
                            count++;
                        }
                        
                    }
                    else
                    {
                        throw new Exception("Somethong has gone wrong during route calculation");
                    }
                    
                }
                else
                {
                    throw new Exception("No map has been loaded. Load a map first");
                }
            }
            catch(Exception err)
            {
                MessageBox.Show("Something's not right: " + err.Message);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            sysMap = new Map();
            MapViewer.Children.Clear();
            btnLoadFile.IsEnabled = true;
            btnFindPath.IsEnabled = true;
            btnShowProcess.IsEnabled = false;            
            btnFwd.IsEnabled = false;
        }

        private void btnShowProcess_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(sysMap.cList.Count > 0)
                {            
                    btnFwd.IsEnabled = true;
                    MapViewer.Children.Clear();

                    for (int index = 0; index < sysMap.MapNodes.Count; index++)
                    {
                        Ellipse mapPoint = new Ellipse();
                        mapPoint.Stroke = System.Windows.Media.Brushes.Red;
                        mapPoint.Width = 5;
                        mapPoint.Height = 5;
                        mapPoint.Fill = System.Windows.Media.Brushes.Red;
                        mapPoint.Margin = new Thickness(sysMap.MapNodes[index].PosX * 15, sysMap.MapNodes[index].PosY * 10, 0, 0);
                        MapViewer.Children.Add(mapPoint);
                    }
                    drawFrame = MapViewer.Children.Count;
                    MessageBox.Show(Convert.ToString(drawFrame));
                }
                else
                {
                    throw new Exception("No instructions?!");
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Something's not right: " + err.Message);
            }
        }

        private void DrawScene()
        {
            string[] INS = sysMap.cList[scene].Split(',');
            if (INS[1] == "ADD")
            {
                drawFrame++;             
                Line newEdge = new Line();
                if (INS[2] == "BLUE")
                {
                    newEdge.Stroke = Brushes.Blue;
                }
                else if(INS[2] == "BLACK")
                {
                    newEdge.Stroke = Brushes.Black;
                }
                else if(INS[2] == "GREEN")
                {
                    newEdge.Stroke = Brushes.Green;
                }                
                newEdge.X1 = Convert.ToInt32(INS[3]) * 15;
                newEdge.Y1 = Convert.ToInt32(INS[4]) * 10;
                newEdge.X2 = Convert.ToInt32(INS[5]) * 15;
                newEdge.Y2 = Convert.ToInt32(INS[6]) * 10;
                newEdge.StrokeThickness = 2;
                MapViewer.Children.Add(newEdge);              

            }
        }
        private void btnFwd_Click(object sender, RoutedEventArgs e)
        {
            if(scene < sysMap.cList.Count-1)
            {
                scene = scene +2;
                if(scene > sysMap.cList.Count - 1)
                {
                    scene = sysMap.cList.Count - 1;
                }
                DrawScene();
            }
        }
    }
}
