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

namespace ExBoxCatalogEditor
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class UseControlKlemmi
    {
        private Dictionary<int, string> ItemType = SQL.DbWorker.GetGlobalItemTypeDictionary();
        private Dictionary<int, string> DinType = SQL.DbWorker.GetDinTypesDictionary();
        private Dictionary<int, string> Groups = GetGroupDictionary();
        private Dictionary<int, string> Owners = SQL.DbWorker.GetOwnersDictionary();
        public UseControlKlemmi()
        {
            InitializeComponent();
            LoadData();
            LoadControlsData();
        }
        private void LoadControlsData()
        {
            foreach (var global in ItemType)
            {
                ComboBoxGlobal.Items.Add(global.Value);
            }

            foreach (var din in DinType)
            {
                ComboBoxDin.Items.Add(din.Value);
            }

            foreach (var group in Groups)
            {
                ComboBoxGroup.Items.Add(group.Value);
            }

            foreach (var owner in Owners)
            {
                ComboBoxOwner.Items.Add(owner.Value);
            }
        }
        private void LoadData()
        {
            try
            {
                var db = new DataClassesExBoxesDataContext();
                var klemmi = db.KLEMMI.Where(x=>x.NAME.ToLower().Contains(TextBoxSearch.Text.ToLower())).ToList();
                DataGridKlemmi.ItemsSource = klemmi;
                if (TextBoxSearch.Text != "Поиск" && TextBoxSearch.Text != "")
                {
                    klemmi = db.KLEMMI.Where(klemma=> (klemma.NAME.ToLower().Contains(TextBoxSearch.Text.ToLower()))).ToList();
                }
                else
                {
                    klemmi = db.KLEMMI.ToList();      
                }
                DataGridKlemmi.ItemsSource = klemmi;   
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void DataGridKlemmi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridKlemmi.SelectedIndex == -1) return;
            var item = (KLEMMI)DataGridKlemmi.SelectedItem;
            
            if (item.GlobalItemTypeId != null)
            {
                ComboBoxGlobal.SelectedValue = ItemType[item.GlobalItemTypeId??0];               
            }
            if (item.DIN_TYPE != null)
            {
                ComboBoxDin.SelectedValue = DinType[item.DIN_TYPE ?? 0];
            }
            else
            {
                ComboBoxDin.SelectedIndex = -1;
            }
            if (item.TYPE != null)
            {
                ComboBoxGroup.SelectedValue = Groups[item.TYPE ?? 0];
            }
            else
            {
                ComboBoxGroup.SelectedIndex = -1;
            }
            if (item.OWNER != null)
            {
                ComboBoxOwner.SelectedValue = Owners[item.OWNER ?? 0];
            }
            else
            {
                ComboBoxOwner.SelectedIndex = -1;
            }
            TextBoxName.Text = item.NAME;
            TextBoxSechenie.Text = item.SECHENIE.ToString();
            TextBoxTok.Text = item.TOK.ToString();
            TextBoxNapr.Text = item.NAPRYAZENIE.ToString();
            TextBoxWidth.Text = item.WIDTH.ToString();
            TextBoxLength.Text = item.LENGTH.ToString();
            TextBoxHeidth.Text = item.HEIGHT.ToString();
            TextBoxIMG.Text = item.VC_IMG_TEMPLATE;
            TextBoxSchema.Text = item.VC_SCHEMA_TEMPLATE;
            TextBoxRose.Text = item.ROSE_CODE;
            TextBoxOwner.Text = item.VC_OWNER_CODE;
            TextBoxNLength.Text = item.N_LENGTH.ToString();
            TextBoxControl.Text = item.IS_CONTROL_ELEMENT.ToString();
            TextBoxTwice.Text = item.N_TWICE.ToString();
            TextBoxEquip.Text = item.IsMainEquipment.ToString();
            TextBoxCertificate.Text = item.CertificatesId.ToString();
            TextBoxHead.Text = item.DissipatedHeat.ToString();
            CheckBoxMinimumBalance.IsChecked = item.IsMinimumBalance == true;
            CheckBoxAtex.IsChecked = item.B_ATEX == 1;
            CheckBoxTRTS.IsChecked = item.B_TRTS == 1;
            CheckBoxIEC.IsChecked = item.B_IEC == 1;
            CheckBoxEquipment.IsChecked = item.IsMainEquipment == true;
            CheckBoxCActive.IsChecked = item.C_ACTIVE == 1;
        }
        private bool CheckUserInput()
        { 
            if (decimal.TryParse(TextBoxTok.Text, out _) == false)            
            {
                MessageBox.Show("введите корректный ток");
                return false;
            }

            if (TextBoxName.Text == string.Empty)
            {
                MessageBox.Show("введите имя");
                return false;
            }
            if (decimal.TryParse(TextBoxSechenie.Text, out _) == false)
            {
                MessageBox.Show("укажите сечение");
                return false;
            }

            if (int.TryParse(TextBoxNapr.Text, out _) == false)
            {
                MessageBox.Show("введите напряжение");
                return false;
            }
         
            if (decimal.TryParse(TextBoxWidth.Text, out _) == false)
            {
                MessageBox.Show("введите ширину");
                return false;
            }
            if (decimal.TryParse(TextBoxLength.Text, out _) == false)
            {
                MessageBox.Show("введите длину");
                return false;
            }
            if (decimal.TryParse(TextBoxHeidth.Text, out _) == false)
            {
                MessageBox.Show("введите высоту");
                return false;
            }
            if (TextBoxIMG.Text == string.Empty)
            {
                MessageBox.Show("укажите картину");
                return false;
            }
            if (TextBoxSchema.Text == string.Empty)
            {
                MessageBox.Show("введите блок для чертежа");
                return false;
            }
            if (TextBoxRose.Text == string.Empty)
            {
                MessageBox.Show("введите код ROSE");
                return false;
            }
            if (TextBoxOwner.Text == string.Empty)
            {
                MessageBox.Show("введите код владельца");
                return false;
            }
            if (decimal.TryParse(TextBoxNLength.Text, out _) == false)
            {
                MessageBox.Show("введите N_высоту");
                return false;
            }
            if (int.TryParse(TextBoxControl.Text, out _) == false)
            {
                MessageBox.Show("введите контрольную высоту");
                return false;
            }
            if (int.TryParse(TextBoxTwice.Text, out _) == false)
            {
                MessageBox.Show("введите Twice");
                return false;
            }
            if (decimal.TryParse(TextBoxEquip.Text, out _) == false)
            {
                MessageBox.Show("введите оборудование");
                return false;
            }
            if (int.TryParse(TextBoxCertificate.Text, out _) == false)
            {
                MessageBox.Show("укажите сертификат");
                return false;
            }
            if (decimal.TryParse(TextBoxHead.Text, out _) == false)
            {
                MessageBox.Show("укажите выделяемое тепло");
                return false;
            }
            if (ComboBoxOwner.SelectedIndex == -1)
            {
                MessageBox.Show("Поле Имя производителя не выбрано");
                return false;
            }
            if (ComboBoxDin.SelectedIndex == -1)
            {
                MessageBox.Show("Поле Тип дин рейки не выбрано");
                return false;
            }
            if (ComboBoxGroup.SelectedIndex == -1)
            {
                MessageBox.Show("Поле Тип клеммы не выбрано");
                return false;
            }
            if (ComboBoxGlobal.SelectedIndex == -1)
            {
                MessageBox.Show("Поле Тип изделия не выбрано");
                return false;
            }
            return true;
        }
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (CheckUserInput() == false) return;
            try
            {
                var db = new DataClassesExBoxesDataContext();
                var copies = db.KLEMMI.Where(k => k.NAME == TextBoxName.Text &&
                                                  k.VC_OWNER_CODE == TextBoxOwner.Text).ToList();
                if (copies.Count > 0)
                {
                    MessageBox.Show("Указанная клемма уже есть в каталоге");
                    return;
                }
                var id = db.KLEMMI.Max(k => k.N_ID) + 1;
                var klemma = new KLEMMI
                {
                    N_ID = id,
                    NAME = TextBoxName.Text,
                    OWNER = int.Parse(ComboBoxOwner.SelectedValue.ToString()),
                    SECHENIE = decimal.Parse(TextBoxSechenie.Text),
                    TOK = decimal.Parse(TextBoxTok.Text),
                    NAPRYAZENIE = int.Parse(TextBoxNapr.Text),
                    WIDTH = Convert.ToDecimal(TextBoxWidth.Text),
                    LENGTH = decimal.Parse(TextBoxLength.Text),
                    HEIGHT = decimal.Parse(TextBoxHeidth.Text),
                    VC_IMG_TEMPLATE = TextBoxIMG.Text,
                    VC_SCHEMA_TEMPLATE = TextBoxSchema.Text,
                    ROSE_CODE = TextBoxRose.Text,
                    VC_OWNER_CODE = TextBoxOwner.Text.ToString(),
                    N_LENGTH = int.Parse(TextBoxNLength.Text),
                    IS_CONTROL_ELEMENT = short.Parse(TextBoxControl.Text),
                    N_TWICE = short.Parse(TextBoxTwice.Text),
                    IsMainEquipment = bool.Parse(TextBoxEquip.Text),
                    CertificatesId = int.Parse(TextBoxCertificate.Text),
                    DissipatedHeat = decimal.Parse(TextBoxHead.Text),
                };
                db.KLEMMI.InsertOnSubmit(klemma);
                db.SubmitChanges();
                LoadData();
                MessageBox.Show("Клемма добавлена");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridKlemmi.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите строку", "Ошибка");
                return;
            }
            if (CheckUserInput() == false) return;
            try
            {
                var tb = new DataClassesExBoxesDataContext();
                var item = (KLEMMI)DataGridKlemmi.SelectedItem;
                var qu = tb.KLEMMI.Where(klemma => klemma.N_ID == item.N_ID);
                foreach (var klemma in qu)
                {
                    klemma.NAME = TextBoxName.Text;
                    klemma.OWNER = int.Parse(ComboBoxOwner.SelectedValue.ToString());
                    klemma.SECHENIE = decimal.Parse(TextBoxSechenie.Text);
                    klemma.TOK = int.Parse(TextBoxTok.Text);
                    klemma.NAPRYAZENIE = int.Parse(TextBoxNapr.Text);
                    klemma.WIDTH = int.Parse(TextBoxWidth.Text);
                    klemma.LENGTH = int.Parse(TextBoxLength.Text);
                    klemma.HEIGHT = int.Parse(TextBoxHeidth.Text);
                    klemma.VC_IMG_TEMPLATE = TextBoxIMG.Text;
                    klemma.VC_SCHEMA_TEMPLATE = TextBoxSchema.Text;
                    klemma.ROSE_CODE = TextBoxRose.Text;
                    klemma.VC_OWNER_CODE = TextBoxOwner.Text;
                    klemma.N_LENGTH = short.Parse(TextBoxNLength.Text);
                    klemma.IS_CONTROL_ELEMENT = short.Parse(TextBoxControl.Text);
                    klemma.N_TWICE = short.Parse(TextBoxTwice.Text);
                    klemma.IsMainEquipment = bool.Parse(TextBoxEquip.Text);
                    klemma.CertificatesId = int.Parse(TextBoxCertificate.Text);
                    klemma.DissipatedHeat = decimal.Parse(TextBoxHead.Text);
                }
                tb.SubmitChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private static Dictionary<int, string> GetGroupDictionary()
        {
            var result = new Dictionary<int, string>();
            try
            {
                var db = new DataClassesExBoxesDataContext();
                result = (from k in db.KLEMMI
                          where k.OWNER == null
                          select new { key = k.N_ID, value = k.NAME })
                         .ToDictionary(k => k.key, k => k.value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return result;
        }
        private void TextBoxSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TextBoxSearch.Text == "Поиск")
            {
                TextBoxSearch.Text = "";
                TextBoxSearch.Focus();
            }    
        }
        private void TextBoxSearch_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter) return;
            LoadData();
        }
        private void ButtonExport_Click(object sender, RoutedEventArgs e)
        {
            var items = DataGridKlemmi.Items.Cast<KLEMMI>().ToList();
            var wb = new XLWorkbook();

            var ws = wb.Worksheets.Add("Основной");
            var i = 2;
            ws.Cell(1, 1).Value = "Наименование клеммы";
            ws.Cell(1, 2).Value = "Производитель";
            ws.Cell(1, 3).Value = "Сечение";
            ws.Cell(1, 4).Value = "Тип";
            ws.Cell(1, 5).Value = "Ток";
            ws.Cell(1, 6).Value = "Напряжение";
            ws.Cell(1, 7).Value = "Тип Динрейки";
            ws.Cell(1, 8).Value = "Ширина";
            ws.Cell(1, 9).Value = "Длина коробки";
            ws.Cell(1, 10).Value = "Высота";
            ws.Cell(1, 11).Value = "Картинка";
            ws.Cell(1, 12).Value = "Блок для чертежа";
            ws.Cell(1, 13).Value = "Код ROSE";
            ws.Cell(1, 14).Value = "Код владельца";
            ws.Cell(1, 15).Value = "B_Atex";
            ws.Cell(1, 16).Value = "B_TRTS";
            ws.Cell(1, 17).Value = "B_IEC";
            ws.Cell(1, 18).Value = "C_ACTIVE";
            ws.Cell(1, 19).Value = "Длина Динрейки";
            ws.Cell(1, 20).Value = "Контрольный элемент";
            ws.Cell(1, 21).Value = "Twice";
            ws.Cell(1, 22).Value = "Оборудование";
            ws.Cell(1, 23).Value = "Тип элемента";
            ws.Cell(1, 24).Value = "ID сертификата";
            ws.Cell(1, 25).Value = "Неснижаемый остаток";
            ws.Cell(1, 26).Value = "Выделяемое тепло";
                        foreach (var o in items)
            {
                ws.Cell(i, 1).Value = "'" + o.NAME;
                ws.Cell(i, 2).Value = o.OWNER;
                ws.Cell(i, 3).Value = o.SECHENIE;
                ws.Cell(i, 4).Value = o.TYPE;
                ws.Cell(i, 5).Value = o.TOK;
                ws.Cell(i, 6).Value = o.NAPRYAZENIE;
                ws.Cell(i, 7).Value = o.DIN_TYPE;
                ws.Cell(i, 8).Value = o.WIDTH;
                ws.Cell(i, 9).Value = o.LENGTH;
                ws.Cell(i, 10).Value = o.HEIGHT;
                ws.Cell(i, 11).Value = o.VC_IMG_TEMPLATE;
                ws.Cell(i, 12).Value = o.VC_SCHEMA_TEMPLATE;
                ws.Cell(i, 13).Value = o.ROSE_CODE;
                ws.Cell(i, 14).Value = o.VC_OWNER_CODE;
                ws.Cell(i, 15).Value = o.B_ATEX;
                ws.Cell(i, 16).Value = o.B_TRTS;
                ws.Cell(i, 17).Value = o.B_IEC;
                ws.Cell(i, 18).Value = o.C_ACTIVE;
                ws.Cell(i, 19).Value = o.N_LENGTH;
                ws.Cell(i, 20).Value = o.IS_CONTROL_ELEMENT;
                ws.Cell(i, 21).Value = o.N_TWICE;
                ws.Cell(i, 22).Value = o.IsMainEquipment;
                ws.Cell(i, 23).Value = o.GlobalItemTypeId;
                ws.Cell(i, 24).Value = o.CertificatesId;
                ws.Cell(i, 25).Value = o.IsMinimumBalance;
                ws.Cell(i, 26).Value = o.DissipatedHeat;
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

            if (opf.ShowDialog() == false) return;
            string filename = opf.FileName;
            var workBook = new XLWorkbook(filename);
            var workSheet = workBook.Worksheet(1);
            var importedBoxes = new List<KLEMMI>();
            TextBoxName.Text = workSheet.Cell(2, 1).Value.ToString();
            TextBoxSechenie.Text = workSheet.Cell(2, 2).Value.ToString();
            TextBoxTok.Text = workSheet.Cell(2, 3).Value.ToString();
            TextBoxNapr.Text = workSheet.Cell(2, 4).Value.ToString();
            TextBoxWidth.Text = workSheet.Cell(2, 6).Value.ToString();
            TextBoxLength.Text = workSheet.Cell(2, 7).Value.ToString();
            TextBoxHeidth.Text = workSheet.Cell(2, 8).Value.ToString();
            TextBoxIMG.Text = workSheet.Cell(2, 9).Value.ToString();
            TextBoxSchema.Text = workSheet.Cell(2, 10).Value.ToString();
            TextBoxRose.Text = workSheet.Cell(2, 11).Value.ToString();
            TextBoxOwner.Text = workSheet.Cell(2, 12).Value.ToString();
            TextBoxNLength.Text = workSheet.Cell(2, 14).Value.ToString();
            TextBoxControl.Text = workSheet.Cell(2, 15).Value.ToString();
            TextBoxTwice.Text = workSheet.Cell(2, 16).Value.ToString();
            TextBoxEquip.Text = workSheet.Cell(2, 17).Value.ToString();
            TextBoxCertificate.Text = workSheet.Cell(2, 19).Value.ToString();
            TextBoxHead.Text = workSheet.Cell(2, 20).Value.ToString();
            ComboBoxOwner.Text = workSheet.Cell(2, 21).Value.ToString();
            ComboBoxGlobal.Text = workSheet.Cell(2, 22).Value.ToString();
            ComboBoxDin.Text = workSheet.Cell(2, 23).Value.ToString();
            ComboBoxGroup.Text = workSheet.Cell(2, 24).Value.ToString();
            CheckBoxMinimumBalance.IsChecked = workSheet.Cell(2, 25).Value.ToString() == "1";
            CheckBoxAtex.IsChecked = workSheet.Cell(2, 26).Value.ToString() == "1";
            CheckBoxTRTS.IsChecked = workSheet.Cell(2, 27).Value.ToString() == "1";
            CheckBoxIEC.IsChecked = workSheet.Cell(2, 28).Value.ToString() == "1";
            CheckBoxEquipment.IsChecked = workSheet.Cell(2, 29).Value.ToString() == "1";
            CheckBoxCActive.IsChecked = workSheet.Cell(2, 30).Value.ToString() == "1";
            DataGridKlemmi.ItemsSource = importedBoxes; 
        }
    }
}

