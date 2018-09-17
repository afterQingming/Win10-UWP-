using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOSAD0.Models;
using System.Diagnostics;

namespace MOSAD0.Services
{
    
    class DatabaseService
    {
        public static DatabaseService databaseService;
        public static DatabaseService GetInstance()
        {
            return databaseService == null ? databaseService = new DatabaseService() : databaseService;
        }
        public static SQLiteConnection _connection;
        private static String DB_NAME = "SQLiteSample.db";
        private static String TABLE_NAME = "SampleTable";
        public DatabaseService()
        {
            CreateTable();
        }
        public void CreateTable()
        {
            _connection = new SQLiteConnection(DB_NAME);
            String SQL_CREATE_TABLE = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " (id TEXT PRIMARY KEY,title TEXT,detail TEXT, size DOUBLE, date DATE,selected BOOL );";
            using (var statement = _connection.Prepare(SQL_CREATE_TABLE))
            {
                statement.Step();
            }
        }
        public void Insert(TodoItem todoItem)
        {
            String SQL_INSERT = "INSERT INTO " + TABLE_NAME + " VALUES(?,?,?,?,?,?);";
            using (var statement = _connection.Prepare(SQL_INSERT))
            {
                statement.Bind(1, todoItem.id);
                statement.Bind(2, todoItem.title);
                statement.Bind(3, todoItem.description);
                statement.Bind(4, todoItem.imgSize);
                statement.Bind(5, todoItem.date.ToString());
                statement.Bind(6, todoItem.selected.ToString());
                statement.Step();
            }
        }
        public void Delete(string id)
        {
            String SQL_DELETE = "DELETE FROM " + TABLE_NAME + " WHERE id = ?";
            using (var statement = _connection.Prepare(SQL_DELETE))
            {
                statement.Bind(1, id);
                statement.Step();
            }
        }
        public void Update(string id, string title, string detail, double size, DateTime date)
        {
            String SQL_UPDATE = "UPDATE " + TABLE_NAME + " SET id = ?,title=?,detail=?,size=?,date=? WHERE id = ?";
            using (var statement = _connection.Prepare(SQL_UPDATE))
            {
                statement.Bind(1, id);
                statement.Bind(2, title);
                statement.Bind(3, detail);
                statement.Bind(4, size);
                statement.Bind(5, date.ToString());
                statement.Bind(6, id);
                statement.Step();
            }
        }
        public string Query(string text)
        {
            try
            {
                string alert = "";
                String SQL= "SELECT title,detail, date FROM " + TABLE_NAME + " WHERE title LIKE (?) OR detail LIKE (?) OR date LIKE(?);";
                using (var statement = _connection.Prepare(SQL))
                {
                    statement.Bind(1, "%%" + text + "%%");
                    statement.Bind(2, "%%" + text + "%%");
                    statement.Bind(3, "%%" + text + "%%");
                    while (SQLiteResult.ROW == statement.Step())
                    {
                        string title = statement[0] as string;
                        string detail = statement[1] as string;
                        string date = statement[2] as string;
                        alert += "Title: " + title + ";\nDetail: " + detail + ";\nDue Date: " + statement[0] + "\n\n";
                    }
                }
                return alert;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Complete(string id, bool IsCompleted)
        {
            String SQL_UPDATE = "UPDATE " + TABLE_NAME + " SET selected=? WHERE id = ?";
            using (var statement = _connection.Prepare(SQL_UPDATE))
            {
                statement.Bind(2, id);
                statement.Bind(1, IsCompleted.ToString());
                statement.Step();
            }
        }
        public void LoadData()
        {
            try
            {
                var sql = "SELECT id, title, detail,size, date, selected FROM "+TABLE_NAME;
                using (var statement = _connection.Prepare(sql))
                {
                    while (SQLiteResult.ROW == statement.Step())
                    {
                        string id = (string)statement[0];
                        string title = (string)statement[1];
                        string detail = (string)statement[2];
                        double size = (double)statement[3];
                        DateTime date = Convert.ToDateTime(statement[4]);
                        bool selected = Convert.ToBoolean(statement[5]);
                        Models.myItem temp = new Models.myItem(id, title, detail, size, date, selected);
                        Models.TodoItem todoitem = new Models.TodoItem(temp);
                        ViewModels.TodoItemViewModel.GetInstance().AllItems.Add(todoitem);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
        public void SaveTemp(string title, string detail, double size, DateTime date)
        {
            var conn = new SQLiteConnection(DB_NAME);
            String SQL = "DROP TABLE IF EXISTS " + "TEMP" + ";";
            using (var statement = conn.Prepare(SQL))
            {
                statement.Step();
            }
            SQL = "CREATE TABLE IF NOT EXISTS " + "TEMP" + " (title TEXT,detail TEXT, size DOUBLE, date DATE );";
            using (var statement = conn.Prepare(SQL))
            {
                statement.Step();
            }
            
            SQL = "INSERT INTO "+"TEMP"+" VALUES(?,?,?,?);";
            using(var statement = conn.Prepare(SQL))
            {
                statement.Bind(1, title);
                statement.Bind(2, detail);
                statement.Bind(3, size);
                statement.Bind(4, date.ToString());
                
                statement.Step();
            }
        }
        public void ResumeTemp(string[] temp)
        {
            try
            {
                var conn = new SQLiteConnection(DB_NAME);
                String SQL = "SELECT title,detail,size,date FROM " + "TEMP" + ";";
                using (var statement = conn.Prepare(SQL))
                {
                    while (SQLiteResult.ROW == statement.Step())
                    {
                        temp[0] =(string) statement[0];
                        temp[1] = statement[1] as string;
                        temp[2] = statement[2] as string;
                        temp[3] = statement[3] as string;
                    }
                }
                using (var statement = conn.Prepare(SQL))
                {
                    statement.Step();
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
