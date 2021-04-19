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

namespace lab03a
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DBConnection address;

        public MainWindow()
        {
            InitializeComponent();

            CommandBinding saveCommand = new CommandBinding(ApplicationCommands.Save, Save_Click);
            CommandBindings.Add(saveCommand);

            Loaded += Window_Loaded;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            address = new DBConnection();
            Update_DataContext();
        }

        private void Update_DataContext()
        {
            list.DataContext = address.TableLoad();
            list.SelectedIndex = 0;
            list.Focus();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            address.AddNote();
            Update_DataContext();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            DataRowView item = (DataRowView)list.SelectedItem;
            address.DeleteNote((int)item.Row.ItemArray[0]);
            Update_DataContext();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            DataRowView item = (DataRowView)list.SelectedItem;
            address.UpdateNote(item.Row.ItemArray);
            Update_DataContext();
        }
    }

    public class DBConnection
    {
        // Извлекаем в поле строку соединения из файла App.config
        String connectionString = System.Configuration.ConfigurationManager
            .ConnectionStrings["connectionStringName"].ConnectionString;

        DataTable dt = null; // Ссылка на объект DataTable

        public DataTable TableLoad()
        {
            if (dt != null) return dt; // Загрузим таблицу только один раз
            // Заполняем объект таблицы данными из БД 
            dt = new DataTable();
            using (SqlConnection сonnection = new SqlConnection(connectionString)) //Создаем объект подключения
            {
                SqlCommand selectedCommand = сonnection.CreateCommand(); //Создаем объект команды
                SqlDataAdapter adapter = new SqlDataAdapter(selectedCommand); //Создаем объект чтения
                //Загружает данные и схему таблицы 
                selectedCommand.CommandText = "Select Id, Title, Content, LastUpdate From Notes Order By LastUpdate desc";
                try
                {
                    // Метод сам открывает БД и сам же ее закрывает
                    adapter.Fill(dt);
                }
                catch (Exception)
                {
                    MessageBox.Show("Ошибка подключения к БД");
                }
            }
            return dt;
        }

        public void AddNote()
        {
            using (SqlConnection сonnection = new SqlConnection(connectionString)) //Создаем объект подключения
            {
                SqlCommand insertCommand = сonnection.CreateCommand();

                insertCommand.CommandText = "Insert Into Notes(LastUpdate) " +
                    "Values(@LastUpdate)";

                insertCommand.Parameters.AddWithValue("@LastUpdate", DateTime.Now);

                try
                {
                    сonnection.Open();
                    insertCommand.ExecuteNonQuery();
                    dt = null;
                }
                catch (Exception)
                {
                    MessageBox.Show("Ошибка подключения к БД");
                }
                finally
                {
                    сonnection.Close();
                }
            }
        }

        public void UpdateNote(object[] properties)
        {
            using (SqlConnection сonnection = new SqlConnection(connectionString)) //Создаем объект подключения
            {
                SqlCommand insertCommand = сonnection.CreateCommand();

                insertCommand.CommandText = "Update Notes " +
                    "Set Title = @Title, Content = @Content, LastUpdate = @LastUpdate " +
                    "where Id = @Id";

                insertCommand.Parameters.AddWithValue("@Id", properties[0]);
                insertCommand.Parameters.AddWithValue("@Title", properties[1]);
                insertCommand.Parameters.AddWithValue("@Content", properties[2]);
                insertCommand.Parameters.AddWithValue("@LastUpdate", DateTime.Now);

                try
                {
                    сonnection.Open();
                    insertCommand.ExecuteNonQuery();
                    dt = null;
                }
                catch (Exception)
                {
                    MessageBox.Show("Ошибка подключения к БД");
                }
                finally
                {
                    сonnection.Close();
                }
            }
        }

        public void DeleteNote(int id)
        {
            using (SqlConnection сonnection = new SqlConnection(connectionString)) //Создаем объект подключения
            {
                SqlCommand insertCommand = сonnection.CreateCommand();

                insertCommand.CommandText = "Delete From Notes" +
                    " Where Id = @Id";

                insertCommand.Parameters.AddWithValue("@Id", id);

                try
                {
                    сonnection.Open();
                    insertCommand.ExecuteNonQuery();
                    dt = null;
                }
                catch (Exception)
                {
                    MessageBox.Show("Ошибка подключения к БД");
                }
                finally
                {
                    сonnection.Close();
                }
            }
        }

    }
}
