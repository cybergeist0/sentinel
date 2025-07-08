using System;
using System.Data.SQLite;
using GUI.Models;

namespace GUI.Services
{
    public class SQLiteService
    {
        private const string Conn = "Data Source=sentinel.db";

        public SQLiteService()
        {
            using var c = new SQLiteConnection(Conn);
            c.Open();
            using var cmd = c.CreateCommand();
            cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS servers (
              id TEXT PRIMARY KEY,
              hostname TEXT,
              ip TEXT,
              os_version TEXT,
              free_space_gb REAL,
              memory_gb REAL,
              uptime_hours INTEGER,
              maintenance_start TEXT,
              maintenance_end TEXT
            );";
            cmd.ExecuteNonQuery();
        }

        public void Upsert(ServerInfo s)
        {
            using var c = new SQLiteConnection(Conn);
            c.Open();
            using var cmd = c.CreateCommand();
            cmd.CommandText = @"
            INSERT INTO servers
             (id,hostname,ip,os_version,free_space_gb,memory_gb,uptime_hours,maintenance_start,maintenance_end)
             VALUES
             (@id,@hn,@ip,@os,@fs,@m,@u,@ms,@me)
             ON CONFLICT(id) DO UPDATE SET
               hostname=@hn, ip=@ip, os_version=@os,
               free_space_gb=@fs, memory_gb=@m,
               uptime_hours=@u,
               maintenance_start=@ms, maintenance_end=@me;
            ";
            cmd.Parameters.AddWithValue("@id", s.Id);
            cmd.Parameters.AddWithValue("@hn", s.Hostname);
            cmd.Parameters.AddWithValue("@ip", s.Ip);
            cmd.Parameters.AddWithValue("@os", s.OSVersion);
            cmd.Parameters.AddWithValue("@fs", s.FreeDiskGB);
            cmd.Parameters.AddWithValue("@m", s.MemoryGB);
            cmd.Parameters.AddWithValue("@u", s.UptimeHours);
            cmd.Parameters.AddWithValue("@ms", s.MaintStart);
            cmd.Parameters.AddWithValue("@me", s.MaintEnd);
            cmd.ExecuteNonQuery();
        }
    }
}
