using System;
using System.Data;
using System.Data.SqlClient;

namespace CrudMahasiswaADO
{
    public class DAL
    {
        private static string connectionString = @"Data Source=DESKTOP-6V58GOQ\PUTRASQL; Initial Catalog=DBAkademikADO; Integrated Security=True";
        private SqlConnection conn = new SqlConnection(connectionString);
        private SqlDataAdapter da;
        private DataTable dtMahasiswa;
        private DataTable dtProdi;

        public string GetConnectionString()
        {
            return connectionString;
        }

        public int CountMhs()
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            SqlCommand cmd = new SqlCommand("sp_CountMahasiswa", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter outputParam = new SqlParameter("@Total", SqlDbType.Int);
            outputParam.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(outputParam);
            cmd.ExecuteNonQuery();
            return (outputParam.Value == DBNull.Value) ? 0 : Convert.ToInt32(outputParam.Value);
        }

        public DataTable GetMhs()
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            SqlCommand cmd = new SqlCommand("sp_GetMahasiswa", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            da = new SqlDataAdapter(cmd);
            dtMahasiswa = new DataTable();
            da.Fill(dtMahasiswa);
            return dtMahasiswa;
        }

        public void InsertMhs(string nim, string nama, string alamat, string jeniskelamin, DateTime tanggallahir, string kodeProdi, byte[] foto)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            SqlTransaction trans = conn.BeginTransaction();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_InsertMahasiswa", conn, trans);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NIM", nim);
                cmd.Parameters.AddWithValue("@Nama", nama);
                cmd.Parameters.AddWithValue("@Alamat", alamat);
                cmd.Parameters.AddWithValue("@TanggalLahir", tanggallahir);
                cmd.Parameters.AddWithValue("@JK", jeniskelamin);
                cmd.Parameters.AddWithValue("@KodeProdi", kodeProdi);

                if (foto != null) cmd.Parameters.AddWithValue("@pFoto", foto);
                else cmd.Parameters.Add("@pFoto", SqlDbType.VarBinary, -1).Value = DBNull.Value;

                cmd.ExecuteNonQuery();
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
            finally { conn.Close(); }
        }

        public void UpdateMhs(string nim, string nama, string alamat, string jeniskelamin, DateTime tanggallahir, string kodeProdi, byte[] foto)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            SqlCommand cmd = new SqlCommand("sp_UpdateMahasiswa", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@NIM", nim);
            cmd.Parameters.AddWithValue("@Nama", nama);
            cmd.Parameters.AddWithValue("@Alamat", alamat);
            cmd.Parameters.AddWithValue("@TanggalLahir", tanggallahir);
            cmd.Parameters.AddWithValue("@JK", jeniskelamin);
            cmd.Parameters.AddWithValue("@KodeProdi", kodeProdi);

            if (foto != null) cmd.Parameters.AddWithValue("@pFoto", foto);
            else cmd.Parameters.Add("@pFoto", SqlDbType.VarBinary, -1).Value = DBNull.Value;

            cmd.ExecuteNonQuery();
        }

        public void DeleteMhs(string nim)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            SqlCommand cmd = new SqlCommand("sp_DeleteMahasiswa", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@NIM", nim);
            cmd.ExecuteNonQuery();
        }

        public void InsertLog(string message)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO LogError VALUES(GETDATE(), @pesan)", conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@pesan", message);
            cmd.ExecuteNonQuery();
        }

        public void resetData()
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            string query = @"
                IF OBJECT_ID('dbo.Mahasiswa_Backup') IS NOT NULL
                BEGIN
                    DELETE FROM dbo.Mahasiswa;
                    INSERT INTO dbo.Mahasiswa SELECT * FROM dbo.Mahasiswa_Backup;
                END";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();
        }

        public void testInject(string nim)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            string query = "UPDATE Mahasiswa SET Nama = 'HACKED' WHERE NIM = '" + nim + "'";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();
        }

        public DataTable GetMhsByNIM(string nim)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            SqlCommand cmd = new SqlCommand("sp_GetMahasiswaByNIM", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@pNIM", nim);
            da = new SqlDataAdapter(cmd);
            dtMahasiswa = new DataTable();
            da.Fill(dtMahasiswa);
            return dtMahasiswa;
        }

        public DataTable getProdi()
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            SqlCommand cmd = new SqlCommand("select namaprodi from prodi", conn);
            cmd.CommandType = CommandType.Text;
            dtProdi = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dtProdi);
            return dtProdi;
        }

        public DataTable getDataRekap(string prodi, DateTime tanggalMasuk)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            SqlCommand cmd = new SqlCommand("sp_Report", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@inProdi", prodi);
            cmd.Parameters.AddWithValue("@inTglMsuk", tanggalMasuk.Year.ToString());
            da = new SqlDataAdapter(cmd);
            dtMahasiswa = new DataTable();
            da.Fill(dtMahasiswa);
            return dtMahasiswa;
        }

        public DataTable getAllDataChart()
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            SqlCommand cmd = new SqlCommand("sp_DashBoard", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            da = new SqlDataAdapter(cmd);
            dtMahasiswa = new DataTable();
            da.Fill(dtMahasiswa);
            return dtMahasiswa;
        }

        public DataTable getDataChartByTahun(DateTime thMasuk)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            SqlCommand cmd = new SqlCommand("sp_DashBoardByTahun", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@inTglMsuk", thMasuk.Year);
            da = new SqlDataAdapter(cmd);
            dtMahasiswa = new DataTable();
            da.Fill(dtMahasiswa);
            return dtMahasiswa;
        }
    }
}