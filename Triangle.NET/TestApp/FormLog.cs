﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TriangleNet.Log;

namespace MeshExplorer
{
    public partial class FormLog : Form
    {
        public FormLog()
        {
            InitializeComponent();
        }

        public void AddItem(string message, bool warning)
        {
            ILog<SimpleLogItem> log = SimpleLog.Instance;

            if (warning)
            {
                log.Warning(message, "Mesh Explorer");
            }
            else
            {
                log.Info(message);
            }
        }

        public void UpdateItems()
        {
            listLog.Items.Clear();

            ILog<SimpleLogItem> log = SimpleLog.Instance;

            foreach (var item in log.Data)
            {
                listLog.Items.Add(CreateListViewItem(item));
            }
        }

        private ListViewItem CreateListViewItem(SimpleLogItem item)
        {
            ListViewItem lvi = new ListViewItem(new string[] { item.Message, item.Info });

            if (item.Level == LogLevel.Error)
            {
                lvi.ForeColor = Color.DarkRed;
            }
            else if (item.Level == LogLevel.Warning)
            {
                lvi.ForeColor = Color.Peru;
            }
            else
            {
                lvi.ForeColor = Color.Black;
            }

            lvi.UseItemStyleForSubItems = true;

            return lvi;
        }

        private void FormLog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        private void listLog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (ModifierKeys == Keys.Control)
                {
                    listLog.Items.Clear();
                    SimpleLog.Instance.Clear();
                }
            }
        }

        private void listLog_DoubleClick(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in listLog.SelectedItems)
            {
                GetRowText(sb, item);
            }

            if (sb.Length > 0)
            {
                Clipboard.SetText(sb.ToString());
            }
        }

        private void GetRowText(StringBuilder sb, object item)
        {
            var row = item as ListViewItem;

            if (row != null)
            {
                foreach (var col in row.SubItems)
                {
                    var lvi = col as ListViewItem.ListViewSubItem;

                    if (lvi != null)
                    {
                        sb.AppendLine(lvi.Text);
                    }
                }
            }
        }
    }
}
