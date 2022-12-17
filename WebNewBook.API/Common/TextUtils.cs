using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace WebNewBook.API.Common
{
    public class TextUtils
    {
        static string connectionString = Config.Connection();
        /// <summary>
        /// Gọi store không có tham số
        /// </summary>
        /// <param name="procedureName"></param>
        /// <returns></returns>
        static public DataSet GetDataSet(string procedureName)
        {
            SqlConnection conn = new SqlConnection(connectionString);

            DataSet myDataSet = new DataSet();
            try
            {
                SqlCommand mySqlCommand = new SqlCommand(procedureName, conn);
                mySqlCommand.CommandType = CommandType.StoredProcedure;
                conn.Open();
                SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter(mySqlCommand);
                mySqlDataAdapter.Fill(myDataSet);

                conn.Close();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return myDataSet;
        }
        /// <summary>
        /// Load DataSet từ StoreProcedure
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="param"></param>
        /// <param name="valueParam"></param>
        /// <returns></returns>
        public static DataSet GetDataSetSP(string commandText, string[] param, object[] valueParam)
        {
            try
            {
                DataSet dataSet = new DataSet();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlParameter sqlParam;
                    SqlCommand cmd = new SqlCommand(commandText, conn);
                    if (param != null)
                    {
                        for (int i = 0; i < param.Length; i++)
                        {
                            sqlParam = new SqlParameter(param[i], valueParam[i]);
                            cmd.Parameters.Add(sqlParam);
                        }
                    }

                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dataSet);
                    conn.Close();
                }
                return dataSet;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        /// <summary>
        /// Convert DataTable to List object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            try
            {
                List<T> data = new List<T>();
                foreach (DataRow row in dt.Rows)
                {
                    T item = GetItem<T>(row);
                    data.Add(item);
                }
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private static T GetItem<T>(DataRow dr)
        {
            try
            {
                Type temp = typeof(T);
                T obj = Activator.CreateInstance<T>();

                foreach (DataColumn column in dr.Table.Columns)
                {
                    foreach (PropertyInfo pro in temp.GetProperties())
                    {
                        if (pro.Name == column.ColumnName)
                        {
                            pro.SetValue(obj, dr[column.ColumnName], null);
                        }

                        else
                        {
                            continue;
                        }

                    }
                }
                return obj;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

    }
}
