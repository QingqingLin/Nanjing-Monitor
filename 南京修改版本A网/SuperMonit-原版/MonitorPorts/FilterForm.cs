using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace MonitorPorts
{
    public partial class FilterForm : Form
    {
        public static double PcapLengthNum;
        public double MaxVOBCToGround;
        public double MaxGroundToVOBC;
        private List<string> BackUps = new List<string>();

        public FilterForm()
        {
            InitializeComponent();
            MaxVOBCToGround = MaxGroundToVOBC = 2.4;
            IPList ReadIP = new IPList();
            LoadIpConfiguration();
            LoadHistoryConfig();
        }

        private void Unit1_CheckedChanged(object sender, EventArgs e)
        {
            if (Unit1.Checked)
            {
                Unit2.Checked = false;
                Unit1.Checked = true;
            }
        }

        private void Unit2_CheckedChanged(object sender, EventArgs e)
        {
            if (Unit2.Checked)
            {
                Unit1.Checked = false;
                Unit2.Checked = true;
            }
        }

        public void LoadIpConfiguration()
        {
            for (int i = 1; i < IPList.ConfigProperties.Count; i++)
            {
                SourceListBox1.Items.Add(IPList.ConfigProperties[i].Name);
                DestListBox2.Items.Add(IPList.ConfigProperties[i].Name);
            }
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            GetAlarmInterval();
            SetPcapLength();
            SaveSetting();
            SaveToTxt();
            this.Hide();
        }

        private void SaveToTxt()
        {
            FileStream fs = new FileStream("BackUps.txt", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);

            sw.WriteLine(this.txtPcapLength.Text);
            sw.WriteLine(this.Unit1.Checked ? "MB" : "GB");

            string Source = "";
            string Dest = "";
            for (int i = 0; i < SourceListBox1.Items.Count; i++)
            {
                if (SourceListBox1.GetItemChecked(i))
                {
                    Source += "#" + SourceListBox1.Items[i].ToString();
                }
            }
            for (int i = 0; i < DestListBox2.Items.Count; i++)
            {
                if (DestListBox2.GetItemChecked(i))
                {
                    Dest += "#" + DestListBox2.Items[i].ToString();
                }
            }
            sw.WriteLine(Source);
            sw.WriteLine(Dest);

            sw.WriteLine(MaxVOBCToGround * 1000);
            sw.WriteLine(MaxGroundToVOBC * 1000);

            sw.Flush();
            sw.Close();
            fs.Close();
        }

        private void GetAlarmInterval()
        {
            if (txtVOBCToGround.Text != "" && txtVOBCToGround.Text != null)
            {
                MaxVOBCToGround = Convert.ToDouble(this.txtVOBCToGround.Text) / 1000;
            }
            if (txtGroundToVOBC.Text != "" && txtGroundToVOBC.Text != null)
            {
                MaxGroundToVOBC = Convert.ToDouble(this.txtGroundToVOBC.Text) / 1000;
            }
        }

        private void SetPcapLength()
        {
            int PcapLength = Convert.ToInt16(this.txtPcapLength.Text);
            string PcapLengthUnit;
            if (this.Unit1.Checked)
            {
                PcapLengthUnit = "MB";
            }
            else
            {
                PcapLengthUnit = "GB";
            }
            GetPcapLengthNum(PcapLength, PcapLengthUnit);
        }

        private void GetPcapLengthNum(int PcapLength,string PcapLengthUnit)
        {
            if (PcapLengthUnit == "MB")
            {
                PcapLengthNum = PcapLength * 1024 * 1024;
            }
            if (PcapLengthUnit == "GB")
            {
                PcapLengthNum = PcapLength * 1024 * 1024 * 1024;
            }
        }

        private void SaveSetting()
        {
            for (int i = 0; i < SourceListBox1.Items.Count; i++)
            {
                foreach (var item in IPList.ConfigProperties)
                {
                    if (item.Name == SourceListBox1.Items[i].ToString())
                    {
                        if (SourceListBox1.GetItemChecked(i))
                        {
                            item.IsSourceChoose = true;
                        }
                        else
                        {
                            item.IsSourceChoose = false;
                        }
                    }
                }
            }
            for (int i = 0; i < DestListBox2.Items.Count; i++)
            {
                foreach (var item in IPList.ConfigProperties)
                {
                    if (item.Name == DestListBox2.Items[i].ToString())
                    {
                        if (DestListBox2.GetItemChecked(i))
                        {
                            item.IsDestChoose = true;
                        }
                        else
                        {
                            item.IsDestChoose = false;
                        }
                    }
                }
            }
        }

        private void FilterForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Hide();
        }
        
        private void LoadHistoryConfig()
        {
            StreamReader sr = new StreamReader("BackUps.txt", Encoding.Default);
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                BackUps.Add(line);
            }
            sr.Close();
            if (BackUps.Count != 0)
            {
                LoadHistory(BackUps);
            }
        }

        private void LoadHistory(List<string> backUps)
        {
            if (backUps[0]!="" && backUps[0]!=null)
            {
                this.txtPcapLength.Text = backUps[0];
            }
            if (backUps[1] == "GB")
            {
                this.Unit2.Checked = true;
                this.Unit1.Checked = false;
            }
            if (backUps[1] == "MB")
            {
                this.Unit1.Checked = true;
                this.Unit2.Checked = false;
            }
            this.txtVOBCToGround.Text = backUps[4];
            this.txtGroundToVOBC.Text = backUps[5];
        }

        private void SelectAll(CheckedListBox ListBox, CheckState state)
        {
            for (int i = 0; i < ListBox.Items.Count; i++)
            {
                ListBox.SetItemCheckState(i, state);
            }
        }

        private void SourceSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (this.SourceSelectAll.Checked)
            {
                SelectAll(SourceListBox1, CheckState.Checked);
            }
            else
            {
                SelectAll(SourceListBox1, CheckState.Unchecked);
            }
        }

        private void DestSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (this.DestSelectAll.Checked)
            {
                SelectAll(DestListBox2, CheckState.Checked);
            }
            else
            {
                SelectAll(DestListBox2, CheckState.Unchecked);
            }
        }
    }
}
