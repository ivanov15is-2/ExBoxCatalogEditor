using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ExBoxCatalogEditor.Dictionary
{
    /// <summary>
    /// Логика взаимодействия для UserControlGlobalItemType.xaml
    /// </summary>
    public partial class UserControlGlobalItemType : UserControl
    {
        public UserControlGlobalItemType()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var bd = new DataClassesExBoxesDataContext();
                var item = bd.GlobalItemType.ToList();
                DataGridItemType.ItemsSource = item;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CheckUserInput()
        {
            if (TextBoxItemType.Text == string.Empty)
            {
                MessageBox.Show("Укажите тип изделия");
                return false;
            }
            return true;
        }
        private void DataGridItemType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridItemType.SelectedIndex == -1) return;
            var global = (GlobalItemType)DataGridItemType.SelectedItem;
            TextBoxItemType.Text = global.Name;
            TextBoxSort.Text = global.N_SORT.ToString();
            CheckBoxVvodAcc.IsChecked = global.IsVvodAcc = true;
        }

        private void ButtonItemType_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var mt = new DataClassesExBoxesDataContext();
                var cop = mt.GlobalItemType.Where(globalItemType => globalItemType.Name == TextBoxItemType.Text).ToList();
                if (cop.Count > 0)
                {
                    MessageBox.Show("Указанное изделие уже есть в каталоге");
                    return;
                }
                var id = mt.GlobalItemType.Max(globalItemType => globalItemType.GlobalItemTypeId) + 1;
                var nSort = TextBoxSort.Text == "" ? (int?) null : int.Parse(TextBoxSort.Text);
                var material = new GlobalItemType
                {
                    GlobalItemTypeId = id,
                    Name = TextBoxItemType.Text,
                    N_SORT = nSort,
                    IsVvodAcc = CheckBoxVvodAcc.IsChecked.GetValueOrDefault()
                };
                mt.GlobalItemType.InsertOnSubmit(material);
                mt.SubmitChanges();
                LoadData();
                MessageBox.Show("Новое изделие добавленно");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ButtonUpdateItemType_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridItemType.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите строку", "Ошибка");
                return;
            }
            if (CheckUserInput() == false) return;
            try
            {
                var tb = new DataClassesExBoxesDataContext();
                var item = (GlobalItemType)DataGridItemType.SelectedItem;
                var nSort = TextBoxSort.Text == "" ? (int?)null : int.Parse(TextBoxSort.Text);
                var ide = tb.GlobalItemType.Where(type => type.GlobalItemTypeId == item.GlobalItemTypeId);

                foreach (var type in ide)
                {
                    type.Name = TextBoxItemType.Text;
                    type.N_SORT = nSort;
                    type.IsVvodAcc = CheckBoxVvodAcc.IsChecked.GetValueOrDefault();
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
