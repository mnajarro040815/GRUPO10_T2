using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GRUPO10_T2
{
    public class Laboratorio
    {
        // INSTANCIAMOS LA CLASE MEDICAMENTOS Y EL LISTADOS DE MEDICAMENTOS
        Medicamento objMedicamentos = new Medicamento();
        List<Medicamento> LISTADO = new List<Medicamento>();

        public List<Medicamento> RegistrarMedicamentos(Medicamento medicamento, string validaRegistro)
        {
            if (validaRegistro != "")
            {
                this.Editar(validaRegistro, medicamento);
                MessageBox.Show(medicamento.G_Nombre + " Modificado");
            }
            else
            {
                LISTADO.Add(medicamento);
                MessageBox.Show(medicamento.G_Nombre + " Registrado");
            }
            return LISTADO;
        }

        public List<Medicamento> OrdenacionMedicamentos(List<Medicamento> lista)
        {
            lista = lista.OrderBy(x => x.G_Nombre).ToList();
            return lista;
        }

        #region "MÉTODOS PARA EDICIÓN DE MEDICAMENTOS"
        private void Editar(string item, Medicamento obj002)
        {
            foreach (Medicamento obj1 in LISTADO)
            {
                if (item == obj1.G_Codigo)
                {
                    objMedicamentos = obj1;
                    break;
                }
            }
            setMedicamentos(objMedicamentos, obj002);
        }
        private void setMedicamentos(Medicamento objNue, Medicamento obj002)
        {
            objNue.G_Codigo = obj002.G_Codigo;
            objNue.G_Nombre = obj002.G_Nombre;
            objNue.G_Cantidad = obj002.G_Cantidad;
            objNue.G_PrecioUnitario = obj002.G_PrecioUnitario;
            objNue.G_Monto = obj002.G_Cantidad * objMedicamentos.G_PrecioUnitario;
        }
        #endregion



    }

}
