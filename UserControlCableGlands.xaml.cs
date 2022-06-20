using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ExBoxCatalogEditor.Dictionary
{
    /// <summary>
    /// Логика взаимодействия для UserControlCableGlands.xaml
    /// </summary>
    public partial class UserControlCableGlands : UserControl
    {
        private Dictionary<int, string> Vvodacc = SQL.DbWorker.GetGlobalItemTypeVvodAccDictionary();
        public UserControlCableGlands()
        {
            InitializeComponent();
            LoadControlsData();
            LoadData();
        }
        private void LoadControlsData()
        {
            foreach (var vvodacc in Vvodacc)
            {
                ComboBoxNomerType.Items.Add(vvodacc.Value);
            }
        }
        private void LoadData()
        {
            try
            {
                var db = new DataClassesExBoxesDataContext();
                var cable = db.CABLE_GLANDS_ACC.ToList();
                DataGridCable.ItemsSource = cable;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CheckUserInput()
        {
            if (TextBoxCableGlands.Text == string.Empty)
            {
                MessageBox.Show("Укажите название аксессуара");
                return false;
            }
            if (TextBoxOwnerCode.Text == string.Empty)
            {
                MessageBox.Show("Укажите код производителя");
                return false;
            }
            if (ComboBoxNomerType.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите № типа");
            }
            return true;
        }
        private void DataGridCable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridCable.SelectedIndex == -1) return;
            var cable = (CABLE_GLANDS_ACC)DataGridCable.SelectedItem;
            TextBoxCableGlands.Text = cable.VC_NAME.ToString();
            TextBoxOwnerCode.Text = cable.OwnerCode != null ? cable.OwnerCode.ToString() : "";
            if (cable.N_TYPE != null)
            {
                ComboBoxNomerType.SelectedValue = Vvodacc[cable.N_TYPE ?? 0];
            }
        }

        private void ButtonAddCableGlands_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var mt = new DataClassesExBoxesDataContext();
                var cop = mt.CABLE_GLANDS_ACC.Where(CABLE_GLANDS_ACC => CABLE_GLANDS_ACC.VC_NAME == TextBoxCableGlands.Text).ToList();
                if (cop.Count > 0)
                {
                    MessageBox.Show("Указанный аксессуар уже есть в каталоге");
                    return;
                }
                var id = mt.CABLE_GLANDS_ACC.Max(CABLE_GLANDS_ACC => CABLE_GLANDS_ACC.N_ID) + 1;
                var material = new CABLE_GLANDS_ACC
                {
                    N_ID = id,
                    VC_NAME = TextBoxCableGlands.Text,
                    OwnerCode = TextBoxOwnerCode.Text,
                    N_TYPE = Vvodacc.First(x=>x.Value == ComboBoxNomerType.SelectedValue.ToString()).Key
                };
                mt.CABLE_GLANDS_ACC.InsertOnSubmit(material);
                mt.SubmitChanges();
                LoadData();
                MessageBox.Show("Новый аксессуар добавлен");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ButtonUpDateCable_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridCable.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите строку", "Ошибка");
                return;
            }
            if (CheckUserInput() == false) return;
            try
            {
                var tb = new DataClassesExBoxesDataContext();
                var item = (CABLE_GLANDS_ACC)DataGridCable.SelectedItem;
                var ide = tb.CABLE_GLANDS_ACC.Where(cable => cable.N_ID == cable.N_ID);
                foreach (var cable in ide)
                {
                    cable.VC_NAME = TextBoxCableGlands.Text;
                    cable.OwnerCode = TextBoxOwnerCode.Text;
                    cable.N_TYPE = Vvodacc.First(x => x.Value == ComboBoxNomerType.SelectedValue.ToString()).Key;
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
