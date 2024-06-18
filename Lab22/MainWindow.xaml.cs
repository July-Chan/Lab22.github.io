using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab22
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Ініціалізація випадаючого списку шрифтів
            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            cmbFontFamily.SelectionChanged += CmbFontFamily_SelectionChanged;
            // Ініціалізація випадаючого списку розмірів шрифту
            cmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            cmbFontSize.SelectionChanged += CmbFontSize_SelectionChanged;
        }

        // Обробник зміни вибраного шрифту в ComboBox
        private void CmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TabControl.SelectedItem is TabItem selectedTabItem && selectedTabItem.Content is Document document)
            {
                ComboBox comboBox = sender as ComboBox;
                if (comboBox != null && comboBox.SelectedItem != null)
                {
                    string fontFamily = comboBox.SelectedItem.ToString();
                    document.ApplyFontFamily(new FontFamily(fontFamily));
                }
            }
        }

        // Обробник зміни вибраного розміру шрифту в ComboBox
        private void CmbFontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TabControl.SelectedItem is TabItem selectedTabItem && selectedTabItem.Content is Document document)
            {
                ComboBox comboBox = sender as ComboBox;
                if (comboBox != null && comboBox.SelectedItem != null)
                {
                    double fontSize = (double)comboBox.SelectedItem;
                    document.ApplyFontSize(fontSize);
                }
            }
        }

        private int number = 1;
        // Кнопка для створення документу (меню)
        private void NewDocument_Click(object sender, RoutedEventArgs e)
        {
            TabItem newTab = new TabItem();
            newTab.Header = $"Документ {number}";
            Document document = new Document(number);
            newTab.Content = document;
            TabControl.Items.Add(newTab);
            TabControl.SelectedItem = newTab;
            number++;
        }

        // Кнопка для відкриття документа (меню)
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                string fileText = System.IO.File.ReadAllText(filePath);

                if (TabControl.SelectedItem is TabItem selectedTabItem && selectedTabItem.Content is Document activeDocument)
                {
                    activeDocument.SetText(fileText);
                }
                else
                {
                    MessageBox.Show("Немає активного документу всередині");
                }
            }
        }

        // Кнопка для збереження документа (меню)
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (TabControl.SelectedItem is TabItem selectedTabItem && selectedTabItem.Content is Document activeDocument)
            {
                activeDocument.SaveDocument();
            }
        }

        //Кнопка для виходу з програми (меню)
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // Кнопка для вирізання тексту (меню)
        private void CutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (TabControl.SelectedItem is TabItem selectedTabItem && selectedTabItem.Content is Document activeDocument)
            {
                activeDocument.Cut();
            }
        }

        // Кнопка для копіювання тексту (меню)
        private void CopyMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (TabControl.SelectedItem is TabItem selectedTabItem && selectedTabItem.Content is Document activeDocument)
            {
                activeDocument.Copy();
            }
        }

        // Кнопка для вставки тексту (меню)
        private void PasteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (TabControl.SelectedItem is TabItem selectedTabItem && selectedTabItem.Content is Document activeDocument)
            {
                activeDocument.Paste();
            }
        }

        // Кнопка для виділення усього тексту (меню)
        private void SelectAllMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (TabControl.SelectedItem is TabItem selectedTabItem && selectedTabItem.Content is Document activeDocument)
            {
                activeDocument.SelectAll();
            }
        }

        // Кнопка для зміни мови на англійську (меню)
        private void EnglishMenuItem_Click(object sender, RoutedEventArgs e)
        {
            fileMenu.Header = "File";
            createMenuItem.Header = "Create";
            openMenuItem.Header = "Open";
            saveMenuItem.Header = "Save";
            exitMenuItem.Header = "Exit";

            editMenu.Header = "Edit";
            copyMenuItem.Header = "Copy";
            selectAllMenuItem.Header = "Select All";
            pasteMenuItem.Header = "Paste";
            cutMenuItem.Header = "Cut";
            pasteimageMenuItem.Header = "Insert image";

            languageMenu.Header = "Language";
        }

        // Кнопка для вставки зображення (меню)
        private void PasteImage_Click(object sender, RoutedEventArgs e)
        {
            if (TabControl.SelectedItem is TabItem selectedTabItem && selectedTabItem.Content is Document activeDocument)
            {
                activeDocument.InsertImage();
            }
        }

        // Кнопка для зміни мови на українську (меню)
        private void UkrainianMenuItem_Click(object sender, RoutedEventArgs e)
        {

            fileMenu.Header = "Файл";
            createMenuItem.Header = "Створити";
            openMenuItem.Header = "Відкрити";
            saveMenuItem.Header = "Зберегти";
            exitMenuItem.Header = "Вийти";

            editMenu.Header = "Редагувати";
            copyMenuItem.Header = "Копіювати";
            selectAllMenuItem.Header = "Виділити все";
            pasteMenuItem.Header = "Вставити";
            cutMenuItem.Header = "Вирізати";
            pasteimageMenuItem.Header = "Вставити зображення";

            languageMenu.Header = "Мова";
        }

        // Кнопка зміни стилю тексту на жирний (увімкнена)
        private void BoldButton_Click(object sender, RoutedEventArgs e)
        {
            if (TabControl.SelectedItem is TabItem selectedTabItem && selectedTabItem.Content is Document document)
            {
                document.ApplyBold();
            }
        }

        // Кнопка зміни стилю тексту на курсив (увімкнена)
        private void ItalicButton_Click(object sender, RoutedEventArgs e)
        {
            if (TabControl.SelectedItem is TabItem selectedTabItem && selectedTabItem.Content is Document document)
            {
                document.ApplyItalic();
            }
        }

        // Кнопка зміни стилю тексту на підкреслений (увімкнена)
        private void UnderlineButton_Click(object sender, RoutedEventArgs e)
        {
            if (TabControl.SelectedItem is TabItem selectedTabItem && selectedTabItem.Content is Document document)
            {
                document.ApplyUnderline();
            }
        }

        // Кнопка зміни стилю тексту на жирний (вимкнена)
        private void RemoveBold_Click(object sender, RoutedEventArgs e)
        {
            if (TabControl.SelectedItem is TabItem selectedTabItem && selectedTabItem.Content is Document document)
            {
                document.RemoveBold();
            }
        }

        // Кнопка зміни стилю тексту на курсив (вимкнена)
        private void RemoveItalic_Click(object sender, RoutedEventArgs e)
        {
            if (TabControl.SelectedItem is TabItem selectedTabItem && selectedTabItem.Content is Document document)
            {
                document.RemoveItalic();
            }
        }

        // Кнопка зміни стилю тексту на підкреслений (вимкнена)
        private void RemoveUnderline_Click(object sender, RoutedEventArgs e)
        {
            if (TabControl.SelectedItem is TabItem selectedTabItem && selectedTabItem.Content is Document document)
            {
                document.RemoveUnderline();
            }
        }

        // Кнопка для вставки зображення (панель інструментів)
        private void PasteImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (TabControl.SelectedItem is TabItem selectedTabItem && selectedTabItem.Content is Document activeDocument)
            {
                activeDocument.InsertImage();
            }
        }

        // Кнопка для вирівнювання тексту ліворуч (панель інструментів)
        private void LeftIcon_Click(object sender, RoutedEventArgs e)
        {
            SetAlignmentForActiveDocument(HorizontalAlignment.Left);
        }

        // Кнопка для вирівнювання тексту по центру (панель інструментів)
        private void CenterIcon_Click(object sender, RoutedEventArgs e)
        {
            SetAlignmentForActiveDocument(HorizontalAlignment.Center);
        }

        // Кнопка для вирівнювання тексту праворуч (панель інструментів)
        private void RightIcon_Click(object sender, RoutedEventArgs e)
        {
            SetAlignmentForActiveDocument(HorizontalAlignment.Right);
        }

        // Метод для встановлення вирівнювання тексту для активного документа
        private void SetAlignmentForActiveDocument(HorizontalAlignment alignment)
        {
            if (TabControl.SelectedItem is TabItem selectedTabItem && selectedTabItem.Content is Document activeDocument)
            {
                activeDocument.SetAlignment(alignment);
            }
        }
    }
}
