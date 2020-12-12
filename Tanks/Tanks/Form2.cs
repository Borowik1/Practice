using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tanks
{
    public partial class Form2 : Form
    {
        private List<Apple> apples;
        private List<EnemyTank> enemies;
        private Kolobok kolobok;
        List<GameObject> dataSource;

        public Form2(List<Apple> apples, List<EnemyTank> enemies, Kolobok kolobok)
        {
            InitializeComponent();
            this.apples = apples;
            this.enemies = enemies;
            this.kolobok = kolobok;
            UpdateData(apples, enemies, kolobok);            
        }


        internal void UpdateData(List<Apple> apples, List<EnemyTank> enemies, Kolobok kolobok)
        {
            dataSource = new List<GameObject>();

            dataSource.Add((GameObject)kolobok);

            foreach(EnemyTank item in enemies)
            {
                dataSource.Add((GameObject)item);
            }

            foreach(Apple item in apples)
            {
                dataSource.Add((GameObject)item);
            }

            dataGridView1.DataSource = dataSource;
        }

        internal void Clear()
        {
            dataSource = new List<GameObject>();
            dataGridView1.DataSource = dataSource;
        }
    }
}
