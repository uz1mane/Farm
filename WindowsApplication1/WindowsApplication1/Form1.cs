using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsApplication1
{
    public partial class Form1 : Form
    {
        Farm farm;

        public Form1()
        {
            InitializeComponent();
            Dictionary<CheckBox, Cell> field = new Dictionary<CheckBox, Cell>();
            foreach (CheckBox cb in panel1.Controls)
                field.Add(cb, new Cell());
            farm = new Farm(field);
        }

        private void UpdateInterface()
        {                       
            labelTime.Text = "Current time: " + farm.GetTime().ToString();
            labelMoney.Text = "Current money: " + farm.GetMoney().ToString();
            UpdateBoxes();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (sender as CheckBox);
            if (cb.Checked) farm.Plant(cb);
            else farm.Harvest(cb);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        { 
            decimal newInterval = 100 / numericUpDownSpeedMultiplier.Value;
            timer1.Interval = (int)newInterval;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {            
            farm.NextTick();
            UpdateInterface();            
        }
               
        private void UpdateBoxes()
        {
            foreach (KeyValuePair<CheckBox, Cell> kv in farm.field)
            {
                CheckBox cb = kv.Key;
                Color c = Color.White;
                switch (kv.Value.state)
                {
                    case CellState.Planted:
                        c = Color.Black;
                        break;
                    case CellState.Green:
                        c = Color.Green;
                        break;
                    case CellState.Immature:
                        c = Color.Yellow;
                        break;
                    case CellState.Mature:
                        c = Color.Red;
                        break;
                    case CellState.Overgrow:
                        c = Color.Maroon;
                        break;
                }
                cb.BackColor = c;
            }
        }    
        
    }

    class Farm
    {   
        public Dictionary<CheckBox, Cell> field;

        private int time;

        private int money;

        public Farm(Dictionary<CheckBox, Cell> field)
        {
            this.field = field;
            this.money = 100;
        }

        public int GetTime()
        {
            return time;
        }

        public int GetMoney()
        {
            return money;
        }

        public void NextTick()
        {
            time++;
            foreach (KeyValuePair<CheckBox, Cell> kv in field)            
                kv.Value.NextTick();            
        }

        public void Plant(CheckBox cb)
        {
            money += field[cb].Plant();            
        }

        public void Harvest(CheckBox cb)
        {
            money += field[cb].Harvest();           
        }
    }

    enum CellState
    {
        Empty,
        Planted,
        Green,
        Immature,
        Mature,
        Overgrow
    }

    class Cell
    {
        public CellState state = CellState.Empty;
        public int progress = 0;

        private const int prPlanted = 20;
        private const int prGreen = 100;
        private const int prImmature = 120;
        private const int prMature = 140;

        public int Plant()
        {
            int price = -2;
            state = CellState.Planted;
            progress = 1;
            return price;
        }

        public int Harvest()
        {
            int income = 0;
            switch (state)
            {
                case CellState.Planted:
                    income = 0;
                    break;
                case CellState.Green:
                    income = 0;
                    break;
                case CellState.Immature:
                    income = 3;
                    break;
                case CellState.Mature:
                    income = 5;
                    break;
                case CellState.Overgrow:
                    income = -1;
                    break;
            }
            state = CellState.Empty;
            progress = 0;
            return income;
        }

        public void NextTick()
        {
            if ((state != CellState.Empty) && (state != CellState.Overgrow))
            {
                progress++;
                if (progress < prPlanted) state = CellState.Planted;
                else if (progress < prGreen) state = CellState.Green;
                else if (progress < prImmature) state = CellState.Immature;
                else if (progress < prMature) state = CellState.Mature;
                else state = CellState.Overgrow;
            }
        }
    }
}