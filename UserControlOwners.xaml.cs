using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace ExBoxCatalogEditor.Dictionary
{
    /// <summary>
    /// Логика взаимодействия для UserControlOwners.xaml
    /// </summary>
    public partial class UserControlOwners : UserControl
    {
        public UserControlOwners()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                var bd = new DataClassesExBoxesDataContext();
                var owners = bd.OWNERS.ToList();
                DataGridOwner.ItemsSource = owners;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CheckUserInput()
        {
            if (TextBoxOwners.Text == string.Empty)
            {
                MessageBox.Show("Укажите название производителя");
                return false;
            }
            return true;
        }
        private void ButtonUpdateOwners_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridOwner.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите строку", "Ошибка");
                return;
            }
            if (CheckUserInput() == false) return;
            try
            {
                var tb = new DataClassesExBoxesDataContext();
                var item = (OWNERS)DataGridOwner.SelectedItem;
                var ide = tb.OWNERS.Where(owner => owner.OwnerId == item.OwnerId);
                foreach (var owner in ide)
                {
                    owner.OwnerName = TextBoxOwners.Text;
                }
                tb.SubmitChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            LoadData();
        }

        private void ButtonAddOwners_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var tb = new DataClassesExBoxesDataContext();
                var copies = tb.OWNERS.Where(OWNERS => OWNERS.OwnerName == TextBoxOwners.Text).ToList();
                if (copies.Count > 0)
                {
                    MessageBox.Show("Указанный производитель уже есть в каталоге");
                    return;
                }
                var id = tb.OWNERS.Max(OWNER => OWNER.OwnerId) + 1;
                var owner = new OWNERS
                {
                    OwnerId = id,
                    OwnerName = TextBoxOwners.Text
                };
                tb.OWNERS.InsertOnSubmit(owner);
                tb.SubmitChanges();
                LoadData();
                MessageBox.Show("Производитель добавлен");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
           

        private void DataGridOwner_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridOwner.SelectedIndex == -1) return;
               var owner = (OWNERS)DataGridOwner.SelectedItem;
               {
                   TextBoxOwners.Text = owner.OwnerName;
               }
        }
    }
}
