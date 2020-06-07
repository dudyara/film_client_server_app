﻿using System.Windows;
using System.Windows.Controls;



namespace Lab4
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModel NM;
        public MainWindow()
        {
            InitializeComponent();
            NM = new ViewModel();
            DataContext = NM;
        }
        

        //нажатие кнопки загрузить
        private void OpenButton_Clik(object sender, RoutedEventArgs e)
        {
            NM.LoadConteiner();
        }

        //Нажатие кнопки сохранить
        private void SaveButton_Clic(object sender, RoutedEventArgs e)
        {

            NM.SaveConteiner();
        }

        //атоматическая генерация заголовков
        private void DG1_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            string headername = e.Column.Header.ToString();

            //Выбираем заголовок который не хотим генерировать
            if (headername == "Id")
            {
                e.Cancel = true;
            }

        }

        private void Window_Close(object sender, System.EventArgs e)
        {
            NM.Close();
        }
    }
}
