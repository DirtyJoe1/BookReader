using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System;
using System.Collections.Concurrent;

namespace BookReader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ConcurrentDictionary<string, string> files = new(); //Обычный словарь иногда выдаёт ошибку привязки данных, может не добавить в PdfView.Source ключ
        public MainWindow()
        {
            InitializeComponent();
            RecentPdfsLabel.Visibility = Visibility.Collapsed;
            RecentPdfs.Visibility = Visibility.Collapsed;
        }

        private void FileOpenButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = "PDF Files|*.pdf"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                PdfView.Source = new Uri(filePath);
                if (files.TryAdd(Path.GetFileNameWithoutExtension(filePath), filePath))
                {
                    RecentPdfs.ItemsSource = files.Keys;
                }
                else
                {
                    MessageBox.Show("Такой файл уже был добавлен, вы его можете открыть из списка");
                }
                RecentPdfs.ItemsSource = files.Keys;
            }
            RecentPdfs.Visibility = Visibility.Visible;
            RecentPdfsLabel.Visibility = Visibility.Visible;
        }
        private void RecentPdfs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PdfView.Source = new Uri(files[RecentPdfs.SelectedItem.ToString()]);
        }
    }
}
