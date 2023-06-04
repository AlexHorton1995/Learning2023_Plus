using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Win32.SafeHandles;
using Dapper;
using System.Data;

namespace CBAccountIntake
{
    public interface IDataAccess
    {
        bool CreateTempTable();
        bool BulkCopyDataTable(DataTable? dt);
        bool InsertAccounts();
        bool InsertMembers();
        bool DropTempTable();

    }

    public class DataAccess : IDataAccess, IDisposable
    {
        private readonly string? ConnString;
        private bool disposedValue;

        public DataAccess()
        {
            ConnString = Environment.GetEnvironmentVariable("CBDataConnString");
        }

        public bool CreateTempTable()
        {
            bool retVal = false;

            string sql = @"
	        CREATE TABLE [TempData](
		        [AccountNumber] VARCHAR(16) NOT NULL,
		        [ExpDate] CHAR(8) NOT NULL,
                [CardID] CHAR(3) NOT NULL,
		        [FName] VARCHAR(50),
		        [LName] VARCHAR(50),
		        [BAddress1] VARCHAR(50),
		        [BAddress2] VARCHAR(50),
		        [BCity] VARCHAR(50),
		        [BState] VARCHAR(50),
		        [BZip] VARCHAR(50)
                )";

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                try
                {
                    conn.Execute(sql);
                    retVal = true;
                }
                catch (Exception)
                {
                }
                return retVal;
            }
        }

        public bool BulkCopyDataTable(DataTable? dt)
        {
            bool retVal = false;

            using (var conn = new SqlConnection(ConnString))
            {
                conn.Open();
                using (var bulkCopy = new SqlBulkCopy(conn))
                {
                    bulkCopy.DestinationTableName = dt?.TableName;
                    try
                    {
                        bulkCopy.WriteToServer(dt);
                        retVal = true;
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            return retVal;
        }

        public bool InsertAccounts()
        {
            bool retVal = false;

            using (var conn = new SqlConnection(ConnString))
            {
                conn.Open();

                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        conn.Execute($@"
                    OPEN SYMMETRIC KEY CB_SymKey DECRYPTION BY CERTIFICATE CB_Protect_Data;

                    INSERT INTO [dbo].[MemberAccountData] 
                        (AccountToken, AccountNumberEncrypted, ExpYearEncrypted, CardIdEncrypted)
                    SELECT HASHBYTES('SHA2_256', AccountNumber),
                        EncryptByKey (Key_GUID('CB_SymKey'), AccountNumber), 
                        EncryptByKey (Key_GUID('CB_SymKey'), ExpDate), 
                        EncryptByKey (Key_GUID('CB_SymKey'), CardID)
                    FROM TEMPDATA
                    LEFT OUTER JOIN [MemberAccountData] 
                        ON AccountToken = HASHBYTES('SHA2_256', AccountNumber)
                    WHERE MemberAccountID IS NULL                        
                    ", 
                            null, 
                            trans);
                        trans.Commit();
                        retVal = true;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                    }
                }
            }
            return retVal;
        }

        public bool InsertMembers()
        {
            bool retVal = false;

            using (var conn = new SqlConnection(ConnString))
            {
                conn.Open();

                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        conn.Execute($@"
                    OPEN SYMMETRIC KEY CB_SymKey DECRYPTION BY CERTIFICATE CB_Protect_Data;

                    INSERT INTO [dbo].[MemberData]
                           ([AccountID],[AccountToken],[FirstName],[LastName],[BillingAddress1],[BillingAddress2],
	                        [BillingCity],[BillingState],[BillingZip])
                    SELECT MemberAccountID, HASHBYTES('SHA2_256', AccountNumber), FName, LName, BAddress1, BAddress2, 
		                    BCity, BState, BZip
                    FROM TempData
                    INNER JOIN MemberAccountData AD ON AccountToken = HASHBYTES('SHA2_256', AccountNumber) 
                    ",
                            null,
                            trans);
                        trans.Commit();
                        retVal = true;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                    }
                }
            }
            return retVal;
        }


        public bool DropTempTable()
        {
            bool retVal = false;

            using (var conn = new SqlConnection(ConnString))
            {
                conn.Open();

                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        conn.Execute("DROP TABLE TEMPDATA", null, trans);
                        trans.Commit();
                        retVal = true;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                    }
                }
            }
            return retVal;
        }


        #region Disposable
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~DataAccess()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
