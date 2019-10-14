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
using System.Collections;

namespace Memory
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       

        public MainWindow()
        {
            InitializeComponent();
        }

        public int ComprobarDificultad(ref ArrayList caracteres)
        {
            if ((bool)FacilRadio.IsChecked)
            {
                caracteres = LlenarArray(6);
                return 3;
            }
            else if ((bool)MediaRadio.IsChecked)
            {
                caracteres = LlenarArray(8);
                return 4;
            }
            else
            {
                caracteres = LlenarArray(10);
                return 5;
            }
        }

        public ArrayList LlenarArray(int parejas)
        {
            int ultimoAscii = 97 + parejas;

            ArrayList array = new ArrayList();
            for (int i = 97; i < ultimoAscii; i++)
            {
                array.Add((char)i);
                array.Add((char)i);
            }

            return array;
        }

        public void CrearBotones(int filas, ArrayList caracteres)
        {
            // Creamos las filas
            for (int i = 0; i < filas; i++)
            {
                TarjetasGrid.RowDefinitions.Add(new RowDefinition());
            }

            int contadorArray = 0;
            // Creamos los tarjetas
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < TarjetasGrid.ColumnDefinitions.Count; j++)
                {
                    Border bordeBoton = new Border();
                    Viewbox botonVB = new Viewbox();
                    TextBlock botonTB = new TextBlock();

                    bordeBoton.Child = botonVB;
                    botonVB.Child = botonTB;
                    botonTB.FontFamily = new FontFamily("Wingdings");
                    botonTB.Text = caracteres[contadorArray].ToString();

                    contadorArray++;

                    Grid.SetRow(bordeBoton, i);
                    Grid.SetColumn(bordeBoton, j);

                    TarjetasGrid.Children.Add(bordeBoton);
                }
            }
            
        }

        private void IniciarButton_Click(object sender, RoutedEventArgs e)
        {
            //Creamos un ArrayList que contendra los caracteres
            ArrayList caracteres = new ArrayList();

            TarjetasGrid.RowDefinitions.Clear();

            int filas = ComprobarDificultad(ref caracteres);
            CrearBotones(filas, caracteres);
        }
    }
}
