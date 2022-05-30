using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace PraktikumWeek_14
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static string connectionString = "server=localhost;uid=root;pwd=;database=premier_league;";
        public MySqlConnection sqlConnect = new MySqlConnection(connectionString);
        public MySqlCommand sqlCommand;
        public MySqlDataAdapter sqlAdapter;
        public string sqlQuery;

        int index = 0;

        DataTable dtDGV = new DataTable();
        DataTable dtTeam = new DataTable();

        public void team(int index)
        {
            sqlQuery = "select t.team_name as 'Team Name', m.manager_name as 'Manager Name', concat(t.home_stadium,', ', t.city,'(', t.capacity,')') as 'Stadium', t.team_id from team t left join manager m on m.manager_id = t.manager_id order by 1;";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(dtTeam);
            lblTeamName.Text = dtTeam.Rows[index][0].ToString();
            lblManager.Text = dtTeam.Rows[index][1].ToString();
            lblStadium.Text = dtTeam.Rows[index][2].ToString();
        }
        public void TopScor(int index)
        {
            sqlQuery = "select p.player_name as 'Player name', (sum(if(dm.`type` = 'GO',1,0)) + sum(if(dm.`type` = 'GP',1,0))) as 'Jumlah goal'  from dmatch dm, player p, team t where p.player_id = dm.player_id and dm.team_id = t.team_id and t.team_name = '" + lblTeamName.Text + "' group by 1 order by 2 desc;";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(dtTeam);
            lblTopScore.Text = dtTeam.Rows[index][0].ToString();
            lblJumlahGoal.Text = dtTeam.Rows[index][1].ToString();
        }
        public void WorstDisc(int index)
        {
            sqlQuery = "select p.player_name as 'Player name', (sum(if(dm.`type` = 'CY',1,0)) + sum(if(dm.`type` = 'CR',3,0))) as 'Poin Merah' from dmatch dm, player p, team t where p.player_id = dm.player_id and dm.team_id = t.team_id and t.team_name = '" + lblTeamName.Text + "' group by 1 order by 2 desc;";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(dtTeam);
            lblWorstDisc.Text = dtTeam.Rows[index][0].ToString();
        }
        public void DGV(int index)
        {
            sqlQuery = $"SELECT * FROM (SELECT m.match_date, IF(m.team_home = '{dtTeam.Rows[index][3].ToString()}', 'HOME', 'AWAY') AS 'Home / Away', CONCAT('vs ', IF(m.team_home = '{dtTeam.Rows[index][3].ToString()}', tAway.team_name, tHome.team_name)) AS 'Lawan', CONCAT(goal_home, ' - ', goal_away) AS 'Score' FROM `match` m LEFT JOIN team tHome ON m.team_home = tHome.team_id LEFT JOIN team tAway ON tAway.team_id = m.team_away WHERE m.team_home = '{dtTeam.Rows[index][3].ToString()}' OR m.team_away = '{dtTeam.Rows[index][3].ToString()}' ORDER BY m.match_id DESC LIMIT 5) dt ORDER BY 1;";
            sqlCommand = new MySqlCommand(sqlQuery, sqlConnect);
            sqlAdapter = new MySqlDataAdapter(sqlCommand);
            sqlAdapter.Fill(dtDGV);
            DGV5Match.DataSource = dtDGV;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            team(index);
            TopScor(index);
            WorstDisc(index);
            DGV(index);
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            index = 0;

            team(index);
            TopScor(index);
            WorstDisc(index);
            DGV(index);
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (index <= 0)
            {
                index = 0;
                MessageBox.Show("Error");
            }
            else
                index--;

            team(index);
            TopScor(index);
            WorstDisc(index);
            DGV(index);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (index >= dtTeam.Rows.Count-1)
            {
                index = dtTeam.Rows.Count - 1;
                MessageBox.Show("Error");
            }
            else
                index++;

            team(index);
            TopScor(index);
            WorstDisc(index);
            DGV(index);
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            index = dtTeam.Rows.Count - 1;

            team(index);
            TopScor(index);
            WorstDisc(index);
            DGV(index);
        }
    }
}
