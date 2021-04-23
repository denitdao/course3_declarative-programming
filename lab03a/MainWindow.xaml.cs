using System;
using System.Collections.Generic;
using System.Data;
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

namespace lab03a
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DBConnection db = new DBConnection();

        public MainWindow()
        {
            InitializeComponent();

            CommandBinding saveCommand = new CommandBinding(ApplicationCommands.Save, Save_Click);
            CommandBindings.Add(saveCommand);

            Loaded += Window_Loaded;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Update_DataContext();
        }

        private void Update_DataContext(string searchString = "")
        {
            list.DataContext = db.GetAllNotes(searchString);
            list.SelectedIndex = 0;
            list.Focus();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            db.AddNote();
            Update_DataContext();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Note item = (Note)list.SelectedItem;
            db.DeleteNote(item);
            Update_DataContext();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Note item = (Note)list.SelectedItem;
            db.UpdateNote(item);
            Update_DataContext();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string text = text_search.Text;
            Update_DataContext(text);
        }
    }

    public class DBConnection
    {
        DBNotesEntities context = new DBNotesEntities();

        public List<Note> GetAllNotes(string searchString)
        {
            var notes = context.Notes
                .Where(n => n.Title.Contains(searchString) || n.Content.Contains(searchString))
                .OrderByDescending(n => n.LastUpdate)
                .ToList();
            return notes;
        }

        public void AddNote()
        {
            var note = new Note
            {
                Title = "New note",
                CreatedAt = DateTime.Now,
                LastUpdate = DateTime.Now
            };
            context.Notes.Add(note);
            context.SaveChanges();
        }

        public void UpdateNote(Note note)
        {
            note.LastUpdate = DateTime.Now;
            context.SaveChanges();
        }

        public void DeleteNote(Note note)
        {
            context.Notes.Remove(note);
            context.SaveChanges();
        }
    }
}
