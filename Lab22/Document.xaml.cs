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
using System.Windows.Shapes;

namespace Lab22
{
    /// <summary>
    /// Логика взаимодействия для Document.xaml
    /// </summary>
    public partial class Document : UserControl
    {
        public Document(int number)
        {
            InitializeComponent();
        }

        // Метод для встановлення тексту документа
        public void SetText(string text)
        {
            TextBoxContent.Document.Blocks.Clear();
            TextBoxContent.Document.Blocks.Add(new Paragraph(new Run(text)));
        }
        // Метод для збереження документу
        public void SaveDocument()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|Rich Text Format (*.rtf)|*.rtf"
            };

            if (saveFileDialog1.ShowDialog() == true)
            {
                string filePath = saveFileDialog1.FileName;
                TextRange textRange = new TextRange(TextBoxContent.Document.ContentStart, TextBoxContent.Document.ContentEnd);

                using (System.IO.FileStream fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
                {
                    if (filePath.EndsWith(".rtf"))
                    {
                        textRange.Save(fileStream, DataFormats.Rtf);
                    }
                    else
                    {
                        textRange.Save(fileStream, DataFormats.Text);
                    }
                }
            }
        }

        // Метод для вирізання тексту
        public void Cut()
        {
            TextBoxContent.Cut();
        }

        // Метод для копіювання тексту
        public void Copy()
        {
            TextBoxContent.Copy();
        }

        // Метод для вставлення тексту
        public void Paste()
        {
            TextBoxContent.Paste();
        }

        // Метод для виділення всього тексту
        public void SelectAll()
        {
            TextBoxContent.SelectAll();
        }

        // Метод для вставки зображення
        public void InsertImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpeg;*.jpg;*.gif)|*.png;*.jpeg;*.jpg;*.gif|All files (*.*)|*.*",
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                BitmapImage bitmap = new BitmapImage(new Uri(filePath));

                Image image = new Image
                {
                    Source = bitmap,
                    Width = bitmap.PixelWidth,
                    Height = bitmap.PixelHeight
                };

                TextPointer tp = TextBoxContent.CaretPosition.GetInsertionPosition(LogicalDirection.Forward);
                InlineUIContainer container = new InlineUIContainer(image, tp);

            }
        }

        // Метод для увімкнення зміни шрифту на жирний
        public void ApplyBold()
        {
            ApplyOrRemoveBold(FontWeights.Bold);
        }

        // Метод для вимкнення зміни шрифту на жирний
        public void RemoveBold()
        {
            ApplyOrRemoveBold(FontWeights.Normal);
        }
        
        private void ApplyOrRemoveBold(FontWeight fontWeight)
        {
            TextRange textRange = new TextRange(TextBoxContent.Selection.Start, TextBoxContent.Selection.End);

            if (textRange.IsEmpty)
            {
                TextBoxContent.FontWeight = fontWeight;
            }
            else
            {
                object currentValue = textRange.GetPropertyValue(TextElement.FontWeightProperty);
                if (currentValue != null && currentValue.Equals(fontWeight))
                {
                    textRange.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);
                }
                else
                {
                    textRange.ApplyPropertyValue(TextElement.FontWeightProperty, fontWeight);
                }
            }
        }

        // Метод для увімкнення зміни шрифту на курсив
        public void ApplyItalic()
        {
            ApplyOrRemoveItalic(FontStyles.Italic);
        }

        // Метод для вимкнення зміни шрифту на курсив
        public void RemoveItalic()
        {
            ApplyOrRemoveItalic(FontStyles.Normal);
        }

        private void ApplyOrRemoveItalic(FontStyle fontStyle)
        {
            TextRange textRange = new TextRange(TextBoxContent.Selection.Start, TextBoxContent.Selection.End);

            if (textRange.IsEmpty)
            {
                TextBoxContent.FontStyle = fontStyle;
            }
            else
            {
                object currentValue = textRange.GetPropertyValue(TextElement.FontStyleProperty);
                if (currentValue != null && currentValue.Equals(fontStyle))
                {
                    textRange.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Normal);
                }
                else
                {
                    textRange.ApplyPropertyValue(TextElement.FontStyleProperty, fontStyle);
                }
            }
        }

        // Метод для увімкнення зміни шрифту на підкреслений
        public void ApplyUnderline()
        {
            ApplyOrRemoveStyle(Inline.TextDecorationsProperty, TextDecorations.Underline);
        }
        // Метод для вимкнення зміни шрифту на підкреслений
        public void RemoveUnderline()
        {
            ApplyOrRemoveStyle(Inline.TextDecorationsProperty, null);
        }

        private void ApplyOrRemoveStyle(DependencyProperty property, object value)
        {
            TextRange textRange = new TextRange(TextBoxContent.Selection.Start, TextBoxContent.Selection.End);
            object currentValue = textRange.GetPropertyValue(property);

            if (currentValue != null && currentValue.Equals(value))
            {
                textRange.ApplyPropertyValue(property, null);
            }
            else
            {
                textRange.ApplyPropertyValue(property, value);
            }
        }

        // Метод, що застосовує вказане сімейство шрифтів до виділеного тексту
        public void ApplyFontFamily(FontFamily fontFamily)
        {
            if (TextBoxContent.Selection.IsEmpty)
                return;

            TextRange textRange = new TextRange(TextBoxContent.Selection.Start, TextBoxContent.Selection.End);
            textRange.ApplyPropertyValue(TextElement.FontFamilyProperty, fontFamily);
        }

        // Метод, що застосовує вказаний розмір шрифту до виділеного тексту.
        public void ApplyFontSize(double fontSize)
        {
            if (TextBoxContent.Selection.IsEmpty)
                return;

            TextRange textRange = new TextRange(TextBoxContent.Selection.Start, TextBoxContent.Selection.End);
            textRange.ApplyPropertyValue(TextElement.FontSizeProperty, fontSize);
        }

        // Метод, що встановлює вирівнювання тексту для вибраного абзацу.
        public void SetAlignment(HorizontalAlignment alignment)
        {
            if (TextBoxContent != null)
            {
                Paragraph paragraph = TextBoxContent.CaretPosition.Paragraph;
                if (paragraph != null)
                {
                    paragraph.TextAlignment = ConvertToTextAlignment(alignment);
                }
                
            }
        }
        // Метод для конвертації значення HorizontalAlignment у відповідне значення TextAlignment
        private TextAlignment ConvertToTextAlignment(HorizontalAlignment alignment)
        {
            switch (alignment)
            {
                case HorizontalAlignment.Left:
                    return TextAlignment.Left;
                case HorizontalAlignment.Center:
                    return TextAlignment.Center;
                case HorizontalAlignment.Right:
                    return TextAlignment.Right;
                default:
                    throw new ArgumentException("Непідтримуване значення HorizontalAlignment");
            }
        }
    }
}
