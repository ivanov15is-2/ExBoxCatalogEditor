using System;
using System.Windows.Controls;
using MahApps.Metro.Controls;

namespace ExBoxCatalogEditor
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    ///     
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            HamburgerMenuItemCollection menuItems = new HamburgerMenuItemCollection
            {
                new HamburgerMenuGlyphItem {Label = "Клеммы" , Tag = new UseControlKlemmi(), Glyph = "\xF404" },
                new HamburgerMenuGlyphItem {Label = "Коробки" , Tag = new UserControlKorobki(), Glyph = "\xEBD2"},
                new HamburgerMenuGlyphItem {Label = "Вводы" , Tag = new UserControlVvodi(), Glyph = "\xEC7A" },
                new HamburgerMenuGlyphItem {Label = "Справочники", Tag = new UserControlOwnersAndMate() , Glyph = "\xE838"}                
            };
            HamburgerMenuControl.ItemsSource = menuItems;            
        }

        private void HamburgerMenu_OnItemClick(object sender, MahApps.Metro.Controls.ItemClickEventArgs e)
        {            
            HamburgerMenuGlyphItem i = e.ClickedItem as HamburgerMenuGlyphItem;
            if (i != null)
            {
                UserControl uc = new UserControl();
                switch (i.Label)
                {
                    case "Клеммы":
                        uc = new UseControlKlemmi();
                        break;

                    case "Коробки":
                        uc = new UserControlKorobki();
                        break;

                    case "Вводы":
                        uc = new UserControlVvodi();
                        break;
                        // добавление таблиц Owners, materials
                    case "Справочники":
                        uc = new UserControlOwnersAndMate();
                        break;
                }
                i.Tag = uc;
                this.HamburgerMenuControl.Content = i;
            }            
        }
    }
}