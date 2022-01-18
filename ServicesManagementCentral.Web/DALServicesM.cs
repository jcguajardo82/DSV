using Soriana.FWK.Datos.SQL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web
{
    public class DALServicesM
    {

        #region Obtener Tiendas
        public static DataSet GetUN()
        {

            DataSet ds = new DataSet();

            //string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            //if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            //{
            //    conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            //}


            try
            {
                //System.Collections.Hashtable parametros = new System.Collections.Hashtable();
                //Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);
                //ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "upCorpOms_Cns_UN", false);
                using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("dbo.upCorpOms_Cns_UN", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                            dataAdapter.Fill(ds);
                    }
                }
                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }

        public static DataSet upCorpOms_Cns_UNbyID(int id_num_UN)
        {

            DataSet ds = new DataSet();

            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                System.Collections.Hashtable parametros = new System.Collections.Hashtable();
                parametros.Add("@id_num_UN", id_num_UN);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "dbo.upCorpOms_Cns_UNbyID", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }
        public static DataSet GetUNTraspaso(int idNumUn)
        {

            DataSet ds = new DataSet();

            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                System.Collections.Hashtable parametros = new System.Collections.Hashtable();
                parametros.Add("@Id_Num_UN", idNumUn);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "upCorpOmsTraspaso_Cns_UN", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Ordenes x Tienda
        public static DataSet GetOrdersByUn(string Id_Num_UN)
        {


            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);


                System.Collections.Hashtable parametros = new System.Collections.Hashtable();
                parametros.Add("@Id_Num_UN", Id_Num_UN);



                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "upCorpOms_Cns_OrdersByUN", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }
        #endregion

        #region Obtener Surtidores
        public static DataSet GetSurtidores(string Id_Num_UN)
        {


            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);


                System.Collections.Hashtable parametros = new System.Collections.Hashtable();
                parametros.Add("@Id_Num_UN", Id_Num_UN);



                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[upCorpOms_Cns_SupplierByUn]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }
        #endregion

        #region ObtenerOrdenes

        public static DataSet GetOrdersByOrderNo(string UeNo)
        {


            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);


                System.Collections.Hashtable parametros = new System.Collections.Hashtable();
                parametros.Add("@UeNo", UeNo);



                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "dbo.upCorpOms_Cns_OrdersByOrderNo_V2", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }

        public static DataSet upCorpOms_Cns_OrdersByOrderNoInit(string UeNo)
        {


            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);


                System.Collections.Hashtable parametros = new System.Collections.Hashtable();
                parametros.Add("@UeNo", UeNo);



                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "upCorpOms_Cns_OrdersByOrderNoInit_V2", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }


        public static DataSet GetOrdersByGuiaEmb(string Id_Num_GuiaEmb)
        {


            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);


                System.Collections.Hashtable parametros = new System.Collections.Hashtable();
                parametros.Add("@Id_Num_GuiaEmb", Id_Num_GuiaEmb);



                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "upCorpOms_Cns_OrdersByGuia", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }

        #endregion

        #region Carriers

        public static DataSet GetCarriers()
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[upCorpOms_Cns_Carrier]", false);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }


        }


        public static DataSet GetCarriersUN(int Id_Num_UN)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                parametros.Add("@Id_Num_UN", Id_Num_UN);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[upCorpOms_Cns_CarrierByUn]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }


        }

        public static DataSet AddCarriers(string num, string name, string un)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);



                parametros.Add("@Name", name);
                parametros.Add("@Id_Num_UN", un);
                parametros.Add("@Id_Num_Empleado", num);



                Soriana.FWK.FmkTools.SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[upCorpOms_Ins_Carrier]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }


        }

        public static DataSet GetShipmentPackingWMS()
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                
                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[upCorpOms_Sel_ShipmentPackingWMS]", false);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }


        }
        public static DataSet GetShipmentPackingWMSByCode(string IdPackingCode)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                parametros.Add("@IdPackingCode", IdPackingCode);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[upCorpOms_Sel_ShipmentPackingWMSDimensionsByPackingCode]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }


        }

        public static DataSet AddShipmentPackingWMS(int IdCnscPacking,string IdPackingCode,string IdPackingType,decimal? PackageLength, decimal? PackageWidth, 
                                        decimal? PackageHeight, decimal? PackageWeight, bool BitActivo, string createdUser)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);



                parametros.Add("@IdCnscPacking", IdCnscPacking);
                parametros.Add("@IdPackingCode", IdPackingCode);
                parametros.Add("@IdPackingType", IdPackingType);
                parametros.Add("@PackageLength", PackageLength);
                parametros.Add("@PackageWidth", PackageWidth);
                parametros.Add("@PackageHeight", PackageHeight);

                parametros.Add("@PackageWeight", PackageWeight);
                parametros.Add("@BitActivo", BitActivo);
                parametros.Add("@createdUser", createdUser);



                Soriana.FWK.FmkTools.SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[upCorpOms_Ins_ShipmentPackingWMS]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }


        }
        public static DataSet DeleteShipmentPackingWMS(int IdCnscPacking)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);



                parametros.Add("@IdCnscPacking", IdCnscPacking);
                



                Soriana.FWK.FmkTools.SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[upCorpOms_Del_ShipmentPackingWMS]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }


        }
        public static DataSet GetShipmentLocksWMS(string IdPackingCode)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                parametros.Add("@IdPackingCode", IdPackingCode);


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[upCorpOms_Sel_ShipmentLocksWMS]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }


        }
        public static DataSet EditCarriers(string num, string name, string un, string status)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);



                parametros.Add("@Id_Carrier", num);
                parametros.Add("@Name", name);
                parametros.Add("@Id_Num_UN", un);
                //parametros.Add("@Id_Num_Empleado", num);
                parametros.Add("@Activo", status);



                Soriana.FWK.FmkTools.SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[upCorpOms_Upd_Carrier]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }


        }

        public static DataSet DeleteCarriers(string num)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                parametros.Add("@Id_Carrier", num);

                Soriana.FWK.FmkTools.SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[upCorpOms_Del_Carrier]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }


        }

        public static DataSet GetCarrier(string num)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);



                parametros.Add("@Id_Carrier", num);



                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[upCorpOms_Cns_CarrierById]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }


        }
        // Agregado DST
        public static DataSet GetCarriersUNDTS(int Id_Num_UN)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                parametros.Add("@Id_Num_UN", Id_Num_UN);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[upCorpOms_Cns_CarrierDTSByUn]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }


        }

        #endregion

        #region SURTIDORES
        public static DataSet GetSuppliers()
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[upCorpOms_Sel_Supplier]", false);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }


        }

        public static DataSet GetSuppliersUN(int Id_Num_UN)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);


                parametros.Add("@Id_Num_UN", Id_Num_UN);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[upCorpOms_Sel_SupplierByUn]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }


        }


        public static DataSet AddSupplier(string num, string name, string un)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);



                parametros.Add("@Name", name);
                parametros.Add("@Id_Num_UN", un);
                parametros.Add("@Id_Num_Empleado", num);



                Soriana.FWK.FmkTools.SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[upCorpOms_Ins_Supplier]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }


        }

        public static DataSet EditSupplier(string num, string name, string un, string status, string Id_Num_Empleado)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);



                parametros.Add("@Id_Supplier", num);
                parametros.Add("@Name", name);
                parametros.Add("@Id_Num_UN", un);
                parametros.Add("@Id_Num_Empleado", Id_Num_Empleado);
                parametros.Add("@Activo", status);



                Soriana.FWK.FmkTools.SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[upCorpOms_Upd_Supplier]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }


        }

        public static DataSet DeleteSupplier(string num)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                parametros.Add("@Id_Supplier", num);

                Soriana.FWK.FmkTools.SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[upCorpOms_Del_Supplier]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }


        }

        public static DataSet GetSupplier(string num)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);



                parametros.Add("@Id_Supplier", num);



                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[upCorpOms_Cns_SupplierById]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }


        }
        #endregion

        #region Embarque

        public static DataSet GetListaEmbarcar(string Id_Num_UN, string Id_Num_Apl = "2")
        {
            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);


                System.Collections.Hashtable parametros = new System.Collections.Hashtable();
                parametros.Add("@Id_Num_UN", Id_Num_UN);
                parametros.Add("@Id_Num_Apl", Id_Num_Apl);



                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "spSurtido_ListaPorEmbarcarNvo_V2_sup", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }

        public static DataSet GetListaSurtir(string Id_Num_UN)
        {
            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);


                System.Collections.Hashtable parametros = new System.Collections.Hashtable();
                parametros.Add("@Id_Num_UN", Id_Num_UN);



                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "spSurtido_ListaPorSurtirNvo_V2_sup", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region ActualizaOrden

        public static DataSet GetMotivoCambioFP(int Id_Num_MotCmbFP = 0, bool Bit_Eliminado = false)
        {
            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }

            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                System.Collections.Hashtable parametros = new System.Collections.Hashtable();
                parametros.Add("@Id_Num_MotCmbFP", Id_Num_MotCmbFP);
                parametros.Add("@Bit_Eliminado", Bit_Eliminado);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "CatMotivoCambioFP_Sup", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet UpdFormaPago(int Id_Num_Orden, int Id_Num_MotCmbFP, string Obs_CambioFP)
        {
            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }

            try
            {
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                parametros.Add("@OrderNo", Id_Num_Orden);
                parametros.Add("@Id_Num_MotCmbFP", Id_Num_MotCmbFP);
                parametros.Add("@Obs_CambioFP", Obs_CambioFP);

                Soriana.FWK.FmkTools.SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[upCorpOms_Upd_FormaPagoOrden]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }
        #endregion

        #region Observaciones
        public static DataSet DelObservaciones(int OrderNo, int Id_Cnsc_CarObs, int CnscOrder)
        {
            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }

            try
            {
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                parametros.Add("@OrderNo", OrderNo);
                parametros.Add("@Id_Cnsc_CarObs", Id_Cnsc_CarObs);
                parametros.Add("@CnscOrder", CnscOrder);

                Soriana.FWK.FmkTools.SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[upCorpOms_Del_OrderComments]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }

        public static DataSet AddObservaciones(int OrderNo, string Desc_CarObs)
        {
            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }

            try
            {
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                parametros.Add("@OrderNo", OrderNo);
                parametros.Add("@Desc_CarObs", Desc_CarObs);


                Soriana.FWK.FmkTools.SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[upCorpOms_Ins_OrderComments]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }
        #endregion

        #region Pasillos especiales    
        public static DataSet GetPasillosEspeciales(int OrderNo)
        {
            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);


                System.Collections.Hashtable parametros = new System.Collections.Hashtable();
                parametros.Add("@OrderNo", OrderNo);



                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "upCorpOms_Cns_ListPasillosEspeciales", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Cancelacion Pass

        public static DataSet GetPassCan(int Id_Num_UN, int Id_Num_TipoClave = 1)

        {

            DataSet ds = new DataSet();



            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];

            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))

            {

                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);

            }





            try

            {

                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);



                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                parametros.Add("@Id_Num_UN ", Id_Num_UN);

                parametros.Add("@Id_Num_TipoClave ", Id_Num_TipoClave);



                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "cPwdCan_Sup1", false, parametros);



                return ds;

            }

            catch (SqlException ex)

            {



                throw ex;

            }

            catch (System.Exception ex)

            {



                throw ex;

            }

        }
        public static DataSet GetPassCan2(int Id_Num_TipoClave, string UeNo)

        {

            DataSet ds = new DataSet();



            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];

            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))

            {

                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);

            }
            try

            {

                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);



                System.Collections.Hashtable parametros = new System.Collections.Hashtable();
                parametros.Add("@Id_Num_TipoClave ", Id_Num_TipoClave);
                parametros.Add("@UeNo ", UeNo);



                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "cPwdCan_Sup2", false, parametros);



                return ds;

            }

            catch (SqlException ex)

            {



                throw ex;

            }

            catch (System.Exception ex)

            {



                throw ex;

            }

        }
        public static DataSet UpdPassCan(int Id_Num_UN, string pwdnom, int Id_Num_TipoClave = 1)

        {

            DataSet ds = new DataSet();



            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];

            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))

            {

                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);

            }





            try

            {

                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);



                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                parametros.Add("@Id_Num_UN ", Id_Num_UN);

                parametros.Add("@Id_Num_TipoClave ", Id_Num_TipoClave);

                parametros.Add("@pwdnom ", pwdnom);



                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "cPwdCan_Uup1", false, parametros);



                return ds;

            }

            catch (SqlException ex)

            {



                throw ex;

            }

            catch (System.Exception ex)

            {



                throw ex;

            }

        }

        #endregion

        #region Catalogo Pasillos

        public static DataSet GetPasillos(int Id_Num_UN)

        {

            DataSet ds = new DataSet();



            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];

            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))

            {

                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);

            }





            try

            {

                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);





                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                parametros.Add("@Id_Num_UN", Id_Num_UN);





                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "PasilloUn_sUp", false, parametros);



                return ds;

            }

            catch (SqlException ex)

            {



                throw ex;

            }

            catch (System.Exception ex)

            {



                throw ex;

            }

        }

        public static DataSet AddPasilloUn(int Id_Num_UN)

        {

            DataSet ds = new DataSet();



            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];

            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))

            {

                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);

            }





            try

            {

                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);





                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                parametros.Add("@Id_Num_UN", Id_Num_UN);





                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "PasilloUn_iUp", false, parametros);



                return ds;

            }

            catch (SqlException ex)

            {



                throw ex;

            }

            catch (System.Exception ex)

            {



                throw ex;

            }

        }

        public static DataSet DelPasilloUn(int Id_Num_UN, int Id_Cnsc_Pasillo)

        {

            DataSet ds = new DataSet();



            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];

            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))

            {

                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);

            }





            try

            {

                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);





                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                parametros.Add("@Id_Num_UN", Id_Num_UN);

                parametros.Add("@Id_Cnsc_Pasillo", Id_Cnsc_Pasillo);





                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "PasilloUN_dup", false, parametros);



                return ds;

            }

            catch (SqlException ex)

            {



                throw ex;

            }

            catch (System.Exception ex)

            {



                throw ex;

            }

        }

        public static DataSet AddPasilloUnEspecial(int Id_Num_UN)

        {

            DataSet ds = new DataSet();



            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];

            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))

            {

                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);

            }





            try

            {

                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);





                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                parametros.Add("@Id_Num_UN", Id_Num_UN);





                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "PasilloUN_CrearEspeciales_iUp", false, parametros);



                return ds;

            }

            catch (SqlException ex)

            {



                throw ex;

            }

            catch (System.Exception ex)

            {



                throw ex;

            }

        }

        public static DataSet ReporteMapeoPasillo(int Id_Num_UN, int Id_Cnsc_Pasillo = 0, int Id_Num_Div = 0, int Id_Num_Categ = 0)

        {

            DataSet ds = new DataSet();



            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];

            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))

            {

                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);

            }





            try

            {

                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);





                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                parametros.Add("@Id_Num_UN", Id_Num_UN);



                if (Id_Cnsc_Pasillo != 0)

                    parametros.Add("@Id_Cnsc_Pasillo", Id_Cnsc_Pasillo);



                if (Id_Num_Div != 0)

                    parametros.Add("@Id_Num_Div", Id_Num_Div);



                if (Id_Num_Categ != 0)

                    parametros.Add("@Id_Num_Categ", Id_Num_Categ);





                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "ReporteMapeoPasillo_sUp", false, parametros);



                return ds;

            }

            catch (SqlException ex)

            {



                throw ex;

            }

            catch (System.Exception ex)

            {



                throw ex;

            }

        }

        public static DataSet GetPasilloCapturaAvance()

        {

            DataSet ds = new DataSet();



            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];

            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))

            {

                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);

            }





            try

            {

                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);



                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "PasilloCapturaAvance_sup", false);



                return ds;

            }

            catch (SqlException ex)

            {



                throw ex;

            }

            catch (System.Exception ex)

            {



                throw ex;

            }

        }

        public static DataSet GetPasilloCapturaAvanceUN(int Id_Num_UN)

        {

            DataSet ds = new DataSet();



            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];

            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))

            {

                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);

            }





            try

            {

                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);





                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                parametros.Add("@Id_Num_UN", Id_Num_UN);





                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "PasilloCapturaAvanceUN_sup", false, parametros);



                return ds;

            }

            catch (SqlException ex)

            {



                throw ex;

            }

            catch (System.Exception ex)

            {



                throw ex;

            }

        }

        public static DataSet UpdPasilloUN(int Id_Num_UN, int Id_Cnsc_Pasillo, string Nom_Pasillo, int Num_Orden)

        {



            DataSet ds = new DataSet();



            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];

            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))

            {

                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);

            }





            try

            {

                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);





                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                parametros.Add("@Id_Num_UN", Id_Num_UN);

                parametros.Add("@Id_Cnsc_Pasillo", Id_Cnsc_Pasillo);

                parametros.Add("@Nom_Pasillo", Nom_Pasillo);

                parametros.Add("@Num_Orden", Num_Orden);





                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "PasilloUN_uUp", false, parametros);



                return ds;

            }

            catch (SqlException ex)

            {



                throw ex;

            }

            catch (System.Exception ex)

            {



                throw ex;

            }

        }

        public static DataSet PasilloUnEditCateg(int Id_Num_UN, int Id_Cnsc_Pasillo, int Id_Num_Div, int Id_Num_Categ)

        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];

            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))

            {

                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);

            }

            try
            {

                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                parametros.Add("@Id_Num_UN", Id_Num_UN);


                if (Id_Cnsc_Pasillo != 0)
                    parametros.Add("@Id_Cnsc_Pasillo", Id_Cnsc_Pasillo);

                if (Id_Num_Div != 0)
                    parametros.Add("@Id_Num_Div", Id_Num_Div);

                if (Id_Num_Categ != 0)
                    parametros.Add("@Id_Num_Categ", Id_Num_Categ);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[PasilloUN_EditCateg_V2_sup]", false, parametros);
                return ds;

            }

            catch (SqlException ex)

            {



                throw ex;

            }

            catch (System.Exception ex)

            {



                throw ex;

            }

        }

        public static DataSet AddPasilloUN_Linea(int Id_Num_UN, int Id_Cnsc_Pasillo, int Id_Num_Linea)

        {

            DataSet ds = new DataSet();
            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))

            {

                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);

            }
            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                System.Collections.Hashtable parametros = new System.Collections.Hashtable();
                parametros.Add("@Id_Num_UN", Id_Num_UN);
                parametros.Add("@Id_Cnsc_Pasillo", Id_Cnsc_Pasillo);
                parametros.Add("@Id_Num_Linea", Id_Num_Linea);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "PasilloUN_Linea_iup", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }

        public static DataSet DelPasilloUN_Linea(int Id_Num_UN, int Id_Cnsc_Pasillo, int Id_Num_Linea)

        {
            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))

            {

                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);

            }
            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                System.Collections.Hashtable parametros = new System.Collections.Hashtable();
                parametros.Add("@Id_Num_UN", Id_Num_UN);
                parametros.Add("@Id_Cnsc_Pasillo", Id_Cnsc_Pasillo);
                parametros.Add("@Id_Num_Linea", Id_Num_Linea);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "PasilloUN_Linea_dup", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet PasilloUN_MoverLinea_uup(int Id_Num_UN, int Id_Cnsc_Pasillo, int Id_Num_Linea, int Num_Orden)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];

            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }

            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                parametros.Add("@Id_Num_UN", Id_Num_UN);
                parametros.Add("@Id_Cnsc_Pasillo", Id_Cnsc_Pasillo);
                parametros.Add("@Id_Num_Linea", Id_Num_Linea);
                parametros.Add("@Num_Orden", Num_Orden);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "PasilloUN_MoverLinea_uup", false, parametros);

                return ds;
            }
            catch (SqlException ex)

            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }



        #endregion

        #region Cancelacion Orden
        public static DataSet CancelaOrden_Uup(int Orden, string motivo = "", int Id_Num_MotCan = 0,string UeNo="")
        {
            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);


                System.Collections.Hashtable parametros = new System.Collections.Hashtable();
                parametros.Add("@Orden", Orden);
                parametros.Add("@motivo", motivo);
                if (Id_Num_MotCan != 0)
                    parametros.Add("@Id_Num_MotCan", Id_Num_MotCan);
                parametros.Add("@UeNo", UeNo);


                //[dbo].[ecCancelaOrden_Uup_V2]
                //@Orden INT
                //, @motivo            VARCHAR(50)
                //,@Id_Num_MotCan TINYINT = NULL
                //, @UeNo            VARCHAR(25


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "ecCancelaOrden_Uup_V2", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }
        public static DataSet CancelaOrden_Uup2(int Orden, string motivo = "", int Id_Num_MotCan = 0, string UeNo = "", string idUsuario ="")
        {
            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);


                System.Collections.Hashtable parametros = new System.Collections.Hashtable();
                parametros.Add("@Orden", Orden);
                parametros.Add("@motivo", motivo);
                if (Id_Num_MotCan != 0)
                    parametros.Add("@Id_Num_MotCan", Id_Num_MotCan);
                parametros.Add("@UeNo", UeNo);
                parametros.Add("@idUsuario", idUsuario);


                //[dbo].[ecCancelaOrden_Uup_V2]
                //@Orden INT
                //, @motivo            VARCHAR(50)
                //,@Id_Num_MotCan TINYINT = NULL
                //, @UeNo            VARCHAR(25


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "ecCancelaOrden_Uup_V3", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }

        //CatMotivoCan_Sup
        public static DataSet CatMotivoCan_Sup(int Id_Num_MotCan = 0, bool Bit_Eliminado = false)
        {
            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }

            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                if (Id_Num_MotCan != 0)
                    parametros.Add("@Id_Num_MotCan", Id_Num_MotCan);

                parametros.Add("@Bit_Eliminado", Bit_Eliminado);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "CatMotivoCan_Sup", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Traspaso

        public static DataSet TraspasaOrdenDeTienda(string UeNo, int NumUnNva)
        {
            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }

            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                parametros.Add("@UeNo", UeNo);
                parametros.Add("@NumUnNva", NumUnNva);


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "SP_TraspasaOrdenDeTienda_V2", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region reportes
        public static DataSet RepDiaSurtido(int opc, string feci, string fecf, int orden, int Id_Num_Un)
        {
            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }

            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                parametros.Add("@opc", opc);
                if (opc != 6)
                {
                    parametros.Add("@fecini", feci);
                    parametros.Add("@fecfin", fecf);
                }
                parametros.Add("@orden", orden);
                if (Id_Num_Un != 0)
                    parametros.Add("@Id_Num_Un", Id_Num_Un);


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "ecRepDiaSurtido_Sup", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }

        public static DataTable GetImportesOEntr(int opc, string feci, string fecf, int orden, int Id_Num_Un, int Id_Num_FormaPago)
        {
            DataTable ds = GetTable();
            try
            {
                var d = RepDiaSurtido(opc, feci, fecf, orden, Id_Num_Un);
                decimal? pedido = 0;
                decimal? cobrado = 0;
                decimal? vlst1 = 0;
                decimal? vlst2 = 0;
                decimal? vlst3 = 0;
                decimal? vlst4 = 0;
                decimal? vlst5 = 0;
                decimal? vltotal = 0;
                decimal? vltotalP = 0;
                foreach (DataRow item in d.Tables[0].Rows)
                {
                    decimal? Importe = (decimal?)item["Importe"];
                    decimal? TotalNormalComp = (decimal?)item["TotalNormalComp"];
                    decimal? TotalOfertaComp = (decimal?)item["TotalOfertaComp"];
                    decimal? TotalNormalSurt = (decimal?)item["TotalNormalSurt"];
                    decimal? TotalOfertaSurt = (decimal?)item["TotalOfertaSurt"];


                    if (TotalOfertaComp < TotalNormalComp)
                    {
                        pedido = TotalOfertaComp;
                        vltotalP = vltotalP + TotalOfertaComp;
                    }
                    else
                    {
                        pedido = TotalNormalComp;
                        vltotalP = vltotalP + TotalNormalComp;
                    }

                    if (Importe == null)
                    {
                        if (TotalOfertaSurt == null & TotalNormalSurt == null)
                        {
                            if (TotalOfertaComp < TotalNormalComp)
                            {
                                cobrado = TotalOfertaComp;
                                vltotal = vltotal + TotalOfertaComp;
                                if (Id_Num_FormaPago == 1) { vlst1 = vlst1 + TotalOfertaComp; }
                                if (Id_Num_FormaPago == 2) { vlst2 = vlst2 + TotalOfertaComp; }
                                if (Id_Num_FormaPago == 3) { vlst3 = vlst3 + TotalOfertaComp; }
                                if (Id_Num_FormaPago == 4) { vlst4 = vlst4 + TotalOfertaComp; }
                                if (Id_Num_FormaPago == 5) { vlst5 = vlst5 + TotalOfertaComp; }
                            }
                            else
                            {
                                cobrado = TotalNormalComp;
                                vltotal = vltotal + TotalNormalComp;
                                if (Id_Num_FormaPago == 1) { vlst1 = vlst1 + TotalNormalComp; }
                                if (Id_Num_FormaPago == 2) { vlst2 = vlst2 + TotalNormalComp; }
                                if (Id_Num_FormaPago == 3) { vlst3 = vlst3 + TotalNormalComp; }
                                if (Id_Num_FormaPago == 4) { vlst4 = vlst4 + TotalNormalComp; }
                                if (Id_Num_FormaPago == 5) { vlst5 = vlst5 + TotalNormalComp; }
                            }
                        }
                        else
                        {
                            if (TotalOfertaSurt < TotalNormalSurt)
                            {
                                cobrado = TotalOfertaSurt;
                                vltotal = vltotal + TotalOfertaSurt;
                                if (Id_Num_FormaPago == 1) { vlst1 = vlst1 + TotalOfertaSurt; }
                                if (Id_Num_FormaPago == 2) { vlst2 = vlst2 + TotalOfertaSurt; }
                                if (Id_Num_FormaPago == 3) { vlst3 = vlst3 + TotalOfertaSurt; }
                                if (Id_Num_FormaPago == 4) { vlst4 = vlst4 + TotalOfertaSurt; }
                                if (Id_Num_FormaPago == 5) { vlst5 = vlst5 + TotalOfertaSurt; }
                            }
                            else
                            {
                                cobrado = TotalNormalSurt;
                                vltotal = vltotal + TotalNormalSurt;
                                if (Id_Num_FormaPago == 1) { vlst1 = vlst1 + TotalNormalSurt; }
                                if (Id_Num_FormaPago == 2) { vlst2 = vlst2 + TotalNormalSurt; }
                                if (Id_Num_FormaPago == 3) { vlst3 = vlst3 + TotalNormalSurt; }
                                if (Id_Num_FormaPago == 4) { vlst4 = vlst4 + TotalNormalSurt; }
                                if (Id_Num_FormaPago == 5) { vlst5 = vlst5 + TotalNormalSurt; }
                            }
                        }
                    }
                    else
                    {
                        cobrado = Importe;
                        if (Id_Num_FormaPago == 1) { vlst1 = vlst1 + Importe; }
                        if (Id_Num_FormaPago == 2) { vlst2 = vlst2 + Importe; }
                        if (Id_Num_FormaPago == 3) { vlst3 = vlst3 + Importe; }
                        if (Id_Num_FormaPago == 4) { vlst4 = vlst4 + Importe; }
                        if (Id_Num_FormaPago == 5) { vlst5 = vlst5 + Importe; }

                    }
                }


                ds.Rows.Add(pedido, cobrado, vlst1, vlst2, vlst3, vlst4, vlst5);
                return ds;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }

        static DataTable GetTable()
        {
            // Step 2: here we create a DataTable.
            // ... We add 4 columns, each with a Type.
            DataTable table = new DataTable();
            table.Columns.Add("ImportePedido", typeof(decimal));
            table.Columns.Add("ImporteSurtido", typeof(decimal));

            table.Columns.Add("vlst1", typeof(decimal));
            table.Columns.Add("vlst2", typeof(decimal));
            table.Columns.Add("vlst3", typeof(decimal));
            table.Columns.Add("vlst4", typeof(decimal));
            table.Columns.Add("vlst5", typeof(decimal));




            return table;
        }

        public static DataTable GetTableProducts()
        {
            // Step 2: here we create a DataTable.
            // ... We add 4 columns, each with a Type.
            DataTable table = new DataTable();
            table.Columns.Add("ProductId", typeof(int));
            table.Columns.Add("Peso_WT", typeof(decimal));

            table.Columns.Add("Cve_CategSAP", typeof(string));
            table.Columns.Add("Cve_GciaCategSAP", typeof(string));
            table.Columns.Add("Cve_GpoCategSAP", typeof(string));
            table.Columns.Add("Desc_CategSAP", typeof(string));

            return table;
        }
        #endregion

        #region Actualiza fecha Entrega
        //CalEntrega_Fec_Entrega_Uup

        public static DataSet Fec_Entrega_Uup(string UeNo, string Fec_Entrega)
        {
            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }

            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                System.Collections.Hashtable parametros = new System.Collections.Hashtable();



                parametros.Add("@UeNo", UeNo);
                parametros.Add("@Fec_Entrega", Fec_Entrega);


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "CalEntrega_Fec_Entrega_V2_Uup", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Insercion Productos a la Orden

        public static void UpdProductsToOrder(string OrderNo, string PosBarcode, string PosProductDescription
            , string PosQuantity, string PosPriceNormalSale, string PosPriceOfferSale
            , string ProductId, string UnitMeasure, string comments,string UeNo)
        {


            //upCorpOms_Upd_ProductsToOrder


            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);


                System.Collections.Hashtable parametros = new System.Collections.Hashtable();
                parametros.Add("@OrderNo", OrderNo);
                parametros.Add("@PosBarcode", PosBarcode);
                parametros.Add("@PosProductDescription", PosProductDescription);
                parametros.Add("@PosQuantity", PosQuantity);
                parametros.Add("@PosPriceNormalSale", PosPriceNormalSale);
                parametros.Add("@PosPriceOfferSale", PosPriceOfferSale);
                parametros.Add("@ProductId", ProductId);
                parametros.Add("@UnitMeasure", UnitMeasure);
                parametros.Add("@Desc_ArtCarObser", comments);
                parametros.Add("@UeNo", UeNo);

                

                Soriana.FWK.FmkTools.SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "upCorpOms_Upd_ProductsToOrder_v2", false, parametros);


            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Carriers DST

        public static DataSet GetCarriersDST()
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[upCorpOms_Cns_CarrierDTS]", false);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }


        }

        public static DataSet GetCarriersUNDST(int Id_Num_UN)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                parametros.Add("@Id_Num_UN", Id_Num_UN);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[upCorpOms_Cns_CarrierDTSByUn]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }


        }

        public static DataSet AddCarriersDST(string num, string name, string un)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);



                parametros.Add("@Name", name);
                parametros.Add("@Id_Num_UN", un);
                parametros.Add("@Id_Num_Empleado", num);



                Soriana.FWK.FmkTools.SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[upCorpOms_Ins_CarrierDTS]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }


        }

        public static DataSet EditCarriersDST(string num, string name, string un, string status)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);



                parametros.Add("@Id_Carrier", num);
                parametros.Add("@Name", name);
                parametros.Add("@Id_Num_UN", un);
                //parametros.Add("@Id_Num_Empleado", num);
                parametros.Add("@Activo", status);



                Soriana.FWK.FmkTools.SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[upCorpOms_Upd_CarrierDTS]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }


        }

        public static DataSet DeleteCarriersDST(string num)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                parametros.Add("@Id_Carrier", num);

                Soriana.FWK.FmkTools.SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[upCorpOms_Del_CarrierDTS]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }


        }

        public static DataSet GetCarrierDST(string num)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);



                parametros.Add("@Id_Carrier", num);



                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[upCorpOms_Cns_CarrierDTSById]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }


        }

        #endregion


        public static DataSet upCorpOms_Sel_ShipmentRequestsFromWMS()
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[upCorpOms_Sel_ShipmentRequestsFromWMS]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }
        public static DataSet upCorpOms_Sel_ShipmentProcessedFromWMS()
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[upCorpOms_Sel_ShipmentProcessedFromWMS]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }
        public static DataSet GetDimensionsByProduct(string ProductId)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DM"].ConnectionString);

                System.Collections.Hashtable parametros = new System.Collections.Hashtable();
                parametros.Add("@Material_MATNR", ProductId);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[up_CorpTMS_cmd_SKU_dimensions]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }
        public static DataSet GetDimensionsByProducts(string ProductId)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DM"].ConnectionString);

                System.Collections.Hashtable parametros = new System.Collections.Hashtable();
                parametros.Add("@Material_MATNR", ProductId);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[up_CorpTMS_cmd_SKU_dimensionsByProducts]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }
        public static DataSet ActiveEnviaCom()
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }


            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[tms].[upCorpTms_Cns_EnviaCom]", false);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }
        public static DataSet UCCProcesada(string ucc)
        {
            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }

            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                parametros.Add("@ucc", ucc);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "upCorpOms_Upd_ShipmentRequestsFromWMSByUCC", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }
        public static DataSet OrdersLogistics(int orderNo, decimal PesoOrder, bool Bigticket)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }
            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                System.Collections.Hashtable parametros = new System.Collections.Hashtable();
                parametros.Add("@orderNo", orderNo);
                parametros.Add("@PesoOrder", PesoOrder); 
                parametros.Add("@Bigticket", Bigticket);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[upCorpOms_Cns_OrdersLogistics]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }
        public static DataSet CarrierSelected(int orderNo)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }
            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                System.Collections.Hashtable parametros = new System.Collections.Hashtable();
                parametros.Add("@orderNo", orderNo);
                
                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[upCorpOms_Cns_CarrierSelected]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }
        public static DataSet GuardarTarifasLogyt(int orderNo, decimal PesoOrder, bool Bigticket, DataTable dt)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }
            try
            {
                //Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                //System.Collections.Hashtable parametros = new System.Collections.Hashtable();
                //parametros.Add("@orderNo", orderNo);
                //parametros.Add("@PesoOrder", PesoOrder);
                //parametros.Add("@Bigticket", Bigticket);
                ////parametros.Add("@TrackingItemsType", dt);
                //SqlParameter param = new SqlParameter("@TrackingItemsType", SqlDbType.Structured)
                //{
                //    TypeName = "dbo.TrackingItemsTableType",
                //    Value = dt
                //};
                //parametros.Add("@TrackingItemsType", param);


                //ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[upCorpOms_Ins_UeNoRatesLogyt]", false, parametros);
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString))
                {
                    using (SqlCommand sqlComm = new SqlCommand("dbo.upCorpOms_Ins_UeNoRatesLogyt", con))
                    {
                        sqlComm.CommandType = CommandType.StoredProcedure;

                        sqlComm.Parameters.AddWithValue("@orderNo", orderNo);
                        sqlComm.Parameters.AddWithValue("@PesoOrder", PesoOrder);
                        sqlComm.Parameters.AddWithValue("@Bigticket", Bigticket);

                        SqlParameter param = new SqlParameter("@TrackingItemsType", SqlDbType.Structured)
                        {
                            TypeName = "dbo.TrackingItemsTableType",
                            Value = dt
                        };
                        sqlComm.Parameters.Add(param);

                        
                        con.Open();
                        //sqlComm.ExecuteReader();



                        SqlDataAdapter adapter = new SqlDataAdapter(sqlComm);
                        adapter.Fill(ds);



                    }
                }

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }
    }
}