using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ExBoxCatalogEditor.SQL
{
    public static class DbWorker
    {
        public static Dictionary<int, string> GetOwnersDictionary()
        {
            var result = new Dictionary<int, string>();
            try
            {
                var bd = new DataClassesExBoxesDataContext();
                result = (from o in bd.OWNERS
                          select new { key = o.OwnerId, value = o.OwnerName })
                         .ToDictionary(v => v.key, v => v.value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return result;
        }
        public static Dictionary<int, string> GetVvodAccessories(int globalItemTypeId)
        {
            var result = new Dictionary<int, string>();
            try
            {
                var db = new DataClassesExBoxesDataContext();
                result = (from cca in db.CABLE_GLANDS_ACC
                          where cca.N_TYPE == globalItemTypeId
                          select new { key = cca.N_ID, value = cca.VC_NAME }).ToDictionary(m => m.key, m => m.value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return result;
        }
        public static int GetVvodAccessoriesIdByName(string name)
        {
            int result = 0;
            try
            {
                var db = new DataClassesExBoxesDataContext();
                result = (from cca in db.CABLE_GLANDS_ACC
                          where cca.VC_NAME == name
                          select cca.N_ID).First();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return result;
        }
        public static int GetOwnerIdbyName(string Name)
        {
            var result = 0;
            try
            {
                var bd = new DataClassesExBoxesDataContext();
                result = (from o in bd.OWNERS
                          where o.OwnerName == Name
                          select o.OwnerId).Single();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return result;
        }
        public static Dictionary<int, string> GetMaterialDictionary()
        {
            var result = new Dictionary<int, string>();
            try
            {
                var tb = new DataClassesExBoxesDataContext();
                result = (from m in tb.KorobkiNameMaterial
                          select new
                          { key = m.KorobkiNameMaterialId, value = m.FullName }).ToDictionary(m => m.key, m => m.value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return result;
        }

        public static int GetMaterialIdByName(string name)
        {
            var result = 0;
            try
            {
                var gg = new DataClassesExBoxesDataContext();
                result = (from m in gg.KorobkiNameMaterial
                          where m.FullName == name
                          select m.KorobkiNameMaterialId).Single();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return result;
        }

        public static Dictionary<int, string> GetGlobalItemTypeVvodAccDictionary()
        {
            var result = new Dictionary<int, string>();
            try
            {
                var tb = new DataClassesExBoxesDataContext();
                result = (from g in tb.GlobalItemType
                          where g.Name != "" &&
                                g.IsVvodAcc == true
                          select new { key = g.GlobalItemTypeId, value = g.Name }).
                    ToDictionary(v => v.key, v => v.value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return result;
        }

        public static Dictionary<int, string> GetGlobalItemTypeDictionary()
        {
            var result = new Dictionary<int, string>();
            try
            {
                var tb = new DataClassesExBoxesDataContext();
                result = (from g in tb.GlobalItemType
                          where g.Name != ""
                          select new { key = g.GlobalItemTypeId, value = g.Name }).
                    ToDictionary(v => v.key, v => v.value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return result;
        }

        public static int GetGlobalItemTypeIdbyName(string Name)
        {
            var result = 0;
            try
            {
                var tb = new DataClassesExBoxesDataContext();
                result = (from g in tb.GlobalItemType
                          where g.Name == Name
                          select g.GlobalItemTypeId).Single();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return result;
        }
        public static Dictionary<int, string> GetDinTypesDictionary()
        {
            var result = new Dictionary<int, string>();
            try
            {
                var dt = new DataClassesExBoxesDataContext();
                result = (from d in dt.DIN_TYPES
                          select new
                          { key = d.N_TYPE_ID, value = d.VC_NAME }).ToDictionary(v => v.key, v => v.value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return result;
        }

        public static int GetDinTypesIdbyNameDictionaty(string Name)
        {
            var result = 0;

            try
            {
                var dn = new DataClassesExBoxesDataContext();
                result = (from d in dn.DIN_TYPES where d.VC_NAME == Name select d.N_TYPE_ID).Single();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return result;
        }
        public static Dictionary<int, string> GetCableGlandsDictionary()
        {
            var result = new Dictionary<int, string>();
            try
            {
                var cg = new DataClassesExBoxesDataContext();
                result = (from c in cg.CABLE_GLANDS_ACC
                          select new { key = c.N_ID, value = c.VC_NAME }).ToDictionary(v => v.key, v => v.value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return result;
        }

        public static int GetCableGlandsIdbyName(string Name)
        {
            var result = 0;
            try
            {
                var cg = new DataClassesExBoxesDataContext();
                result = (from c in cg.CABLE_GLANDS_ACC where c.VC_NAME == Name select c.N_ID).Single();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return result;
        }
        public static Dictionary<int, string> GetSpecificationDictionary()
        {
            var result = new Dictionary<int, string>();
            try
            {
                var bd = new DataClassesExBoxesDataContext();
                result = (from r in bd.RazdelSp
                          select new { key = r.RazdelSpId, value = r.NameRazdel})
                         .ToDictionary(v => v.key, v => v.value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return result;
        }
            public static int GetRazdelSpIdbyName(string Name)
        {
            var result = 0;
            try
            {
                var bd = new DataClassesExBoxesDataContext();
                result = (from r in bd.RazdelSp
                          where r.NameRazdel == Name
                          select r.RazdelSpId).Single();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return result;
        }
    }
}
