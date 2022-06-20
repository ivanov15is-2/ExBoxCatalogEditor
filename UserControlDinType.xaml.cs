using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ExBoxCatalogEditor.Dictionary
{
    /// <summary>
    /// Логика взаимодействия для UserControlDinType.xaml
    /// </summary>
    public partial class UserControlDinType : UserControl
    {
        public UserControlDinType()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                var bd = new DataClassesExBoxesDataContext();
                var din = bd.DIN_TYPES.ToList();
                DataGridDinTypes.ItemsSource = din;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CheckUserInput()
        {
            if (TextBoxDinType.Text == string.Empty)
            {
                MessageBox.Show("Укажите тип Дин рейки");
                return false;
            }
            if (TextBoxRose.Text == string.Empty)
            {
                MessageBox.Show("Укажите код ROSE");
                return false;
            }
            if (TextBoxSchema.Text == string.Empty)
            {
                MessageBox.Show("Укажите блок для чертежа");
                return false;
            }
            return true;
        }

        private void DataGridDinTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridDinTypes.SelectedIndex == -1) return;
            var din = (DIN_TYPES)DataGridDinTypes.SelectedItem;
            TextBoxRose.Text = din.ROSE_CODE ?? "";
            TextBoxSchema.Text = din.VC_SCHEMA_TEMPLATE;
            TextBoxDinType.Text = din.VC_NAME;
        }

        private void ButtonAddDinType_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var mt = new DataClassesExBoxesDataContext();
                var copies = mt.DIN_TYPES.Where(DIN_TYPE => DIN_TYPE.VC_NAME == TextBoxDinType.Text).ToList();
                if (copies.Count > 0)
                {
                    MessageBox.Show("Указанная рейка уже есть в каталоге");
                    return;
                }
                var id = mt.DIN_TYPES.Max(DIN_TYPE => DIN_TYPE.N_TYPE_ID) + 1;
                var material = new DIN_TYPES
                {
                    N_TYPE_ID = id,
                    VC_NAME = TextBoxDinType.Text,
                    ROSE_CODE = TextBoxRose.Text,
                    VC_SCHEMA_TEMPLATE = TextBoxSchema.Text
                };
                mt.DIN_TYPES.InsertOnSubmit(material);
                mt.SubmitChanges();
                LoadData();
                MessageBox.Show("Новая рейка добавлена");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ButtonUpdateDin_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridDinTypes.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите строку", "Ошибка");
                return;
            }
            if (CheckUserInput() == false) return;
            try
            {
                var tb = new DataClassesExBoxesDataContext();
                var item = (DIN_TYPES)DataGridDinTypes.SelectedItem;
                var ide = tb.DIN_TYPES.Where(din => din.N_TYPE_ID == din.N_TYPE_ID);
                foreach (var din in ide)
                {
                    din.VC_NAME = TextBoxDinType.Text;
                    din.ROSE_CODE = TextBoxRose.Text;
                    din.VC_SCHEMA_TEMPLATE = TextBoxSchema.Text;
                }
                tb.SubmitChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            LoadData();
        }
    }
}
