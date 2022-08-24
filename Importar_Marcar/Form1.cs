using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Importar_Marcar
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        double total, selecao;

        private void Form1_Load(object sender, EventArgs e)
        {
            // Cria as coluunas definindo o tipo de cada uma
            dvg_despesa.Columns.Insert(0, new DataGridViewCheckBoxColumn());
            dvg_despesa.Columns.Insert(1, new DataGridViewTextBoxColumn());
            dvg_despesa.Columns.Insert(2, new DataGridViewTextBoxColumn());
            dvg_despesa.Columns.Insert(3, new DataGridViewTextBoxColumn());

            // Renomeia as colunas
            dvg_despesa.Columns[0].Name = "OK";
            dvg_despesa.Columns[1].Name = "CODIGO";
            dvg_despesa.Columns[2].Name = "DESCRICAO";
            dvg_despesa.Columns[3].Name = "VALOR";

            // Configura a DataGridView
            dvg_despesa.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dvg_despesa.ReadOnly = true;
            dvg_despesa.AllowUserToAddRows = false;
            dvg_despesa.AllowUserToDeleteRows = false;
            dvg_despesa.AllowUserToOrderColumns = true;

            // Configura a coluna de valor formatando e alinhado
            dvg_despesa.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dvg_despesa.Columns[3].DefaultCellStyle.Format = "###,###,##0.00";

        }



        private void btn_Importar_Click(object sender, EventArgs e)
        {
            dvg_despesa.RowCount = 0;

            ofdArquivo.FileName = "";
            ofdArquivo.ShowDialog();

            StreamReader arquivo = File.OpenText(ofdArquivo.FileName);

            String linha;
            while ((linha = arquivo.ReadLine()) != null)
            {
                string[] dados = linha.Split(';');
                string codigo = dados[0];
                string descricao = dados[1];
                double valor = double.Parse(dados[2]);

                dvg_despesa.Rows.Add(false, codigo, descricao, valor);
                total += valor;
            }
            lbl_total.Text = total.ToString("C");
        }

        private void dvg_despesa_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dvg_despesa.RowCount > 0 && e.ColumnIndex == 0)
            {
                if (Convert.ToBoolean(dvg_despesa.CurrentRow.Cells[0].Value) == false)
                {
                    dvg_despesa.CurrentRow.Cells[0].Value = true;
                    selecao += Convert.ToDouble(dvg_despesa.CurrentRow.Cells[3].Value);
                }
                else
                {
                    dvg_despesa.CurrentRow.Cells[0].Value = false;
                    selecao -= Convert.ToDouble(dvg_despesa.CurrentRow.Cells[3].Value);
                }
                lbl_selecao.Text = selecao.ToString("C");
            }
        }


        private void btn_Marcar_Click(object sender, EventArgs e)
        {
            selecao = 0;
            foreach(DataGridViewRow linha in dvg_despesa.Rows)
            {
                linha.Cells[0].Value = true;
                selecao += Convert.ToDouble(linha.Cells[3].Value);
            }
            lbl_selecao.Text = selecao.ToString("C");
        }

        private void btn_Desmarcar_Click(object sender, EventArgs e)
        {
            selecao = 0;
            foreach (DataGridViewRow linha in dvg_despesa.Rows)
                    linha.Cells[0].Value = false;
            lbl_selecao.Text = selecao.ToString("C");
        }

        private void btn_Baixar_Click(object sender, EventArgs e)
        {
            for (int i = dvg_despesa.RowCount - 1; i >= 0; i--)
            {
                if (Convert.ToBoolean(dvg_despesa.Rows[i].Cells[0].Value) == true)
                {
                    selecao -= Convert.ToDouble(dvg_despesa.Rows[i].Cells[3].Value);
                    total -= Convert.ToDouble(dvg_despesa.Rows[i].Cells[3].Value);
                    dvg_despesa.Rows.Remove(dvg_despesa.Rows[i]);
                }
            }
            lbl_selecao.Text = selecao.ToString("C");
            lbl_total.Text = total.ToString("C");
        }
    }
}
