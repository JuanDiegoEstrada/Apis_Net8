namespace WebApplicationAPI_2.Connections
{
    public class AccessSQL
    {
        private string cadConnectionSQL;
        public string CadConnection { get => cadConnectionSQL; }
        public AccessSQL(string ConnectionSQL)
        {
            cadConnectionSQL = ConnectionSQL;
        }
    }
}
