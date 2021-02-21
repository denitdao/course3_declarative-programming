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

namespace lab01
{

    public partial class MainWindow : Window
    {
        void CanExecute_Save(object sender, CanExecuteRoutedEventArgs e)
        {
            if (inputTextBox.Text.Trim().Length > 0)
                e.CanExecute = true;
            else
                e.CanExecute = false;
        }

        void Execute_Save(object sender, ExecutedRoutedEventArgs e)
        {
            System.IO.File.WriteAllText(@".\myFile.txt", inputTextBox.Text);
            MessageBox.Show("The file was saved!");
        }

        void CanExecute_Open(object sender, CanExecuteRoutedEventArgs e)
        {
            if (System.IO.File.ReadAllText(@".\myFile.txt").Trim().Length > 0)
                e.CanExecute = true;
            else
                e.CanExecute = false;
        }

        void Execute_Open(object sender, ExecutedRoutedEventArgs e)
        {
            inputTextBox.Text = System.IO.File.ReadAllText(@".\myFile.txt");
        }

        void CanExecute_Delete(object sender, CanExecuteRoutedEventArgs e)
        {
            if (inputTextBox.SelectedText.Trim().Length > 0)
                e.CanExecute = true;
            else
                e.CanExecute = false;
        }

        void Execute_Delete(object sender, ExecutedRoutedEventArgs e)
        {
            inputTextBox.SelectedText = "";
        }

        public MainWindow()
        {
            InitializeComponent();
            CommandBinding saveCommand = new CommandBinding(ApplicationCommands.Save, Execute_Save, CanExecute_Save);
            CommandBinding openCommand = new CommandBinding(ApplicationCommands.Open, Execute_Open, CanExecute_Open);
            CommandBinding deleteCommand = new CommandBinding(ApplicationCommands.Delete, Execute_Delete, CanExecute_Delete);

            CommandBindings.Add(saveCommand);
            CommandBindings.Add(openCommand);
            CommandBindings.Add(deleteCommand);

        }
    }
}
