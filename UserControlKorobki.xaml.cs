using ClosedXML.Excel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ExBoxCatalogEditor
{
    /// <summary>
    /// Логика взаимодействия для UserControl3.xaml
    /// </summary>
    public partial class UserControlKorobki
    {
        private Dictionary<int, string> Owners = SQL.DbWorker.GetOwnersDictionary();
        private Dictionary<int, string> GlobalItemType = SQL.DbWorker.GetGlobalItemTypeDictionary();
        private Dictionary<int, string> Material = SQL.DbWorker.GetMaterialDictionary();
        public UserControlKorobki()
        {
            InitializeComponent();
            LoadContrilsData();
            LoadData();            
        }
        private void LoadContrilsData()
        {
            foreach (var owner in Owners)
            {
                ComboBoxOwner.Items.Add(owner.Value);
            }
            foreach (var mat in Material)
            {
                ComboBoxMaterial.Items.Add(mat.Value);
            }
            foreach (var global in GlobalItemType)
            {
                ComboBoxGlobalType.Items.Add(global.Value);
            }
        }
        private void LoadData()
        {
            try
            {
                var bd = new DataClassesExBoxesDataContext();
                var korobki = new List<KOROBKI>();
                if (TextBoxSearch.Text != "Поиск" && TextBoxSearch.Text != "")
                {
                    korobki = bd.KOROBKI.Where(korobka => (korobka.VC_NAME.ToLower().Contains(TextBoxSearch.Text.ToLower()))).ToList();                                                                                  
                }
                else
                {
                    korobki = bd.KOROBKI.ToList();
                }
                DataGridKorobki.ItemsSource = korobki;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void DataGridKorobki_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridKorobki.SelectedIndex == -1) return;
            var item = (KOROBKI)DataGridKorobki.SelectedItem;

            if (item.MATERIAL != null)
            {
                ComboBoxMaterial.SelectedValue = Material[item.MATERIAL ?? 0 ];
            }

            if (item.OwnerId !=null)
            {
                ComboBoxOwner.SelectedValue = Owners[item.OwnerId ?? 0];
            }

            if (item.GlobalItemTypeId !=null)
            {
                ComboBoxGlobalType.SelectedValue = GlobalItemType[item.GlobalItemTypeId ?? 0];
            }
            TextBoxName.Text = item.VC_NAME.ToString();
            TextBoxTok.Text = item.TOK.ToString();
            TextBoxHeight.Text = item.HEIGHT.ToString();
            TextBoxWidth.Text = item.WIDTH.ToString();
            TextBoxDepth.Text = item.DEPTH.ToString();
            TextBoxFullDepth.Text = item.DEPTH_FULL.ToString();
            TextBoxDinType.Text = item.DIN_TYPE.ToString();
            TextBoxDinLength.Text = item.N_LENGTH.ToString();
            TextBoxPrituplenie.Text = item.B_PRITUPLENIE.ToString();
            TextBoxTop.Text = item.A_TOP.ToString();
            TextBoxLeft.Text = item.A_LEFT.ToString();
            TextBoxRight.Text = item.A_RIGHT.ToString();
            TextBoxADLeft.Text = item.A_D_LEFT.ToString();
            TextBoxADWidth.Text = item.A_D_WIDTH.ToString();
            TextBoxBlock.Text = item.BlockName;
            TextBoxBprituplenie.Text = item.B_PRITUPLENIE.ToString();
            TextBoxBTop.Text = item.B_TOP.ToString();
            TextBoxBLeft.Text = item.B_LEFT.ToString();
            TextBoxBRight.Text = item.B_RIGHT.ToString();
            TextBoxBDLeft.Text = item.B_D_LEFT.ToString();
            TextBoxBDWidth.Text = item.B_D_WIDTH.ToString();
            TextBoxCprituplenie.Text = item.C_PRITUPLENIE.ToString();
            TextBoxCTop.Text = item.C_TOP.ToString();
            TextBoxCLeft.Text = item.C_LEFT.ToString();
            TextBoxCRight.Text = item.C_RIGHT.ToString();
            TextBoxCDLength.Text = item.C_D_LEFT.ToString();
            TextBoxCDWidth.Text = item.C_D_WIDTH.ToString();
            TextBoxD_PRITUPLENIE.Text = item.D_PRITUPLENIE.ToString();
            TextBoxDTop.Text = item.D_TOP.ToString();
            TextBoxDLeft.Text = item.D_LEFT.ToString();
            TextBoxDRight.Text = item.D_RIGHT.ToString();
            TextBoxDDWidth.Text = item.D_D_WIDTH.ToString();
            TextBoxDDLeft.Text = item.D_D_LEFT.ToString();
            TextBoxOwner.Text = item.VC_NAME_INT;
           //TextBoxValue.Text = item.M_VALUE.ToString();
           //TextBoxPower.Text = item.N_POWER_EX.ToString();
           //TextBoxNpower.Text = item.N_POWER.ToString();
            TextBoxPrefix.Text = item.NamePrefix.ToString();
            CheckBoxEquipment.IsChecked = item.IsMainEquipment == true;
            CheckBoxBalance.IsChecked = item.GlobalItemTypeId == 1;
            //TextBoxIDowner.Text = item.OwnerId.ToString();
            TextBoxIDcertificate.Text = item.CertificatesId.ToString();
            TextBoxN15.Text = item.N1_5.ToString();
            TextBoxN25.Text = item.N2_5.ToString();
            TextBoxN4.Text = item.N4.ToString();
            TextBoxN6.Text = item.N6.ToString();
            TextBoxN10.Text = item.N10.ToString();
            TextBoxN16.Text = item.N16.ToString();
            TextBoxN252.Text = item.N25.ToString();
            TextBoxN35.Text = item.N35.ToString();
            TextBoxN50.Text = item.N50.ToString();
            TextBoxN70.Text = item.N70.ToString();
            TextBoxN95.Text = item.N95.ToString();
            TextBoxN120.Text = item.N120.ToString();
            TextBoxN150.Text = item.N150.ToString();
            TextBoxN185.Text = item.N185.ToString();        
        }
        private bool CheckUserInput()
        {
            if (TextBoxName.Text == string.Empty)
            {
                MessageBox.Show("Наименование не должно быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (ComboBoxOwner.SelectedIndex == -1)
            {
                MessageBox.Show("Поле Производители не выбрано");
                return false;
            }
            if (ComboBoxGlobalType.SelectedIndex == -1)
            {
                MessageBox.Show("Поле Тип изделия не выбрано");
                    return false;
            }
            if (int.TryParse(TextBoxTok.Text, out _) == false)
            {
                MessageBox.Show("Поле Ток не должно быть пустым", "Ошибка", MessageBoxButton.OK);
                return false;
            }
            if (ComboBoxMaterial.SelectedIndex == -1)
            {
                MessageBox.Show("Поле Материалы не выбрано", "Ошибка", MessageBoxButton.OK);
                return false;
            }
            if (decimal.TryParse(TextBoxHeight.Text, out _) == false)
            {
                MessageBox.Show("Поле Высота не должно быть пустым", "Ошибка", MessageBoxButton.OK);
                return false;
            }
            if (decimal.TryParse(TextBoxWidth.Text, out _) == false)
            {
                MessageBox.Show("Поле Длина не должно быть пустым", "Ошибка", MessageBoxButton.OK);
                return false;
            }
            if (decimal.TryParse(TextBoxPrituplenie.Text, out _) == false)
            {
                MessageBox.Show("Поле Притупление не должно быть пустым", "Ошибка", MessageBoxButton.OK);
                return false;
            }
            if (decimal.TryParse(TextBoxTop.Text, out _) == false)
            {
                MessageBox.Show("Поле А_Вверх не должно быть пустым", "Ошибка", MessageBoxButton.OK);
                return false;
            }
            if (decimal.TryParse(TextBoxLeft.Text, out _) == false)
            {
                MessageBox.Show("Поле А_Лево не должно быть пустым", "Ошибка", MessageBoxButton.OK);
                return false;
            }
            if (decimal.TryParse(TextBoxRight.Text, out _) == false)
            {
                MessageBox.Show("Поле А_Право не должно быть пустым", "Ошибка", MessageBoxButton.OK);
                return false;
            }
            if (TextBoxOwner.Text == string.Empty)
            {
                MessageBox.Show("Поле Номер владельца не должно быть пустым", "Ошибка", MessageBoxButton.OK);
                return false;
            }
          // if (decimal.TryParse(TextBoxValue.Text, out _) == false)
          // {
          //     MessageBox.Show("Поле Величина не должно быть пустым", "Ошибка", MessageBoxButton.OK);
          //     return false;
          // }
            if (TextBoxBlock.Text == string.Empty)
            {
                MessageBox.Show("Поле Имя блока не должно быть пустым", "Ошибка", MessageBoxButton.OK);
                return false;
            }
            if (decimal.TryParse(TextBoxBTop.Text, out _) == false)
            {
                MessageBox.Show("Поле B_вверх не должно быть пустым", "Ошибка", MessageBoxButton.OK);
                return false;
            }
            if (decimal.TryParse(TextBoxBLeft.Text, out _) == false)
            {
                MessageBox.Show("Поле B_Лево не должно быть пустым", "Ошибка", MessageBoxButton.OK);
                return false;
            }
            if (decimal.TryParse(TextBoxBRight.Text, out _) == false)
            {
                MessageBox.Show("Поле В_Право не должно быть пустым", "Ошибка", MessageBoxButton.OK);
                return false;
            }
            if (decimal.TryParse(TextBoxBDLeft.Text, out _) == false)
            {
                MessageBox.Show("Поле B_D_Лево не должно быть пустым", "Ошибка", MessageBoxButton.OK);
                return false;
            }
            if (decimal.TryParse(TextBoxBDWidth.Text, out _) == false)
            {
                MessageBox.Show("Поле B_D_Ширина не должно быть пустым", "Ошибка", MessageBoxButton.OK);
                return false;
            }
            if (decimal.TryParse(TextBoxCTop.Text, out _) == false)
            {
                MessageBox.Show("Поле С_Вверх не должно быть пустым", "Ошибка", MessageBoxButton.OK);
                return false;
            }
            if (decimal.TryParse(TextBoxCLeft.Text, out _) == false)
            {
                MessageBox.Show("Поле С_Лево не должно быть пустым", "Ошибка", MessageBoxButton.OK);
                return false;
            }
            if (decimal.TryParse(TextBoxCRight.Text, out _) == false)
            {
                MessageBox.Show("Поле С_Право не должно быть пустым", "Ошибка", MessageBoxButton.OK);
                return false;
            }
            if (decimal.TryParse(TextBoxCDWidth.Text, out _) == false)
            {
                MessageBox.Show("Поле С_D_Ширина не можеть быть пустым", "Ошибка", MessageBoxButton.OK);
                return false;
            }
            if (decimal.TryParse(TextBoxDTop.Text, out _) == false)
            {
                MessageBox.Show("Поле D_вверх не может быть пустым", "Ошибка", MessageBoxButton.OK);
                return false;
            }
            if (decimal.TryParse(TextBoxDLeft.Text, out _) == false)
            {
                MessageBox.Show("Поле D_Лево не может быть пустым", "Ошибка", MessageBoxButton.OK);
                return false;
            }
            if (decimal.TryParse(TextBoxDRight.Text, out _) == false)
            {
                MessageBox.Show("Поле D_Право не может быть пустым", "Ошибка", MessageBoxButton.OK);
                return false;
            }
            if (decimal.TryParse(TextBoxDDWidth.Text, out _) == false)
            {
                MessageBox.Show("Поле D_D_Ширина не может быть пустым", "Ошибка", MessageBoxButton.OK);
                return false;
            }
            if (SQL.DbWorker.GetGlobalItemTypeIdbyName(ComboBoxGlobalType.SelectedValue.ToString()) == 0)
            {
                MessageBox.Show("Выбранный тип коробки не найден");
                return false;
            }
            return true;
        }
        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridKorobki.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите строку", "Ошибка");
                return;
            }
            if (CheckUserInput() == false) return;
            try
            {
                //int? N_POWER = null;
            //   if (TextBoxPower.Text != "")
            //   {
            //       N_POWER = int.Parse(TextBoxPower.Text);
            //   }
            //   decimal? N_POWER_EX = null;
            //   if (TextBoxNpower.Text != "")
            //   {
            //       N_POWER_EX = decimal.Parse(TextBoxNpower.Text);
            //   }
                decimal? N_LENGTH = null;
                if (TextBoxDinLength.Text != "")
                {
                    N_LENGTH = decimal.Parse(TextBoxDinLength.Text);
                    var bd = new DataClassesExBoxesDataContext();
                    var item = (KOROBKI)DataGridKorobki.SelectedItem;
                    var query = bd.KOROBKI.Where(korobka => korobka.N_ID == item.N_ID);
                    foreach (var korobka in query)
                    {
                        korobka.VC_NAME = TextBoxName.Text;
                        korobka.TOK = int.Parse(TextBoxTok.Text);
                        korobka.MATERIAL = Material.First(m => m.Value == ComboBoxMaterial.SelectedValue.ToString()).Key;
                        korobka.HEIGHT = int.Parse(TextBoxHeight.Text);
                        korobka.WIDTH = int.Parse(TextBoxWidth.Text);
                        korobka.DEPTH = decimal.Parse(TextBoxDepth.Text);
                        korobka.DEPTH_FULL = decimal.Parse(TextBoxFullDepth.Text);
                        korobka.A_PRITUPLENIE = decimal.Parse(TextBoxPrituplenie.Text);
                        korobka.DIN_TYPE = int.Parse(TextBoxDinType.Text);
                        korobka.N_LENGTH = N_LENGTH;
                        korobka.A_PRITUPLENIE = decimal.Parse(TextBoxPrituplenie.Text);
                        korobka.A_TOP = decimal.Parse(TextBoxTop.Text);
                        korobka.A_LEFT = decimal.Parse(TextBoxLeft.Text);
                        korobka.A_RIGHT = decimal.Parse(TextBoxRight.Text);
                        korobka.A_D_WIDTH = decimal.Parse(TextBoxADWidth.Text);
                        korobka.B_PRITUPLENIE = decimal.Parse(TextBoxBprituplenie.Text);
                        korobka.B_TOP = decimal.Parse(TextBoxBTop.Text);
                        korobka.B_LEFT = decimal.Parse(TextBoxBLeft.Text);
                        korobka.B_RIGHT = decimal.Parse(TextBoxBRight.Text);
                        korobka.B_D_LEFT = decimal.Parse(TextBoxBDLeft.Text);
                        korobka.B_D_WIDTH = decimal.Parse(TextBoxBDWidth.Text);
                        korobka.C_PRITUPLENIE = decimal.Parse(TextBoxCprituplenie.Text);
                        korobka.C_TOP = decimal.Parse(TextBoxCTop.Text);
                        korobka.C_LEFT = decimal.Parse(TextBoxCLeft.Text);
                        korobka.C_RIGHT = decimal.Parse(TextBoxCRight.Text);
                        korobka.C_D_LEFT = decimal.Parse(TextBoxCDLength.Text);
                        korobka.C_D_WIDTH = decimal.Parse(TextBoxCDWidth.Text);
                        korobka.D_PRITUPLENIE = decimal.Parse(TextBoxD_PRITUPLENIE.Text);
                        korobka.D_TOP = decimal.Parse(TextBoxDTop.Text);
                        korobka.D_LEFT = decimal.Parse(TextBoxDLeft.Text);
                        korobka.D_RIGHT = decimal.Parse(TextBoxDRight.Text);
                        korobka.D_D_LEFT = decimal.Parse(TextBoxDDLeft.Text);
                        korobka.D_D_WIDTH = decimal.Parse(TextBoxDDWidth.Text);
                        korobka.VC_NAME_INT = TextBoxOwner.Text;
                       // korobka.M_VALUE = int.Parse(TextBoxValue.Text);
                        //korobka.N_POWER = N_POWER;
                        //korobka.N_POWER_EX = N_POWER_EX;
                        korobka.NamePrefix = TextBoxPrefix.Text;
                        korobka.BlockName = TextBoxBlock.Text;
                        korobka.IsMainEquipment = CheckBoxEquipment.IsChecked.GetValueOrDefault();
                        korobka.OwnerId = SQL.DbWorker.GetOwnerIdbyName(ComboBoxOwner.SelectedValue.ToString());
                        korobka.GlobalItemTypeId = SQL.DbWorker.GetGlobalItemTypeIdbyName(ComboBoxGlobalType.SelectedValue.ToString());
                        korobka.CertificatesId = int.Parse(TextBoxIDcertificate.Text);
                    }
                    bd.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (CheckUserInput() == false) return;
            try
            {
                var bd = new DataClassesExBoxesDataContext();
                var copies = bd.KOROBKI.Where(KOROBKI => KOROBKI.VC_NAME == TextBoxName.Text).ToList();
                if (copies.Count > 0)
                {
                    MessageBox.Show("Указанная коробка уже есть в каталоге");
                    return;
                }
                var id = bd.KOROBKI.Max(KOROBKI => KOROBKI.N_ID) + 1;

                //int? N_POWER = null;
             //  if (TextBoxPower.Text != "")
             //  {
             //      N_POWER = int.Parse(TextBoxPower.Text);
             //  }
             //  decimal? N_POWER_EX = null;
             //  if (TextBoxNpower.Text != "")
             //  {
             //      N_POWER_EX = decimal.Parse(TextBoxNpower.Text);
             //  }
                decimal? N_LENGTH = null;
                if (TextBoxDinLength.Text != "")
                {
                    N_LENGTH = decimal.Parse(TextBoxDinLength.Text);
                }
                var korobka = new KOROBKI
                {
                    N_ID = id,
                    VC_NAME = TextBoxName.Text,
                    TOK = int.Parse(TextBoxTok.Text),
                    MATERIAL = SQL.DbWorker.GetMaterialIdByName(ComboBoxMaterial.SelectedValue.ToString()),
                    HEIGHT = decimal.Parse(TextBoxHeight.Text),
                    WIDTH = decimal.Parse(TextBoxWidth.Text),
                    DEPTH = decimal.Parse(TextBoxDepth.Text),
                    DEPTH_FULL = decimal.Parse(TextBoxFullDepth.Text),
                    DIN_TYPE = int.Parse(TextBoxDinType.Text),
                    N_LENGTH = N_LENGTH,
                    A_PRITUPLENIE = decimal.Parse(TextBoxPrituplenie.Text),
                    A_TOP = decimal.Parse(TextBoxTop.Text),
                    A_LEFT = decimal.Parse(TextBoxLeft.Text),
                    A_RIGHT = decimal.Parse(TextBoxRight.Text),
                    A_D_LEFT = decimal.Parse(TextBoxADLeft.Text),
                    A_D_WIDTH = decimal.Parse(TextBoxADWidth.Text),
                    B_PRITUPLENIE = decimal.Parse(TextBoxBprituplenie.Text),
                    B_TOP = decimal.Parse(TextBoxBTop.Text),
                    B_LEFT = decimal.Parse(TextBoxBLeft.Text),
                    B_RIGHT = decimal.Parse(TextBoxBRight.Text),
                    B_D_LEFT = decimal.Parse(TextBoxBDLeft.Text),
                    B_D_WIDTH = decimal.Parse(TextBoxBDWidth.Text),
                    C_PRITUPLENIE = decimal.Parse(TextBoxCprituplenie.Text),
                    C_TOP = decimal.Parse(TextBoxCTop.Text),
                    C_LEFT = decimal.Parse(TextBoxCLeft.Text),
                    C_RIGHT = decimal.Parse(TextBoxCRight.Text),
                    C_D_LEFT = decimal.Parse(TextBoxCDLength.Text),
                    C_D_WIDTH = decimal.Parse(TextBoxCDWidth.Text),
                    D_PRITUPLENIE = decimal.Parse(TextBoxD_PRITUPLENIE.Text),
                    D_TOP = decimal.Parse(TextBoxDTop.Text),
                    D_LEFT = decimal.Parse(TextBoxDLeft.Text),
                    D_RIGHT = decimal.Parse(TextBoxDRight.Text),
                    D_D_WIDTH = decimal.Parse(TextBoxDDWidth.Text),
                    D_D_LEFT = decimal.Parse(TextBoxDDLeft.Text),
                    VC_NAME_INT = TextBoxOwner.Text,
                   // M_VALUE = decimal.Parse(TextBoxValue.Text),
                    //N_POWER = N_POWER,
                   // N_POWER_EX = N_POWER_EX,
                    NamePrefix = TextBoxPrefix.Text,
                    BlockName = TextBoxBlock.Text,
                    IsMainEquipment = (CheckBoxEquipment.IsChecked.GetValueOrDefault() ? true : false),
                    OwnerId = SQL.DbWorker.GetOwnerIdbyName(ComboBoxOwner.SelectedValue.ToString()),
                    GlobalItemTypeId = SQL.DbWorker.GetGlobalItemTypeIdbyName(ComboBoxGlobalType.SelectedValue.ToString()),
                    CertificatesId = int.Parse(TextBoxIDcertificate.Text)
                };
                bd.KOROBKI.InsertOnSubmit(korobka);
                bd.SubmitChanges();
                LoadData();
                MessageBox.Show("Данные добавлены");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void TextBoxSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TextBoxSearch.Text == "Поиск")
            {
                TextBoxSearch.Text = "";
                TextBoxSearch.Focus();
            }    
        }
        private void TextBoxSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            LoadData();
        }

        private void ButtonExport_Click(object sender, RoutedEventArgs e)
        {
            var items = DataGridKorobki.Items.Cast<KOROBKI>().ToList();
            var wb = new XLWorkbook();
            
            var ws = wb.Worksheets.Add("Основной");
            var i = 2;
            ws.Cell(1, 1).Value = "Наименование коробки";
            ws.Cell(1, 2).Value = "Ток";
            ws.Cell(1, 3).Value = "N1_5";
            ws.Cell(1, 4).Value = "N2_5";
            ws.Cell(1, 5).Value = "N4";
            ws.Cell(1, 6).Value = "N6";
            ws.Cell(1, 7).Value = "N10";
            ws.Cell(1, 8).Value = "N16";
            ws.Cell(1, 9).Value = "N25";
            ws.Cell(1, 10).Value = "N35";
            ws.Cell(1, 11).Value = "N50";
            ws.Cell(1, 12).Value = "N70";
            ws.Cell(1, 13).Value = "N95";
            ws.Cell(1, 14).Value = "N120";
            ws.Cell(1, 15).Value = "N150";
            ws.Cell(1, 16).Value = "N185";
            ws.Cell(1, 17).Value = "Материал";
            ws.Cell(1, 18).Value = "Высота";
            ws.Cell(1, 19).Value = "Ширина";
            ws.Cell(1, 20).Value = "Глубина";
            ws.Cell(1, 21).Value = "Полная глубина";
            ws.Cell(1, 22).Value = "Тип Динрейки";
            ws.Cell(1, 23).Value = "Длина";
            ws.Cell(1, 24).Value = "A_Притупление";
            ws.Cell(1, 25).Value = "A_Верх_Низ";
            ws.Cell(1, 26).Value = "A_Лево";
            ws.Cell(1, 27).Value = "A_Право";
            ws.Cell(1, 28).Value = "A_D_Лево";
            ws.Cell(1, 29).Value = "A_D_Ширина";
            ws.Cell(1, 30).Value = "B_Притупление";
            ws.Cell(1, 31).Value = "B_Верх_Низ";
            ws.Cell(1, 32).Value = "B_Лево";
            ws.Cell(1, 33).Value = "B_Право";
            ws.Cell(1, 34).Value = "B_D_Лево";
            ws.Cell(1, 35).Value = "B_D_Ширина";
            ws.Cell(1, 36).Value = "C_Притупление";
            ws.Cell(1, 37).Value = "C_Верх_Низ";
            ws.Cell(1, 38).Value = "C_Лево";
            ws.Cell(1, 39).Value = "C_Право";
            ws.Cell(1, 40).Value = "C_D_Лево";
            ws.Cell(1, 41).Value = "C_D_Ширина";
            ws.Cell(1, 42).Value = "D_Притупление";
            ws.Cell(1, 43).Value = "D_Верх_Низ";
            ws.Cell(1, 44).Value = "D_Лево";
            ws.Cell(1, 45).Value = "D_Право";
            ws.Cell(1, 46).Value = "D_D_Лево";
            ws.Cell(1, 47).Value = "D_D_Ширина";
            ws.Cell(1, 48).Value = "Артикул производителя";
            //ws.Cell(1, 49).Value = "Величина";
           //ws.Cell(1, 50).Value = "Мощность";
           //ws.Cell(1, 51).Value = "Мощность EX";
            ws.Cell(1, 49).Value = "Имя префикса";
            ws.Cell(1, 50).Value = "Имя блока";
            ws.Cell(1, 51).Value = "Основное оборудование";
            ws.Cell(1, 52).Value = "Глобальный тип элемента";
            //ws.Cell(1, 56).Value = "ID производителя";
            ws.Cell(1, 53).Value = "ID сертификата";
            foreach (var o in items)
            {
                ws.Cell(i, 1).Value = "'" + o.VC_NAME;
                ws.Cell(i, 2).Value = o.TOK;
                ws.Cell(i, 3).Value = o.N1_5;
                ws.Cell(i, 4).Value = o.N2_5;
                ws.Cell(i, 5).Value = o.N4;
                ws.Cell(i, 6).Value = o.N6;
                ws.Cell(i, 7).Value = o.N10;
                ws.Cell(i, 8).Value = o.N16;
                ws.Cell(i, 9).Value = o.N25;
                ws.Cell(i, 10).Value = o.N35;
                ws.Cell(i, 11).Value = o.N50;
                ws.Cell(i, 12).Value = o.N70;
                ws.Cell(i, 13).Value = o.N95;
                ws.Cell(i, 14).Value = o.N120;
                ws.Cell(i, 15).Value = o.N150;
                ws.Cell(i, 16).Value = o.N185;
                ws.Cell(i, 17).Value = o.MATERIAL;
                ws.Cell(i, 18).Value = o.HEIGHT;
                ws.Cell(i, 19).Value = o.WIDTH;
                ws.Cell(i, 20).Value = o.DEPTH;
                ws.Cell(i, 21).Value = o.DEPTH_FULL;
                ws.Cell(i, 22).Value = o.DIN_TYPE;
                ws.Cell(i, 23).Value = o.N_LENGTH;
                ws.Cell(i, 24).Value = o.A_PRITUPLENIE;
                ws.Cell(i, 25).Value = o.A_TOP;
                ws.Cell(i, 26).Value = o.A_LEFT;
                ws.Cell(i, 27).Value = o.A_RIGHT;
                ws.Cell(i, 28).Value = o.A_D_LEFT;
                ws.Cell(i, 29).Value = o.A_D_WIDTH;
                ws.Cell(i, 30).Value = o.B_PRITUPLENIE;
                ws.Cell(i, 31).Value = o.B_TOP;
                ws.Cell(i, 32).Value = o.B_LEFT;
                ws.Cell(i, 33).Value = o.B_RIGHT;
                ws.Cell(i, 34).Value = o.B_D_LEFT;
                ws.Cell(i, 35).Value = o.B_D_WIDTH;
                ws.Cell(i, 36).Value = o.C_PRITUPLENIE;
                ws.Cell(i, 37).Value = o.C_TOP;
                ws.Cell(i, 38).Value = o.C_LEFT;
                ws.Cell(i, 39).Value = o.C_RIGHT;
                ws.Cell(i, 40).Value = o.C_D_LEFT;
                ws.Cell(i, 41).Value = o.C_D_WIDTH;
                ws.Cell(i, 42).Value = o.D_PRITUPLENIE;
                ws.Cell(i, 43).Value = o.D_TOP;
                ws.Cell(i, 44).Value = o.D_LEFT;
                ws.Cell(i, 45).Value = o.D_RIGHT;
                ws.Cell(i, 46).Value = o.D_D_LEFT;
                ws.Cell(i, 47).Value = o.D_D_WIDTH;
                ws.Cell(i, 48).Value = o.VC_NAME_INT;
               // ws.Cell(i, 49).Value = o.M_VALUE;
                ws.Cell(i, 49).Value = o.NamePrefix;
                ws.Cell(i, 50).Value = o.BlockName;
                ws.Cell(i, 51).Value = o.IsMainEquipment;
                ws.Cell(i, 52).Value = o.GlobalItemTypeId;
                //ws.Cell(i, 56).Value = o.OwnerId;
                ws.Cell(i, 53).Value = o.CertificatesId;
                i++;
            }

            var rngHeaders = ws.Range("A1:L1");
            rngHeaders.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngHeaders.Style.Font.Bold = true;
            rngHeaders.Style.Font.FontColor = XLColor.DarkBlue;
            rngHeaders.Style.Fill.BackgroundColor = XLColor.LightBlue;
            rngHeaders.SetAutoFilter();

            var rngText = ws.Range(1, 1, items.Count + 1, 12);
            rngText.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            rngText.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            rngText.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            rngText.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

            ws.Columns().AdjustToContents();

            ws.SheetView.FreezeRows(1);

            wb.SaveAs(Path.GetTempPath() + "test.xlsx");
            Process.Start(Path.GetTempPath() + "test.xlsx");
        }

        private void ButtonImport_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Excel (*.xlsx)|*.xlsx";
            ;
            //KOROBKI tb = new KOROBKI();
            
            if (opf.ShowDialog() == false) return;
            string filename = opf.FileName;
            var workBook = new XLWorkbook(filename);
            var workSheet = workBook.Worksheet(1);
            var importedBoxes = new List<KOROBKI>();
            TextBoxName.Text = workSheet.Cell(2, 1).Value.ToString();
            TextBoxTok.Text = workSheet.Cell(2, 2).Value.ToString();
            ComboBoxMaterial.Text = workSheet.Cell(2, 3).Value.ToString();
            TextBoxHeight.Text = workSheet.Cell(2, 4).Value.ToString();
            TextBoxWidth.Text = workSheet.Cell(2, 5).Value.ToString();
            TextBoxDepth.Text = workSheet.Cell(2, 6).Value.ToString();
            TextBoxFullDepth.Text = workSheet.Cell(2, 7).Value.ToString();
            TextBoxDinType.Text = workSheet.Cell(2, 8).Value.ToString();
            TextBoxPrituplenie.Text = workSheet.Cell(2, 9).Value.ToString();
            TextBoxTop.Text = workSheet.Cell(2, 10).Value.ToString();
            TextBoxLeft.Text = workSheet.Cell(2, 11).Value.ToString();
            TextBoxRight.Text = workSheet.Cell(2, 12).Value.ToString();
            TextBoxADLeft.Text = workSheet.Cell(2, 13).Value.ToString();
            TextBoxADWidth.Text = workSheet.Cell(2, 14).Value.ToString();
            TextBoxBprituplenie.Text = workSheet.Cell(2, 15).Value.ToString();
            TextBoxBTop.Text = workSheet.Cell(2, 16).Value.ToString();
            TextBoxBLeft.Text = workSheet.Cell(2, 17).Value.ToString();
            TextBoxBRight.Text = workSheet.Cell(2, 18).Value.ToString();
            TextBoxBDLeft.Text = workSheet.Cell(2, 19).Value.ToString();
            TextBoxBDWidth.Text = workSheet.Cell(2, 20).Value.ToString();
            TextBoxCprituplenie.Text = workSheet.Cell(2, 21).Value.ToString();
            TextBoxCTop.Text = workSheet.Cell(2, 22).Value.ToString();
            TextBoxCDLength.Text = workSheet.Cell(2, 23).Value.ToString();
            TextBoxCRight.Text = workSheet.Cell(2, 24).Value.ToString();
            TextBoxCDWidth.Text = workSheet.Cell(2, 25).Value.ToString();
            TextBoxD_PRITUPLENIE.Text = workSheet.Cell(2, 26).Value.ToString();
            TextBoxDTop.Text = workSheet.Cell(2, 27).Value.ToString();
            TextBoxDLeft.Text = workSheet.Cell(2, 28).Value.ToString();
            TextBoxDRight.Text = workSheet.Cell(2, 29).Value.ToString();
            TextBoxDDLeft.Text = workSheet.Cell(2, 30).Value.ToString();
            TextBoxDDWidth.Text = workSheet.Cell(2, 31).Value.ToString();
            TextBoxOwner.Text = workSheet.Cell(2, 32).Value.ToString();
          // TextBoxValue.Text = workSheet.Cell(2, 33).Value.ToString();
          // TextBoxPower.Text = workSheet.Cell(2, 34).Value.ToString();
          // TextBoxNpower.Text = workSheet.Cell(2, 35).Value.ToString();
            CheckBoxEquipment.IsChecked = workSheet.Cell(2, 36).Value.ToString() == "1";
            CheckBoxBalance.IsChecked = workSheet.Cell(2, 37).Value.ToString() == "1";
            //TextBoxIDowner.Text = workSheet.Cell(2, 38).Value.ToString();
            TextBoxIDcertificate.Text = workSheet.Cell(2, 39).Value.ToString();
            ComboBoxOwner.Text = workSheet.Cell(2, 40).Value.ToString();
            ComboBoxGlobalType.Text = workSheet.Cell(2, 41).Value.ToString();
            TextBoxN15.Text = workSheet.Cell(2, 42).Value.ToString();
            TextBoxN25.Text = workSheet.Cell(2, 43).Value.ToString();
            TextBoxN4.Text = workSheet.Cell(2, 44).Value.ToString();
            TextBoxN6.Text = workSheet.Cell(2, 45).Value.ToString();
            TextBoxN10.Text = workSheet.Cell(2, 46).Value.ToString();
            TextBoxN16.Text = workSheet.Cell(2, 47).Value.ToString();
            TextBoxN25.Text = workSheet.Cell(2, 48).Value.ToString();
            TextBoxN35.Text = workSheet.Cell(2, 49).Value.ToString();
            TextBoxN50.Text = workSheet.Cell(2, 50).Value.ToString();
            TextBoxN70.Text = workSheet.Cell(2, 51).Value.ToString();
            TextBoxN95.Text = workSheet.Cell(2, 52).Value.ToString();
            TextBoxN120.Text = workSheet.Cell(2, 53).Value.ToString();
            TextBoxN185.Text = workSheet.Cell(2, 54).Value.ToString();
            DataGridKorobki.ItemsSource = importedBoxes;
        }
    }
 }

