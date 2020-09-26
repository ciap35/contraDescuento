using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContraDescuento.BE;

namespace ContraDescuento.DAL
{
    public class Comercio
    {
        #region Propiedades privadas
        private Acceso.Conexion conexion = null;
        #endregion

        public Comercio() { }

        public void Alta(ref BE.Comercio comercio)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "ComercioAlta";

                    conexion.sqlCmd.Parameters.AddWithValue("@codUsuario", comercio.Responsable.CodUsuario); //Usuario responsable del comercio
                    conexion.sqlCmd.Parameters.AddWithValue("@nombre", comercio.NombreComercio);
                    if (comercio.Logo != null)
                        conexion.sqlCmd.Parameters.AddWithValue("@logo", comercio.Logo);
                    else
                        conexion.sqlCmd.Parameters.AddWithValue("@logo", System.Data.SqlTypes.SqlBinary.Null);
                    conexion.sqlCmd.Parameters.AddWithValue("@codTelefono", comercio.Telefono.CodTelefono);

                    conexion.sqlCmd.ExecuteNonQuery();

                    conexion.Cerrar();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Baja(BE.Comercio comercio)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "ComercioBaja";
                    conexion.sqlCmd.Parameters.AddWithValue("@codComercio", comercio.CodComercio);
                    conexion.sqlCmd.ExecuteNonQuery();

                    conexion.Cerrar();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Modificar(BE.Comercio comercio)
        {
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "ComercioModificar";
                    conexion.sqlCmd.Parameters.AddWithValue("@codComercio", comercio.CodComercio);
                    conexion.sqlCmd.Parameters.AddWithValue("@nombre", comercio.NombreComercio);
                    conexion.sqlCmd.Parameters.AddWithValue("@descripcion", comercio.Descripcion);
                    //if (!comercio.MantenerLogo)
                        conexion.sqlCmd.Parameters.AddWithValue("@logo", comercio.Logo);
                    //else
                    //    conexion.sqlCmd.Parameters.AddWithValue("@logo", System.Data.SqlTypes.SqlBinary.Null);

                    conexion.sqlCmd.ExecuteNonQuery();

                    conexion.Cerrar();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BE.Comercio Obtener(BE.Comercio comercio)
        {

            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "ComercioObtener";

                    conexion.sqlCmd.Parameters.AddWithValue("@codComercio", comercio.CodComercio);
                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();

                    while (conexion.sqlReader.Read())
                    {
                        comercio.NombreComercio = Convert.ToString(conexion.sqlReader["Nombre"]);
                        comercio.Descripcion = Convert.ToString(conexion.sqlReader["Descripcion"]);
                        if (conexion.sqlReader["Logo"] != DBNull.Value)
                            comercio.Logo = (byte[])(conexion.sqlReader["Logo"]);

                        if (conexion.sqlReader["CodTelefono"] != DBNull.Value)
                            comercio.Telefono.CodTelefono = Convert.ToInt32(conexion.sqlReader["CodTelefono"]);
                        comercio.FechaCreacion = Convert.ToDateTime(conexion.sqlReader["FechaCreacion"]);
                        if (conexion.sqlReader["FechaBaja"] != DBNull.Value)
                            comercio.FechaBaja = Convert.ToDateTime(conexion.sqlReader["FechaBaja"]);
                        if (conexion.sqlReader["FechaModificacion"] != DBNull.Value)
                            comercio.FechaModificacion = Convert.ToDateTime(conexion.sqlReader["FechaModificacion"]);
                    }
                    conexion.Cerrar();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return comercio;
        }

        public List<BE.Comercio> ListarProductosVigentesPorCategoriaSubCategoriaDescuento(BE.Categoria categoria, BE.SubCategoria subCategoria, decimal descuento)
        {
            List<BE.Comercio> lstComercio = null;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "ProductoListarVigentesPorCategoriaSubCategoriaDescuento";
                    conexion.sqlCmd.Parameters.AddWithValue("@codCategoria", categoria.CodCategoria);
                    conexion.sqlCmd.Parameters.AddWithValue("@codSubCategoria", subCategoria.CodSubCategoria);
                    if(descuento == 0)
                        conexion.sqlCmd.Parameters.AddWithValue("@descuento", DBNull.Value);
                    else
                    conexion.sqlCmd.Parameters.AddWithValue("@descuento", descuento);

                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();

                    if (conexion.sqlReader.FieldCount > 0)
                        lstComercio = new List<BE.Comercio>();

                    while (conexion.sqlReader.Read())
                    {
                        BE.Comercio comercio = new BE.Comercio();
                        BE.Producto producto = new BE.Producto();
                        BE.Domicilio puntoDeVenta = new BE.Domicilio();


                        producto.CodProducto = Convert.ToInt32(conexion.sqlReader["Producto.codProducto"]);

                        if (conexion.sqlReader["Producto.Categoria.codCategoria"] != DBNull.Value)
                        {
                            producto.Categoria = new BE.Categoria();
                            producto.Categoria.CodCategoria = Convert.ToInt32(conexion.sqlReader["Producto.Categoria.codCategoria"]);
                            producto.Categoria.Descripcion = Convert.ToString(conexion.sqlReader["Producto.Categoria.descripcion"]);
                        }

                        if (conexion.sqlReader["Producto.SubCategoria.codSubCategoria"] != DBNull.Value)
                        {
                            producto.SubCategoria = new BE.SubCategoria();
                            producto.SubCategoria.CodSubCategoria = Convert.ToInt32(conexion.sqlReader["Producto.Subcategoria.codSubCategoria"]);
                            producto.SubCategoria.Descripcion = Convert.ToString(conexion.sqlReader["Producto.SubCategoria.descripcion"]);
                        }

                        if (conexion.sqlReader["Producto.descripcion"] != DBNull.Value)
                        {
                            producto.Descripcion = Convert.ToString(conexion.sqlReader["Producto.descripcion"]);
                        }

                        //if (conexion.sqlReader["Producto.cantidad"] != DBNull.Value)
                        //{
                        //    producto.Cantidad = Convert.ToInt32(conexion.sqlReader["Producto.cantidad"]);
                        //}

                        if (conexion.sqlReader["Producto.descuento"] != DBNull.Value)
                        {
                            producto.Descuento = Convert.ToDecimal(conexion.sqlReader["Producto.descuento"]);
                        }

                        if (conexion.sqlReader["Producto.fechaVigenciaDesde"] != DBNull.Value)
                        {
                            producto.FechaVigenciaDesde = Convert.ToDateTime(conexion.sqlReader["Producto.fechaVigenciaDesde"]);
                        }

                        if (conexion.sqlReader["Producto.fechaVigenciaHasta"] != DBNull.Value)
                        {
                            producto.FechaVigenciaHasta = Convert.ToDateTime(conexion.sqlReader["Producto.fechaVigenciaHasta"]);
                        }

                        if (conexion.sqlReader["Producto.foto"] != DBNull.Value)
                        {
                            producto.Foto = (byte[])(conexion.sqlReader["Producto.foto"]);
                        }
                        if (conexion.sqlReader["Producto.precio"] != DBNull.Value)
                        {
                            producto.Precio = Convert.ToDecimal(conexion.sqlReader["Producto.precio"]);
                        }
                        if (conexion.sqlReader["Producto.titulo"] != DBNull.Value)
                        {
                            producto.Titulo = Convert.ToString(conexion.sqlReader["Producto.titulo"]);
                        }

                        if (conexion.sqlReader["Producto.descripcion"] != DBNull.Value)
                        {
                            producto.Descripcion = Convert.ToString(conexion.sqlReader["Producto.descripcion"]);
                        }

                        //COMERCIO
                        if (conexion.sqlReader["Comercio.codComercio"] != DBNull.Value)
                        {
                            comercio.CodComercio = Convert.ToInt32(conexion.sqlReader["Comercio.codComercio"]);
                        }

                        if (conexion.sqlReader["Comercio.nombre"] != DBNull.Value)
                        {
                            comercio.NombreComercio = Convert.ToString(conexion.sqlReader["Comercio.nombre"]);
                        }

                        if (conexion.sqlReader["Comercio.logo"] != DBNull.Value)
                        {
                            comercio.Logo = (byte[])(conexion.sqlReader["Comercio.logo"]);
                        }

                        //PUNTO DE VENTA
                        if (conexion.sqlReader["Domicilio.codDomicilio"] != DBNull.Value)
                        {
                            puntoDeVenta.CodDomicilio = Convert.ToInt32(conexion.sqlReader["Domicilio.codDomicilio"]);
                        }
                        if (conexion.sqlReader["Domicilio.calle"] != DBNull.Value)
                        {
                            puntoDeVenta.Calle = Convert.ToString(conexion.sqlReader["Domicilio.calle"]);
                        }
                        if (conexion.sqlReader["Domicilio.numero"] != DBNull.Value)
                        {
                            puntoDeVenta.Numero = Convert.ToString(conexion.sqlReader["Domicilio.numero"]);
                        }
                        if (conexion.sqlReader["Domicilio.piso"] != DBNull.Value)
                        {
                            puntoDeVenta.Piso = Convert.ToString(conexion.sqlReader["Domicilio.piso"]);
                        }
                        if (conexion.sqlReader["Domicilio.departamento"] != DBNull.Value)
                        {
                            puntoDeVenta.Departamento = Convert.ToString(conexion.sqlReader["Domicilio.departamento"]);
                        }

                        if (conexion.sqlReader["Producto_PuntoDeVenta.cantidad"] != DBNull.Value)
                            producto.PuntosDeVentaStock.Add(puntoDeVenta, Convert.ToInt32(conexion.sqlReader["Producto_PuntoDeVenta.cantidad"]));



                        producto.PuntosDeVenta.Add(puntoDeVenta);
                        comercio.PuntoDeVenta.Add(puntoDeVenta);
                        comercio.LstProducto.Add(producto);
                        if(lstComercio.Count == 0)
                            lstComercio.Add(comercio);

                        bool existe = false;
                        foreach (BE.Comercio c in lstComercio)
                        {
                            foreach(BE.Producto p in c.LstProducto) {
                                if (p.CodProducto == producto.CodProducto)
                                    existe = true;
                            }
                            
                        }
                        if (!existe)
                        {
                            lstComercio.Add(comercio);
                        }
                        
                        
                    }
                    conexion.Cerrar();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstComercio;
        }

    

        public List<BE.Comercio> ListarProductosVigentes()
        {
            List<BE.Comercio> lstComercio = null;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "ProductoListarVigentes";

                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();

                    if (conexion.sqlReader.FieldCount > 0)
                        lstComercio = new List<BE.Comercio>();

                    while (conexion.sqlReader.Read())
                    {
                        BE.Comercio comercio = new BE.Comercio();
                        BE.Producto producto = new BE.Producto();
                        BE.Domicilio puntoDeVenta = new BE.Domicilio();


                        producto.CodProducto = Convert.ToInt32(conexion.sqlReader["Producto.codProducto"]);

                        if (conexion.sqlReader["Producto.Categoria.codCategoria"] != DBNull.Value)
                        {
                            producto.Categoria = new BE.Categoria();
                            producto.Categoria.CodCategoria = Convert.ToInt32(conexion.sqlReader["Producto.Categoria.codCategoria"]);
                            producto.Categoria.Descripcion = Convert.ToString(conexion.sqlReader["Producto.Categoria.descripcion"]);
                        }

                        if (conexion.sqlReader["Producto.SubCategoria.codSubCategoria"] != DBNull.Value)
                        {
                            producto.SubCategoria = new BE.SubCategoria();
                            producto.SubCategoria.CodSubCategoria = Convert.ToInt32(conexion.sqlReader["Producto.Subcategoria.codSubCategoria"]);
                            producto.SubCategoria.Descripcion = Convert.ToString(conexion.sqlReader["Producto.SubCategoria.descripcion"]);
                        }

                        if (conexion.sqlReader["Producto.descripcion"] != DBNull.Value)
                        {
                            producto.Descripcion = Convert.ToString(conexion.sqlReader["Producto.descripcion"]);
                        }

                        //if (conexion.sqlReader["Producto.cantidad"] != DBNull.Value)
                        //{
                        //    producto.Cantidad = Convert.ToInt32(conexion.sqlReader["Producto.cantidad"]);
                        //}

                        if (conexion.sqlReader["Producto.descuento"] != DBNull.Value)
                        {
                            producto.Descuento = Convert.ToDecimal(conexion.sqlReader["Producto.descuento"]);
                        }

                        if (conexion.sqlReader["Producto.fechaVigenciaDesde"] != DBNull.Value)
                        {
                            producto.FechaVigenciaDesde = Convert.ToDateTime(conexion.sqlReader["Producto.fechaVigenciaDesde"]);
                        }

                        if (conexion.sqlReader["Producto.fechaVigenciaHasta"] != DBNull.Value)
                        {
                            producto.FechaVigenciaHasta = Convert.ToDateTime(conexion.sqlReader["Producto.fechaVigenciaHasta"]);
                        }

                        if (conexion.sqlReader["Producto.foto"] != DBNull.Value)
                        {
                            producto.Foto = (byte[])(conexion.sqlReader["Producto.foto"]);
                        }
                        if (conexion.sqlReader["Producto.precio"] != DBNull.Value)
                        {
                            producto.Precio = Convert.ToDecimal(conexion.sqlReader["Producto.precio"]);
                        }
                        if (conexion.sqlReader["Producto.titulo"] != DBNull.Value)
                        {
                            producto.Titulo = Convert.ToString(conexion.sqlReader["Producto.titulo"]);
                        }

                        if (conexion.sqlReader["Producto.descripcion"] != DBNull.Value)
                        {
                            producto.Descripcion = Convert.ToString(conexion.sqlReader["Producto.descripcion"]);
                        }

                        //COMERCIO
                        if (conexion.sqlReader["Comercio.codComercio"] != DBNull.Value)
                        {
                            comercio.CodComercio = Convert.ToInt32(conexion.sqlReader["Comercio.codComercio"]);
                        }

                        if (conexion.sqlReader["Comercio.nombre"] != DBNull.Value)
                        {
                            comercio.NombreComercio = Convert.ToString(conexion.sqlReader["Comercio.nombre"]);
                        }

                        if (conexion.sqlReader["Comercio.logo"] != DBNull.Value)
                        {
                            comercio.Logo = (byte[])(conexion.sqlReader["Comercio.logo"]);
                        }

                        //PUNTO DE VENTA
                        if (conexion.sqlReader["Domicilio.codDomicilio"] != DBNull.Value)
                        {
                            puntoDeVenta.CodDomicilio = Convert.ToInt32(conexion.sqlReader["Domicilio.codDomicilio"]);
                        }
                        if (conexion.sqlReader["Domicilio.calle"] != DBNull.Value)
                        {
                            puntoDeVenta.Calle= Convert.ToString(conexion.sqlReader["Domicilio.calle"]);
                        }
                        if (conexion.sqlReader["Domicilio.numero"] != DBNull.Value)
                        {
                            puntoDeVenta.Numero = Convert.ToString(conexion.sqlReader["Domicilio.numero"]);
                        }
                        if (conexion.sqlReader["Domicilio.piso"] != DBNull.Value)
                        {
                            puntoDeVenta.Piso = Convert.ToString(conexion.sqlReader["Domicilio.piso"]);
                        }
                        if (conexion.sqlReader["Domicilio.departamento"] != DBNull.Value)
                        {
                            puntoDeVenta.Departamento = Convert.ToString(conexion.sqlReader["Domicilio.departamento"]);
                        }

                        if (conexion.sqlReader["Producto_PuntoDeVenta.cantidad"] != DBNull.Value)
                            producto.PuntosDeVentaStock.Add(puntoDeVenta, Convert.ToInt32(conexion.sqlReader["Producto_PuntoDeVenta.cantidad"]));




                        producto.PuntosDeVenta.Add(puntoDeVenta);
                        comercio.PuntoDeVenta.Add(puntoDeVenta);
                        comercio.LstProducto.Add(producto);
                        

                        lstComercio.Add(comercio);
                    }
                    conexion.Cerrar();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstComercio;
        }

        public List<BE.Comercio> ListarOfertasDelDia()
        {
            List<BE.Comercio> lstComercio = null;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "DescuentoDelDiaListar";

                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();

                    if (conexion.sqlReader.FieldCount > 0)
                        lstComercio = new List<BE.Comercio>();

                    while (conexion.sqlReader.Read())
                    {
                        BE.Comercio comercio = new BE.Comercio();
                        BE.Producto producto = new BE.Producto();
                        
                        BE.Domicilio puntoDeVenta = new BE.Domicilio();


                        producto.CodProducto = Convert.ToInt32(conexion.sqlReader["Producto.codProducto"]);

                        if (conexion.sqlReader["Producto.Categoria.codCategoria"] != DBNull.Value)
                        {
                            producto.Categoria = new BE.Categoria();
                            producto.Categoria.CodCategoria = Convert.ToInt32(conexion.sqlReader["Producto.Categoria.codCategoria"]);
                            producto.Categoria.Descripcion = Convert.ToString(conexion.sqlReader["Producto.Categoria.descripcion"]);
                        }

                        if (conexion.sqlReader["Producto.SubCategoria.codSubCategoria"] != DBNull.Value)
                        {
                            producto.SubCategoria = new BE.SubCategoria();
                            producto.SubCategoria.CodSubCategoria = Convert.ToInt32(conexion.sqlReader["Producto.Subcategoria.codSubCategoria"]);
                            producto.SubCategoria.Descripcion = Convert.ToString(conexion.sqlReader["Producto.SubCategoria.descripcion"]);
                        }

                        if (conexion.sqlReader["Producto.descripcion"] != DBNull.Value)
                        {
                            producto.Descripcion = Convert.ToString(conexion.sqlReader["Producto.descripcion"]);
                        }

                        //if (conexion.sqlReader["Producto.cantidad"] != DBNull.Value)
                        //{
                        //    producto.Cantidad = Convert.ToInt32(conexion.sqlReader["Producto.cantidad"]);
                        //}

                        if (conexion.sqlReader["Producto.descuento"] != DBNull.Value)
                        {
                            producto.Descuento = Convert.ToDecimal(conexion.sqlReader["Producto.descuento"]);
                        }

                        if (conexion.sqlReader["Producto.fechaVigenciaDesde"] != DBNull.Value)
                        {
                            producto.FechaVigenciaDesde = Convert.ToDateTime(conexion.sqlReader["Producto.fechaVigenciaDesde"]);
                        }

                        if (conexion.sqlReader["Producto.fechaVigenciaHasta"] != DBNull.Value)
                        {
                            producto.FechaVigenciaHasta = Convert.ToDateTime(conexion.sqlReader["Producto.fechaVigenciaHasta"]);
                        }

                        if (conexion.sqlReader["Producto.foto"] != DBNull.Value)
                        {
                            producto.Foto = (byte[])(conexion.sqlReader["Producto.foto"]);
                        }
                        if (conexion.sqlReader["Producto.precio"] != DBNull.Value)
                        {
                            producto.Precio = Convert.ToDecimal(conexion.sqlReader["Producto.precio"]);
                        }
                        if (conexion.sqlReader["Producto.titulo"] != DBNull.Value)
                        {
                            producto.Titulo = Convert.ToString(conexion.sqlReader["Producto.titulo"]);
                        }

                        if (conexion.sqlReader["Producto.descripcion"] != DBNull.Value)
                        {
                            producto.Descripcion = Convert.ToString(conexion.sqlReader["Producto.descripcion"]);
                        }

                        //COMERCIO
                        if (conexion.sqlReader["Comercio.codComercio"] != DBNull.Value)
                        {
                            comercio.CodComercio = Convert.ToInt32(conexion.sqlReader["Comercio.codComercio"]);
                        }

                        if (conexion.sqlReader["Comercio.nombre"] != DBNull.Value)
                        {
                            comercio.NombreComercio = Convert.ToString(conexion.sqlReader["Comercio.nombre"]);
                        }

                        if (conexion.sqlReader["Comercio.logo"] != DBNull.Value)
                        {
                            comercio.Logo = (byte[])(conexion.sqlReader["Comercio.logo"]);
                        }

                        //PUNTO DE VENTA
                        if (conexion.sqlReader["Domicilio.codDomicilio"] != DBNull.Value)
                        {
                            puntoDeVenta.CodDomicilio = Convert.ToInt32(conexion.sqlReader["Domicilio.codDomicilio"]);
                        }
                        if (conexion.sqlReader["Domicilio.calle"] != DBNull.Value)
                        {
                            puntoDeVenta.Calle = Convert.ToString(conexion.sqlReader["Domicilio.calle"]);
                        }
                        if (conexion.sqlReader["Domicilio.numero"] != DBNull.Value)
                        {
                            puntoDeVenta.Numero = Convert.ToString(conexion.sqlReader["Domicilio.numero"]);
                        }
                        if (conexion.sqlReader["Domicilio.piso"] != DBNull.Value)
                        {
                            puntoDeVenta.Piso = Convert.ToString(conexion.sqlReader["Domicilio.piso"]);
                        }
                        if (conexion.sqlReader["Domicilio.departamento"] != DBNull.Value)
                        {
                            puntoDeVenta.Departamento = Convert.ToString(conexion.sqlReader["Domicilio.departamento"]);
                        }
                        if (conexion.sqlReader["Producto_PuntoDeVenta.cantidad"] != DBNull.Value)
                            producto.PuntosDeVentaStock.Add(puntoDeVenta, Convert.ToInt32(conexion.sqlReader["Producto_PuntoDeVenta.cantidad"]));


                        producto.PuntosDeVenta.Add(puntoDeVenta);
                        comercio.PuntoDeVenta.Add(puntoDeVenta);
                        comercio.LstProducto.Add(producto);


                        lstComercio.Add(comercio);
                    }
                    conexion.Cerrar();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstComercio;
        }

        public Chart EstadisticaProductoMasCanjeado(BE.Comercio comercio)
        {
            BE.Chart chart = null;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "EstadisticaComercio_CuponesCanjeadosPorProducto";
                    conexion.sqlCmd.Parameters.AddWithValue("@codComercio", comercio.CodComercio);
                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();

                    if (conexion.sqlReader.FieldCount > 0)
                        chart = new Chart();
                    while (conexion.sqlReader.Read())
                    {
                        ChartItem chartItem = new ChartItem();
                        chartItem.Etiqueta = Convert.ToString(conexion.sqlReader["etiqueta"]);
                        chartItem.Valor = Convert.ToString(conexion.sqlReader["valor"]);
                        chart.LstChartItems.Add(chartItem);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return chart;
        }

        public Chart EstadisticaCantidadProductosPorPuntoVenta(BE.Comercio comercio)
        {
            BE.Chart chart = null;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "EstadisticaComercio_CantidadProductosPorPuntoVenta";
                    conexion.sqlCmd.Parameters.AddWithValue("@codComercio", comercio.CodComercio);
                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();

                    if (conexion.sqlReader.FieldCount > 0)
                        chart = new Chart();
                    while (conexion.sqlReader.Read())
                    {
                        ChartItem chartItem = new ChartItem();
                        chartItem.Etiqueta = Convert.ToString(conexion.sqlReader["etiqueta"]);
                        chartItem.Valor = Convert.ToString(conexion.sqlReader["valor"]);
                        chart.LstChartItems.Add(chartItem);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return chart;
        }

        public Chart EstadisticaCantidadCuponesCanjeadosPorFecha(BE.Comercio comercio)
        {
            BE.Chart chart = null;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "EstadisticaComercio_CantidadCuponesCanjeadosPorFecha";
                    conexion.sqlCmd.Parameters.AddWithValue("@codComercio", comercio.CodComercio);
                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();

                    if (conexion.sqlReader.FieldCount > 0)
                        chart = new Chart();
                    while (conexion.sqlReader.Read())
                    {
                        ChartItem chartItem = new ChartItem();
                        chartItem.Etiqueta = Convert.ToString(Convert.ToDateTime(conexion.sqlReader["etiqueta"]).Date.ToShortDateString());
                        chartItem.Valor = Convert.ToString(conexion.sqlReader["valor"]);
                        chart.LstChartItems.Add(chartItem);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return chart;
        }

        public int EstadisticaCantidadTicketsPendientes(BE.Comercio comercio)
        {
            int resultado = 0;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "EstadisticaComercio_CantidadCuponesPendientes";
                    conexion.sqlCmd.Parameters.AddWithValue("@codComercio", comercio.CodComercio);
                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();

                    while (conexion.sqlReader.Read())
                    {
                        
                        resultado = Convert.ToInt32(conexion.sqlReader["resultado"]);
                        
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resultado;
        }

        public int EstadisticaCantidadProductos(BE.Comercio comercio)
        {
            int resultado = 0;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "EstadisticaComercio_CantidadProductos";
                    conexion.sqlCmd.Parameters.AddWithValue("@codComercio", comercio.CodComercio);
                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();

                    while (conexion.sqlReader.Read())
                    {

                        resultado = Convert.ToInt32(conexion.sqlReader["resultado"]);

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resultado;
        }

        public int EstadisticaCantidadPuntosDeVenta(BE.Comercio comercio)
        {
            int resultado = 0;
            try
            {
                using (conexion = new Acceso.Conexion())
                {
                    conexion.Abrir();
                    conexion.sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conexion.sqlCmd.CommandText = "EstadisticaComercio_CantidadPuntosDeVenta";
                    conexion.sqlCmd.Parameters.AddWithValue("@codComercio", comercio.CodComercio);
                    conexion.sqlReader = conexion.sqlCmd.ExecuteReader();

                    while (conexion.sqlReader.Read())
                    {
                        resultado = Convert.ToInt32(conexion.sqlReader["resultado"]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resultado;
        }
    }
}
