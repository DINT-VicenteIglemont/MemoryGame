﻿using System;
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
        const char interrogante = 'N';
        bool nuevaRonda;
        int cantidadPulsadas;

        int contadorJugadas;

        Border bordeGuardado1;
        Border bordeGuardado2;

        DispatcherTimer temporizador = new DispatcherTimer();
        static Random semilla = new Random();
        
        public MainWindow()
        {
            InitializeComponent();
            temporizador.Tick += ComprobarCartas;
            temporizador.Interval = TimeSpan.FromSeconds(1);
        }

        public int ComprobarDificultad(ref ArrayList caracteres)
        {
            if ((bool)FacilRadio.IsChecked)
            {
                caracteres = LlenarArray(6);
                ProgressBar.Maximum = 6;
                return 3;
            }
            else if ((bool)MediaRadio.IsChecked)
            {
                caracteres = LlenarArray(8);
                ProgressBar.Maximum = 8;
                return 4;
            }
            else
            {
                caracteres = LlenarArray(10);
                ProgressBar.Maximum = 10;
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

                    botonTB.Style = (Style)Resources["Fuente"];
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
            Border borde = (Border)sender;
            Viewbox vb = (Viewbox)borde.Child;
            TextBlock texto = (TextBlock)vb.Child;

            if (temporizador.IsEnabled || (texto.Text != interrogante.ToString()))
            {
                return;
            }

            if (nuevaRonda)
            {
                bordeGuardado1 = null;
                bordeGuardado2 = null;
                cantidadPulsadas = 0;
                nuevaRonda = false;
                contadorJugadas++;
            }

            texto.Text = borde.Tag.ToString();
            borde.Style = (Style)Resources["TarjetaVolteada"];
            cantidadPulsadas++;

            if (cantidadPulsadas == 1)
            {
                bordeGuardado1 = borde;
            }
            else if (cantidadPulsadas == 2)
            {
                bordeGuardado2 = borde;

                temporizador.Start();

                nuevaRonda = true;
            }
        }

        private void ComprobarCartas(object sender, EventArgs e)
        {
            Border borde1 = bordeGuardado1;
            Viewbox vb1 = (Viewbox)borde1.Child;
            TextBlock texto1 = (TextBlock)vb1.Child;

            Border borde2 = bordeGuardado2;
            Viewbox vb2 = (Viewbox)borde2.Child;
            TextBlock texto2 = (TextBlock)vb2.Child;

            if(texto1.Text != texto2.Text)
            {
                texto1.Text = interrogante.ToString();
                texto2.Text = interrogante.ToString();
                borde1.Style = (Style)Resources["Tarjetas"];
                borde2.Style = (Style)Resources["Tarjetas"];

            }
            else
            {
                ProgressBar.Value++;

                borde1.Style = (Style)Resources["ParejaEncontrada"];
                borde2.Style = (Style)Resources["ParejaEncontrada"];

                if (ProgressBar.Value == ProgressBar.Maximum)
                {
                    FinDelJuego();
                }
            }

            temporizador.Stop();
            return;
        }

        private void IniciarButton_Click(object sender, RoutedEventArgs e)
        {
            //Creamos un ArrayList que contendra los caracteres
            ArrayList caracteres = new ArrayList();

            TarjetasGrid.RowDefinitions.Clear();
            TarjetasGrid.Children.Clear();

            int filas = ComprobarDificultad(ref caracteres);

            MostrarButton.IsEnabled = true;
            bordeGuardado1 = null;
            bordeGuardado2 = null;
            nuevaRonda = true;
            ProgressBar.Value = 0;
            cantidadPulsadas = 0;
            contadorJugadas = 0;

            CrearBotones(filas, caracteres);
        }

        private void MostrarButton_Click(object sender, RoutedEventArgs e)
        {
            ProgressBar.Value = ProgressBar.Maximum;

            foreach(Border borde in TarjetasGrid.Children)
            {
                Viewbox vb = (Viewbox) borde.Child;
                TextBlock texto = (TextBlock) vb.Child;

                texto.Text = borde.Tag.ToString();
                borde.Style = (Style)Resources["ParejaEncontrada"];
            }

            FinDelJuego();
        }

        public void FinDelJuego()
        {
            MessageBox.Show("Fin del juego \nRondas necesitadas: " + contadorJugadas, "Juego terminado");
        }
    }
}
