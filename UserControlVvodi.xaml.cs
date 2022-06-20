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
    /// Логика взаимодействия для UserControl2.xaml
    /// </summary>
    public partial class UserControlVvodi
    {
        private Dictionary<int, string> Groups = GetGroupDictionary();
        private Dictionary<int, string> GlobalTypes = DbWorker.GetGlobalItemTypeDictionary();
        private Dictionary<int, string> Seals = DbWorker.GetVvodAccessories(41);//41 - уплотнение
        private Dictionary<int, string> Locknuts = DbWorker.GetVvodAccessories(42);
        private Dictionary<int, string> Earthtags = DbWorker.GetVvodAccessories(43);
        private Dictionary<int, string> OwnerCode = DbWorker.GetOwnersDictionary();
        public UserControlVvodi()
        {
            InitializeComponent();
            LoadControlsData();
            LoadData();
        }
        private void LoadControlsData()
        {
            foreach (var group in Groups)
            {
                ComboBoxGroup.Items.Add(group.Value);
            }
            foreach (var globalType in GlobalTypes)
            {
                ComboBoxGlobalType.Items.Add(globalType.Value);
            }

            foreach (var item in Seals)
            {
                ComboBoxSeal.Items.Add(item.Value);
            }

            foreach (var item in Locknuts)
            {
                ComboBoxLocknut.Items.Add(item.Value);
            }

            foreach (var item in Earthtags)
            {
                ComboBoxEarthtag.Items.Add(item.Value);
            }
            foreach (var owner in OwnerCode)
            {
                ComboBoxOwnerCode.Items.Add(owner.Value);
            }
        }
        private void LoadData()
        {
            try
            {
                var db = new DataClassesExBoxesDataContext();

                var vvodi = new List<VVODI>();

                if (TextBoxSearch.Text != "Поиск" && TextBoxSearch.Text != "")
                {
                    vvodi = db.VVODI.Where(vvod => vvod.RAZMER != null && (
                                                   vvod.VC_NAME.ToLower().Contains(TextBoxSearch.Text.ToLower()) ||
                                                   vvod.VC_OWNER_CODE.ToLower().Contains(TextBoxSearch.Text.ToLower())
                                                   )).ToList();
                }
                else
                {
                    vvodi = db.VVODI.Where(vvod => vvod.RAZMER != null).ToList();
                }
                DataGridVvVodi.ItemsSource = vvodi;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void DataGridVVVodi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridVvVodi.SelectedIndex == -1) return;
            var vvod = (VVODI)DataGridVvVodi.SelectedItem;
            if (vvod.GlobalItemTypeId != null)
            {
                //vvod.GlobalItemTypeId ?? 0 равнозначно vvod.GlobalItemTypeId == null ? 0 : vvod.GlobalItemTypeId.Value
                ComboBoxGlobalType.SelectedValue = GlobalTypes[vvod.GlobalItemTypeId == null ? 0 : vvod.GlobalItemTypeId.Value];
            }
            if (vvod.VC_OWNER_CODE != null)
            {
                ComboBoxOwnerCode.SelectedValue = vvod.VC_OWNER_CODE;
            }
            CheckBoxOstatok.IsChecked = vvod.IsMinimumBalance.GetValueOrDefault();
            TextBoxName.Text = vvod.VC_NAME;
            TextBoxDiametr.Text = vvod.DIAMETR;
            TextBoxRazmer.Text = vvod.RAZMER;
            TextBoxCabel.Text = vvod.KABEL.ToString();
            TextBoxVvodDiametr.Text = vvod.OUTER_DIAMETR_KABEL;
            TextBoxLength.Text = vvod.LENGTH_R.ToString();
            TextBoxColor.Text = vvod.COLOR.ToString();
            TextBoxArmorDiametr.Text = vvod.DIAMETR_ARMOR.ToString();
            TextBoxMinTemper.Text = vvod.TemperatureMin.ToString();
            TextBoxMaxTemper.Text = vvod.TemperatureMax.ToString();
            TextBoxManufac.Text = vvod.MANUFACTURER.ToString();
            TextBoxSchema.Text = vvod.VC_SCHEMA_TEMPLATE;
            TextBoxThread.Text = vvod.N_LENGTH_THREAD.ToString();
            TextBoxProtrusion.Text = vvod.N_LENGTH_PROTRUSION.ToString();
            TextBoxRoseCode.Text = vvod.ROSE_CODE != null ? vvod.ROSE_CODE.ToString() : "";
            CheckBoxCActive.IsChecked = vvod.C_ACTIVE == 1;
            CheckBoxAtex.IsChecked = vvod.B_ATEX == 1;
            CheckBoxIEC.IsChecked = vvod.B_TRTS == 1;
            TextBoxNType.Text = vvod.N_TYPE.ToString();
            TextBoxMaterial.Text = vvod.VC_MATERIAL;
            TextBoxShroud.Text = vvod.VC_SHROUD_CODE;
            ComboBoxSeal.SelectedValue = vvod.N_SEAL_ID;
            ComboBoxLocknut.SelectedValue = vvod.N_LOCKNUT_ID;
            ComboBoxEarthtag.SelectedValue = vvod.N_EARTHTAG_ID;
            CheckBoxEquipment.IsChecked = vvod.IsMainEquipment == true;
            TextBoxCertificate.Text = vvod.CertificatesId.ToString();
            TextBoxMountingDiameter.Text = vvod.MountingDiameter.ToString();
            CheckBoxOstatok.IsChecked = vvod.IsMinimumBalance == true;
            CheckBoxIsNeedHole.IsChecked = vvod.IsNeedHole == true;
        }
        private bool CheckUserInput()
        {
            if (TextBoxName.Text == string.Empty)
            {
                MessageBox.Show("Наименование не должно быть пустым");
                return false;
            }
            if (short.TryParse(TextBoxCabel.Text, out _) == false)
            {
                MessageBox.Show("Поле Кабель не должно быть пустым");
                return false;
            }
            if (TextBoxDiametr.Text == string.Empty)
            {
                MessageBox.Show("Поле Диаметр не должно быть пустым");
                return false;
            }
            if (TextBoxRazmer.Text == string.Empty)
            {
                MessageBox.Show("Поле Размер не должно быть пустым");
                return false;
            }
            if (decimal.TryParse(TextBoxLength.Text, out _) == false)
            {
                MessageBox.Show("Поле Длина не должно быть пустым");
                return false;
            }
            if (int.TryParse(TextBoxMinTemper.Text, out _) == false)
            {
                MessageBox.Show("Поле Мин.Температура не должно быть пустым");
                return false;
            }
            if (int.TryParse(TextBoxMaxTemper.Text, out _) == false)
            {
                MessageBox.Show("Поле Макс.Температура не должно быть пустым");
                return false;
            }
            if (decimal.TryParse(TextBoxManufac.Text, out _) == false)
            {
                MessageBox.Show("Поле Производитель не должно быть пустым");
                return false;
            }
            if (TextBoxSchema.Text == string.Empty)
            {
                MessageBox.Show("Поле Блок для чертежа не должно быть пустым");
                return false;
            }
            if (TextBoxOutDiametr.Text == string.Empty)
            {
                MessageBox.Show("Поле внешний диаметр не должно быть пустым");
                return false;
            }
            if (TextBoxDiametrMetal.Text == string.Empty)
            {
                MessageBox.Show("Поле Диаметр металла не должно быть пустым");
                return false;
            }
            if (decimal.TryParse(TextBoxThread.Text, out _) == false)
            {
                MessageBox.Show("Поле Длина резьбы не должно быть пустым");
                return false;
            }
            if (decimal.TryParse(TextBoxProtrusion.Text, out _) == false)
            {
                MessageBox.Show("Поле Длина выступа не должно быть пустым");
                return false;
            }
           
            if (decimal.TryParse(TextBoxCertificate.Text, out _) == false)
            {
                MessageBox.Show("Поле сертификат не должно быть пустым");
                return false;
            }
            if (decimal.TryParse(TextBoxMountingDiameter.Text, out _) == false)
            {
                MessageBox.Show("Поле монтажный диаметр не должно пустым");
                return false;
            }
            if (ComboBoxSubGroup.SelectedIndex == -1)
            {
                MessageBox.Show("Подгруппа не выбрана");
                return false;
            }
            if (ComboBoxGlobalType.SelectedIndex == -1)
            {
                MessageBox.Show("Поле Тип изделий не выбрано");
                return false;
            }
            if (ComboBoxSeal.SelectedIndex == -1)
            {
                MessageBox.Show("Поле Уплотнение не выбрано");
                return false;
            }
            if (ComboBoxLocknut.SelectedIndex == -1)
            {
                MessageBox.Show("Поле Контргайка не выбрано");
                return false;
            }
            if (ComboBoxEarthtag.SelectedIndex == -1)
            {
                MessageBox.Show("Поле Кольцо заземления не выбрано");
                return false;
            }
            if (ComboBoxOwnerCode.SelectedIndex == -1)
            {
                MessageBox.Show("Поле Код производителя не выбрано");
            }
            return true;
        }
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (CheckUserInput() == false) return;
            try
            {
                var db = new DataClassesExBoxesDataContext();
                var copies = db.VVODI.Where(k => k.VC_NAME == TextBoxName.Text &&
                                                  k.VC_OWNER_CODE == TextBoxDiametr.Text).ToList();
                if (copies.Count > 0)
                {
                    MessageBox.Show("Указанный ввод уже есть в каталоге");
                    return;
                }
                var id = db.VVODI.Max(VVODI => VVODI.N_ID) + 1;
                var globalTypeId = DbWorker.GetGlobalItemTypeIdbyName(ComboBoxGlobalType.SelectedValue.ToString());
                var vvod = new VVODI
                {
                    // поля
                    N_ID = id,
                    VC_NAME = TextBoxName.Text,
                    DIAMETR = TextBoxDiametr.Text,
                    RAZMER = TextBoxRazmer.Text,
                    KABEL = (short)GetSubGroupIdbyName(
                        ComboBoxGroup.SelectedValue.ToString(),
                        ComboBoxSubGroup.SelectedValue.ToString()),
                    OUTER_DIAMETR = decimal.Parse(TextBoxOutDiametr.Text),
                    LENGTH_R = decimal.Parse(TextBoxLength.Text),
                    DIAMETR_ARMOR = decimal.Parse(TextBoxArmorDiametr.Text),
                    TemperatureMin = int.Parse(TextBoxMinTemper.Text),
                    TemperatureMax = int.Parse(TextBoxMaxTemper.Text),
                    OUTER_DIAMETR_KABEL = TextBoxVvodDiametr.Text,
                    DIAMETR_METAL = TextBoxDiametrMetal.Text,
                    MANUFACTURER = int.Parse(TextBoxManufac.Text),
                    VC_SCHEMA_TEMPLATE = TextBoxSchema.Text,
                    N_LENGTH_THREAD = decimal.Parse(TextBoxThread.Text),
                    N_LENGTH_PROTRUSION = decimal.Parse(TextBoxProtrusion.Text),
                    ROSE_CODE = TextBoxRoseCode.Text,
                    C_ACTIVE = (short)(CheckBoxCActive.IsChecked.GetValueOrDefault() ? 0 : 1),
                    VC_OWNER_CODE = ComboBoxOwnerCode.SelectedValue.ToString(),
                    B_ATEX = (short)(CheckBoxAtex.IsChecked.GetValueOrDefault() ? 0 : 1),
                    B_TRTS = (short)(CheckBoxTRTS.IsChecked.GetValueOrDefault() ? 0 : 1),
                    B_IEC = (short)(CheckBoxIEC.IsChecked.GetValueOrDefault() ? 0 : 1),
                    N_TYPE = globalTypeId,
                    VC_MATERIAL = TextBoxMaterial.Text,
                    VC_SHROUD_CODE = TextBoxShroud.Text,
                    N_SEAL_ID = DbWorker.GetVvodAccessoriesIdByName(ComboBoxSeal.SelectedValue.ToString()),
                    N_LOCKNUT_ID = DbWorker.GetVvodAccessoriesIdByName(ComboBoxLocknut.SelectedValue.ToString()),
                    N_EARTHTAG_ID = DbWorker.GetVvodAccessoriesIdByName(ComboBoxEarthtag.SelectedValue.ToString()),
                    CertificatesId = int.Parse(TextBoxCertificate.Text),
                    MountingDiameter = decimal.Parse(TextBoxMountingDiameter.Text),
                    GlobalItemTypeId = globalTypeId,
                    IsMinimumBalance = CheckBoxOstatok.IsChecked.GetValueOrDefault() ? true : false,
                    IsNeedHole = CheckBoxIsNeedHole.IsChecked.GetValueOrDefault() ? true : false,
                };
                db.VVODI.InsertOnSubmit(vvod);
                db.SubmitChanges();
                LoadData();
                MessageBox.Show("Ввод добавлен");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridVvVodi.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите строку", "Ошибка");
                return;
            }
            if (CheckUserInput() == false) return;
            try
            {
                var bd = new DataClassesExBoxesDataContext();
                var item = (VVODI)DataGridVvVodi.SelectedItem;
                var globalTypeId = DbWorker.GetGlobalItemTypeIdbyName(ComboBoxGlobalType.SelectedValue.ToString());
                var qulity = bd.VVODI.Where(vvod => vvod.N_ID == item.N_ID);
                foreach (var vvod in qulity)
                {
                    vvod.VC_NAME = TextBoxName.Text;
                    vvod.KABEL = short.Parse(TextBoxCabel.Text);
                    vvod.DIAMETR = TextBoxDiametr.Text;
                    vvod.RAZMER = TextBoxRazmer.Text;
                    vvod.LENGTH_R = decimal.Parse(TextBoxLength.Text);
                    vvod.COLOR = int.Parse(TextBoxColor.Text);
                    vvod.TemperatureMin = int.Parse(TextBoxMinTemper.Text);
                    vvod.TemperatureMax = int.Parse(TextBoxMaxTemper.Text);
                    vvod.MANUFACTURER = int.Parse(TextBoxManufac.Text);
                    vvod.VC_SCHEMA_TEMPLATE = TextBoxSchema.Text;
                    vvod.VC_MATERIAL = TextBoxMaterial.Text;
                    vvod.OUTER_DIAMETR = decimal.Parse(TextBoxOutDiametr.Text);
                    vvod.DIAMETR_ARMOR = decimal.Parse(TextBoxArmorDiametr.Text);
                    vvod.OUTER_DIAMETR_KABEL = TextBoxOutDiametr.Text;
                    vvod.DIAMETR_METAL = TextBoxDiametrMetal.Text;
                    vvod.N_LENGTH_THREAD = decimal.Parse(TextBoxThread.Text);
                    vvod.N_LENGTH_PROTRUSION = decimal.Parse(TextBoxProtrusion.Text);
                    vvod.ROSE_CODE = TextBoxRoseCode.Text;
                    vvod.VC_OWNER_CODE = ComboBoxOwnerCode.SelectedValue.ToString();
                    vvod.GlobalItemTypeId = globalTypeId;
                    vvod.N_TYPE = globalTypeId;
                    vvod.VC_SHROUD_CODE = TextBoxShroud.Text;
                    vvod.N_LOCKNUT_ID = DbWorker.GetVvodAccessoriesIdByName(ComboBoxLocknut.SelectedValue.ToString());
                    vvod.N_EARTHTAG_ID = DbWorker.GetVvodAccessoriesIdByName(ComboBoxEarthtag.SelectedValue.ToString());
                    vvod.N_SEAL_ID = DbWorker.GetVvodAccessoriesIdByName(ComboBoxSeal.SelectedValue.ToString());
                    vvod.CertificatesId = int.Parse(TextBoxCertificate.Text);
                    vvod.MountingDiameter = decimal.Parse(TextBoxMountingDiameter.Text);
                    vvod.IsMinimumBalance = CheckBoxOstatok.IsChecked.GetValueOrDefault();
                }
                bd.SubmitChanges();
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
                result = (from v in db.VVODI
                          where v.KABEL == null &&
                          v.RAZMER == null
                          select new { key = v.N_ID, value = v.VC_NAME })
                         .ToDictionary(v => v.key, v => v.value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return result;
        }
        private Dictionary<int, string> GetSubGroupDictionary(int parentId)
        {
            var result = new Dictionary<int, string>();
            try
            {
                var db = new DataClassesExBoxesDataContext();
                result = (from v in db.VVODI
                          where v.KABEL == parentId &&
                          v.RAZMER == null
                          select new { key = v.N_ID, value = v.VC_NAME })
                         .ToDictionary(v => v.key, v => v.value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return result;
        }
        private int GetSubGroupIdbyName(string nameGroup, string nameSubgroup)
        {
            var result = 0;
            try
            {
                var groupId = GetGroupIdbyName(nameGroup);
                var db = new DataClassesExBoxesDataContext();
                result = (from v in db.VVODI
                          where v.KABEL == groupId &&
                                v.VC_NAME == nameSubgroup
                          select v.N_ID).First();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return result;
        }
        private int GetGroupIdbyName(string name)
        {
            var result = 0;
            try
            {
                var db = new DataClassesExBoxesDataContext();
                result = (from v in db.VVODI
                          where v.VC_NAME == name &&
                          v.KABEL == null
                          select v.N_ID).First();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return result;
        }
        private void ComboBoxSubGroup_DropDownOpened(object sender, EventArgs e)
        {
            ComboBoxSubGroup.Items.Clear();
            if (ComboBoxGroup.SelectedIndex == -1) return;
            var parentId = (from g in Groups
                            where g.Value == ComboBoxGroup.SelectedValue.ToString()
                            select g.Key).First();
            var subGroups = GetSubGroupDictionary(parentId);
            foreach (var item in subGroups)
            {
                ComboBoxSubGroup.Items.Add(item.Value);
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
        private void TextBoxSearch_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter) return;
            //if (TextBoxSearch.Text.Length < 3) return;
            LoadData();
        }
        private void ComboBoxGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxSubGroup.SelectedIndex = -1;
        }

        private void ButtonExport_Click(object sender, RoutedEventArgs e)
        {
            var items = DataGridVvVodi.ItemsSource.Cast<VVODI>().ToList();
            var wb = new XLWorkbook();

            var ws = wb.Worksheets.Add("Основной");
            var i = 2;
            ws.Cell(1, 1).Value = "Имя ввода";
            ws.Cell(1, 2).Value = "Диаметр";
            ws.Cell(1, 3).Value = "Размер";
            ws.Cell(1, 4).Value = "Кабель";
            ws.Cell(1, 5).Value = "Внешний диаметр кабеля";
            ws.Cell(1, 6).Value = "Длинна";
            ws.Cell(1, 7).Value = "Цвет";
            ws.Cell(1, 8).Value = "Диаметр брони";
            ws.Cell(1, 9).Value = "Мин. температура";
            ws.Cell(1, 10).Value = "Макс. температура";
            ws.Cell(1, 11).Value = "Внешний диаметр";
            ws.Cell(1, 12).Value = "Диаметр металла";
            ws.Cell(1, 13).Value = "Производитель";
            ws.Cell(1, 14).Value = "Блок для чертежа";
            ws.Cell(1, 15).Value = "Длинна резьбы";
            ws.Cell(1, 16).Value = "Длинна выступа";
            ws.Cell(1, 17).Value = "Код Rose";
            ws.Cell(1, 18).Value = "С_ACTIVE";
            ws.Cell(1, 19).Value = "Код владельца";
            ws.Cell(1, 20).Value = "B_ATEX";
            ws.Cell(1, 21).Value = "B_TRTS";
            ws.Cell(1, 22).Value = "B_IEC";
            ws.Cell(1, 23).Value = "Тип ввода";
            ws.Cell(1, 24).Value = "Материал";
            ws.Cell(1, 25).Value = "Код кожуха";
            ws.Cell(1, 26).Value = "Уплотнение";
            ws.Cell(1, 27).Value = "Контргайка";
            ws.Cell(1, 28).Value = "Заземление";
            ws.Cell(1, 29).Value = "ID_сертификата";
            ws.Cell(1, 30).Value = "Монтажный диаметр";
            ws.Cell(1, 31).Value = "Неснижаемый остаток";
            ws.Cell(1, 32).Value = "Необходимое отверстие";
            foreach (var o in items)
            {
                ws.Cell(i, 1).Value = "'" + o.VC_NAME;         
                ws.Cell(i, 2).Value = o.DIAMETR;
                ws.Cell(i, 3).Value = o.RAZMER;
                ws.Cell(i, 4).Value = o.KABEL;
                ws.Cell(i, 5).Value = o.OUTER_DIAMETR_KABEL;
                ws.Cell(i, 6).Value = o.LENGTH_R;
                ws.Cell(i, 7).Value = o.COLOR;
                ws.Cell(i, 8).Value = o.DIAMETR_ARMOR;
                ws.Cell(i, 9).Value = o.TemperatureMin;
                ws.Cell(i, 10).Value = o.TemperatureMax;
                ws.Cell(i, 11).Value = o.OUTER_DIAMETR;
                ws.Cell(i, 12).Value = o.DIAMETR_METAL;
                ws.Cell(i, 13).Value = o.MANUFACTURER;
                ws.Cell(i, 14).Value = o.VC_SCHEMA_TEMPLATE;
                ws.Cell(i, 15).Value = o.N_LENGTH_THREAD;
                ws.Cell(i, 16).Value = o.N_LENGTH_PROTRUSION;
                ws.Cell(i, 17).Value = o.ROSE_CODE;
                ws.Cell(i, 18).Value = o.C_ACTIVE;
                ws.Cell(i, 19).Value = o.VC_OWNER_CODE;
                ws.Cell(i, 20).Value = o.B_ATEX;
                ws.Cell(i, 21).Value = o.B_TRTS;
                ws.Cell(i, 22).Value = o.B_IEC;
                ws.Cell(i, 23).Value = o.N_TYPE;
                ws.Cell(i, 24).Value = o.VC_MATERIAL;
                ws.Cell(i, 25).Value = o.VC_SHROUD_CODE;
                ws.Cell(i, 26).Value = o.N_LOCKNUT_ID;
                ws.Cell(i, 27).Value = o.N_EARTHTAG_ID;
                ws.Cell(i, 28).Value = o.CertificatesId;
                ws.Cell(i, 29).Value = o.MountingDiameter;
                ws.Cell(i, 30).Value = o.IsMinimumBalance;
                ws.Cell(i, 31).Value = o.IsNeedHole;
                ws.Cell(i, 32).Value = o.IsMainEquipment;
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
            var importedBoxes = new List<VVODI>();
            TextBoxName.Text = workSheet.Cell(2, 1).Value.ToString();
            TextBoxRazmer.Text = workSheet.Cell(2, 2).Value.ToString();
            TextBoxCabel.Text = workSheet.Cell(2, 3).Value.ToString();
            TextBoxVvodDiametr.Text = workSheet.Cell(2, 4).Value.ToString();
            TextBoxLength.Text = workSheet.Cell(2, 5).Value.ToString();
            TextBoxColor.Text = workSheet.Cell(2, 6).Value.ToString();
            TextBoxArmorDiametr.Text = workSheet.Cell(2, 7).Value.ToString();
            TextBoxMinTemper.Text = workSheet.Cell(2, 8).Value.ToString();
            TextBoxMaxTemper.Text = workSheet.Cell(2, 9).Value.ToString();
            TextBoxOutDiametr.Text = workSheet.Cell(2, 10).Value.ToString();
            TextBoxDiametrMetal.Text = workSheet.Cell(2, 11).Value.ToString();
            TextBoxManufac.Text = workSheet.Cell(2, 12).Value.ToString();
            TextBoxSchema.Text = workSheet.Cell(2, 13).Value.ToString();
            TextBoxThread.Text = workSheet.Cell(2, 14).Value.ToString();
            TextBoxProtrusion.Text = workSheet.Cell(2, 15).Value.ToString();
            TextBoxRoseCode.Text = workSheet.Cell(2, 16).Value.ToString();
            CheckBoxCActive.IsChecked= workSheet.Cell(2, 17).Value.ToString() == "1";
            ComboBoxOwnerCode.Text = workSheet.Cell(2, 18).Value.ToString();
            CheckBoxAtex.IsChecked = workSheet.Cell(2, 19).Value.ToString() == "1";
            CheckBoxTRTS.IsChecked = workSheet.Cell(2, 20).Value.ToString() == "1";
            CheckBoxIEC.IsChecked = workSheet.Cell(2, 21).Value.ToString() == "1";
            TextBoxNType.Text = workSheet.Cell(2, 22).Value.ToString();
            TextBoxMaterial.Text = workSheet.Cell(2, 23).Value.ToString();
            TextBoxShroud.Text= (workSheet.Cell(2, 24).Value.ToString());
            ComboBoxLocknut.Text= (workSheet.Cell(2, 25).Value.ToString());
            ComboBoxEarthtag.Text= (workSheet.Cell(2, 26).Value.ToString());
            TextBoxCertificate.Text= (workSheet.Cell(2, 27).Value.ToString());
            TextBoxMountingDiameter.Text= (workSheet.Cell(2, 28).Value.ToString());
            CheckBoxOstatok.IsChecked = workSheet.Cell(2, 29).Value.ToString() == "1";
            CheckBoxIsNeedHole.IsChecked= workSheet.Cell(2, 30).Value.ToString() == "1";
            CheckBoxEquipment.IsChecked= workSheet.Cell(2, 31).Value.ToString() == "1";
            ComboBoxSeal.Text = workSheet.Cell(2, 32).Value.ToString();
            ComboBoxLocknut.Text = workSheet.Cell(2, 33).Value.ToString();
            ComboBoxEarthtag.Text = workSheet.Cell(2, 34).Value.ToString();
            DataGridVvVodi.ItemsSource = importedBoxes;
        }
    }
   
}


