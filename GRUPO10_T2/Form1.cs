using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace GRUPO10_T2
{
    public partial class Form1 : Form
    {
        // INSTANCIAMOS LA CLASE MEDICAMENTOS, LABORATORIO Y EL LISTADOS DE MEDICAMENTOS
        Medicamento objMedicamentos = new Medicamento();
        Laboratorio objLaboratorio = new Laboratorio();
        List<Medicamento> LISTADO = new List<Medicamento>();

        public Form1()
        {
            InitializeComponent();
            // VALIDA SOLO INGRESO DE NUMÉRICO EN TEXTBOX txtPrecioUnitario
            this.txtPrecioUnitario.KeyPress += new KeyPressEventHandler(this.Validar_Ingreso_Numeral);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarIngresoTexbox()) return;
            objMedicamentos = new Medicamento();
            objMedicamentos.G_Codigo = txtCodigo.Text;
            objMedicamentos.G_Nombre = txtNombre.Text;
            objMedicamentos.G_Cantidad = Convert.ToInt32(nudCantidad.Value);
            objMedicamentos.G_PrecioUnitario = Convert.ToDouble(txtPrecioUnitario.Text);
            objMedicamentos.G_Monto = (objMedicamentos.G_Cantidad * objMedicamentos.G_PrecioUnitario);

            LISTADO = objLaboratorio.RegistrarMedicamentos(objMedicamentos, lblCodigo.Text);
            CargarDataGridView(LISTADO);
            LimpiarControles();
            tabControl1.SelectTab(1);
        }
        
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarControles();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            var resultadoBusqueda = LISTADO.Where(x => x.G_Nombre.Contains(txtDatoBuscar.Text)).ToList();
            if (resultadoBusqueda.Count <= 0)
            {
                MessageBox.Show("No hay medicamentos para mostrar");
            }
            CargarDataGridView(resultadoBusqueda);
        }
        private void dgvListaMedicamentos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            if (e.ColumnIndex == -1) return;
            int OPK = dgvListaMedicamentos.CurrentCell.RowIndex;
            try
            {
                if (this.dgvListaMedicamentos.Columns[e.ColumnIndex].Name == "Eliminar")
                {
                    if (MessageBox.Show("¿Desea Eliminar Registro.?", "Eliminación Registro", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        string Codigo = Convert.ToString(dgvListaMedicamentos.CurrentRow.Cells["G_Codigo"].Value);
                        objMedicamentos = LISTADO.Find(X => X.G_Codigo == Codigo);
                        LISTADO.Remove(objMedicamentos);
                        dgvListaMedicamentos.DataSource = null;
                        dgvListaMedicamentos.Refresh();
                        dgvListaMedicamentos.DataSource = objLaboratorio.OrdenacionMedicamentos(LISTADO);
                        dgvListaMedicamentos.Refresh();

                    }
                }
                else if (this.dgvListaMedicamentos.Columns[e.ColumnIndex].Name == "Modificar")
                {
                    string Codigo = Convert.ToString(dgvListaMedicamentos.CurrentRow.Cells["G_Codigo"].Value);
                    objMedicamentos = LISTADO.Find(X => X.G_Codigo == Codigo);
                    lblCodigo.Text = objMedicamentos.G_Codigo.ToString();
                    txtCodigo.Text = objMedicamentos.G_Codigo;
                    txtNombre.Text = objMedicamentos.G_Nombre;
                    nudCantidad.Value = objMedicamentos.G_Cantidad;
                    txtPrecioUnitario.Text = Convert.ToString(objMedicamentos.G_PrecioUnitario);

                    tabControl1.SelectTab(0);
                }
            }
            catch (Exception) { }
        }

       
        #region "SECCIÓN DE MÉTODOS AUXILIARES"
        private void LimpiarControles()
        {
            txtCodigo.Text = "";
            txtNombre.Text = "";
            nudCantidad.Value = 0;
            txtPrecioUnitario.Text = "";
            lblCodigo.Text = "";

        }
        private void CargarDataGridView(List<Medicamento> LISTADO)
        {
            dgvListaMedicamentos.DataSource = null;
            dgvListaMedicamentos.Refresh();
            dgvListaMedicamentos.DataSource = objLaboratorio.OrdenacionMedicamentos(LISTADO);
            dgvListaMedicamentos.Refresh();
        }

        public Boolean ValidarIngresoTexbox()
        {
            if (txtCodigo.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese Código del medicamento", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCodigo.Focus();
                return false;
            }
            if (txtNombre.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese nombre del medicamento", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }
            if (nudCantidad.Value == 0)
            {
                MessageBox.Show("Cantidad ingresada debe ser superior a Cero", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nudCantidad.Focus();
                return false;
            }
            if (txtPrecioUnitario.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese precio unitario", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecioUnitario.Focus();
                return false;
            }
            return true;
        }
        #endregion

        private void Validar_Ingreso_Numeral(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 8)
            {
                e.Handled = false;
                return;
            }
            bool IsDec = false;
            int nroDec = 0;
            for (int i = 0; i < txtPrecioUnitario.Text.Length; i++)
            {
                if (txtPrecioUnitario.Text[i] == '.')
                    IsDec = true;
                if (IsDec && nroDec++ >= 5)
                {
                    e.Handled = true;
                    return;
                }
            }
            if (e.KeyChar >= 48 && e.KeyChar <= 57)
                e.Handled = false;
            else if (e.KeyChar == 46)
                e.Handled = (IsDec) ? true : false;
            else
                e.Handled = true;
        }
    }
}
