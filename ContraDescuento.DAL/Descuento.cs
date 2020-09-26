using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContraDescuento.BE;

namespace ContraDescuento.DAL
{
    public class Descuento
    {
        #region Propiedades privadas
        private Acceso.Conexion conexion = null;
        #endregion

        public Descuento() { }

        public Int32 ValidarStockPuntoDeVenta(BE.Producto producto, BE.Domicilio puntoDeVenta)
        {
            Int32 Stock = 0;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "ProductoValidarStockPuntoDeVenta";

                    conexion.sqlCmd.Parameters.AddWithValue("@codProducto", producto.CodProducto); //Usuario responsable del comercio
                    conexion.sqlCmd.Parameters.AddWithValue("@codPuntoDeVenta", puntoDeVenta.CodDomicilio);

                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();

                    while (conexion.sqlReader.Read())
                    {
                        Stock = Convert.ToInt32(conexion.sqlReader["cantidad"]);
                    }
                    conexion.Cerrar();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Stock;
        }

        public void GenerarCupon(BE.Descuento descuento)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "DescuentoGenerarCupon";

                    conexion.sqlCmd.Parameters.AddWithValue("@codUsuario", descuento.Usuario.CodUsuario);
                    conexion.sqlCmd.Parameters.AddWithValue("@codProducto", descuento.Producto.CodProducto);
                    conexion.sqlCmd.Parameters.AddWithValue("@codComercio", descuento.Comercio.CodComercio);
                    conexion.sqlCmd.Parameters.AddWithValue("@codPuntoDeVenta", descuento.PuntoDeVenta.CodDomicilio);
                    conexion.sqlCmd.Parameters.AddWithValue("@cantidad", descuento.Cantidad);
                    conexion.sqlCmd.Parameters.AddWithValue("@cupon", descuento.Cupon);
                    conexion.sqlCmd.Parameters.AddWithValue("@precio", descuento.Producto.Precio);
                    conexion.sqlCmd.Parameters.AddWithValue("@descuento", descuento.Producto.Descuento);
                    conexion.sqlCmd.Parameters.AddWithValue("@precioFinal", descuento.Producto.Precio);
                    conexion.sqlCmd.Parameters.AddWithValue("@ahorroTotal", descuento.AhorroTotal);
                    conexion.sqlCmd.Parameters.AddWithValue("@fechaFinVigencia", descuento.Producto.FechaVigenciaHasta);

                    conexion.sqlCmd.ExecuteNonQuery();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BE.Descuento> ObtenerDescuentos(BE.Usuario usuario, bool codUsuario, bool email, string cupon)
        {
            List<BE.Descuento> lstDescuentos = null;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "DescuentoObtenerCupones";
                    if (codUsuario)
                    {
                        conexion.sqlCmd.Parameters.AddWithValue("@codUsuario", usuario.CodUsuario);
                        conexion.sqlCmd.Parameters.AddWithValue("@email", DBNull.Value);
                        conexion.sqlCmd.Parameters.AddWithValue("@cupon", DBNull.Value);
                    }
                    else if (email)
                    {
                        conexion.sqlCmd.Parameters.AddWithValue("@codUsuario", DBNull.Value);
                        conexion.sqlCmd.Parameters.AddWithValue("@email", usuario.Email);
                        conexion.sqlCmd.Parameters.AddWithValue("@cupon", DBNull.Value);
                    }
                    else if (cupon != string.Empty)
                    {
                        conexion.sqlCmd.Parameters.AddWithValue("@codUsuario", DBNull.Value);
                        conexion.sqlCmd.Parameters.AddWithValue("@email", DBNull.Value);
                        conexion.sqlCmd.Parameters.AddWithValue("@cupon", cupon);
                    }

                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();

                    if (conexion.sqlReader.FieldCount > 0)
                        lstDescuentos = new List<BE.Descuento>();
                    while (conexion.sqlReader.Read())
                    {
                        BE.Descuento descuento = new BE.Descuento();
                        descuento.Usuario = usuario;
                        if (conexion.sqlReader["codCupon"] != DBNull.Value)
                        {
                            descuento.CodCupon = Convert.ToInt32(conexion.sqlReader["codCupon"]);
                        }
                        if (conexion.sqlReader["codProducto"] != DBNull.Value)
                        {
                            descuento.Producto = new BE.Producto();
                            descuento.Producto.CodProducto = Convert.ToInt32(conexion.sqlReader["codProducto"]);
                        }
                        if (conexion.sqlReader["codComercio"] != DBNull.Value)
                        {
                            descuento.Comercio = new BE.Comercio();
                            descuento.Comercio.CodComercio = Convert.ToInt32(conexion.sqlReader["codComercio"]);
                        }
                        if (conexion.sqlReader["codPuntoDeVenta"] != DBNull.Value)
                        {
                            descuento.PuntoDeVenta = new BE.Domicilio();
                            descuento.PuntoDeVenta.CodDomicilio = Convert.ToInt32(conexion.sqlReader["codPuntoDeVenta"]);
                        }

                        if (conexion.sqlReader["cupon"] != DBNull.Value)
                        {
                            descuento.Cupon = Convert.ToString(conexion.sqlReader["cupon"]);
                        }
                        if (conexion.sqlReader["cantidad"] != DBNull.Value)
                        {
                            descuento.Cantidad = Convert.ToInt32(conexion.sqlReader["cantidad"]);
                        }
                        if (conexion.sqlReader["descuento"] != DBNull.Value)
                        {
                            descuento._Descuento = Convert.ToInt32(conexion.sqlReader["descuento"]);
                        }
                        if (conexion.sqlReader["ahorroTotal"] != DBNull.Value)
                        {
                            descuento.AhorroTotal = Convert.ToDecimal(conexion.sqlReader["ahorroTotal"]);
                        }
                        if (conexion.sqlReader["fechaCupon"] != DBNull.Value)
                        {
                            descuento.FechaCupon = Convert.ToDateTime(conexion.sqlReader["fechaCupon"]);
                        }
                        if (conexion.sqlReader["fechaCanje"] != DBNull.Value)
                        {
                            descuento.FechaCanje = Convert.ToDateTime(conexion.sqlReader["fechaCanje"]);
                        }
                        lstDescuentos.Add(descuento);
                    };

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstDescuentos;
        }

        public void Confirmar(BE.Descuento descuento)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "DescuentoAcreditar";

                    conexion.sqlCmd.Parameters.AddWithValue("@codCupon", descuento.CodCupon);
                    conexion.sqlCmd.Parameters.AddWithValue("@codUsuario", descuento.Usuario.CodUsuario);
                    conexion.sqlCmd.Parameters.AddWithValue("@codProducto", descuento.Producto.CodProducto);
                    conexion.sqlCmd.Parameters.AddWithValue("@codComercio", descuento.Comercio.CodComercio);
                    conexion.sqlCmd.Parameters.AddWithValue("@codPuntoDeVenta", descuento.PuntoDeVenta.CodDomicilio);
                    conexion.sqlCmd.Parameters.AddWithValue("@cupon", descuento.Cupon);
                    conexion.sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void Cancelar(BE.Descuento descuento)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "DescuentoBaja";

                    conexion.sqlCmd.Parameters.AddWithValue("@codCupon", descuento.CodCupon);
                    conexion.sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BE.Descuento> ObtenerDescuentos(BE.Comercio comercio)
        {
            List<BE.Descuento> lstDescuentos = null;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "DescuentoObtenerCuponesComercio";

                    conexion.sqlCmd.Parameters.AddWithValue("@codComercio", comercio.CodComercio);
                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();

                    if (conexion.sqlReader.FieldCount > 0)
                        lstDescuentos = new List<BE.Descuento>();

                    while (conexion.sqlReader.Read())
                    {
                        BE.Descuento descuento = new BE.Descuento();
                        if(conexion.sqlReader["codUsuario"]!= DBNull.Value) {
                            descuento.Usuario = new BE.Usuario();
                            descuento.Usuario.CodUsuario = Convert.ToInt32(conexion.sqlReader["codUsuario"]);
                        }
                        if (conexion.sqlReader["codCupon"] != DBNull.Value)
                        {
                            descuento.CodCupon = Convert.ToInt32(conexion.sqlReader["codCupon"]);
                        }
                        if (conexion.sqlReader["codProducto"] != DBNull.Value)
                        {
                            descuento.Producto = new BE.Producto();
                            descuento.Producto.CodProducto = Convert.ToInt32(conexion.sqlReader["codProducto"]);
                        }
                        if (conexion.sqlReader["codComercio"] != DBNull.Value)
                        {
                            descuento.Comercio = new BE.Comercio();
                            descuento.Comercio.CodComercio = Convert.ToInt32(conexion.sqlReader["codComercio"]);
                        }
                        if (conexion.sqlReader["codPuntoDeVenta"] != DBNull.Value)
                        {
                            descuento.PuntoDeVenta = new BE.Domicilio();
                            descuento.PuntoDeVenta.CodDomicilio = Convert.ToInt32(conexion.sqlReader["codPuntoDeVenta"]);
                        }

                        if (conexion.sqlReader["cupon"] != DBNull.Value)
                        {
                            descuento.Cupon = Convert.ToString(conexion.sqlReader["cupon"]);
                        }
                        if (conexion.sqlReader["cantidad"] != DBNull.Value)
                        {
                            descuento.Cantidad = Convert.ToInt32(conexion.sqlReader["cantidad"]);
                        }
                        if (conexion.sqlReader["descuento"] != DBNull.Value)
                        {
                            descuento._Descuento = Convert.ToInt32(conexion.sqlReader["descuento"]);
                        }
                        if (conexion.sqlReader["ahorroTotal"] != DBNull.Value)
                        {
                            descuento.AhorroTotal = Convert.ToDecimal(conexion.sqlReader["ahorroTotal"]);
                        }
                        if (conexion.sqlReader["fechaCupon"] != DBNull.Value)
                        {
                            descuento.FechaCupon = Convert.ToDateTime(conexion.sqlReader["fechaCupon"]);
                        }
                        if (conexion.sqlReader["fechaCanje"] != DBNull.Value)
                        {
                            descuento.FechaCanje = Convert.ToDateTime(conexion.sqlReader["fechaCanje"]);
                        }
                        lstDescuentos.Add(descuento);
                    };

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstDescuentos;
        }
    }
}
