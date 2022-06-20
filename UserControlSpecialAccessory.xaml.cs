using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using ClosedXML.Excel;
using ExBoxCatalogEditor.SQL;

namespace ExBoxCatalogEditor.Dictionary
{
    /// <summary>
    /// Логика взаимодействия для UserControlSpecialAccessory.xaml
    /// </summary>
    public partial class UserControlSpecialAccessory
    {
        private Dictionary<int, string> Owners = DbWorker.GetOwnersDictionary();
        private Dictionary<int, string> Specification = DbWorker.GetSpecificationDictionary();


        public UserControlSpecialAccessory()
        {
            InitializeComponent();
            LoadData();
            LoadControlsData();
        }

        private void LoadControlsData()
        {
            {
                foreach (var item in Owners)
                {
                    ComboBoxSpecialAccessoryOwners.Items.Add(item.Value);
                }
            }   
            {
                foreach (var item in Specification)
                {
                    ComboBoxSpecialAccessoryRazdelSpId.Items.Add(item.Value);
                }
            }
        }
        private void LoadData()
        {
            try
            {
                var bd = new DataClassesExBoxesDataContext();
                var accessory = bd.SpecialAccessoryView.ToList();
                DataGridSpecialAccessory.ItemsSource = accessory;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CheckUserInput()
        {
            if (decimal.TryParse(TextBoxSpeciallAccessoryID.Text, out _) == false)
            {
                MessageBox.Show("ID Аксессуара не должно быть пустым");
                return false;
            }

            if (TextBoxSpecialAccessoryName.Text == string.Empty)
            {
                MessageBox.Show("Наименование аксессуара не должно быть пустым");
                return false;
            }

            if (TextBoxSpecialAccessoryDesignatio.Text == string.Empty)
            {
                MessageBox.Show("Обозначение не должно быть пустым");
                return false;
            }
            if (ComboBoxSpecialAccessoryOwners.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите наименование");
                return false;
            }
            if (ComboBoxSpecialAccessoryRazdelSpId.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите раздел спецификации");
                return false;
            }
            return true;
        }
        private void DataGridSpecialAccessory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridSpecialAccessory.SelectedIndex == -1) return;
            var accessory = (SpecialAccessoryView)DataGridSpecialAccessory.SelectedItem;
            if (accessory.OwnerId != null)
            {   
                ComboBoxSpecialAccessoryOwners.SelectedValue = Owners[accessory.OwnerId.Value];
            }
            if (accessory.RazdelSpId != null)
            {
                ComboBoxSpecialAccessoryRazdelSpId.SelectedValue = Specification[accessory.RazdelSpId.Value];
            }
            TextBoxSpeciallAccessoryID.Text = accessory.N_ID.ToString();
            TextBoxSpecialAccessoryName.Text = accessory.SpecialAccessoriName;
            TextBoxSpecialAccessoryDesignatio.Text = accessory.Designatio;
            
        }
        
        private void ButtonAddSpecialAccessory_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                var acc = new DataClassesExBoxesDataContext();
                var copy = acc.SpecialAccessories.Where
                    (SpecialAccessories => SpecialAccessories.SpecialAccessoriName == TextBoxSpecialAccessoryName.Text).ToList();
                if (copy.Count > 0)
                {
                    MessageBox.Show("Указанный аксессуар уже есть в каталоге");
                    return;
                }
                var id = acc.SpecialAccessories.Max(SpecialAccessories => SpecialAccessories.N_ID) + 1;
                var specacc = new SpecialAccessories
                {
                    N_ID = id,
                    SpecialAccessoriName = TextBoxSpecialAccessoryName.Text,
                    Designatio = TextBoxSpecialAccessoryDesignatio.Text,
                    OwnerId = DbWorker.GetOwnerIdbyName(ComboBoxSpecialAccessoryOwners.SelectedValue.ToString()),
                    RazdelSpId = DbWorker.GetRazdelSpIdbyName(ComboBoxSpecialAccessoryRazdelSpId.SelectedValue.ToString())
                };
                acc.SpecialAccessories.InsertOnSubmit(specacc);
                acc.SubmitChanges();
                LoadData();
                MessageBox.Show("Новый аксессуар добавлен");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ButtonUpdateSpecialAccessory_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridSpecialAccessory.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите строку", "Ошибка");
                return;
            }
            if (CheckUserInput() == false) return;
            try
            {
                var tb = new DataClassesExBoxesDataContext();
                var item = (SpecialAccessoryView)DataGridSpecialAccessory.SelectedItem;
                var id = tb.SpecialAccessories.Where(accessory => accessory.N_ID == item.N_ID);
                foreach (var access in id)
                {
                    access.SpecialAccessoriName = TextBoxSpecialAccessoryName.Text;
                    access.Designatio = TextBoxSpecialAccessoryDesignatio.Text;
                    access.OwnerId = DbWorker.GetOwnerIdbyName(ComboBoxSpecialAccessoryOwners.SelectedValue.ToString());
                    access.RazdelSpId = DbWorker.GetRazdelSpIdbyName(ComboBoxSpecialAccessoryRazdelSpId.SelectedValue.ToString());
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
