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
using System.Windows.Threading;
using System.Timers;
using System.Threading;

namespace Memory
{
    
    public partial class MainWindow : Window
    {
        int cantidadPulsadas = 0;
        Border objetoGuardado = null;
        static Random semilla = new Random();
        const int interrogante = 78;

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
                    bordeBoton.Style = (Style) Resources["Tarjetas"];

                    int posicionAleatoria = semilla.Next(caracteres.Count);
                    botonTB.Text = Convert.ToChar(interrogante).ToString();
                    bordeBoton.Tag = caracteres[posicionAleatoria].ToString();

                    Grid.SetRow(bordeBoton, i);
                    Grid.SetColumn(bordeBoton, j);

                    bordeBoton.MouseDown += BordeBoton_MouseDown;

                    TarjetasGrid.Children.Add(bordeBoton);

                    caracteres.RemoveAt(posicionAleatoria);
                }
            }
            
        }

        private void BordeBoton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Border borde1 = (Border)sender;
            Viewbox view1 = (Viewbox)borde1.Child;
            TextBlock texto1 = (TextBlock)view1.Child;

            Border borde2 = null;
            Viewbox view2 = null;
            TextBlock texto2 = null;

            if (objetoGuardado != null)
            {
                borde2 = objetoGuardado;
                view2 = (Viewbox)borde2.Child;
                texto2 = (TextBlock)view2.Child;
            }
            


            if (borde1 != borde2)
            {
                cantidadPulsadas++;

                texto1.Text = Convert.ToChar(interrogante).ToString();

                if (cantidadPulsadas == 2)
                {
                    if(texto1 == texto2)
                    {
                        borde1.Background = Brushes.Green;
                        borde2.Background = Brushes.Green;
                    }

                }
                else
                {
                    objetoGuardado = borde1;
                }
            }
            

        }

        private void IniciarButton_Click(object sender, RoutedEventArgs e)
        {
            //Creamos un ArrayList que contendra los caracteres
            ArrayList caracteres = new ArrayList();

            TarjetasGrid.RowDefinitions.Clear();
            TarjetasGrid.Children.Clear();

            int filas = ComprobarDificultad(ref caracteres);
            CrearBotones(filas, caracteres);
        }
    }
}
