using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ExBoxCatalogEditor.Dictionary
{
    /// <summary>
    /// Логика взаимодействия для UserControlMaterial.xaml
    /// </summary>
    public partial class UserControlMaterial : UserControl
    {
        public UserControlMaterial()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                var bd = new DataClassesExBoxesDataContext();
                var material = bd.KorobkiNameMaterial.ToList();
                DataGridMaterial.ItemsSource = material;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CheckUserInput()
        {
            if (TextBoxMaterials.Text == string.Empty)
            {
                MessageBox.Show("Укажите название материала");
                return false;
            }
            return true;
        }

        private void DataGridMaterial_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridMaterial.SelectedIndex == -1) return;
            var material = (KorobkiNameMaterial)DataGridMaterial.SelectedItem;
            {
                TextBoxMaterials.Text = material.FullName;
            }
        }

        private void ButtonAddMaterials_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var mt = new DataClassesExBoxesDataContext();
                var cop = mt.KorobkiNameMaterial.Where(KorobkiNameMaterial => KorobkiNameMaterial.FullName == TextBoxMaterials.Text).ToList();
                if (cop.Count > 0)
                {
                    MessageBox.Show("Указанный материал уже есть в каталоге");
                    return;
                }
                var id = mt.KorobkiNameMaterial.Max(korobkiNameMaterial => korobkiNameMaterial.KorobkiNameMaterialId) + 1;
                var material = new KorobkiNameMaterial
                {
                    KorobkiNameMaterialId = id,
                    FullName = TextBoxMaterials.Text
                };
                mt.KorobkiNameMaterial.InsertOnSubmit(material);
                mt.SubmitChanges();
                LoadData();
                MessageBox.Show("Новый материал добавлен");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ButtonUpdateMaterials_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridMaterial.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите строку", "Ошибка");
                return;
            }
            if (CheckUserInput() == false) return;
            try
            {
                var tb = new DataClassesExBoxesDataContext();
                var item = (KorobkiNameMaterial)DataGridMaterial.SelectedItem;
                var ide = tb.KorobkiNameMaterial.Where(material => material.KorobkiNameMaterialId == item.KorobkiNameMaterialId);
                foreach (var material in ide)
                {
                    material.FullName = TextBoxMaterials.Text;
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
