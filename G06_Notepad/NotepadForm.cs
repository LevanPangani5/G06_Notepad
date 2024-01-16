using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace G06_Notepad
{
  public partial class NotepadForm : Form
  {
    private string _fileName;

    public string FileName
    {
      get { return _fileName; }
      set
      {
        if (!string.IsNullOrEmpty(value))
        {
          Text = $"Notepad - {Path.GetFileName(value)}";
        }
        else
        {
          Text = "Notepad - Untitled.txt";
        }
        _fileName = value;
      }
    }

    public bool IsNewFile
    {
      get { return string.IsNullOrEmpty(FileName); }
    }

    public NotepadForm()
    {
      InitializeComponent();
    }

    private void NotepadForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (!ConfirmAction())
      {
        e.Cancel = true;
      }
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void setBackgroundToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (dlgColor.ShowDialog() == DialogResult.OK)
      {
        txtContent.BackColor = dlgColor.Color;
      }
    }

    private void newToolStripMenuItem_Click(object sender, EventArgs e)
    {
      NewFile();
    }

    private void openToolStripMenuItem_Click(object sender, EventArgs e)
    {
      OpenFile();
    }

    private void saveToolStripMenuItem_Click(object sender, EventArgs e)
    {
      SaveFile();
    }

    private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      SaveFile(true);
    }

    private bool NewFile()
    {
      if (!ConfirmAction())
      {
        return false;
      }
      FileName = null;
      txtContent.Clear();
      txtContent.Modified = false;
      return true;
    }

    private bool OpenFile()
    {
      if (!ConfirmAction())
      {
        return false;
      }
      if (dlgOpen.ShowDialog() == DialogResult.OK)
      {
        FileName = dlgOpen.FileName;
        txtContent.Text = File.ReadAllText(dlgOpen.FileName);
        txtContent.Modified = false;
        return true;
      }
      return false;
    }

    private bool SaveFile(bool saveAs = false)
    {
      if (IsNewFile || saveAs)
      {
        dlgSave.FileName = FileName;
        if (dlgSave.ShowDialog() == DialogResult.OK)
        {
          FileName = dlgSave.FileName;
        }
        else
        {
          return false;
        }
      }

      try
      {
        File.WriteAllText(FileName, txtContent.Text);
        txtContent.Modified = false;
        return true;
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }
    }

    private bool ConfirmAction()
    {
      if (!txtContent.Modified)
      {
        return true;
      }

      var result = MessageBox.Show(
        "Do you want to save changes?", "Confirmation",
        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3
      );

      switch (result)
      {
        case DialogResult.Yes: return SaveFile();
        case DialogResult.No: return true;
      }
      return false;
    }
  }
}
