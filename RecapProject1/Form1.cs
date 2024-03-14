using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RecapProject1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //Bu merhod bir eventtir bunu açtığımızda buna abone oluyoruz
        private void Form1_Load(object sender, EventArgs e)
        {
            //Burası listeyi programı çalıştırdığımızda geitrmek için kullanılan kodlardır.
            ListProducts();
            ListCategories();
        }

        private void ListProducts()
        {
            /*Garbage collector'ü beklemeden Northwindcontext'i direkt uçur anlamına geliyor bu kod bloğu
            Sırf performans kaynaklı bir hamledir bu...
            */
            using (NorthwindContext context = new NorthwindContext())
            {
                //bu veri tabanına select*from sorgusunu yollar
                dgwProducts.DataSource = context.Products.ToList();
            }
        }

        private void ListProductsByCategory(int categoryId)
        {
            //Aşağıda delege kullanımı var p=>p.CategoryId ile...
            using (NorthwindContext context = new NorthwindContext())
            { 
                dgwProducts.DataSource = context.Products.Where(p=>p.CategoryId==categoryId).ToList();
            }
        }

        private void ListProductsByProductName(string key)
        {
            //Aşağıda delege kullanımı var p=>p.CategoryId ile...
            using (NorthwindContext context = new NorthwindContext())
            {
                //sql server büyük küçük harf duyarsızdır.
                dgwProducts.DataSource = context.Products.Where(p => p.ProductName.ToLower().Contains(key.ToLower())).ToList();
            }
        }

        private void ListCategories()
        {
            using (NorthwindContext context = new NorthwindContext())
            {
                
                cbxCategory.DataSource = context.Categories.ToList();
                //Activator ile göstermeye çalışıcaz şimdi
                cbxCategory.DisplayMember = "CategoryName";
                cbxCategory.ValueMember = "CategoryId";
            }
        }

        private void cbxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Seçilen kategorinin id sine göre listeleme işlemini yaptırdık bunun içinde
                ListProductsByCategory(Convert.ToInt32(cbxCategory.SelectedValue));
            }
            catch
            { 
                
            }
        }

        /*Deffensive programming yapalım biraz mesela arama kutusuna bir şeyler yazığ sildiğimizde null değeri 
        göndericektir bunun önüne geçmeye çalışalım */
        /*Eğer hem id hem de isime göre arama yapmak istiyorsak where koşuluna ve koşul ifadeleri eklemesi 
         yaparak iş ihtiyacını karşılayabilirz
         */
        private void tbxSearch_TextChanged(object sender, EventArgs e)
        {
            string key = tbxSearch.Text;
            if (string.IsNullOrEmpty(key))
            {
                ListProducts();
            }
            else
            {
                ListProductsByProductName(tbxSearch.Text);
            }
        }
    }
}
