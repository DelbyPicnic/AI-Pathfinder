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
            drawMap();
        }
    }
}
