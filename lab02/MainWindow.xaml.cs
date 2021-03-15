using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace lab02
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MyDB address = new MyDB();
            list.SelectedIndex = 0;
            list.Focus();
            list.DataContext = address.TableLoad();
        }
    }

    public class MyDB
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
                selectedCommand.CommandText = "Select Id, " +
                    "(Surname + ' ' + Name) AS FullName, " +
                    "Email, Age, About From Professor";
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
    }

}
