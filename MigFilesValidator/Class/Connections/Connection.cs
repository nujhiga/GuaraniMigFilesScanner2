using Npgsql;

using Renci.SshNet;

using System;
using System.Data;
using System.Threading.Tasks;

namespace GuaraniMigFilesScanner.Class.Connections
{
    public class Connection : IDisposable
    {
        private const uint PORT = 5432;

        private const string LOCAL_HOST = "127.0.0.1";
        public ConnectionType ConnectionType { get; set; }
        public string SShHost { get; set; }
        public string SShUser { get; set; }
        public string SShPassword { get; set; }
        public string Database { get; set; }
        public string DbUser { get; set; }
        public string DbPassword { get; set; }

        private NpgsqlConnection _conn;

        private SshClient _client;

        private ForwardedPortLocal _lPort;

        public async ValueTask<NpgsqlConnection> OpenAsync()
        {
            _client = new SshClient(SShHost, SShUser, SShPassword);
            _client.Connect();

            _lPort = new ForwardedPortLocal(LOCAL_HOST, LOCAL_HOST, PORT);
            _client.AddForwardedPort(_lPort);

            _lPort.Start();

            string connString = $"Host={_lPort.BoundHost};Database={Database};Port={_lPort.BoundPort};Username={DbUser};Password={DbPassword};Timeout=60";

            _conn = new NpgsqlConnection(connString);

            await _conn.OpenAsync();

            return _conn;

        }
        public async void Dispose()
        {
            if (_conn.State != ConnectionState.Open) return;

            await _conn.CloseAsync();

            _client.Disconnect();

            _lPort.Stop();

            _lPort.Dispose();

            _client.Dispose();

            await _conn.DisposeAsync();
        }
    }
}
