namespace DayStaySQLQueries
{
    public partial class Form1 : Form
    {
        const string queryStart = "update u_ds set status = 0 where dsid in (", queryEnd = ")";
        OpenFileDialog ofd = new OpenFileDialog();
        List<string> sqlList = new List<string>();

        public Form1()
        {
            InitializeComponent();
            ofd.FileOk += OpenFileDialogClose;
            Shown += Form1_Shown;
        }

        private void Form1_Shown(object? sender, EventArgs e)
        {
            ofd.ShowDialog();
        }

        private void OpenFileDialogClose(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ofd.CheckFileExists) CreateQueries(ofd.OpenFile());
            else ofd.ShowDialog();
        }

        private void CreateQueries(Stream dsidFile)
        {
            List<string> stringsQueries = new List<string>();
            StreamReader streamReader = new StreamReader(dsidFile);
            streamReader.ReadLine();
            string dsid;
            while ((dsid = streamReader.ReadLine()) != null)
                sqlList.Add((dsid.Replace('\"',' ')).Trim());
            int dsidIndex = 0;
            while (dsidIndex < sqlList.Count)
            {
                string query = queryStart + sqlList[dsidIndex++];

                while (dsidIndex < sqlList.Count && (query + sqlList[dsidIndex]).Length + 1 < 2048)
                {
                    query += ',' + sqlList[dsidIndex++];
                }
                query += queryEnd;
                stringsQueries.Add(query);
            }
            File.WriteAllLines(ofd.FileName.Split('.')[0] + ".txt",stringsQueries);
            Close();
        }
    }
}